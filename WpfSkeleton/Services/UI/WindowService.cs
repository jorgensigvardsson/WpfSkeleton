using System;
using System.Collections.Generic;
using System.Windows;

namespace WpfSkeleton.Services.UI
{
    public class WindowService : IWindowService
    {
        private readonly IDictionary<Type, Type> m_viewMapping = new Dictionary<Type, Type>();

        public void Register<TViewModel, TView>() where TViewModel : IWindowViewModel where TView : Window, new()
        {
            m_viewMapping[typeof (TViewModel)] = typeof (TView);
        }

        public void ShowModal(IWindowViewModel windowViewModel)
        {
            if (windowViewModel == null)
                throw new ArgumentNullException(nameof(windowViewModel));

            CreateWindow(windowViewModel).ShowDialog();
        }

        public void Show(IWindowViewModel windowViewModel)
        {
            if (windowViewModel == null)
                throw new ArgumentNullException(nameof(windowViewModel));

            CreateWindow(windowViewModel).Show();
        }

        private Window CreateWindow(IWindowViewModel windowViewModel)
        {
            var window = (Window)Activator.CreateInstance(m_viewMapping[windowViewModel.GetType()]);

            window.DataContext = windowViewModel;
            window.Loaded += (sender, args) => windowViewModel.OnViewIsLoaded();
            window.Closed += (sender, args) => windowViewModel.OnViewIsClosed();

            return window;
        }
    }
}