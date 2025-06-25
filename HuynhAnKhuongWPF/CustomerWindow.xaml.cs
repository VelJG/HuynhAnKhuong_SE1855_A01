using BusinessObjects;
using HuynhAnKhuongWPF.Views;
using Microsoft.Extensions.DependencyInjection;
using Services.IServices;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HuynhAnKhuongWPF
{
    /// <summary>
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ICustomerService _customerService;
        private readonly IOrderService _orderService;
        private readonly IOrderDetailService _orderDetailService;
        private int _customerId;
        private Customer _customer;

        public CustomerWindow(
            IServiceProvider serviceProvider,
            ICustomerService customerService,
            IOrderService orderService,
            IOrderDetailService orderDetailService)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _customerService = customerService;
            _orderService = orderService;
            _orderDetailService = orderDetailService;
        }

        // Method to set the customer ID from the login window
        public void SetCustomerId(int customerId)
        {
            _customerId = customerId;
            _customer = _customerService.GetById(customerId);

            if (_customer == null)
            {
                MessageBox.Show("Could not retrieve customer information. Please log in again.");
                Logout_Click(null, null);
                return;
            }

            this.Title = $"Customer Portal - {_customer.ContactName}";

            Profile_Click(null, null);
        }

        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            if (_customer != null)
            {
                ContentFrame.Content = new CustomerProfileView(_customerService, _customer);
            }
        }

        private void OrderHistory_Click(object sender, RoutedEventArgs e)
        {
            if (_customer != null)
            {
                var employeeService = _serviceProvider.GetRequiredService<IEmployeeService>();
                var productService = _serviceProvider.GetRequiredService<IProductService>();
                
                ContentFrame.Content = new OrderHistoryView(
                    _orderService, 
                    _orderDetailService, 
                    employeeService, 
                    productService, 
                    _customer);
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            _customerId = 0;
            _customer = null;

            var loginWindow = _serviceProvider.GetRequiredService<LoginWindow>();
            loginWindow.Show();
            this.Close();
        }
    }
}
