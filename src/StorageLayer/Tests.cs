using NUnit.Framework;

using StorageLayer.Core;

namespace StorageLayer.Tests {
    #region example code
    public interface IExampleImplementation : IStorageLayer { }
    public class ExampleImplementation : IExampleImplementation {
        public ExampleImplementation() {

        }

        public void Store(object o) {

        }
    }
    public interface IOtherExampleImplementation : IStorageLayer { } 
    #endregion

    [TestFixture]
    public class StorageLayerTest {
        [SetUp]
        public void Setup() {
            RegisterEndPoints(StorageEndPoints.EndPoints);
        }

        [TearDown]
        public void Teardown() {
            StorageEndPoints.EndPoints.Clear();
        }

        [Test]
        public void add_single_endpoints_should_do_return_count_1() {
            Assert.AreEqual(StorageEndPoints.EndPoints.Count, 1);
        }
        
        [Test]
        public void add_2_endpoints_should_do_return_count_2() {
            StorageEndPoints.EndPoints.Clear();
            RegisterMultipleEndpoints(StorageEndPoints.EndPoints,2);
            Assert.AreEqual(StorageEndPoints.EndPoints.Count, 2);
        }

        [Test]
        public void clearing_storageendpoints_should_return_count_0() {
            StorageEndPoints.EndPoints.Clear();
            Assert.AreEqual(0, StorageEndPoints.EndPoints.Count);
        }



        [Test]
        public void retreived_endpoint_should_not_be_null() {
            IExampleImplementation example = Storage.Instance.GetEndPoint<IExampleImplementation>();

            Assert.NotNull(example);
        }

        [Test]
        public void retreived_endpoint_should_match_added_endpoint() {
            IExampleImplementation example = Storage.Instance.GetEndPoint<IExampleImplementation>();

            Assert.IsInstanceOf<IExampleImplementation>(example);
        }

        [Test]
        public void retreived_endpoint_should_be_null_if_not_found() {
            IOtherExampleImplementation example = Storage.Instance.GetEndPoint<IOtherExampleImplementation>();

            Assert.Null(example);
        }

        void RegisterEndPoints(StorageEndPointsCollecton endPoints) {
            endPoints.MapStorageEndPoint(typeof(IExampleImplementation), new ExampleImplementation().GetType());
        }

        void RegisterMultipleEndpoints(StorageEndPointsCollecton endPoints, int count) {
            endPoints.MapStorageEndPoint(typeof(IExampleImplementation), new ExampleImplementation().GetType());
            endPoints.MapStorageEndPoint(typeof(IOtherExampleImplementation), new ExampleImplementation().GetType());
        }

    }
}
