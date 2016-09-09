using System.Collections.Generic;
using System.Windows.Input;
using WpfSkeleton.ViewModels;

namespace WpfSkeleton.DesignTimeData.ViewModels
{
    public class MainViewModel : IMainViewModel
    {
        public IEnumerable<string> Data => new [] { "design", "time", "data"};
        public ICommand ClickMe => null;
    }
}