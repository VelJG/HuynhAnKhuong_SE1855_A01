using System;
using System.Windows;
using DataAccessLayer.DAOs;
using Microsoft.Extensions.DependencyInjection;
using Repositories.IRepositories;
using Repositories.Repositories;
using Services.IServices;
using Services.Services;
using HuynhAnKhuongWPF.Views;

namespace HuynhAnKhuongWPF
{
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;

        public App()
        {
            try
            {
                ServiceCollection services = new ServiceCollection();
                ConfigureServices(services);
                _serviceProvider = services.BuildServiceProvider();
            }
            catch (Exception ex)
            {
                MessageBox.Show("A fatal error occurred during DI configuration: \n\n" +
                                ex.ToString(), "Startup Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // DAOs
            services.AddSingleton<CustomerDAO>();
            services.AddSingleton<EmployeeDAO>();
            services.AddSingleton<OrderDAO>();
            services.AddSingleton<OrderDetailDAO>();
            services.AddSingleton<ProductDAO>(); 
            services.AddSingleton<CategoryDAO>();

            // Repositories
            services.AddSingleton<ICustomerRepository, CustomerRepository>();
            services.AddSingleton<IEmployeeRepository, EmployeeRepository>();
            services.AddSingleton<IOrderRepository, OrderRepository>();
            services.AddSingleton<IOrderDetailRepository, OrderDetailRepository>();
            services.AddSingleton<IProductRepository, ProductRepository>();
            services.AddSingleton<ICategoryRepository, CategoryRepository>();

            // Services
            services.AddTransient<ICustomerService, CustomerService>();
            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IOrderDetailService, OrderDetailService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<ICategoryService, CategoryService>();

            // Views
            services.AddTransient<CustomerManagementView>();
            services.AddTransient<CustomerProfileView>();
            services.AddTransient<ProductManagementView>();
            services.AddTransient<OrderManagementView>();

            // Windows
            services.AddTransient<LoginWindow>();
            services.AddTransient<AdminWindow>();
            services.AddTransient<CustomerWindow>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (_serviceProvider == null) return;

            try
            {
                var loginWindow = _serviceProvider.GetRequiredService<LoginWindow>();
                loginWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("A fatal error occurred while creating the main window: \n\n" +
                               ex.ToString(), "Startup Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}