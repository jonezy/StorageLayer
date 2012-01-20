using NUnit.Framework;

using ServiceLocator;

namespace StorageLayer.Tests {
    #region example code
    public interface IStorageLayer {
        void Store(object o);
    }
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
            RegisterEndPoints(ServiceTable.Services);
        }

        [TearDown]
        public void Teardown() {
            ServiceTable.Services.Clear();
        }

        [Test]
        public void add_single_endpoints_should_do_return_count_1() {
            Assert.AreEqual(ServiceTable.Services.Count, 1);
        }
        
        [Test]
        public void add_2_endpoints_should_do_return_count_2() {
            ServiceTable.Services.Clear();
            RegisterMultipleEndpoints(ServiceTable.Services, 2);
            Assert.AreEqual(ServiceTable.Services.Count, 2);
        }

        [Test]
        public void clearing_storageendpoints_should_return_count_0() {
            ServiceTable.Services.Clear();
            Assert.AreEqual(0, ServiceTable.Services.Count);
        }

        [Test]
        public void retreived_endpoint_should_not_be_null() {
            IExampleImplementation example = Locator.Instance.GetEndPoint<IExampleImplementation>();

            Assert.NotNull(example);
        }

        [Test]
        public void retreived_endpoint_should_match_added_endpoint() {
            IExampleImplementation example = Locator.Instance.GetEndPoint<IExampleImplementation>();
            
            Assert.IsInstanceOf<IExampleImplementation>(example);
        }

        [Test]
        public void retreived_endpoint_should_be_null_if_not_found() {
            IOtherExampleImplementation example = Locator.Instance.GetEndPoint<IOtherExampleImplementation>();

            Assert.Null(example);
        }

        void RegisterEndPoints(ServiceCollection endPoints) {
            endPoints.MapStorageEndPoint(typeof(IExampleImplementation), typeof(ExampleImplementation));
        }

        void RegisterMultipleEndpoints(ServiceCollection endPoints, int count) {
            endPoints.MapStorageEndPoint(typeof(IExampleImplementation), typeof(ExampleImplementation));
            endPoints.MapStorageEndPoint(typeof(IOtherExampleImplementation), typeof(ExampleImplementation));
        }

    }
}
