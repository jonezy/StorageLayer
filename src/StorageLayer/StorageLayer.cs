using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using NUnit.Framework;

namespace StorageLayer.Core {
    public interface IStorageLayer {
        void Store(object o);
    }

    public class StorageLayer {
        internal static StorageEndPointsCollection _endPoints;

        public StorageLayer() {

        }

        public static void RegisterEndPoints(StorageEndPointsCollection endPoints) {
            _endPoints.Add(new StorageEndPoint());
        }
    }

    public class StorageEndPoint {
        public Type EndPointType { get; set; }
        public Type EndPointImplementation { get; set; }
    }

    public class StorageEndPointsCollection : ICollection<StorageEndPoint> {
        ICollection<StorageEndPoint> _endPoints;
        private static readonly object TheLock = new Object();
        private static StorageEndPointsCollection instance;

        public static StorageEndPointsCollection Instance {
            get {
                lock (TheLock) // THREAD SAFETY
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
        }

        public void Add(StorageEndPoint item) {
            _endPoints.Add(item);
        }

        public void Clear() {
            _endPoints.Clear();
        }

        public bool Contains(StorageEndPoint item) {
            return _endPoints.Contains(item);
        }

        public void CopyTo(StorageEndPoint[] array, int arrayIndex) {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            throw new NotImplementedException();
        }
    }

    public class StorageEndPoints {
        private static readonly object TheLock = new Object();
        private static StorageEndPointsCollection instance;
        
        public static StorageEndPointsCollection EndPoints {
            get {
                lock (TheLock) // THREAD SAFETY
                {
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
            StorageEndPoint endPoint = new StorageEndPoint() {
                EndPointType = endPointType,
                EndPointImplementation = endPointImplementation
            };

            endPoints.Add(endPoint);
        }
    }

    public class ExampleImplementation : IStorageLayer {
        public void Store(object o) {
            throw new NotImplementedException();
        }
    }

    [TestFixture]
    class StorageLayerTest {
        [Test]
        public void add_endpoint_should_do_something() {
            StorageEndPointsCollection endPoints = StorageEndPoints.EndPoints;
            endPoints.MapStorageEndPoint(typeof(IStorageLayer), new ExampleImplementation().GetType());

            Assert.AreEqual(endPoints.Count, 1);

        }
    }
}
