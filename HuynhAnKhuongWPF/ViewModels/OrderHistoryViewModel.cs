using BusinessObjects;
using Services.IServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace HuynhAnKhuongWPF.ViewModels
{
    public class OrderHistoryViewModel : ViewModelBase
    {
        private readonly IOrderService _orderService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IEmployeeService _employeeService;
        private readonly IProductService _productService;
        private readonly Customer _customer;

        private ObservableCollection<OrderViewModel> _orders;
        public ObservableCollection<OrderViewModel> Orders
        {
            get { return _orders; }
            set { SetProperty(ref _orders, value); }
        }

        private OrderViewModel _selectedOrder;
        public OrderViewModel SelectedOrder
        {
            get { return _selectedOrder; }
            set
            {
                if (SetProperty(ref _selectedOrder, value))
                {
                    LoadOrderDetails();
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        private ObservableCollection<OrderDetailViewModel> _orderDetails;
        public ObservableCollection<OrderDetailViewModel> OrderDetails
        {
            get { return _orderDetails; }
            set { SetProperty(ref _orderDetails, value); }
        }

        private decimal _orderTotal;
        public decimal OrderTotal
        {
            get { return _orderTotal; }
            set { SetProperty(ref _orderTotal, value); }
        }

        public OrderHistoryViewModel(IOrderService orderService,
            IOrderDetailService orderDetailService,
            IEmployeeService employeeService,
            IProductService productService,
            Customer customer)
        {
            _orderService = orderService;
            _orderDetailService = orderDetailService;
            _employeeService = employeeService;
            _productService = productService;
            _customer = customer;

            OrderDetails = new ObservableCollection<OrderDetailViewModel>();
            LoadOrders();
        }

        private void LoadOrders()
        {
            // Get all orders and filter by customer ID
            var customerOrders = _orderService.GetAll()
                .Where(o => o.CustomerId == _customer.CustomerId)
                .OrderByDescending(o => o.OrderDate)
                .Select(o => new OrderViewModel
                {
                    OrderId = o.OrderId,
                    CustomerId = o.CustomerId,
                    EmployeeId = o.EmployeeId,
                    OrderDate = o.OrderDate,
                    EmployeeName = GetEmployeeName(o.EmployeeId)
                })
                .ToList();

            Orders = new ObservableCollection<OrderViewModel>(customerOrders);

            // If we have orders, select the first one
            if (Orders.Count > 0)
            {
                SelectedOrder = Orders[0];
            }
        }

        private string GetEmployeeName(int employeeId)
        {
            var employee = _employeeService.GetById(employeeId);
            return employee?.Name ?? "Unknown";
        }

        private void LoadOrderDetails()
        {
            if (_selectedOrder == null)
            {
                OrderDetails.Clear();
                OrderTotal = 0;
                return;
            }

            try
            {
                // Get order details for the selected order
                var details = _orderDetailService.GetOrderDetailsByOrderId(_selectedOrder.OrderId);

                // Debug info
                System.Diagnostics.Debug.WriteLine($"Found {details.Count()} order details for order {_selectedOrder.OrderId}");

                // Transform to view models with proper product names
                var detailViewModels = new List<OrderDetailViewModel>();
                decimal total = 0;

                foreach (var d in details)
                {
                    // Calculate line total manually - ignore the field in the original
                    decimal lineTotal = d.UnitPrice * d.Quantity * (decimal)(1 - d.Discount);
                    total += lineTotal;

                    string productName = GetProductName(d.ProductId);

                    var detailVM = new OrderDetailViewModel
                    {
                        OrderId = d.OrderId,
                        ProductId = d.ProductId,
                        UnitPrice = d.UnitPrice,
                        Quantity = d.Quantity,
                        Discount = d.Discount,
                        LineTotal = lineTotal,
                        ProductName = productName
                    };

                    detailViewModels.Add(detailVM);

                    System.Diagnostics.Debug.WriteLine($"OrderDetail: Product={productName}, Price={d.UnitPrice}, Qty={d.Quantity}, Discount={d.Discount}, Total={lineTotal}");
                }

                OrderDetails = new ObservableCollection<OrderDetailViewModel>(detailViewModels);
                OrderTotal = total; // Use our manually calculated total

                System.Diagnostics.Debug.WriteLine($"Set OrderDetails collection with {OrderDetails.Count} items");
                System.Diagnostics.Debug.WriteLine($"OrderTotal: {OrderTotal}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                OrderDetails = new ObservableCollection<OrderDetailViewModel>();
                OrderTotal = 0;
            }
        }

        private string GetProductName(int productId)
        {
            var product = _productService.GetById(productId);
            return product?.ProductName ?? "Unknown Product";
        }
    }

    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime OrderDate { get; set; }
        public string EmployeeName { get; set; }
    }

    public class OrderDetailViewModel
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public double Discount { get; set; }
        public decimal LineTotal { get; set; } 
        public string ProductName { get; set; }
    }
}