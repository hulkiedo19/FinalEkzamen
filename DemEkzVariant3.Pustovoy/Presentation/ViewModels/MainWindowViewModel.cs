using DemEkzVariant3.Pustovoy.Domain.Entities;
using DemEkzVariant3.Pustovoy.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace DemEkzVariant3.Pustovoy.Presentation.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private List<Product> _products = new List<Product>();
        private List<Product> _page = new List<Product>();
        private List<string> _comboBoxSort = new List<string>();
        private List<string> _comboBoxFilter = new List<string>();
        private List<Button> _buttonList = new List<Button>();
        private int _selectedIndex;
        private int _currentPage = 0;
        private int _pageItemCount = 20;
        private int _pageMax = 1;

        public List<Product> Products
        {
            get => _page;
            set => Set(ref _page, value, nameof(Products));
        }
        public List<string> ComboBoxSort
        {
            get => _comboBoxSort;
            set => Set(ref _comboBoxSort, value, nameof(ComboBoxSort));
        }
        public List<string> ComboBoxFilter
        {
            get => _comboBoxFilter;
            set => Set(ref _comboBoxFilter, value, nameof(ComboBoxFilter));
        }
        public List<Button> ButtonList
        {
            get => _buttonList;
            set => Set(ref _buttonList, value, nameof(ButtonList));
        }
        public int SelectedIndex
        {
            get => _selectedIndex;
            set => Set(ref _selectedIndex, value, nameof(SelectedIndex));
        }

        public MainWindowViewModel()
        {
            GetProducts(null);
            InitializePages();
            InitializeComboBoxes();
        }

        private void GetProducts(string? text)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                if(text == null)
                {
                    _products = context.Products
                        .Include(pm => pm.ProductMaterials)
                        .ThenInclude(m => m.Material)
                        .Include(p => p.ProductType)
                        .ToList();
                }
                else
                {
                    _products = context.Products
                        .Include(pm => pm.ProductMaterials)
                        .ThenInclude(m => m.Material)
                        .Include(p => p.ProductType)
                        .Where(p => p.Title.ToLower().Contains(text.ToLower()))
                        .ToList();
                }
            }
        }

        private void InitializeComboBoxes()
        {
            ComboBoxSort.Add("Название >");
            ComboBoxSort.Add("Название <");
            ComboBoxSort.Add("Номер цеха >");
            ComboBoxSort.Add("Номер цеха <");
            ComboBoxSort.Add("Мин стоимость >");
            ComboBoxSort.Add("Мин стоимость <");

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                List<ProductType> productTypes = context.ProductTypes.ToList();

                foreach (var productType in productTypes)
                    ComboBoxFilter.Add(productType.Title);
            }
        }

        // pages
        private void InitializePages()
        {
            _currentPage = 0;
            _pageMax = (_products.Count / _pageItemCount) + ((_products.Count % _pageItemCount) > 0 ? 1 : 0);
            SetPage();
            SetButtons();
        }

        private void SetPage()
        {
            Products = _products
                .Skip(_currentPage * _pageItemCount)
                .Take(_pageItemCount)
                .ToList();
        }

        private void SetButtons()
        {
            List<Button> buttons = new List<Button>();

            Button leftPage = new Button();
            leftPage.Content = "<";
            leftPage.Margin = new System.Windows.Thickness(0, 0, 2, 0);
            leftPage.Background = new SolidColorBrush(Colors.White);
            leftPage.BorderBrush = new SolidColorBrush(Colors.White);
            leftPage.BorderThickness = new System.Windows.Thickness(0);
            leftPage.Width = 15;
            leftPage.Click += LeftPage_Click;
            buttons.Add(leftPage);

            for(int i = 0; i < _pageMax; i++)
            {
                Button specifiedPage = new Button();
                specifiedPage.Content = $"{i + 1}";
                specifiedPage.Margin = new System.Windows.Thickness(0, 0, 2, 0);
                specifiedPage.Background = new SolidColorBrush(Colors.White);
                specifiedPage.BorderBrush = new SolidColorBrush(Colors.White);
                specifiedPage.BorderThickness = new System.Windows.Thickness(0);
                specifiedPage.Width = 15;
                specifiedPage.Click += SpecifiedPage_Click;
                buttons.Add(specifiedPage);
            }

            Button rightPage = new Button();
            rightPage.Content = ">";
            rightPage.Margin = new System.Windows.Thickness(0, 0, 2, 0);
            rightPage.Background = new SolidColorBrush(Colors.White);
            rightPage.BorderBrush = new SolidColorBrush(Colors.White);
            rightPage.BorderThickness = new System.Windows.Thickness(0);
            rightPage.Width = 15;
            rightPage.Click += RightPage_Click;
            buttons.Add(rightPage);

            ButtonList = buttons;
        }

        private void SpecifiedPage_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            int index = Convert.ToInt32((sender as Button).Content);

            _currentPage = index - 1;
            SetPage();
        }

        private void RightPage_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_currentPage >= _pageMax - 1)
                return;

            _currentPage++;
            SetPage();
        }

        private void LeftPage_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_currentPage <= 0)
                return;

            _currentPage--;
            SetPage();
        }

        // called methods
        public void Search(string text)
        {
            if (text == "")
                GetProducts(null);
            else
                GetProducts(text);

            InitializePages();
        }

        public void Sort(int index)
        {
            switch(index)
            {
                case 0:
                    _products = _products.OrderBy(p => p.Title).ToList();
                    break;
                case 1:
                    _products = _products.OrderByDescending(p => p.Title).ToList();
                    break;
                case 2:
                    _products = _products.OrderBy(p => p.ProductionWorkshopNumber).ToList();
                    break;
                case 3:
                    _products = _products.OrderByDescending(p => p.ProductionWorkshopNumber).ToList();
                    break;
                case 4:
                    _products = _products.OrderBy(p => p.MinCostForAgent).ToList();
                    break;
                case 5:
                    _products = _products.OrderByDescending(p => p.MinCostForAgent).ToList();
                    break;
            }

            InitializePages();
        }

        public void Filter(List<string> Types, int index)
        {
            GetProducts(null);
            _products = _products.Where(p => p.ProductType.Title == Types[index]).ToList();
            InitializePages();
        }

        public void ShowProductWindow()
        {
            Product product = Products.ElementAt(_selectedIndex);
            ProductWindow window = new ProductWindow(product);

            if (window.ShowDialog() == false)
                return;

            GetProducts(null);
            InitializePages();
        }
    }
}
