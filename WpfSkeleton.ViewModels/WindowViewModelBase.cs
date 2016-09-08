using GalaSoft.MvvmLight;
using WpfSkeleton.Services.UI;

namespace WpfSkeleton.ViewModels
{
    public abstract class WindowViewModelBase : ViewModelBase, IWindowViewModel
    {
        public virtual void OnViewIsLoaded()
        {
            // Override in subclasses when needed
        }

        public virtual void OnViewIsClosed()
        {
            // Override in subclasses when needed
        }
    }
}
