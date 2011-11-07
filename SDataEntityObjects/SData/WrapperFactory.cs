using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sage.Entity.Interfaces;
using Sage.SData.Client.Extensions;
using System.CodeDom;
using System.Reflection;
using System.CodeDom.Compiler;
using System.IO;
using System.Diagnostics;

namespace SDataEntityObjects.SData
{
    /// <summary>
    /// The Client Factory is an internal class that creates the wrappers for SData to Entity.Interfaces dynamically.
    /// </summary>
    internal static class WrapperFactory
    {
        internal static Type GenerateProxyClass(Type interfaceType)
        {
            CodeCompileUnit compileUnit = new CodeCompileUnit();
            
            //Generate Namespace
            CodeNamespace codeNamespace = new CodeNamespace("SDataEntityObjects.Generated");
            {
                compileUnit.Namespaces.Add(codeNamespace);
            }

            //Generate Class
            CodeTypeDeclaration typeDeclatation = new CodeTypeDeclaration(String.Format("{0}", interfaceType.Name));
            {
                typeDeclatation.BaseTypes.Add(new CodeTypeReference(typeof(SDataClientEntityBase)));
                typeDeclatation.BaseTypes.Add(new CodeTypeReference(interfaceType));
                typeDeclatation.BaseTypes.Add(new CodeTypeReference(typeof(Sage.Platform.ComponentModel.INameable)));
                codeNamespace.Types.Add(typeDeclatation);
            }

            //Generate Constructor
            CodeConstructor constructor = new CodeConstructor();
            {
                typeDeclatation.Members.Add(constructor);
                constructor.Parameters.Add(new CodeParameterDeclarationExpression(typeof(SDataPayload), "payload"));
                constructor.Parameters.Add(new CodeParameterDeclarationExpression(typeof(SDataClientContext), "context"));
                constructor.BaseConstructorArgs.Add(new CodeVariableReferenceExpression("payload"));
                constructor.BaseConstructorArgs.Add(new CodeVariableReferenceExpression("context"));
                constructor.Attributes = MemberAttributes.Public;
            }

            // Generate INameable Implementation (Use attributes or best guess)

            string DisplayValue = "";
            if (interfaceType.GetMember("CreateDate").Count() > 0)
                DisplayValue = "CreateDate";
            if (interfaceType.GetMember("Id").Count() > 0)
                DisplayValue = "Id";
            if (interfaceType.GetMember("Description").Count() > 0)
                DisplayValue = "Description";
            if (interfaceType.GetMember("Name").Count() > 0)
                DisplayValue = "Name";
            if (interfaceType.GetMember("Id").Count() > 0)
                DisplayValue = "Id";

            if (interfaceType.GetCustomAttributes(typeof(Sage.Platform.Orm.Attributes.DisplayValuePropertyNameAttribute), false).Count() > 0)
            {
                DisplayValue =
                    (
                        (Sage.Platform.Orm.Attributes.DisplayValuePropertyNameAttribute)
                        interfaceType.GetCustomAttributes(
                            typeof(
                                Sage.Platform.Orm.Attributes.DisplayValuePropertyNameAttribute
                            )
                            , false
                        )
                        .First()
                    )
                    .PropertyName.ToString();
            }

            CodeMemberProperty codeProperty = new CodeMemberProperty();
            if (DisplayValue != "")
            {

                codeProperty.Name = "DisplayValue";
                codeProperty.Type = new CodeTypeReference(typeof(object));
                codeProperty.Attributes = MemberAttributes.Public;
                codeProperty.GetStatements.Add(
                    new CodeMethodReturnStatement(
                            new CodePropertyReferenceExpression(
                                new CodeThisReferenceExpression(),
                                DisplayValue
                            )
                        )
                );
            }
            else
            {
                codeProperty.Name = "DisplayValue";
                codeProperty.Type = new CodeTypeReference(typeof(object));
                codeProperty.Attributes = MemberAttributes.Public;
                codeProperty.GetStatements.Add(
                    new CodeMethodReturnStatement(
                            new CodePropertyReferenceExpression(
                                new CodeThisReferenceExpression(),
                                "Id"
                            )
                        )
                );



            }
            typeDeclatation.Members.Add(codeProperty);





            // If composite properites are contained in this interface, the Ids method for interface ICompositeKeyEntity needs to be implemented.
            // This method keeps track of the composite properties.
            List<KeyValuePair<int, PropertyInfo>> compositeKeyList = new List<KeyValuePair<int, PropertyInfo>>();

            //Generate Properties           
            foreach (PropertyInfo property in interfaceType.GetProperties())
            {

                GeneratePropertyCode(typeDeclatation, property);

                CheckForCompositeKey(property, compositeKeyList);

            }

            //Generate Composite Interface
            if (compositeKeyList.Count > 0)
            {
                // Sort composite keys by "ordinal"
                compositeKeyList.Sort((x, y) => x.Key.CompareTo(y.Key));

                ImplementCompositeInterface(typeDeclatation, compositeKeyList);
            }

            //Generate Methods
            foreach (MethodInfo method in interfaceType.GetMethods())
                GenerateMethodCode(typeDeclatation, method);


            if (Debugger.IsAttached)
                OutputGeneratedCode(compileUnit);

            return CompileCode(compileUnit);
        }

        /// <summary>
        /// This method implements the ICompositeKeyEntity interface method Ids that returns a list of all composite keys.
        /// </summary>
        /// <param name="typeDeclatation"></param>
        /// <param name="compositeKeyList"></param>
        private static void ImplementCompositeInterface(CodeTypeDeclaration typeDeclatation, List<KeyValuePair<int, PropertyInfo>> compositeKeyList)
        {

            CodeMemberProperty property = new CodeMemberProperty();

            property.Type = new CodeTypeReference(typeof(object[]));
            property.Name = "Ids";
            property.Attributes = MemberAttributes.Public;

            List<CodeExpression> parameter = new List<CodeExpression>();

            foreach (var item in compositeKeyList)            
                parameter.Add(new CodeVariableReferenceExpression(item.Value.Name));

            property.GetStatements.Add(
                new CodeMethodReturnStatement(
                    new CodeArrayCreateExpression(
                        new CodeTypeReference(typeof(object[])),
                        parameter.ToArray())));

            typeDeclatation.Members.Add(property);

        }

        private static void CheckForCompositeKey(PropertyInfo property, List<KeyValuePair<int, PropertyInfo>> compositeKeyList)
        {
            object[] attributes = property.GetCustomAttributes(typeof(Sage.Platform.Orm.Attributes.CompositeKeyAttribute), false);

            if (attributes.Length > 0)
            {
                // Just inspect first attribute
                Sage.Platform.Orm.Attributes.CompositeKeyAttribute compositeKeyAttribute = (Sage.Platform.Orm.Attributes.CompositeKeyAttribute)attributes[0];

                compositeKeyList.Add(new KeyValuePair<int, PropertyInfo>(compositeKeyAttribute.Ordinal, property));
            }
        }

        private static Type CompileCode(CodeCompileUnit compileUnit)
        {
            Microsoft.CSharp.CSharpCodeProvider codeProvider = new Microsoft.CSharp.CSharpCodeProvider();
            CompilerParameters compilerParameters = new CompilerParameters();

            compilerParameters.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().Location);
            string currentBinLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            compilerParameters.ReferencedAssemblies.Add(Path.Combine(currentBinLocation, "System.dll"));
            compilerParameters.ReferencedAssemblies.Add(Path.Combine(currentBinLocation, "Sage.Entity.Interfaces.dll"));
            compilerParameters.ReferencedAssemblies.Add(Path.Combine(currentBinLocation, "Sage.Platform.dll"));
            compilerParameters.ReferencedAssemblies.Add(Path.Combine(currentBinLocation, "Sage.SData.Client.dll"));
            compilerParameters.ReferencedAssemblies.Add(Path.Combine(currentBinLocation, "SDataEntityObjects.dll"));

            CompilerResults result = codeProvider.CompileAssemblyFromDom(compilerParameters, compileUnit);

            if (result.Errors.Count > 0)
                throw new InvalidOperationException(result.Errors[0].ErrorText);

            //The only type within the Assembly is the type requested here.
            return result.CompiledAssembly.GetTypes()[0];
        }

        private static void OutputGeneratedCode(CodeCompileUnit compileUnit)
        {
            Microsoft.CSharp.CSharpCodeProvider codeProvider = new Microsoft.CSharp.CSharpCodeProvider();

            using (MemoryStream stream = new MemoryStream())
            using (TextWriter textWriter = new StreamWriter(stream))
            {
                codeProvider.GenerateCodeFromCompileUnit(compileUnit, textWriter, new System.CodeDom.Compiler.CodeGeneratorOptions());

                textWriter.Flush();

                stream.Seek(0, SeekOrigin.Begin);

                using (StreamReader reader = new StreamReader(stream))
                {
                    string output = reader.ReadToEnd();

                    //Trace.WriteLine(output);
                }
            }
        }

        private static void GenerateMethodCode(CodeTypeDeclaration typeDeclatation, MethodInfo method)
        {
            if (!(method.Name.StartsWith("get_") || method.Name.StartsWith("set_")))
            {
                CodeMemberMethod codeMethod = new CodeMemberMethod();

                codeMethod.Name = method.Name;
                codeMethod.ReturnType = new CodeTypeReference(method.ReturnType);
                codeMethod.Attributes = MemberAttributes.Public | MemberAttributes.Final;

                List<ParameterInfo> parameter = new List<ParameterInfo>(method.GetParameters());

                if (parameter.Count > 0)
                {
                    codeMethod.Statements.Add(
                        new CodeVariableDeclarationStatement(
                            typeof(List<KeyValuePair<string, object>>),
                            "parameter",
                            new CodeObjectCreateExpression(
                                typeof(List<KeyValuePair<string, object>>))));

                    foreach (ParameterInfo parameterInfo in parameter)
                    {
                        codeMethod.Statements.Add(
                            new CodeMethodInvokeExpression(
                                new CodeVariableReferenceExpression("parameter"),
                                "Add",
                                new CodeObjectCreateExpression(
                                    typeof(KeyValuePair<string, object>),
                                    new CodePrimitiveExpression(parameterInfo.Name),
                                    new CodeVariableReferenceExpression(parameterInfo.Name))));

                        codeMethod.Parameters.Add(new CodeParameterDeclarationExpression(parameterInfo.ParameterType, parameterInfo.Name));
                    }
                }

                CodeTypeReference returnType = new CodeTypeReference(method.ReturnType);

                CodeMethodInvokeExpression invokeMethod = new CodeMethodInvokeExpression(
                    new CodeBaseReferenceExpression(),
                    "CallMethod",
                    new CodePrimitiveExpression(method.Name),
                    new CodeTypeOfExpression(method.ReturnType),
                    parameter.Count > 0 ?
                    ((CodeExpression)(new CodeVariableReferenceExpression("parameter"))) :
                    ((CodeExpression)(new CodePrimitiveExpression(null))));

                if (method.ReturnType == typeof(void))
                    codeMethod.Statements.Add(invokeMethod);
                else
                    codeMethod.Statements.Add(
                        new CodeMethodReturnStatement(
                            new CodeCastExpression(
                                method.ReturnType,
                                invokeMethod)));

                typeDeclatation.Members.Add(codeMethod);
            }
        }

        private static void GeneratePropertyCode(CodeTypeDeclaration typeDeclatation, PropertyInfo property)
        {
            CodeMemberProperty codeProperty = new CodeMemberProperty();

            codeProperty.Name = property.Name;
            codeProperty.Type = new CodeTypeReference(property.PropertyType);
            codeProperty.Attributes = MemberAttributes.Public;

            if (property.PropertyType.Name == "ICollection`1")
            {

                GenerateCollectionPropertyCode(property, codeProperty);

            }
            else
                if (property.PropertyType.FullName.StartsWith("Sage.Entity.Interfaces."))
                {
                    GenerateEntityPropertyGetterCode(property, codeProperty);

                    if (property.CanWrite)
                        GeneratePropertySetterCode(property, codeProperty);

                }
                else
                {
                    GeneratePropertyGetterCode(property, codeProperty);

                    if (property.CanWrite)
                        GeneratePropertySetterCode(property, codeProperty);
                }

            typeDeclatation.Members.Add(codeProperty);
        }

        private static void GeneratePropertyGetterCode(PropertyInfo property, CodeMemberProperty codeProperty)
        {
            codeProperty.GetStatements.Add(
                new CodeMethodReturnStatement(
                    new CodeMethodInvokeExpression(
                        new CodeMethodReferenceExpression(
                            new CodeBaseReferenceExpression(),
                            "GetPrimitiveValue",
                            new CodeTypeReference(property.PropertyType)),
                        new CodePrimitiveExpression(property.Name))));
        }

        private static void GeneratePropertySetterCode(PropertyInfo property, CodeMemberProperty codeProperty)
        {
            codeProperty.SetStatements.Add(
                new CodeMethodInvokeExpression(
                        new CodeMethodReferenceExpression(
                            new CodeBaseReferenceExpression(),
                            "SetValue",
                            new CodeTypeReference(property.PropertyType)),
                        new CodePrimitiveExpression(property.Name),
                        new CodeVariableReferenceExpression("value")));
        }

        private static void GenerateEntityPropertyGetterCode(PropertyInfo property, CodeMemberProperty codeProperty)
        {
            codeProperty.GetStatements.Add(
               new CodeMethodReturnStatement(
                   new CodeMethodInvokeExpression(
                       new CodeMethodReferenceExpression(
                           new CodeBaseReferenceExpression(),
                           "GetEntityValue",
                           new CodeTypeReference(property.PropertyType)),
                       new CodePrimitiveExpression(property.Name))));
        }

        private static void GenerateCollectionPropertyCode(PropertyInfo property, CodeMemberProperty codeProperty)
        {
            Type propertyType = property.PropertyType.GetGenericArguments()[0];


            codeProperty.GetStatements.Add(
               new CodeMethodReturnStatement(
                   new CodeMethodInvokeExpression(
                       new CodeMethodReferenceExpression(
                           new CodeBaseReferenceExpression(),
                           "GetCollectionValue",
                           new CodeTypeReference(propertyType),
                           new CodeTypeReference(property.DeclaringType)),
                       new CodePrimitiveExpression(property.Name))));
        }
    }
}
