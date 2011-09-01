using System;
using System.Collections.Generic;
using System.Reflection;

namespace StorageLayer.Core {
    public interface IStorageLayer {
        void Store(object o);
    }

    public class Storage {
        Dictionary<Type, Type> _endPoints = StorageEndPoints.EndPoints;
        private static readonly object l = new Object();
        private static Storage instance;

        public static Storage Instance {
            get {
                lock (l) {
                    if (instance == null) {
                        instance = new Storage();
                    }
                }

                return instance;
            }
        }

        internal Storage() {    }
        
        public T GetEndPoint<T>() {
            try {
                ConstructorInfo constructor = null;
                foreach (KeyValuePair<Type, Type> item in _endPoints) {
                    if(item.Key == typeof(T)) {
                        constructor = item.Value.GetConstructor(new Type[0]);
                    }
                }

                if (constructor == null)
                    return default(T);

                T service = (T)constructor.Invoke(null);

                //// ADD THE SERVICE TO THE ONES THAT WE HAVE ALREADY INSTANTIATED
                //instantiatedServices.Add(typeof(T), service);

                return service;
            } catch (KeyNotFoundException) {
                throw new ApplicationException("The requested service is not registered");
            }
        }
    }

    public class StorageEndPointsCollecton : Dictionary<Type, Type> {

    }

    public class StorageEndPoints {
        private static readonly object l = new Object();
        private static StorageEndPointsCollecton instance;

        public static StorageEndPointsCollecton EndPoints {
            get {
                lock (l) {
                    if (instance == null) {
                        instance = new StorageEndPointsCollecton();
                    }
                }

                return instance;
            }
        }
    }

    static class Extensions {

        public static void MapStorageEndPoint(this StorageEndPointsCollecton endPoints, Type endPointType, Type endPointImplementation) {
            endPoints.Add(endPointType, endPointImplementation);
        }
    }
}