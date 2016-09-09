using NUnit.Framework;

namespace WpfSkeleton.Services.Tests
{
    [TestFixture]
    public class ServiceTests
    {
        [Test]
        public void DataReturnsCorrectSequenceOfData()
        {
            // Arrange
            var service = new Service();

            // Act
            var data = service.GetData();

            // Assert
            CollectionAssert.AreEqual(new[] { "A", "bunch", "of", "sample", "data" }, data);
        }
    }
}
