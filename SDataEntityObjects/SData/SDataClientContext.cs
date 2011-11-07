using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sage.SData.Client.Extensions;
using Sage.SData.Client.Atom;
using Sage.SData.Client.Core;
using Sage.Platform.Orm.Attributes;
using System.Collections;
using SDataEntityObjects.SData.Linq;
using System.Reflection;
using System.Diagnostics;

namespace SDataEntityObjects.SData
{
    /// <summary>
    /// Public context to access sdata in a strongly typed manner. The context offers both, linq functionality and retrieving items using direct methods.
    /// </summary>
    public class SDataClientContext : IClientContext
    {
        private SDataContextConfiguration _configuration;        
        public SDataClientContext(SDataContextConfiguration configuration)
        {
            this._configuration = configuration;
        }

        private SDataService _Service;
        internal SDataService Service
        {
            get
            {
                if (_Service == null)
                {
                    _Service = new SDataService(String.Format("http://{0}:{1}/ExtSData/sage1000/dynamic/demo", _configuration.Servername, _configuration.Port), _configuration.Username, _configuration.Password);
                    // Obsolete
                    //_Service.Initialize();
                }

                return _Service;
            }
        }

        private static Dictionary<Type, Type> _ClientProxyClasses;

        private static Dictionary<Type, Type> ClientProxyClasses
        {
            get
            {
                if (_ClientProxyClasses == null)
                    _ClientProxyClasses = new Dictionary<Type, Type>();

                return _ClientProxyClasses;
            }
        }



        public ICollection<T> GetProxyClients<T>(SDataPayloadCollection items)
        {
            List<T> result = new List<T>();

            foreach (SDataPayload payload in items)
                result.Add(GetProxyClient<T>(payload));

            return result;
        }

        public T GetProxyClients<T>(AtomEntry entry) 
        {
            return GetProxyClient<T>(entry.GetSDataPayload());
        }

        public ICollection<T> GetProxyClients<T>(AtomFeed feed)
        {
            List<T> result = new List<T>();

            foreach (AtomEntry entry in feed.Entries)
                result.Add(GetProxyClient<T>(entry.GetSDataPayload()));

            return result;
        }

        public T GetProxyClient<T>(AtomEntry entry)
        {
            return GetProxyClient<T>(entry.GetSDataPayload(), entry);
        }

        public T GetProxyClient<T>(SDataPayload entry, AtomEntry atomEntry)
        {
            return (T)GetProxyClient(typeof(T), entry, atomEntry);
        }

        public T GetProxyClient<T>(SDataPayload entry)
        {
            return GetProxyClient<T>(entry, null);
        }

        internal object GetProxyClient(Type entityType, SDataPayload entry, AtomEntry atomEntry)
        {
            Type proxyClass = null;

            if (!ClientProxyClasses.ContainsKey(entityType))
            {
                proxyClass = WrapperFactory.GenerateProxyClass(entityType);
                ClientProxyClasses.Add(entityType, proxyClass);
            }
            else
                proxyClass = ClientProxyClasses[entityType];

            object result = Activator.CreateInstance(proxyClass, entry, this);

            ((SDataClientEntityBase)result)._AtomEntry = atomEntry;

            return result;
        }

        #region IDisposable Members

        public void Dispose()
        {
            //Noting to do yet
        }

        #endregion

        internal SDataSingleResourceRequest GetRequestForCRUD(SDataClientEntityBase entity)
        {

            entity.RemoveNonDirtyValues();
            Type interfaceClass = GetEntityInterface(entity.GetType());

            SDataSingleResourceRequest request = new SDataSingleResourceRequest(Service);

            request.ResourceKind = GetResourceKind(interfaceClass);

            request.Entry = LoadByIdInternal(interfaceClass, entity.Id.ToString(), null);

            request.Entry.SetSDataPayload(entity._Payload);
            request.ResourceSelector = String.Format("('{0}')", entity.Id);

            return request;

        }

        internal SDataSingleResourceRequest GetRequestForCreate(SDataClientEntityBase entity)
        {
            Type interfaceClass = GetEntityInterface(entity.GetType());

            SDataSingleResourceRequest request = new SDataSingleResourceRequest(Service);

            request.ResourceKind = GetResourceKind(interfaceClass);

            // If the entity has been loaded using the include statement, a new Atom-Entry has to be loaded
            if (entity._AtomEntry == null)
            {
                SDataTemplateResourceRequest requestTemplate = new SDataTemplateResourceRequest(Service);

                requestTemplate.ResourceKind = request.ResourceKind;

                entity._AtomEntry = requestTemplate.Read();
                entity._AtomEntry.SetSDataPayload(entity._Payload);
            }

            request.Entry = entity._AtomEntry;

            return request;
        }

        /// <summary>
        /// Calls a business rule method on the specified entity
        /// </summary>
        /// <param name="entity">The entity, that the business rule is called for</param>
        /// <param name="returnType">The expected return type of the call</param>
        /// <param name="methodName">The name of the method</param>
        /// <param name="parameters">The parameters in for the method in order</param>
        /// <returns>The result of the method call</returns>
        internal object CallEntityMethod(SDataClientEntityBase entity, string methodName, Type returnType, List<KeyValuePair<string, object>> parameters)
        {

            Type entityInterfaceClass = GetEntityInterface(entity.GetType());

            // Just feed in the entity id for context
            AtomEntry response = CallEntityMethodInternal(entityInterfaceClass, entity.Id.ToString(), methodName, parameters);

            SDataPayload fullPayloadOut = response.GetSDataPayload();

            if (returnType == typeof(void))
                return null;

            SDataPayload responsePayload = fullPayloadOut.Values["response"] as SDataPayload;

            if (responsePayload == null)
                throw new InvalidOperationException(String.Format("Method {0} did not return response", methodName));

            if (!responsePayload.Values.ContainsKey("Result"))
                throw new InvalidOperationException(String.Format("Method {0} did not return 'return' section in response", methodName));

            return responsePayload.Values["Result"];

        }

        private AtomEntry CallEntityMethodInternal(Type type, string id, string methodName, List<KeyValuePair<string, object>> parameters)
        {
            SDataServiceOperationRequest request = new SDataServiceOperationRequest(Service);
            string entityName = GetEntityName(type);

            request.ResourceKind = GetResourceKind(type);
            request.OperationName = methodName;

            AtomEntry requestEntry = new AtomEntry();

            SDataPayload requestPayloadIn = new SDataPayload
            {
                ResourceName = entityName + methodName,
                Namespace = "http://schemas.sage.com/dynamic/2007",
                Values = {
                    {"request", new SDataPayload 
                        {
                            ResourceName = "request",
                            Values = 
                            {
                                {entityName + "Id", id }                            
                            }
                        }
                    }
                }
            };

            // Include any other parameters
            if (parameters != null && parameters.Count > 0)
            {
                foreach (KeyValuePair<string, object> parameter in parameters)
                {
                    SDataClientEntityBase entityParameter = parameter.Value as SDataClientEntityBase;

                    if (entityParameter != null)
                    {
                        requestPayloadIn.Values.Add(parameter.Key, entityParameter._Payload);
                    }
                    else
                    {
                        ICollection collectionParameter = parameter.Value as ICollection;

                        if (collectionParameter != null)
                        {
                            SDataPayloadCollection payloadCollection = new SDataPayloadCollection();

                            foreach (SDataClientEntityBase subEntity in collectionParameter)
                                payloadCollection.Add(subEntity._Payload);

                            requestPayloadIn.Values.Add(parameter.Key, payloadCollection);
                        }
                        else
                            requestPayloadIn.Values.Add(parameter.Key, parameter.Value);
                    }
                }
            }

            requestEntry.SetSDataPayload(requestPayloadIn);

            AtomEntry response = Service.CreateEntry(request, requestEntry);
            return response;
        }

        /// <summary>
        /// Returns the entityname starting with a capital letter, follow by lowercase letters
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static string GetEntityName(Type type)
        {
            
            //object[] attributes = type.GetCustomAttributes(typeof(ActiveRecordAttribute), false);
            //if (attributes.Length == 0)
            //    throw new InvalidOperationException(String.Format("Type {0} does not contain a valid SalesLogix Table-Attribute", type.FullName));
            //string entityName = ((ActiveRecordAttribute)attributes[0]).Table;
            //entityName = entityName.Substring(0, 1).ToUpper() + entityName.Substring(1).ToLower();

            // Just use the name based in, just without the leading "I"
            string entityName = type.Name.Substring(1).Replace("_", "");


            return entityName;
        }

        private Type GetEntityInterface(Type implementedType)
        {
            //Getting Interface
            foreach (var interfaceType in implementedType.GetInterfaces())
                if (interfaceType.FullName.StartsWith("Sage.Entity.Interfaces."))
                    return interfaceType;

            throw new InvalidOperationException(String.Format("Type '{0}' does not implement a SalesLogix interface", implementedType.FullName));
        }

        private string GetResourceKind(Type type)
        {
            return GetPlural(GetEntityName(type));


        }

        /// <summary>
        /// This is stupid! The plural from used for sdata should be in an attribute of the interface
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string GetPlural(string name)
        {

            return name;

            //// SLX Special Names (Do not obey naming convention)
            //if (name.ToUpper() == "USERINFO")
            //    return "UserInfo";
            //if (name.ToUpper() == "SECCODE")
            //    return "Owners";
            //if (name.ToUpper() == "USERSECURITY")
            //    return "Users";
            //if (name.ToUpper() == "HISTORY")
            //    return "History";
            //if (name.ToUpper() == "OWNERRIGHTS")
            //    return "OwnerRights";

            //// Standard Pluralization
            //name = name.ToLowerInvariant().Trim();
            //if (name.EndsWith("y"))
            //    return name.Substring(0, name.Length - 1) + "ies";
            //if (name.EndsWith("s"))
            //    return name + "es";
            //if (name.EndsWith("o"))
            //    return name + "es";
            //return name + "s";
        }

        #region Public methods to retrieve Entities

        private AtomEntry LoadByIdInternal(Type type, string id, string include)
        {
            SDataSingleResourceRequest request = new SDataSingleResourceRequest(Service);

            request.ResourceKind = GetResourceKind(type);
            request.ResourceSelector = String.Format("('{0}')", id);

            if (!String.IsNullOrEmpty(include))
                request.Include = include;

            return request.Read();
        }

        internal IEnumerable<T> Load<T>(SDataResourceCollectionRequest request)
        {
            int CurrentCount = 0;
            int ItemsPerPage;
            int TotalResults;
            long ResultLimit;
            AtomFeed feed;
            ICollection<T> resultList;

            // Set LINQ passed expression limit (if set)
            if (request.Count.HasValue == false)
                ResultLimit = long.MaxValue;
            else
                ResultLimit = request.Count.Value;

            // Use ContextContiguration ResultLimit if lower
            if (_configuration.MaxRequestSize < ResultLimit)
                ResultLimit = _configuration.MaxRequestSize;


            List<object> result = new List<object>();

            // Get the SData selector name (Still needs work)
            request.ResourceKind = GetResourceKind(typeof(T));

            // Make the first request and process the results
            request.StartIndex = 1;
            feed = request.Read();
            resultList = GetProxyClients<T>(feed);

            // Kick the iterator off
            foreach (var item in resultList)
            {
                CurrentCount++;
                yield return item;
                if (CurrentCount >= ResultLimit)
                    break;
            }

            // Get some counts
            ItemsPerPage = feed.GetOpenSearchItemsPerPage().Value;
            TotalResults = feed.GetOpenSearchTotalResults().Value;
            
            // Loop as needed
            while ((CurrentCount < TotalResults) && (CurrentCount < ResultLimit))
            {
                request.StartIndex = CurrentCount + 1;
                feed = request.Read();
                resultList = GetProxyClients<T>(feed);

                foreach (var item in resultList)
                {
                    CurrentCount++;
                    yield return item;
                    if (CurrentCount >= ResultLimit)
                        break;
                }                
            }            
        }

        public T CreateNew<T>()
        {
            SDataTemplateResourceRequest request = new SDataTemplateResourceRequest(Service);

            request.ResourceKind = GetResourceKind(typeof(T));

            object result = this.GetProxyClient<T>(request.Read());            

            ((SDataClientEntityBase)result)._Payload.Values.Clear();

            return (T)result;
        }

        public T CreateNew<T>(T item)
        {
            SDataSingleResourceRequest request = new SDataSingleResourceRequest(Service);
            request.ResourceKind = GetResourceKind(typeof(T));

            {
                SDataTemplateResourceRequest requestTemplate = new SDataTemplateResourceRequest(Service);

                requestTemplate.ResourceKind = GetResourceKind(typeof(T));

                request.Entry = requestTemplate.Read();
            }

            request.Entry.SetSDataPayload((((SDataClientEntityBase)((object)item)))._Payload);

            return GetProxyClient<T>(request.Create());
        }

        #endregion

        /// <summary>
        /// Creates a linq Query that can be used to consume SData information
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IQueryable<T> CreateQuery<T>()
        {
            return new SDataQuery<T>(this, new SDataQueryProvider(this, typeof(T)));
        }


        #region IClientContext Members

        public T GetById<T>(object id)
        {
            return GetById<T>(id, null);
        }

        public T GetById<T>(object id, string include)
        {
            if (id == null)
                throw new ArgumentNullException("id has to be specified");

            SDataSingleResourceRequest request = new SDataSingleResourceRequest(Service);

            request.ResourceKind = GetResourceKind(typeof(T));
            request.ResourceSelector = String.Format("('{0}')", id.ToString());
            if (!String.IsNullOrEmpty(include))
                request.Include = include;

            return GetProxyClients<T>(request.Read());
        }

        public T GetByIdAndResourceName<T>(object id, string resourceName)
        {
            if (id == null)
                throw new ArgumentNullException("id has to be specified");

            SDataSingleResourceRequest request = new SDataSingleResourceRequest(Service);

            request.ResourceKind = resourceName;
            request.ResourceSelector = String.Format("('{0}')", id.ToString());
            return GetProxyClients<T>(request.Read());
        }


        #endregion

        /// <summary>
        /// Performs the Save operation for an entity
        /// </summary>
        /// <param name="sDataClientEntityBase"></param>
        internal void SaveEntity(SDataClientEntityBase entity)
        {

            AtomEntry result = null;

            // New elements need to be treated diffrently from existing
            if (entity.Id == null)
            {
                SDataSingleResourceRequest request = GetRequestForCreate(entity);

                result = request.Create();

                if (result.GetSDataHttpStatus() != System.Net.HttpStatusCode.Created)
                    throw new InvalidOperationException(String.Format("Error inserting {0} {1}. Errorcode: {2}", request.ResourceKind, request.ResourceSelector, result.GetSDataHttpStatus()));

                entity._Payload = result.GetSDataPayload();
            }
            else
            {
                SDataSingleResourceRequest request = GetRequestForCRUD(entity);

                result = request.Update();

                if (result.GetSDataHttpStatus() != System.Net.HttpStatusCode.OK)
                    throw new InvalidOperationException(String.Format("Error updating {0} {1}. Errorcode: {2}", request.ResourceKind, request.ResourceSelector, result.GetSDataHttpStatus()));

                entity._Payload = result.GetSDataPayload();
            }
        }
    }
}
