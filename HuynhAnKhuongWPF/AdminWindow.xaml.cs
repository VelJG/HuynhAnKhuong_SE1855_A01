using System;
using Microsoft.Extensions.DependencyInjection;
using Services.IServices;
using System.Windows;
using HuynhAnKhuongWPF.Views;

namespace HuynhAnKhuongWPF
{
    public partial class AdminWindow : Window
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ICustomerService _customerService;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly ICategoryService _categoryService;
        private readonly IEmployeeService _employeeService;

        public AdminWindow(
            IServiceProvider serviceProvider,
            ICustomerService customerService,
            IProductService productService,
            IOrderService orderService,
            IOrderDetailService orderDetailService,
            ICategoryService categoryService,
            IEmployeeService employeeService)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _customerService = customerService;
            _productService = productService;
            _orderService = orderService;
            _orderDetailService = orderDetailService;
            _categoryService = categoryService;
            _employeeService = employeeService;

            CustomerManagement_Click(null, null);
        }

        private void CustomerManagement_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = new CustomerManagementView(_customerService);
        }

        private void ProductManagement_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = new ProductManagementView(_productService, _categoryService);
        }

        private void OrderManagement_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = new OrderManagementView(
                _orderService, 
                _orderDetailService, 
                _customerService, 
                _employeeService, 
                _productService);
        }

        private void Reports_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = new ReportsView(
                _orderService,
                _orderDetailService,
                _customerService,
                _employeeService,
                _productService,
                _categoryService);
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = _serviceProvider.GetRequiredService<LoginWindow>();
            loginWindow.Show();
            this.Close();
        }
    }
}