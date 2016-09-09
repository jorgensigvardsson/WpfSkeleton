using Autofac;
using Autofac.Extras.CommonServiceLocator;
using Microsoft.Practices.ServiceLocation;
using WpfSkeleton.Services.UI;
using WpfSkeleton.ViewModels;
using WpfSkeleton.Views.Windows;

namespace WpfSkeleton
{
    public class DependencyInjectionSetup
    {
        #region Don't modify
        public DependencyInjectionSetup()
        {
            var builder = new ContainerBuilder();
            SetupServices(builder);

            // UI Services
            builder.RegisterType<WindowService>().As<IWindowService>();
            builder.RegisterType<MessageBoxService>().As<IMessageBoxService>();
            builder.RegisterType<MainViewModel>().As<MainViewModel>();

            // Dialogs
            var dialogService = new WindowService();
            builder.RegisterInstance(dialogService).As<IWindowService>();
            SetupWindows(dialogService);

            Container = builder.Build();
            ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(Container));
        }

        public IContainer Container { get; }
        #endregion

        // Setup any services and view models
        private void SetupServices(ContainerBuilder builder)
        {
            // Services
            builder.RegisterType<Services.Service>().As<Services.IService>();
        }

        // Set up any dialog view models and views
        private void SetupWindows(IWindowService windowService)
        {
            // TODO: Implement window view model -> view mappings here
            windowService.Register<MainViewModel, MainWindow>();
        }
    }
}