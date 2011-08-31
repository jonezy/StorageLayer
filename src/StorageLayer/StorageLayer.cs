using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using NUnit.Framework;

namespace StorageLayer.Core {
    public interface IStorageLayer {
        void Store(object o);
    }

    public class StorageLayer {
        static StorageEndPointsCollection _endPoints = StorageEndPoints.EndPoints;
        private static readonly object l = new Object();
        private static StorageLayer instance;

        public static StorageLayer Instance {
            get {
                lock (l) {
                    if (instance == null) {
                        instance = new StorageLayer();
                    }
                }

                return instance;
            }
        }

        internal StorageLayer() {
            _endPoints = new StorageEndPointsCollection();
        }
        
        public T GetService<T>() {

            // LAZY INITIALIZATION
            ConstructorInfo constructor = null;
            try {
                if (_endPoints.ContainsKey(T)) {

                }
                 foreach (var item in StorageEndPoints.EndPoints) {
                    if(item.GetType() == typeof(T)) {
                        constructor = item[typeof(T)].GetConstructor(new Type[0]);
                    }

                }

                T service = (T)constructor.Invoke(null);
                // USE REFLECTION TO INVOKE THE SERVICE
                //ConstructorInfo constructor = _endPoints[typeof(T)].GetConstructor(new Type[0]);
                //Debug.Assert(constructor != null, "Cannot find a suitable constructor for " + typeof(T));

                //T service = (T)constructor.Invoke(null);

                //// ADD THE SERVICE TO THE ONES THAT WE HAVE ALREADY INSTANTIATED
                //instantiatedServices.Add(typeof(T), service);

                return service;
            } catch (KeyNotFoundException) {
                throw new ApplicationException("The requested service is not registered");
            }
        }
    }

    public class Enumerator : IEnumerator {
        private int[] intArr;
        private int Cursor;

        public Enumerator(int[] intarr) {
            this.intArr = intarr;
            Cursor = -1;

        }

        void IEnumerator.Reset() {
            Cursor = -1;
        }
        bool IEnumerator.MoveNext() {
            if (Cursor < intArr.Length)
                Cursor++;

            return (!(Cursor == intArr.Length));
        }

        object IEnumerator.Current {
            get {
                if ((Cursor < 0) || (Cursor == intArr.Length))
                    throw new InvalidOperationException();
                return intArr[Cursor];
            }
        }
    }

    public class StorageEndPoint : Dictionary<Type, Type> {

    }

    public class StorageEndPointsCollection : ICollection<StorageEndPoint> {
        private int[] intArr = { 1, 5, 9 };
        private int Ct;

        ICollection<StorageEndPoint> _endPoints;
        private static readonly object l = new Object();
        private static StorageEndPointsCollection instance;
        List<StorageEndPoint> _List;

        public static StorageEndPointsCollection Instance {
            get {
                lock (l) // THREAD SAFETY
                {
                    if (instance == null) {
                        instance = new StorageEndPointsCollection();
                    }
                }

                return instance;
            }
        }

        internal StorageEndPointsCollection() {
            this._endPoints = new Collection<StorageEndPoint>();
            this._List = new List<StorageEndPoint>();
        }

        public void Add(StorageEndPoint item) {
            _endPoints.Add(item);
            _List.Add(item);
        }

        public void Clear() {
            _endPoints.Clear();
            _List.Clear();
        }

        public bool Contains(StorageEndPoint item) {
            return _endPoints.Contains(item);
        }

        public bool ContainsKey(StorageEndPoint type) {
            foreach (var item in _endPoints) {
                if(item.ContainsKey(type.GetType()))
                    return true;
            }

            return false;
        }

        public void CopyTo(StorageEndPoint[] array, int arrayIndex) {
            foreach (int i in intArr) {
                array.SetValue(i, arrayIndex);
                arrayIndex = arrayIndex + 1;
            }
        }

        public int Count {
            get { return _endPoints.Count; }
        }

        public bool IsReadOnly {
            get { return false; }
        }

        public bool Remove(StorageEndPoint item) {
            return _endPoints.Remove(item);
        }

        
        public IEnumerator<StorageEndPoint> GetEnumerator() {
            return _List.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return new Enumerator(intArr);
        }
    }

    public class StorageEndPoints {
        private static readonly object l = new Object();
        private static StorageEndPointsCollection instance;

        public static StorageEndPointsCollection EndPoints {
            get {
                lock (l) {
                    if (instance == null) {
                        instance = new StorageEndPointsCollection();
                    }
                }

                return instance;
            }
        }
    }

    static class Extensions {
        public static void MapStorageEndPoint(this StorageEndPointsCollection endPoints, Type endPointType, Type endPointImplementation) {
            StorageEndPoint endPoint = new StorageEndPoint();
            endPoint.Add(endPointType, endPointImplementation);

            endPoints.Add(endPoint);
        }
    }

    /// <summary>
    /// Example implementations
    /// </summary>
    public interface IExampleImplementation : IStorageLayer { }
    public class ExampleImplementation : IExampleImplementation {
        public ExampleImplementation() {

        }
        
        public void Store(object o) {
            
        }
    }

    [TestFixture]
    class StorageLayerTest {
        [SetUp]
        public void Setup() {
            RegisterEndPoints(StorageEndPoints.EndPoints);
        }

        [TearDown]
        public void Teardown() {
            StorageEndPoints.EndPoints.Clear();
        }

        [Test]
        public void add_endpoint_should_do_something() {
            Assert.AreEqual(StorageEndPoints.EndPoints.Count, 1);
        }

        [Test]
        public void retreived_endpoint_should_match_added_endpoint() {
            IExampleImplementation example = StorageLayer.Instance.GetService<IExampleImplementation>();

            Assert.IsNotNull(example);
            
        }

        void RegisterEndPoints(StorageEndPointsCollection endPoints) {
            endPoints.MapStorageEndPoint(typeof(IExampleImplementation), new ExampleImplementation().GetType());
        }
    }
}
