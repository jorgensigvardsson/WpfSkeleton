using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using AutoConstruct;
using Moq;
using NUnit.Framework;

namespace WpfSkeleton.ViewModels.Tests
{
    [TestFixture]
    public class MainViewModelTests
    {
        [Test]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public void ConstructorThrowsOnNullArguments()
        {
            var service = new Mock<Services.IService>();
            var messageBoxService = new Mock<Services.UI.IMessageBoxService>();

            Assert.Throws<ArgumentNullException>(() => new MainViewModel(null, messageBoxService.Object));
            Assert.Throws<ArgumentNullException>(() => new MainViewModel(service.Object, null));
        }

        [Test]
        public void DataIsFetchedFromService()
        {
            // Arrange
            var context = new ConstructorContext<MainViewModel>();
            var service = context.Inject(new Mock<Services.IService>());

            service.Setup(s => s.GetData()).Returns(new[] { "a", "b", "c" });

            var viewModel = context.New();

            // Act
            var result = viewModel.Data;

            // Assert
            CollectionAssert.AreEqual(new[] { "a", "b", "c" }, result);
        }

        [Test]
        public void ClickMeRequestsMessageBox()
        {
            // Arrange
            var context = new ConstructorContext<MainViewModel>();
            var messageBoxService = context.Inject(new Mock<Services.UI.IMessageBoxService>());

            messageBoxService.Setup(s => s.Show("Hello!", "Test"))
                             .Returns(MessageBoxResult.OK);

            var viewModel = context.New();

            // Act
            viewModel.ClickMe.Execute(null);
        }
    }
}
