using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sage.SData.Client.Extensions;
using Sage.SData.Client.Core;
using Sage.SData.Client.Atom;
using System.Globalization;
using System.Diagnostics;
using Sage.Platform.ComponentModel;

namespace SDataEntityObjects.SData
{

    /// <summary>
    /// Base class for all dynamically generated sdata-entity-wrappers
    /// </summary>
    public abstract class SDataClientEntityBase
    {


        /// <summary>
        /// The SDataPayload
        /// </summary>
        internal protected SDataPayload _Payload;

        /// <summary>
        /// The AtomEntry
        /// </summary>
        internal protected AtomEntry _AtomEntry;

        /// <summary>
        /// The Context
        /// </summary>
        protected SDataClientContext _Context;

        /// <summary>
        /// Contains a list of all values that have changed. Only these values will be submitted on Save
        /// </summary>
        private List<string> _DirtyValues;


        /// <summary>
        /// Contains a list of all properties that have been loaded.
        /// </summary>
        private List<string> _LoadedProperties;

        public SDataClientEntityBase(SDataPayload payload, SDataClientContext context)
        {
            this._Payload = payload;
            this._Context = context;
            this._DirtyValues = new List<string>();
            this._LoadedProperties = new List<string>();
        }

        #region IPersistentEntity Members

        /// <summary>
        /// Method is called in the generated properties to return the value of a primitive typed property (string, int, etc...)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        protected T GetPrimitiveValue<T>(string key)
        {
            if (!_Payload.Values.ContainsKey(key))
                return default(T);

            object value = _Payload.Values[key];

            Type targetType = typeof(T);

            //Different types used, require casts
            if (targetType == typeof(String))
                return (T)value;

            if (targetType == typeof(bool) || targetType == typeof(Nullable<bool>))
                return (T)((object)Convert.ToBoolean(value, CultureInfo.InvariantCulture));

            if (targetType == typeof(DateTime) || targetType == typeof(Nullable<DateTime>))
                return (T)((object)Convert.ToDateTime(value, CultureInfo.InvariantCulture));

            if (targetType == typeof(Int16) || targetType == typeof(Nullable<Int16>))
                return (T)((object)Convert.ToInt16(value, CultureInfo.InvariantCulture));

            if (targetType == typeof(Int32) || targetType == typeof(Nullable<Int32>))
                return (T)((object)Convert.ToInt32(value, CultureInfo.InvariantCulture));

            if (targetType == typeof(Int64) || targetType == typeof(Nullable<Int64>))
                return (T)((object)Convert.ToInt64(value, CultureInfo.InvariantCulture));

            if (targetType == typeof(double) || targetType == typeof(Nullable<double>))
                return (T)((object)Convert.ToDouble(value, CultureInfo.InvariantCulture));

            if (targetType == typeof(decimal) || targetType == typeof(Nullable<decimal>))
                return (T)((object)Convert.ToDecimal(value, CultureInfo.InvariantCulture));

            return (T)((object)Convert.ChangeType(value, targetType, CultureInfo.InvariantCulture));

        }

        /// <summary>
        /// Method is called in the generated properties to return an interface entity property (ISL_Customer, IContact).
        /// Lazy loading is performed if the value is not present
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        protected T GetEntityValue<T>(string key)
        {
            if ((_Payload.Values.ContainsKey(key) == false))
            {
                return default(T);
            }

            object typedEntity = _Payload.Values[key] as SDataClientEntityBase;

            if (typedEntity == null)
            {
                SDataPayload payload = (SDataPayload)_Payload.Values[key];

                // If we've got nothing then we've got nothing
                if (payload == null)
                    return default(T);

                // If values are already present, the Include Statement has been used -> Convert to typed entity
                if (payload.Values.Count > 0)
                    typedEntity = _Context.GetProxyClient<T>(payload);
                else
                {
                    // If no key is present, a new Item should be created
                    if (payload.Key == null)
                    {
                        //Console.WriteLine("Creating {0} for {1}", key, typeof(T).Name);

                        typedEntity = _Context.CreateNew<T>();
                    }
                    else
                    {
                        //Console.WriteLine("Lazy loading {0} for {1}('{2}')", key, typeof(T).Name, Id);
                        // MW - Need to change this to leverage payload URI if provided (2 levels)

                        string SDataResourceName = payload.Lookup.Split('/').Last();

                        if (SDataResourceName != "")
                        {
                            typedEntity = _Context.GetByIdAndResourceName<T>(payload.Key, SDataResourceName);
                        }
                        else
                        {
                            // This can fail if we guess the resource string wrong
                            typedEntity = _Context.GetById<T>(payload.Key);
                        }
                    }
                }

                _Payload.Values[key] = typedEntity;
            }

            return (T)typedEntity;
        }

        /// <summary>
        /// Method is called in the generated properties to return a collection of entites (ICollection<IContact>)
        /// Lazy loading is performed if the collection is not present
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="P"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        protected ICollection<T> GetCollectionValue<T, P>(string key)
        {
            if ((_Payload.Values.ContainsKey(key) == false))
            {
                return default(System.Collections.Generic.ICollection<T>);
            }

            //Empty string indicates, that a reload is required
            List<T> collectionTyped = _Payload.Values[key] as List<T>;

            if (collectionTyped == null)
            {

                SDataPayloadCollection childItems = _Payload.Values[key] as SDataPayloadCollection;

                if (childItems == null || (childItems.Count == 0 && !_LoadedProperties.Contains(key)))
                {
                    _LoadedProperties.Add(key);

                    // Console.WriteLine("Lazy loading {0} for {1}('{2}')", key, typeof(P).Name, Id);

                    object typedEntity = _Context.GetById<P>(Id, key);

                    childItems = (SDataPayloadCollection)((SDataClientEntityBase)typedEntity)._Payload.Values[key];
                }

                collectionTyped = new List<T>();

                foreach (SDataPayload item in childItems)
                    collectionTyped.Add(_Context.GetProxyClient<T>(item));


                _Payload.Values[key] = collectionTyped;
            }

            return collectionTyped;
        }


        /// <summary>
        /// Method is used in the generated properties to set any type of value to the underlying payload
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        protected void SetValue<T>(string key, T value)
        {
            if (!_DirtyValues.Contains(key))
                _DirtyValues.Add(key);

            _Payload.Values[key] = ConvertToPayloadValue<T>(value);
        }

        /// <summary>
        /// Method is called in the generated code to invoke a business rule
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        protected object CallMethod(string methodName, Type returnType, List<KeyValuePair<string, object>> parameters)
        {

            object resultValue = _Context.CallEntityMethod(this, methodName, returnType, parameters);
            // Right... Now we have to figure out what we got back...

            if (returnType == typeof(void))
                return null;

            // Cast to collection of wrapper classes
            if (returnType.FullName.StartsWith("System.Collections.Generic.ICollection`1"))
            {                

                Type subType = returnType.GetGenericArguments()[0];

                SDataPayloadCollection items = resultValue as SDataPayloadCollection;

                if (items == null)
                    return null;



                System.Collections.IList collectionType = (System.Collections.IList)Activator.CreateInstance(Type.GetType("System.Collections.Generic.List`1").MakeGenericType(subType));

                foreach (SDataPayload item in items)
                    collectionType.Add(_Context.GetProxyClient(subType, item, null));


                return collectionType;
            }

            // Cast to wrapper class for complex types
            if (returnType.FullName.StartsWith("Sage.Entity.Interfaces."))
            {
                SDataPayload payload = resultValue as SDataPayload;

                // Returned value is intended to be null
                if (payload == null)
                    return null;

                return _Context.GetProxyClient(returnType, payload, null);
            }

            try
            {
                object resultValueTyped = Convert.ChangeType(resultValue, returnType);

                return resultValueTyped;
            }
            catch(InvalidCastException ex)
            {
                throw new InvalidCastException(String.Format("Could not properly convert result from Method {0} for Entity {1} ({2}) to expected type.", methodName, GetType().Name, Id), ex);                
            }
        }

        /// <summary>
        /// Internal function to convert a value to a value that can be stored in a payload.
        /// Certain culture issues are considered here.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        private object ConvertToPayloadValue<T>(T value)
        {
            Type targetType = typeof(T);

            if (value == null || targetType == typeof(String))
                return value;

            //There seems to be some issues with formats of values
            if (targetType.FullName.StartsWith("Sage.Entity.Interfaces."))
                return ((SDataClientEntityBase)((object)value))._Payload;

            if (targetType == typeof(bool) || targetType == typeof(Nullable<bool>))
                return ((bool)((object)value)).ToString(CultureInfo.InvariantCulture);

            if (targetType == typeof(DateTime) || targetType == typeof(Nullable<DateTime>))
                return ((DateTime)((object)value)).ToString(CultureInfo.InvariantCulture);

            if (targetType == typeof(Int16) || targetType == typeof(Nullable<Int16>))
                return ((Int16)((object)value)).ToString(CultureInfo.InvariantCulture);

            if (targetType == typeof(Int32) || targetType == typeof(Nullable<Int32>))
                return ((Int32)((object)value)).ToString(CultureInfo.InvariantCulture);

            if (targetType == typeof(Int64) || targetType == typeof(Nullable<Int64>))
                return ((Int64)((object)value)).ToString(CultureInfo.InvariantCulture);

            if (targetType == typeof(double) || targetType == typeof(Nullable<double>))
                return ((double)((object)value)).ToString(CultureInfo.InvariantCulture);

            if (targetType == typeof(decimal) || targetType == typeof(Nullable<decimal>))
                return ((decimal)((object)value)).ToString();

            return value;
        }

        /// <summary>
        /// Deletes the entity from the database
        /// </summary>
        public void Delete()
        {
            SDataSingleResourceRequest request = _Context.GetRequestForCRUD(this);

            if (request.Delete() == false)
                throw new InvalidOperationException(String.Format("Error deleting {0} {1}.", request.ResourceKind, request.ResourceSelector));

            OnPersisted(new EventArgs());
        }

        /// <summary>
        /// Event that gets fired when entity is updated or deleted
        /// </summary>
        public event EventHandler<EventArgs> Persisted;

        private void OnPersisted(EventArgs e)
        {
            if (Persisted != null)
                Persisted(this, e);
        }

        public Sage.Platform.Orm.Interfaces.PersistentState PersistentState
        {
            // Not supported, but that's no reason to throw an error
            get { return Sage.Platform.Orm.Interfaces.PersistentState.Unmodified; }
        }

        /// <summary>
        /// Saves any changes to the entity
        /// </summary>
        public void Save()
        {

            _Context.SaveEntity(this);

            OnPersisted(new EventArgs());
        }

        /// <summary>
        /// Removes all values from the Payload that haven't been changed
        /// </summary>
        internal void RemoveNonDirtyValues()
        {

            List<string> allProperties = new List<string>(_Payload.Values.Keys);

            foreach (string propertyName in allProperties)
                if (!_DirtyValues.Contains(propertyName))
                    _Payload.Values.Remove(propertyName);

            // Reset dirty values
            _DirtyValues.Clear();
        }

        #endregion

        #region IDynamicEntity Members

        public object this[string propertyName]
        {
            get
            {
                return _Payload.Values[propertyName];
            }
            set
            {
                _Payload.Values[propertyName] = value;
            }
        }

        #endregion

        #region IComponentReference Members

        public object Id
        {
            get { return _Payload.Key; }
        }
        #endregion

    }
}
