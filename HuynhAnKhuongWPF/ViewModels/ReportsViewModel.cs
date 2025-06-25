using BusinessObjects;
using HuynhAnKhuongWPF.Commands;
using Services.IServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace HuynhAnKhuongWPF.ViewModels
{
    public class ReportsViewModel : ViewModelBase
    {
        private readonly IOrderService _orderService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly ICustomerService _customerService;
        private readonly IEmployeeService _employeeService;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        #region Properties

        private ObservableCollection<DateRangeOption> _dateRanges;
        public ObservableCollection<DateRangeOption> DateRanges
        {
            get { return _dateRanges; }
            set { SetProperty(ref _dateRanges, value); }
        }

        private DateRangeOption _selectedDateRange;
        public DateRangeOption SelectedDateRange
        {
            get { return _selectedDateRange; }
            set 
            { 
                if (SetProperty(ref _selectedDateRange, value))
                {
                    IsCustomDateRange = _selectedDateRange?.Id == "custom";
                    
                    if (_selectedDateRange != null && _selectedDateRange.Id != "custom")
                    {
                        // Update date range based on selection
                        switch (_selectedDateRange.Id)
                        {
                            case "today":
                                StartDate = DateTime.Today;
                                EndDate = DateTime.Today;
                                break;
                            case "yesterday":
                                StartDate = DateTime.Today.AddDays(-1);
                                EndDate = DateTime.Today.AddDays(-1);
                                break;
                            case "thisWeek":
                                StartDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
                                EndDate = DateTime.Today;
                                break;
                            case "lastWeek":
                                StartDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek - 7);
                                EndDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek - 1);
                                break;
                            case "thisMonth":
                                StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                                EndDate = DateTime.Today;
                                break;
                            case "lastMonth":
                                StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-1);
                                EndDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddDays(-1);
                                break;
                            case "last3Months":
                                StartDate = DateTime.Today.AddMonths(-3);
                                EndDate = DateTime.Today;
                                break;
                            case "thisYear":
                                StartDate = new DateTime(DateTime.Today.Year, 1, 1);
                                EndDate = DateTime.Today;
                                break;
                            case "allTime":
                                StartDate = DateTime.MinValue;
                                EndDate = DateTime.MaxValue;
                                break;
                        }
                    }
                }
            }
        }

        private bool _isCustomDateRange;
        public bool IsCustomDateRange
        {
            get { return _isCustomDateRange; }
            set { SetProperty(ref _isCustomDateRange, value); }
        }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get { return _startDate; }
            set { SetProperty(ref _startDate, value); }
        }

        private DateTime _endDate;
        public DateTime EndDate
        {
            get { return _endDate; }
            set { SetProperty(ref _endDate, value); }
        }

        private ObservableCollection<OrderSummaryViewModel> _orderSummaries;
        public ObservableCollection<OrderSummaryViewModel> OrderSummaries
        {
            get { return _orderSummaries; }
            set { SetProperty(ref _orderSummaries, value); }
        }

        private ObservableCollection<ProductStatViewModel> _topProducts;
        public ObservableCollection<ProductStatViewModel> TopProducts
        {
            get { return _topProducts; }
            set { SetProperty(ref _topProducts, value); }
        }

        private int _totalOrders;
        public int TotalOrders
        {
            get { return _totalOrders; }
            set { SetProperty(ref _totalOrders, value); }
        }

        private decimal _totalRevenue;
        public decimal TotalRevenue
        {
            get { return _totalRevenue; }
            set { SetProperty(ref _totalRevenue, value); }
        }

        private decimal _averageOrderValue;
        public decimal AverageOrderValue
        {
            get { return _averageOrderValue; }
            set { SetProperty(ref _averageOrderValue, value); }
        }

        #endregion

        #region Commands

        public ICommand GenerateReportCommand { get; private set; }

        #endregion

        public ReportsViewModel(
            IOrderService orderService,
            IOrderDetailService orderDetailService,
            ICustomerService customerService,
            IEmployeeService employeeService,
            IProductService productService,
            ICategoryService categoryService)
        {
            _orderService = orderService;
            _orderDetailService = orderDetailService;
            _customerService = customerService;
            _employeeService = employeeService;
            _productService = productService;
            _categoryService = categoryService;

            // Initialize commands
            GenerateReportCommand = new RelayCommand(GenerateReport);

            // Initialize collections
            OrderSummaries = new ObservableCollection<OrderSummaryViewModel>();
            TopProducts = new ObservableCollection<ProductStatViewModel>();

            // Setup date ranges
            SetupDateRanges();

            // Default to "This Month" date range
            SelectedDateRange = DateRanges.FirstOrDefault(d => d.Id == "thisMonth");

            // Generate initial report
            GenerateReport(null);
        }

        private void SetupDateRanges()
        {
            DateRanges = new ObservableCollection<DateRangeOption>
            {
                new DateRangeOption { Id = "today", Name = "Today" },
                new DateRangeOption { Id = "yesterday", Name = "Yesterday" },
                new DateRangeOption { Id = "thisWeek", Name = "This Week" },
                new DateRangeOption { Id = "lastWeek", Name = "Last Week" },
                new DateRangeOption { Id = "thisMonth", Name = "This Month" },
                new DateRangeOption { Id = "lastMonth", Name = "Last Month" },
                new DateRangeOption { Id = "last3Months", Name = "Last 3 Months" },
                new DateRangeOption { Id = "thisYear", Name = "This Year" },
                new DateRangeOption { Id = "allTime", Name = "All Time" },
                new DateRangeOption { Id = "custom", Name = "Custom Range" }
            };
        }

        private void GenerateReport(object parameter)
        {
            try
            {
                // Get orders within date range
                var orders = _orderService.GetAll()
                    .Where(o => o.OrderDate >= StartDate && o.OrderDate <= EndDate.AddDays(1).AddSeconds(-1))
                    .OrderByDescending(o => o.OrderDate)
                    .ToList();

                // Clear previous data
                OrderSummaries.Clear();
                TopProducts.Clear();

                // Prepare data structures
                var productStats = new Dictionary<int, ProductStatViewModel>();
                decimal totalRevenue = 0;

                // Process orders and details
                foreach (var order in orders)
                {
                    var customerName = GetCustomerName(order.CustomerId);
                    var employeeName = GetEmployeeName(order.EmployeeId);

                    // Get order details
                    var details = _orderDetailService.GetOrderDetailsByOrderId(order.OrderId).ToList();
                    
                    // Calculate order totals
                    int totalItems = details.Sum(d => d.Quantity);
                    decimal orderTotal = details.Sum(d => d.LineTotal);
                    
                    // Add to total revenue
                    totalRevenue += orderTotal;

                    // Create order summary
                    var orderSummary = new OrderSummaryViewModel
                    {
                        OrderId = order.OrderId,
                        OrderDate = order.OrderDate,
                        CustomerName = customerName,
                        EmployeeName = employeeName,
                        TotalItems = totalItems,
                        OrderTotal = orderTotal
                    };

                    OrderSummaries.Add(orderSummary);

                    // Process product stats
                    foreach (var detail in details)
                    {
                        if (!productStats.ContainsKey(detail.ProductId))
                        {
                            var product = _productService.GetById(detail.ProductId);
                            var category = _categoryService.GetById(product.CategoryId);
                            var categoryName = category?.CategoryName ?? "Unknown";

                            productStats[detail.ProductId] = new ProductStatViewModel
                            {
                                ProductId = product.ProductId,
                                ProductName = product.ProductName,
                                CategoryName = categoryName,
                                UnitsSold = 0,
                                TotalRevenue = 0
                            };
                        }
                        productStats[detail.ProductId].UnitsSold += detail.Quantity;
                        productStats[detail.ProductId].TotalRevenue += detail.LineTotal;
                    }
                }

                // Update summary metrics
                TotalOrders = orders.Count;
                TotalRevenue = totalRevenue;
                AverageOrderValue = TotalOrders > 0 ? totalRevenue / TotalOrders : 0;

                // Update top products
                var topProductsList = productStats.Values
                    .OrderByDescending(p => p.TotalRevenue)
                    .ToList();

                foreach (var product in topProductsList)
                {
                    TopProducts.Add(product);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating report: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string GetCustomerName(int customerId)
        {
            var customer = _customerService.GetById(customerId);
            return customer != null ? customer.ContactName : "Unknown";
        }

        private string GetEmployeeName(int employeeId)
        {
            var employee = _employeeService.GetById(employeeId);
            return employee != null ? employee.Name : "Unknown";
        }
    }

    public class DateRangeOption
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class OrderSummaryViewModel
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerName { get; set; }
        public string EmployeeName { get; set; }
        public int TotalItems { get; set; }
        public decimal OrderTotal { get; set; }
    }

    public class ProductStatViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public int UnitsSold { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}