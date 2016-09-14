# Project structure
## WpfSkeleton
Generic skeleton solution for WPF applications.

WpfSkeleton project is where the application is at. It bootstraps the application by setting up dependency injections (see class DependencyInjectionSetup). MainViewModel is the main view model, and it is mapped against the MainWindow view class (see DependencyInjectionSetup.SetupWindows()).

## WpfSkeleton.ViewModels
WpfSkeleton.ViewModels is where the view models reside. All view models should implement a view model interface. The view model interface may then be implemented by a mirror design time class in WpfSkeleton.DesignTimeData. This way, the design time types will always reflect the interface of the real view model.

## WpfSkeleton.Services.UI
WpfSkeleton.Services.UI contains interfaces for basic UI services: IMessageBoxService (used to show message boxes), IWindowService (used to show windows and dialogs), and IWindowViewModel which is used by view models for top level windows/dialogs. The message box service and window service is implemented in the WpfSkeleton project.

## WpfSkeleton.Services
WpfSkeleton.Services is where services, and corresponding DTOs should reside.

## WpfSkeleton.Views
WpfSkeleton.Views is where the XAML views reside.

# Template Wizard
In wizard, there is a file called `WpfSkeleton.zip`. Simply drop it into your project templates folder (`%USERPROFILE%\Documents\Visual Studio 2015\Templates\ProjectTemplates`). Restart Visual Studio, and there should be a new project type called *WPF Application Solution Template*.

The structure of the source tree does not match this repository's structure. If you choose the name *MyProject*, the directory
structure will look like this: 

    %SOLUTIONROOT%
        \MyProject.sln
        \MyProject
            \MyProject
            \MyProject.DesignTimeData
            \MyProject.Services
            \MyProject.Services.Tests
            \MyProject.Services.UI            
            \MyProject.ViewModels
            \MyProject.ViewModels.Tests
            \MyProject.Views


This is very different from:

    %SOLUTIONROOT%
        \MyProject.sln
            \design
                \MyProject.DesignTimeData
            \src
                \MyProject                    
                \MyProject.Services
                \MyProject.Services.UI            
                \MyProject.ViewModels
                \MyProject.Views            
            \tests
                \MyProject.Services.Tests
                \MyProject.ViewModels.Tests


In order to have that structure, I'd have to write a VSIX. Some day. Maybe.
