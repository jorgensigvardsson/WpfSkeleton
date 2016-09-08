# WpfSkeleton
Generic skeleton solution for WPF applications.

WpfSkeleton project is where the application is at. It bootstraps the application by setting up dependency injections (see class DependencyInjectionSetup). MainViewModel is the main view model, and it is mapped against the MainWindow view class (see DependencyInjectionSetup.SetupWindows()).

# WpfSkeleton.ViewModels
WpfSkeleton.ViewModels is where the view models reside. All view models should implement a view model interface. The view model interface may then be implemented by a mirror design time class in WpfSkeleton.DesignTimeData. This way, the design time types will always reflect the interface of the real view model.

# WpfSkeleton.Services.UI
WpfSkeleton.Services.UI contains interfaces for basic UI services: IMessageBoxService (used to show message boxes), IWindowService (used to show windows and dialogs), and IWindowViewModel which is used by view models for top level windows/dialogs. The message box service and window service is implemented in the WpfSkeleton project.

# WpfSkeleton.Services
WpfSkeleton.Services is where services, and corresponding DTOs should reside.

# WpfSkeleton.Views
WpfSkeleton.Views is where the XAML views reside.
