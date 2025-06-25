using HuynhAnKhuongWPF.ViewModels;
using Services.IServices;
using System.Windows.Controls;

namespace HuynhAnKhuongWPF.Views
{
    public partial class ProductManagementView : UserControl
    {
        public ProductManagementView(IProductService productService, ICategoryService categoryService)
        {
            InitializeComponent();
            DataContext = new ProductManagementViewModel(productService, categoryService);
        }
    }
}