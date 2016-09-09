using System.Windows;

namespace WpfSkeleton.Services.UI
{
    public interface IWindowService
    {
        void Register<TViewModel, TWindow>() where TViewModel : IWindowViewModel
            where TWindow : Window, new();

        void ShowModal(IWindowViewModel windowViewModel);
        void Show(IWindowViewModel windowViewModel);
    }
}
