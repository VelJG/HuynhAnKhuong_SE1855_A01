using BusinessObjects;
using HuynhAnKhuongWPF.Commands;
using Services.IServices;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace HuynhAnKhuongWPF.ViewModels
{
    public class CustomerManagementViewModel : ViewModelBase
    {
        private readonly ICustomerService _customerService;
        
        private ObservableCollection<Customer> _customers;
        public ObservableCollection<Customer> Customers
        {
            get { return _customers; }
            set { SetProperty(ref _customers, value); }
        }

        private Customer _selectedCustomer;
        public Customer SelectedCustomer
        {
            get { return _selectedCustomer; }
            set 
            { 
                if (SetProperty(ref _selectedCustomer, value))
                {
                    if (_selectedCustomer != null)
                    {
                        CustomerID = _selectedCustomer.CustomerId;
                        CompanyName = _selectedCustomer.CompanyName;
                        ContactName = _selectedCustomer.ContactName;
                        ContactTitle = _selectedCustomer.ContactTitle;
                        Address = _selectedCustomer.Address;
                        Phone = _selectedCustomer.Phone;
                    }
                    
                    // Update commands that depend on selected customer
                    DeleteCommand.CanExecute(null);
                    EditCommand.CanExecute(null);
                }
            }
        }

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

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set 
            { 
                SetProperty(ref _searchText, value);
                SearchCommand.Execute(null);
            }
        }

        public ICommand AddCommand { get; private set; }
        public ICommand EditCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand SearchCommand { get; private set; }
        public ICommand ClearCommand { get; private set; }

        public CustomerManagementViewModel(ICustomerService customerService)
        {
            _customerService = customerService;
            
            AddCommand = new RelayCommand(ExecuteAdd);
            EditCommand = new RelayCommand(ExecuteEdit, CanEdit);
            DeleteCommand = new RelayCommand(ExecuteDelete, CanDelete);
            SearchCommand = new RelayCommand(ExecuteSearch);
            ClearCommand = new RelayCommand(ExecuteClear);
            
            // Load data
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            var customerList = _customerService.GetAll();
            Customers = new ObservableCollection<Customer>(customerList);
        }

        private void ExecuteSearch(object obj)
        {
            var results = _customerService.Search(_searchText);
            Customers = new ObservableCollection<Customer>(results);
        }

        private void ExecuteClear(object obj)
        {
            SearchText = string.Empty;
            CustomerID = 0;
            CompanyName = string.Empty;
            ContactName = string.Empty;
            ContactTitle = string.Empty;
            Address = string.Empty;
            Phone = string.Empty;
            SelectedCustomer = null;
            LoadCustomers();
        }

        private void ExecuteAdd(object obj)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(CompanyName) || 
                string.IsNullOrWhiteSpace(ContactName) ||
                string.IsNullOrWhiteSpace(Phone))
            {
                MessageBox.Show("Please enter required fields");
                return;
            }

            // Create new customer
            var newCustomer = new Customer
            {
                CustomerId = CustomerID,
                CompanyName = CompanyName,
                ContactName = ContactName,
                ContactTitle = ContactTitle,
                Address = Address,
                Phone = Phone
            };

            bool success = _customerService.Add(newCustomer);
            
            if (success)
            {
                MessageBox.Show("Customer added successfully!");
                LoadCustomers();
                ExecuteClear(null);
            }
            else
            {
                MessageBox.Show("Failed to add customer. Please try again.");
            }
        }

        private void ExecuteEdit(object obj)
        {
            if (SelectedCustomer == null)
            {
                MessageBox.Show("Please select a customer to edit");
                return;
            }

            if (string.IsNullOrWhiteSpace(CompanyName) || 
                string.IsNullOrWhiteSpace(ContactName) ||
                string.IsNullOrWhiteSpace(Phone))
            {
                MessageBox.Show("Please enter required fields");
                return;
            }

            SelectedCustomer.CompanyName = CompanyName;
            SelectedCustomer.ContactName = ContactName;
            SelectedCustomer.ContactTitle = ContactTitle;
            SelectedCustomer.Address = Address;
            SelectedCustomer.Phone = Phone;

            bool success = _customerService.Update(SelectedCustomer);
            
            if (success)
            {
                MessageBox.Show("Customer updated successfully!");
                LoadCustomers();
            }
            else
            {
                MessageBox.Show("Failed to update customer. Please try again.");
            }
        }

        private bool CanEdit(object obj)
        {
            return SelectedCustomer != null;
        }

        private void ExecuteDelete(object obj)
        {
            if (SelectedCustomer == null)
            {
                MessageBox.Show("Please select a customer to delete");
                return;
            }

            MessageBoxResult result = MessageBox.Show(
                $"Are you sure you want to delete {SelectedCustomer.ContactName}?", 
                "Confirm Deletion",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                bool success = _customerService.Delete(SelectedCustomer.CustomerId);
                
                if (success)
                {
                    MessageBox.Show("Customer deleted successfully!");
                    LoadCustomers();
                    ExecuteClear(null);
                }
                else
                {
                    MessageBox.Show("Failed to delete customer. Please try again.");
                }
            }
        }

        private bool CanDelete(object obj)
        {
            return SelectedCustomer != null;
        }
    }
}