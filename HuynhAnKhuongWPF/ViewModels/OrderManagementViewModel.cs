using BusinessObjects;
using HuynhAnKhuongWPF.Commands;
using Services.IServices;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace HuynhAnKhuongWPF.ViewModels
{
    public class OrderManagementViewModel : ViewModelBase
    {
        private readonly IOrderService _orderService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly ICustomerService _customerService;
        private readonly IEmployeeService _employeeService;
        private readonly IProductService _productService;

        private ObservableCollection<Order> _orders;
        public ObservableCollection<Order> Orders
        {
            get { return _orders; }
            set { SetProperty(ref _orders, value); }
        }

        private ObservableCollection<OrderDetail> _selectedOrderDetails;
        public ObservableCollection<OrderDetail> SelectedOrderDetails
        {
            get { return _selectedOrderDetails; }
            set
            {
                if (SetProperty(ref _selectedOrderDetails, value))
                {
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        private ObservableCollection<Customer> _customers;
        public ObservableCollection<Customer> Customers
        {
            get { return _customers; }
            set { SetProperty(ref _customers, value); }
        }

        private ObservableCollection<Employee> _employees;
        public ObservableCollection<Employee> Employees
        {
            get { return _employees; }
            set { SetProperty(ref _employees, value); }
        }

        private ObservableCollection<Product> _products;
        public ObservableCollection<Product> Products
        {
            get { return _products; }
            set { SetProperty(ref _products, value); }
        }

        private Order _selectedOrder;
        public Order SelectedOrder
        {
            get { return _selectedOrder; }
            set
            {
                if (SetProperty(ref _selectedOrder, value))
                {
                    if (_selectedOrder != null)
                    {
                        OrderId = _selectedOrder.OrderId;
                        CustomerId = _selectedOrder.CustomerId;
                        EmployeeId = _selectedOrder.EmployeeId;
                        OrderDate = _selectedOrder.OrderDate;
                        IsNewOrder = false;

                        LoadOrderDetails();
                    }
                    else
                    {
                        SelectedOrderDetails = new ObservableCollection<OrderDetail>();
                    }

                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        private OrderDetail _selectedOrderDetail;
        public OrderDetail SelectedOrderDetail
        {
            get { return _selectedOrderDetail; }
            set
            {
                if (SetProperty(ref _selectedOrderDetail, value))
                {
                    if (_selectedOrderDetail != null)
                    {
                        SelectedProductId = _selectedOrderDetail.ProductId;
                        Quantity = _selectedOrderDetail.Quantity;
                        Discount = _selectedOrderDetail.Discount;

                        var product = _productService.GetById(SelectedProductId);
                        if (product != null)
                        {
                            UnitPrice = product.UnitPrice;
                        }
                    }

                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        private int _orderId;
        public int OrderId
        {
            get { return _orderId; }
            set
            {
                if (SetProperty(ref _orderId, value))
                {
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        private int _customerId;
        public int CustomerId
        {
            get { return _customerId; }
            set
            {
                if (SetProperty(ref _customerId, value))
                {
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        private int _employeeId;
        public int EmployeeId
        {
            get { return _employeeId; }
            set
            {
                if (SetProperty(ref _employeeId, value))
                {
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        private DateTime _orderDate = DateTime.Now;
        public DateTime OrderDate
        {
            get { return _orderDate; }
            set { SetProperty(ref _orderDate, value); }
        }

        private int _selectedProductId;
        public int SelectedProductId
        {
            get { return _selectedProductId; }
            set
            {
                if (SetProperty(ref _selectedProductId, value))
                {
                    var product = _productService.GetById(value);
                    if (product != null)
                    {
                        UnitPrice = product.UnitPrice;
                    }
                }
            }
        }

        private decimal _unitPrice;
        public decimal UnitPrice
        {
            get { return _unitPrice; }
            set { SetProperty(ref _unitPrice, value); }
        }

        private int _quantity = 1;
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                if (SetProperty(ref _quantity, value))
                {
                    CalculateItemTotal();
                }
            }
        }

        private double _discount;
        public double Discount
        {
            get { return _discount; }
            set
            {
                if (SetProperty(ref _discount, value))
                {
                    CalculateItemTotal();
                }
            }
        }

        private decimal _orderTotal;
        public decimal OrderTotal
        {
            get { return _orderTotal; }
            set { SetProperty(ref _orderTotal, value); }
        }

        private decimal _itemTotal;
        public decimal ItemTotal
        {
            get { return _itemTotal; }
            set { SetProperty(ref _itemTotal, value); }
        }

        private bool _isNewOrder;
        public bool IsNewOrder
        {
            get { return _isNewOrder; }
            set { SetProperty(ref _isNewOrder, value); }
        }

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set { SetProperty(ref _searchText, value); }
        }

        public ICommand SaveOrderCommand { get; private set; }
        public ICommand DeleteOrderCommand { get; private set; }
        public ICommand AddOrderDetailCommand { get; private set; }
        public ICommand UpdateOrderDetailCommand { get; private set; }
        public ICommand RemoveOrderDetailCommand { get; private set; }
        public ICommand SearchCommand { get; private set; }
        public ICommand ClearCommand { get; private set; }
        public ICommand NewOrderCommand { get; private set; }

        public OrderManagementViewModel(
            IOrderService orderService,
            IOrderDetailService orderDetailService,
            ICustomerService customerService,
            IEmployeeService employeeService,
            IProductService productService)
        {
            _orderService = orderService;
            _orderDetailService = orderDetailService;
            _customerService = customerService;
            _employeeService = employeeService;
            _productService = productService;

            // Initialize commands
            SaveOrderCommand = new RelayCommand(ExecuteSaveOrder, CanSaveOrder);
            DeleteOrderCommand = new RelayCommand(ExecuteDeleteOrder, CanDeleteOrder);
            AddOrderDetailCommand = new RelayCommand(ExecuteAddOrderDetail, CanAddOrderDetail);
            UpdateOrderDetailCommand = new RelayCommand(ExecuteUpdateOrderDetail, CanUpdateOrderDetail);
            RemoveOrderDetailCommand = new RelayCommand(ExecuteRemoveOrderDetail, CanRemoveOrderDetail);
            SearchCommand = new RelayCommand(ExecuteSearch);
            ClearCommand = new RelayCommand(ExecuteClear);
            NewOrderCommand = new RelayCommand(ExecuteNewOrder);

            SelectedOrderDetails = new ObservableCollection<OrderDetail>();

            LoadCustomers();
            LoadEmployees();
            LoadProducts();
            LoadOrders();
        }

        private void LoadCustomers()
        {
            var customerList = _customerService.GetAll();
            Customers = new ObservableCollection<Customer>(customerList);
        }

        private void LoadEmployees()
        {
            var employeeList = _employeeService.GetAll();
            Employees = new ObservableCollection<Employee>(employeeList);
        }

        private void LoadProducts()
        {
            var productList = _productService.GetAll().Where(p => !p.Discountinued && p.UnitsInStock > 0);
            Products = new ObservableCollection<Product>(productList);
        }

        private void LoadOrders()
        {
            var orderList = _orderService.GetAll().OrderByDescending(o => o.OrderDate);
            Orders = new ObservableCollection<Order>(orderList);

            foreach (var order in Orders)
            {
                order.Customer = _customerService.GetById(order.CustomerId);
                order.Employee = _employeeService.GetById(order.EmployeeId);
            }
        }

        private void LoadOrderDetails()
        {
            if (SelectedOrder == null)
            {
                SelectedOrderDetails = new ObservableCollection<OrderDetail>();
                return;
            }

            var details = _orderDetailService.GetOrderDetailsByOrderId(SelectedOrder.OrderId);
            SelectedOrderDetails = new ObservableCollection<OrderDetail>(details);

            foreach (var detail in SelectedOrderDetails)
            {
                detail.Product = _productService.GetById(detail.ProductId);
            }

            CalculateOrderTotal();
        }

        private void CalculateOrderTotal()
        {
            if (SelectedOrderDetails == null || !SelectedOrderDetails.Any())
            {
                OrderTotal = 0;
                return;
            }

            OrderTotal = SelectedOrderDetails.Sum(d => d.UnitPrice * d.Quantity * (decimal)(1 - d.Discount));
        }

        private void CalculateItemTotal()
        {
            ItemTotal = UnitPrice * Quantity * (decimal)(1 - Discount);
        }

        private void RefreshOrderDetails()
        {
            var refreshedDetails = new ObservableCollection<OrderDetail>();
            foreach (var detail in SelectedOrderDetails)
            {
                refreshedDetails.Add(detail);
            }
            SelectedOrderDetails = refreshedDetails;

            CalculateOrderTotal();

            CommandManager.InvalidateRequerySuggested();
        }

        private void ExecuteSaveOrder(object obj)
        {
            if (CustomerId <= 0)
            {
                MessageBox.Show("Please select a customer");
                return;
            }

            if (EmployeeId <= 0)
            {
                MessageBox.Show("Please select an employee");
                return;
            }

            try
            {
                var order = new Order
                {
                    OrderId = OrderId,
                    CustomerId = CustomerId,
                    EmployeeId = EmployeeId,
                    OrderDate = OrderDate
                };

                bool success = _orderService.Update(order);

                if (!success)
                {
                    MessageBox.Show("Failed to save order. Please try again.");
                    return;
                }

                MessageBox.Show("Order saved successfully!");
                LoadOrders();
                IsNewOrder = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving order: {ex.Message}");
            }
        }

        private bool CanSaveOrder(object obj)
        {
            return OrderId > 0 && CustomerId > 0 && EmployeeId > 0;
        }

        private void ExecuteDeleteOrder(object obj)
        {
            if (SelectedOrder == null)
            {
                MessageBox.Show("Please select an order to delete");
                return;
            }

            MessageBoxResult result = MessageBox.Show(
                $"Are you sure you want to delete Order #{SelectedOrder.OrderId}?",
                "Confirm Deletion",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var details = _orderDetailService.GetOrderDetailsByOrderId(SelectedOrder.OrderId);
                    foreach (var detail in details)
                    {
                        _orderDetailService.DeleteOrderDetail(detail.OrderId, detail.ProductId);
                    }

                    bool success = _orderService.Delete(SelectedOrder.OrderId);

                    if (success)
                    {
                        MessageBox.Show("Order deleted successfully!");
                        LoadOrders();
                        ExecuteClear(null);
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete order. Please try again.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting order: {ex.Message}");
                }
            }
        }

        private bool CanDeleteOrder(object obj)
        {
            return SelectedOrder != null;
        }

        private void ExecuteAddOrderDetail(object obj)
        {
            if (SelectedProductId <= 0)
            {
                MessageBox.Show("Please select a product");
                return;
            }

            if (Quantity < 0)
            {
                MessageBox.Show("Quantity must be greater than zero");
                return;
            }
            if(Quantity == 0)
            {
                MessageBox.Show("Quantity must be a number");
                return;
            }

            if (Discount < 0 || Discount > 1)
            {
                MessageBox.Show("Discount must be between 0 and 1 (0% to 100%)");
                return;
            }

            var product = _productService.GetById(SelectedProductId);
            if (product == null)
            {
                MessageBox.Show("Selected product not found");
                return;
            }

            try
            {
                var existingDetail = SelectedOrderDetails.FirstOrDefault(d => d.ProductId == SelectedProductId);
                if (existingDetail != null && SelectedOrderDetail != existingDetail)
                {
                    MessageBoxResult result = MessageBox.Show(
                        $"{product.ProductName} is already in the order. Do you want to update the quantity?",
                        "Product Already Added",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        existingDetail.Quantity = Quantity;
                        existingDetail.Discount = Discount;

                        bool success = _orderDetailService.Update(existingDetail);
                        if (!success)
                        {
                            MessageBox.Show("Failed to update order detail. Please try again.");
                            return;
                        }

                        RefreshOrderDetails();
                        MessageBox.Show("Product quantity updated successfully!");
                    }
                }
                else if (SelectedOrderDetail != null && SelectedOrderDetail.ProductId == SelectedProductId)
                {
                    SelectedOrderDetail.Quantity = Quantity;
                    SelectedOrderDetail.Discount = Discount;

                    bool success = _orderDetailService.Update(SelectedOrderDetail);
                    if (!success)
                    {
                        MessageBox.Show("Failed to update order detail. Please try again.");
                        return;
                    }

                    RefreshOrderDetails();
                    MessageBox.Show("Product updated successfully!");
                }
                else
                {
                    var newDetail = new OrderDetail
                    {
                        OrderId = OrderId,
                        ProductId = SelectedProductId,
                        UnitPrice = product.UnitPrice,
                        Quantity = Quantity,
                        Discount = Discount,
                        Product = product
                    };

                    bool success = _orderDetailService.Add(newDetail);
                    if (!success)
                    {
                        MessageBox.Show("Failed to add product to order. Please try again.");
                        return;
                    }

                    SelectedOrderDetails.Add(newDetail);
                    CalculateOrderTotal();
                    CommandManager.InvalidateRequerySuggested();
                    MessageBox.Show("Product added to order successfully!");
                }

                SelectedProductId = 0;
                Quantity = 1;
                Discount = 0;
                UnitPrice = 0;
                ItemTotal = 0;
                SelectedOrderDetail = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding product to order: {ex.Message}");
            }
        }

        private bool CanAddOrderDetail(object obj)
        {
            return OrderId > 0 && SelectedProductId > 0 && Quantity > 0 && Discount >= 0 && Discount <= 1;
        }

        private void ExecuteRemoveOrderDetail(object obj)
        {
            if (SelectedOrderDetail == null)
            {
                MessageBox.Show("Please select an item to remove");
                return;
            }

            MessageBoxResult result = MessageBox.Show(
                $"Are you sure you want to remove {SelectedOrderDetail.Product.ProductName} from this order?",
                "Confirm Removal",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            try
            {
                bool success = _orderDetailService.DeleteOrderDetail(
                    SelectedOrderDetail.OrderId,
                    SelectedOrderDetail.ProductId);

                if (!success)
                {
                    MessageBox.Show("Failed to remove item from order. Please try again.");
                    return;
                }

                SelectedOrderDetails.Remove(SelectedOrderDetail);
                SelectedOrderDetail = null;

                CalculateOrderTotal();

                CommandManager.InvalidateRequerySuggested();

                MessageBox.Show("Item removed from order successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error removing item from order: {ex.Message}");
            }
        }

        private bool CanRemoveOrderDetail(object obj)
        {
            return SelectedOrderDetail != null;
        }

        private void ExecuteUpdateOrderDetail(object obj)
        {
            if (SelectedOrderDetail == null)
            {
                MessageBox.Show("Please select an item to update");
                return;
            }

            if (SelectedProductId <= 0)
            {
                MessageBox.Show("Please select a product");
                return;
            }

            if (Quantity < 0)
            {
                MessageBox.Show("Quantity must be greater than zero");
                return;
            }
            if (Quantity == 0)
            {
                MessageBox.Show("Quantity must be a number");
                return;
            }

            if (Discount < 0 || Discount > 1)
            {
                MessageBox.Show("Discount must be between 0 and 1 (0% to 100%)");
                return;
            }

            try
            {
                SelectedOrderDetail.Quantity = Quantity;
                SelectedOrderDetail.Discount = Discount;

                bool success = _orderDetailService.Update(SelectedOrderDetail);
                if (!success)
                {
                    MessageBox.Show("Failed to update order detail. Please try again.");
                    return;
                }

                RefreshOrderDetails();

                SelectedProductId = 0;
                Quantity = 1;
                Discount = 0;
                UnitPrice = 0;
                ItemTotal = 0;
                SelectedOrderDetail = null;

                MessageBox.Show("Order detail updated successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating order detail: {ex.Message}");
            }
        }

        private bool CanUpdateOrderDetail(object obj)
        {
            return SelectedOrderDetail != null &&
                   SelectedProductId > 0 &&
                   Quantity > 0 &&
                   Discount >= 0 &&
                   Discount <= 1;
        }

        private void ExecuteSearch(object obj)
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                LoadOrders();
                return;
            }

            string search = SearchText.ToLower();
            var filteredOrders = _orderService.GetAll()
                .Where(o =>
                    o.OrderId.ToString().Contains(search) ||
                    _customerService.GetById(o.CustomerId).CompanyName.ToLower().Contains(search) ||
                    _employeeService.GetById(o.EmployeeId).Name.ToLower().Contains(search))
                .OrderByDescending(o => o.OrderDate);

            Orders = new ObservableCollection<Order>(filteredOrders);

            foreach (var order in Orders)
            {
                order.Customer = _customerService.GetById(order.CustomerId);
                order.Employee = _employeeService.GetById(order.EmployeeId);
            }
        }

        private void ExecuteClear(object obj)
        {
            OrderId = 0;
            CustomerId = 0;
            EmployeeId = 0;
            OrderDate = DateTime.Now;
            SelectedOrder = null;
            SelectedOrderDetail = null;
            SelectedOrderDetails.Clear();
            SearchText = string.Empty;
            SelectedProductId = 0;
            Quantity = 1;
            Discount = 0;
            UnitPrice = 0;
            ItemTotal = 0;
            OrderTotal = 0;
            IsNewOrder = false;
        }

        private void ExecuteNewOrder(object obj)
        {
            ExecuteClear(null);

            try
            {
                int maxOrderId = 0;
                var orders = _orderService.GetAll();
                if (orders.Any())
                {
                    maxOrderId = orders.Max(o => o.OrderId);
                }

                OrderId = maxOrderId + 1;
                OrderDate = DateTime.Now;
                IsNewOrder = true;

                var newOrder = new Order
                {
                    OrderId = OrderId,
                    OrderDate = OrderDate,
                    CustomerId = 0,
                    EmployeeId = 0
                };

                bool success = _orderService.Add(newOrder);
                if (!success)
                {
                    MessageBox.Show("Failed to create new order. Please try again.");
                    OrderId = 0;
                    IsNewOrder = false;
                    return;
                }

                SelectedOrderDetails = new ObservableCollection<OrderDetail>();

                LoadCustomers();
                LoadEmployees();
                LoadProducts();

                MessageBox.Show($"New order #{OrderId} created. Please add customer, employee, and products.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating new order: {ex.Message}");
                OrderId = 0;
                IsNewOrder = false;
            }
        }
    }
}