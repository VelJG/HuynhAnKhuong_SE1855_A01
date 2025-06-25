using HuynhAnKhuongWPF.ViewModels;
using Services.IServices;
using System;
using System.Windows.Controls;

namespace HuynhAnKhuongWPF.Views
{
    public partial class CustomerManagementView : UserControl
    {
        public CustomerManagementView(ICustomerService customerService)
        {
            InitializeComponent();
            DataContext = new CustomerManagementViewModel(customerService);
        }
    }
}