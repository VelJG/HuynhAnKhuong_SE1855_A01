using BusinessObjects;
using Microsoft.Extensions.DependencyInjection; // <-- Add this using statement
using Services.IServices;
using System;
using System.Windows;

namespace HuynhAnKhuongWPF
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        // These fields are correct
        private readonly IEmployeeService _employeeService;
        private readonly ICustomerService _customerService;

        private readonly IServiceProvider _serviceProvider;

        public LoginWindow(IEmployeeService employeeService, ICustomerService customerService, IServiceProvider serviceProvider)
        {
            InitializeComponent();

            _employeeService = employeeService;
            _customerService = customerService;
            _serviceProvider = serviceProvider;


            rbEmployee.Checked += (s, e) =>
            {
                gridEmployeeLogin.Visibility = Visibility.Visible;
                gridCustomerLogin.Visibility = Visibility.Collapsed;
            };

            rbCustomer.Checked += (s, e) =>
            {
                gridEmployeeLogin.Visibility = Visibility.Collapsed;
                gridCustomerLogin.Visibility = Visibility.Visible;
            };
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (rbEmployee.IsChecked == true)
                {
                    string username = txtUsername.Text;
                    string password = txtPassword.Password;

                    if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                    {
                        MessageBox.Show("Please enter username and password");
                        return;
                    }

                    Employee employee = _employeeService.Login(username, password);

                    if (employee != null)
                    {
                        var adminWindow = _serviceProvider.GetRequiredService<AdminWindow>();
                        adminWindow.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Invalid username or password.");
                    }
                }
                else
                {
                    string phone = txtPhoneNumber.Text;

                    if (string.IsNullOrWhiteSpace(phone))
                    {
                        MessageBox.Show("Please enter phone number");
                        return;
                    }

                    Customer customer = _customerService.Login(phone);

                    if (customer != null)
                    {
                        var customerWindow = _serviceProvider.GetRequiredService<CustomerWindow>();
                        // Set the customer ID directly
                        customerWindow.SetCustomerId(customer.CustomerId);
                        customerWindow.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Invalid phone number.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }
    }
}