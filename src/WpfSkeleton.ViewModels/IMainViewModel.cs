using System.Collections.Generic;
using System.Windows.Input;

namespace WpfSkeleton.ViewModels
{
    public interface IMainViewModel
    {
        IEnumerable<string> Data { get; }
        ICommand ClickMe { get; }
    }
}