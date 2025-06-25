using BusinessObjects;
using HuynhAnKhuongWPF.Commands;
using Services.IServices;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace HuynhAnKhuongWPF.ViewModels
{
    public class ProductManagementViewModel : ViewModelBase
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        
        // Collections
        private ObservableCollection<Product> _products;
        public ObservableCollection<Product> Products
        {
            get { return _products; }
            set { SetProperty(ref _products, value); }
        }

        private ObservableCollection<Category> _categories;
        public ObservableCollection<Category> Categories
        {
            get { return _categories; }
            set { SetProperty(ref _categories, value); }
        }

        // Selected product
        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get { return _selectedProduct; }
            set 
            { 
                if (SetProperty(ref _selectedProduct, value))
                {
                    // When selected product changes, update form data
                    if (_selectedProduct != null)
                    {
                        ProductId = _selectedProduct.ProductId;
                        ProductName = _selectedProduct.ProductName;
                        CategoryId = _selectedProduct.CategoryId;
                        QuantityPerUnit = _selectedProduct.QuantityPerUnit;
                        UnitPrice = _selectedProduct.UnitPrice;
                        UnitsInStock = _selectedProduct.UnitsInStock;
                        Discontinued = _selectedProduct.Discountinued;
                    }
                    
                    // Update commands that depend on selected product
                    DeleteCommand.CanExecute(null);
                    EditCommand.CanExecute(null);
                }
            }
        }

        // Form properties
        private int _productId;
        public int ProductId
        {
            get { return _productId; }
            set { SetProperty(ref _productId, value); }
        }

        private string _productName;
        public string ProductName
        {
            get { return _productName; }
            set { SetProperty(ref _productName, value); }
        }

        private int _categoryId;
        public int CategoryId
        {
            get { return _categoryId; }
            set { SetProperty(ref _categoryId, value); }
        }

        private string _quantityPerUnit;
        public string QuantityPerUnit
        {
            get { return _quantityPerUnit; }
            set { SetProperty(ref _quantityPerUnit, value); }
        }

        private decimal _unitPrice;
        public decimal UnitPrice
        {
            get { return _unitPrice; }
            set { SetProperty(ref _unitPrice, value); }
        }

        private int _unitsInStock;
        public int UnitsInStock
        {
            get { return _unitsInStock; }
            set { SetProperty(ref _unitsInStock, value); }
        }

        private bool _discontinued;
        public bool Discontinued
        {
            get { return _discontinued; }
            set { SetProperty(ref _discontinued, value); }
        }

        // Search functionality
        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set 
            { 
                SetProperty(ref _searchText, value);
            }
        }

        // Commands
        public ICommand AddCommand { get; private set; }
        public ICommand EditCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand SearchCommand { get; private set; }
        public ICommand ClearCommand { get; private set; }

        public ProductManagementViewModel(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
            
            // Initialize commands
            AddCommand = new RelayCommand(ExecuteAdd);
            EditCommand = new RelayCommand(ExecuteEdit, CanEdit);
            DeleteCommand = new RelayCommand(ExecuteDelete, CanDelete);
            SearchCommand = new RelayCommand(ExecuteSearch);
            ClearCommand = new RelayCommand(ExecuteClear);
            
            // Load data
            LoadCategories();
            LoadProducts();
        }

        private void LoadProducts()
        {
            var productList = _productService.GetAll();
            Products = new ObservableCollection<Product>(productList);
            
            // Add category name to each product
            foreach (var product in Products)
            {
                product.Category = _categoryService.GetById(product.CategoryId);
            }
        }
        
        private void LoadCategories()
        {
            var categoryList = _categoryService.GetAll();
            Categories = new ObservableCollection<Category>(categoryList);
        }

        private void ExecuteSearch(object obj)
        {
            var results = _productService.Search(_searchText);
            Products = new ObservableCollection<Product>(results);
            
            // Add category name to each product
            foreach (var product in Products)
            {
                product.Category = _categoryService.GetById(product.CategoryId);
            }
        }

        private void ExecuteClear(object obj)
        {
            SearchText = string.Empty;
            ProductId = 0;
            ProductName = string.Empty;
            CategoryId = 0;
            QuantityPerUnit = string.Empty;
            UnitPrice = 0;
            UnitsInStock = 0;
            Discontinued = false;
            SelectedProduct = null;
            LoadProducts();
        }

        private void ExecuteAdd(object obj)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(ProductName) || CategoryId <= 0)
            {
                MessageBox.Show("Please enter required fields (Product Name and Category)");
                return;
            }

            if (UnitPrice < 0)
            {
                MessageBox.Show("Price cannot be negative");
                return;
            }

            if (UnitsInStock < 0)
            {
                MessageBox.Show("Units in stock cannot be negative");
                return;
            }
            if (UnitPrice == 0)
            {
                MessageBox.Show("Units price must be number");
                return;
            }
            if (UnitsInStock ==0)
            {
                MessageBox.Show("Units in stock must be number");
                return;
            }

            // Create new product
            var newProduct = new Product
            {
                ProductId = ProductId,
                ProductName = ProductName,
                CategoryId = CategoryId,
                QuantityPerUnit = QuantityPerUnit,
                UnitPrice = UnitPrice,
                UnitsInStock = UnitsInStock,
                Discountinued = Discontinued
            };

            // Add to repository
            bool success = _productService.Add(newProduct);
            
            if (success)
            {
                MessageBox.Show("Product added successfully!");
                LoadProducts();
                ExecuteClear(null);
            }
            else
            {
                MessageBox.Show("Failed to add product. Please try again.");
            }
        }

        private void ExecuteEdit(object obj)
        {
            // Validate input
            if (SelectedProduct == null)
            {
                MessageBox.Show("Please select a product to edit");
                return;
            }

            if (string.IsNullOrWhiteSpace(ProductName) || CategoryId <= 0)
            {
                MessageBox.Show("Please enter required fields (Product Name and Category)");
                return;
            }

            if (UnitPrice < 0)
            {
                MessageBox.Show("Price cannot be negative");
                return;
            }

            if (UnitsInStock < 0)
            {
                MessageBox.Show("Units in stock cannot be negative");
                return;
            }
            if (UnitPrice == 0)
            {
                MessageBox.Show("Units price must be number");
                return;
            }
            if (UnitsInStock == 0)
            {
                MessageBox.Show("Units in stock must be number");
                return;
            }

            // Update product properties
            SelectedProduct.ProductName = ProductName;
            SelectedProduct.CategoryId = CategoryId;
            SelectedProduct.QuantityPerUnit = QuantityPerUnit;
            SelectedProduct.UnitPrice = UnitPrice;
            SelectedProduct.UnitsInStock = UnitsInStock;
            SelectedProduct.Discountinued = Discontinued;

            // Update in repository
            bool success = _productService.Update(SelectedProduct);
            
            if (success)
            {
                MessageBox.Show("Product updated successfully!");
                LoadProducts();
            }
            else
            {
                MessageBox.Show("Failed to update product. Please try again.");
            }
        }

        private bool CanEdit(object obj)
        {
            return SelectedProduct != null;
        }

        private void ExecuteDelete(object obj)
        {
            if (SelectedProduct == null)
            {
                MessageBox.Show("Please select a product to delete");
                return;
            }

            // Ask for confirmation
            MessageBoxResult result = MessageBox.Show(
                $"Are you sure you want to delete {SelectedProduct.ProductName}?", 
                "Confirm Deletion",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                bool success = _productService.Delete(SelectedProduct.ProductId);
                
                if (success)
                {
                    MessageBox.Show("Product deleted successfully!");
                    LoadProducts();
                    ExecuteClear(null);
                }
                else
                {
                    MessageBox.Show("Failed to delete product. Please try again.");
                }
            }
        }

        private bool CanDelete(object obj)
        {
            return SelectedProduct != null;
        }
    }
}