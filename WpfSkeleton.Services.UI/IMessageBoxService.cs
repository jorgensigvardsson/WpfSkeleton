using System.Windows;

namespace WpfSkeleton.Services.UI
{
    public interface IMessageBoxService
    {
        MessageBoxResult Show(string messageBoxText);

        MessageBoxResult Show(string messageBoxText,
                              string caption);

        MessageBoxResult Show(string messageBoxText,
                              string caption,
                              MessageBoxButton button);

        MessageBoxResult Show(string messageBoxText,
                              string caption,
                              MessageBoxButton button,
                              MessageBoxImage icon);

        MessageBoxResult Show(string messageBoxText,
                              string caption,
                              MessageBoxButton button,
                              MessageBoxImage icon,
                              MessageBoxResult defaultResult);

    }
}
