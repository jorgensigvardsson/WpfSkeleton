using System.Windows;
using Autofac;
using WpfSkeleton.Services.UI;
using WpfSkeleton.ViewModels;

namespace WpfSkeleton
{
    public partial class App
    {
        private readonly DependencyInjectionSetup m_setup = new DependencyInjectionSetup();

        protected override void OnStartup(StartupEventArgs e)
        {
            m_setup.Container.Resolve<IWindowService>().Show(m_setup.Container.Resolve<MainViewModel>());
        }
    }
}
