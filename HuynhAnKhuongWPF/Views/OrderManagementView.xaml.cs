using HuynhAnKhuongWPF.ViewModels;
using Services.IServices;
using System.Windows.Controls;

namespace HuynhAnKhuongWPF.Views
{
    public partial class OrderManagementView : UserControl
    {
        public OrderManagementView(
            IOrderService orderService,
            IOrderDetailService orderDetailService,
            ICustomerService customerService,
            IEmployeeService employeeService,
            IProductService productService)
        {
            InitializeComponent();
            DataContext = new OrderManagementViewModel(
                orderService, 
                orderDetailService, 
                customerService, 
                employeeService, 
                productService);
        }
    }
}