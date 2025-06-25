using BusinessObjects;
using HuynhAnKhuongWPF.ViewModels;
using Services.IServices;
using System.Windows.Controls;

namespace HuynhAnKhuongWPF.Views
{
    public partial class OrderHistoryView : UserControl
    {
        public OrderHistoryView(
            IOrderService orderService, 
            IOrderDetailService orderDetailService, 
            IEmployeeService employeeService,
            IProductService productService,
            Customer customer)
        {
            InitializeComponent();
            DataContext = new OrderHistoryViewModel(
                orderService, 
                orderDetailService, 
                employeeService, 
                productService, 
                customer);
        }
    }
}