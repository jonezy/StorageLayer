using System;
using System.Collections.Generic;
using System.Reflection;

namespace ServiceLocator.Core {
    public class Locator {
        private Dictionary<Type, Type> _endPoints;
        private Dictionary<Type, object> _endPointsCache;

        private static readonly object l = new Object();
        private static Locator instance;

        public static Locator Instance {
            get {
                lock (l) {
                    if (instance == null) {
                        instance = new Locator();
                    }
                }

                return instance;
            }
        }

        internal Locator() {
            _endPoints = ServiceTable.Services;
            _endPointsCache = new Dictionary<Type, object>();
        }
        
        public T GetEndPoint<T>() {
            if(_endPointsCache.ContainsKey(typeof(T))) {
                return (T)_endPointsCache[typeof(T)];
            }

            try {
                ConstructorInfo constructor = null;
                foreach (KeyValuePair<Type, Type> item in _endPoints) {
                    if(item.Key == typeof(T)) {
                        constructor = item.Value.GetConstructor(new Type[0]);
                        break;
                    }
                }

                if (constructor == null)
                    return default(T);

                T service = (T)constructor.Invoke(null);

                _endPointsCache.Add(typeof(T), service);

                return service;
            } catch (KeyNotFoundException) {
                throw new ApplicationException("The requested service is not registered");
            }
        }
    }




}