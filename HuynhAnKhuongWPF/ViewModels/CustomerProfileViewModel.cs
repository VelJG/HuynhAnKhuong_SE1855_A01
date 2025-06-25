using BusinessObjects;
using HuynhAnKhuongWPF.Commands;
using Services.IServices;
using System.Windows;
using System.Windows.Input;

namespace HuynhAnKhuongWPF.ViewModels
{
    public class CustomerProfileViewModel : ViewModelBase
    {
        private readonly ICustomerService _customerService;
        private Customer _currentCustomer;

        private int _customerID;
        public int CustomerID
        {
            get { return _customerID; }
            set { SetProperty(ref _customerID, value); }
        }

        private string _companyName;
        public string CompanyName
        {
            get { return _companyName; }
            set { SetProperty(ref _companyName, value); }
        }

        private string _contactName;
        public string ContactName
        {
            get { return _contactName; }
            set { SetProperty(ref _contactName, value); }
        }

        private string _contactTitle;
        public string ContactTitle
        {
            get { return _contactTitle; }
            set { SetProperty(ref _contactTitle, value); }
        }

        private string _address;
        public string Address
        {
            get { return _address; }
            set { SetProperty(ref _address, value); }
        }

        private string _phone;
        public string Phone
        {
            get { return _phone; }
            set { SetProperty(ref _phone, value); }
        }

        public ICommand SaveCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        public CustomerProfileViewModel(ICustomerService customerService, Customer customer)
        {
            _customerService = customerService;
            _currentCustomer = customer;
            
            SaveCommand = new RelayCommand(ExecuteSave);
            CancelCommand = new RelayCommand(ExecuteCancel);
            
            LoadCustomerData();
        }

        private void LoadCustomerData()
        {
            if (_currentCustomer != null)
            {
                CustomerID = _currentCustomer.CustomerId;
                CompanyName = _currentCustomer.CompanyName;
                ContactName = _currentCustomer.ContactName;
                ContactTitle = _currentCustomer.ContactTitle;
                Address = _currentCustomer.Address;
                Phone = _currentCustomer.Phone;
            }
        }

        private void ExecuteSave(object obj)
        {
            if (string.IsNullOrWhiteSpace(CompanyName) || 
                string.IsNullOrWhiteSpace(ContactName) ||
                string.IsNullOrWhiteSpace(Phone))
            {
                MessageBox.Show("Please enter required fields (Company Name, Contact Name, and Phone)");
                return;
            }

            _currentCustomer.CompanyName = CompanyName;
            _currentCustomer.ContactName = ContactName;
            _currentCustomer.ContactTitle = ContactTitle;
            _currentCustomer.Address = Address;
            _currentCustomer.Phone = Phone;

            bool success = _customerService.Update(_currentCustomer);
            
            if (success)
            {
                MessageBox.Show("Profile updated successfully!");
            }
            else
            {
                MessageBox.Show("Failed to update profile. Please try again.");
            }
        }

        private void ExecuteCancel(object obj)
        {
            LoadCustomerData();
        }
    }
}