using DemEkzVariant3.Pustovoy.Presentation.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DemEkzVariant3.Pustovoy.Presentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = (MainWindowViewModel)DataContext;
        }

        private void ComboBoxSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_viewModel != null)
                _viewModel.Sort(ComboBoxSort.SelectedIndex);
        }

        private void ComboBoxFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_viewModel != null)
                _viewModel.Filter(ComboBoxFilter.ItemsSource as List<string>, ComboBoxFilter.SelectedIndex);
        }

        private void TextBoxInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_viewModel != null)
                _viewModel.Search(TextBoxInput.Text);
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_viewModel != null)
                _viewModel.ShowProductWindow();
        }
    }
}
