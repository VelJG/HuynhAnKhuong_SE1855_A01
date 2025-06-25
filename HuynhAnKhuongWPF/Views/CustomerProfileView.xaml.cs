using BusinessObjects;
using HuynhAnKhuongWPF.ViewModels;
using Services.IServices;
using System.Windows.Controls;

namespace HuynhAnKhuongWPF.Views
{
    public partial class CustomerProfileView : UserControl
    {
        public CustomerProfileView(ICustomerService customerService, Customer customer)
        {
            InitializeComponent();
            DataContext = new CustomerProfileViewModel(customerService, customer);
        }
    }
}