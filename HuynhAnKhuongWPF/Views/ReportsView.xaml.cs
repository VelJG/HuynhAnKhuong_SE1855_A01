using BusinessObjects;
using HuynhAnKhuongWPF.ViewModels;
using Services.IServices;
using System.Windows.Controls;

namespace HuynhAnKhuongWPF.Views
{
    public partial class ReportsView : UserControl
    {
        public ReportsView(
            IOrderService orderService,
            IOrderDetailService orderDetailService,
            ICustomerService customerService,
            IEmployeeService employeeService,
            IProductService productService,
            ICategoryService categoryService)
        {
            InitializeComponent();
            DataContext = new ReportsViewModel(
                orderService,
                orderDetailService,
                customerService,
                employeeService,
                productService,
                categoryService);
        }
    }
}