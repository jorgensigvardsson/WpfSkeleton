using System;
using System.Collections.Generic;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using WpfSkeleton.Services;
using WpfSkeleton.Services.UI;

namespace WpfSkeleton.ViewModels
{
    public class MainViewModel : WindowViewModelBase, IMainViewModel
    {
        private readonly IService m_service;
        private readonly IMessageBoxService m_messageBoxService;

        public MainViewModel(IService service, IMessageBoxService messageBoxService)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));

            m_service = service;
            m_messageBoxService = messageBoxService;
        }

        public IEnumerable<string> Data => m_service.GetData();

        public ICommand ClickMe => new RelayCommand(() => m_messageBoxService.Show("Hello!", "Test"));
    }
}