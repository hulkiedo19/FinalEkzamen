using DemEkzVariant3.Pustovoy.Domain.Entities;
using DemEkzVariant3.Pustovoy.Infrastructure.Persistence;
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
    /// Interaction logic for ProductWindow.xaml
    /// </summary>
    public partial class ProductWindow : Window
    {
        private bool IsChanged = false;
        private Product Item;

        public ProductWindow(Product item)
        {
            InitializeComponent();

            Item = item;
            InitializeWindow();
        }

        // sets
        private void InitializeWindow()
        {
            // initialize textboxes
            TB_Title.Text = Item.Title;
            TB_Article.Text = Item.ArticleNumber;
            TB_Description.Text = Item.Description;
            TB_Image.Text = Item.Image;
            TB_PersonCount.Text = Convert.ToString(Item.ProductionPersonCount);
            TB_WorkshopNumber.Text = Convert.ToString(Item.ProductionWorkshopNumber);
            TB_MinCount.Text = Convert.ToString(Item.MinCostForAgent);

            // initialize materials listbox
            List<Material> materials = new ApplicationDbContext().Materials.ToList();

            foreach (var material in materials)
                LB_Materials.Items.Add(material.Title);

            // initialize product types comboBox
            List<ProductType> productTypes = new ApplicationDbContext().ProductTypes.ToList();
            ProductType selectedProductType = new ApplicationDbContext().ProductTypes.Where(p => p.Id == Item.ProductTypeId).First();
            int selectedIndex = productTypes.FindIndex(p => p.Id == selectedProductType.Id);

            foreach (var productType in productTypes)
                CB_ProductType.Items.Add(productType.Title);

            CB_ProductType.SelectedIndex = selectedIndex;
        }

        // gets
        private Product GetProduct()
        {
            Product product = new Product();

            var productTypeValue = CB_ProductType.Items[CB_ProductType.SelectedIndex].ToString();
            product.ProductTypeId = new ApplicationDbContext().ProductTypes.Where(p => p.Title.Equals(productTypeValue)).First().Id;

            product.Title = TB_Title.Text;
            product.ArticleNumber = TB_Article.Text;
            product.Description = TB_Description.Text;
            product.Image = TB_Image.Text;
            product.ProductionPersonCount = Convert.ToInt32(TB_PersonCount.Text);
            product.ProductionWorkshopNumber = Convert.ToInt32(TB_WorkshopNumber.Text);
            product.MinCostForAgent = Convert.ToDecimal(TB_MinCount.Text);

            return product;
        }

        private void AddProduct(Product product)
        {
            using(ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Products.Add(product);
                context.SaveChanges();
            }
        }

        private void EditProduct(Product product)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                Product item = context.Products
                    .Where(p => p.Id == Item.Id)
                    .First();

                item.Title = product.Title;
                item.ProductTypeId = product.ProductTypeId;
                item.ArticleNumber = product.ArticleNumber;
                item.MinCostForAgent = product.MinCostForAgent;
                item.Description = product.Description;
                item.ProductionPersonCount = product.ProductionPersonCount;
                item.ProductionWorkshopNumber = product.ProductionWorkshopNumber;
                item.Image = product.Image;

                context.SaveChanges();
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = IsChanged;
            this.Close();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddProduct(GetProduct());
            IsChanged = true;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            EditProduct(GetProduct());
            IsChanged = true;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            using(ApplicationDbContext context = new ApplicationDbContext())
            {
                var productMaterials = context.ProductMaterials
                    .Where(p => p.ProductId == Item.Id)
                    .ToList();

                if(productMaterials.Count > 0)
                {
                    foreach (var pm in productMaterials)
                        context.ProductMaterials.Remove(pm);

                    context.SaveChanges();
                }

                context.Products.Remove(Item);
                context.SaveChanges();
            }
            IsChanged = true;
        }
    }
}
