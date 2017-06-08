using System.Windows;
using System.Collections.Generic;
using AFarmWorkplace.WCFServer;
using System;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Linq;

namespace AFarmWorkplace
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public ObservableCollection<Product> SearchList = new ObservableCollection<Product>();
        public ObservableCollection<CheckItem> CheckList = new ObservableCollection<CheckItem>();

        public MainWindow()
        {
            InitializeComponent();
            StatusLabel.Content = "Ожидание";
            dataGrid.ItemsSource = SearchList;
            CheckGrid.ItemsSource = CheckList;
            BdQuery();
        }

        private void BdQuery()
        {
            try
            {
                using (var server = new ContractClient())
                {
                    server.TestServer("Старт работы");
                    StatusLabel.Content = "Соединен";
                    DoSearch(server,"");
                }
            }
            catch
            {
                StatusLabel.Content = "Ошибка.";
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var server = new ContractClient())
                {
                    DoSearch(server, SearchTextBox.Text);
                }
            }
            catch
            {
                StatusLabel.Content = "Ошибка выполнения поиска.";
            }
        }

        private void DoSearch(ContractClient server, string searchText)
        {
            SearchList.Clear();
            var tempList = server.FindProducts(searchText);
            foreach (var item in tempList) SearchList.Add(item);
        }

        private void ChkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var server = new ContractClient())
                {
                    var processCheck = new List<Product>();
                    foreach (var item in CheckList)
                    {
                        Product tempProduct = item.Product;
                        tempProduct.Quantity = item.BuyCount;
                        processCheck.Add(tempProduct);
                    }
                    CheckList.Clear();
                    SumCounter.Content = 0;
                    if (server.ProcessCheck(processCheck).Count!=0) throw new Exception();
                    DoSearch(server, SearchTextBox.Text);
                }
            }
            catch
            {
                StatusLabel.Content = "Ошибка продажи";
            }
        }

        private void Button_Add_Click(object sender, RoutedEventArgs e)
        {
            Product clickedProduct = ((FrameworkElement)sender).DataContext as Product;
            foreach (var item in SearchList)
                if (clickedProduct.ProductID == item.ProductID)
                {
                    if (item.Quantity > 0)
                    {
                        item.Quantity--;
                        Add2CheckList(clickedProduct);
                        CalculateSum();
                    }
                }
        }

        private void Add2CheckList(Product product)
        {
            foreach (var item in CheckList)
                if (product.ProductID == item.Product.ProductID) // Данная запись уже имеется, надо изменить количество.
                {
                    item.BuyCount++;
                    return;
                }
            CheckList.Add(new CheckItem() { Product = product, BuyCount = 1 });
        }

        private void Button_RM_Click(object sender, RoutedEventArgs e)
        {
            var clickedProduct = ((FrameworkElement)sender).DataContext as CheckItem;
            foreach (var item in CheckList)
                if (clickedProduct.Product.ProductID == item.Product.ProductID)
                {
                    if (item.BuyCount > 1)
                    {
                        Substract(clickedProduct, item);
                    }
                    else
                    {
                        Substract(clickedProduct, item);
                        CheckList.Remove(clickedProduct);
                        break;
                    }
                }

        }

        private void Substract(CheckItem clickedProduct, CheckItem item)
        {
            item.BuyCount--;
            Back2SearchList(clickedProduct.Product);
            CalculateSum();
        }

        private void Back2SearchList(Product product)
        {
            foreach (var item in SearchList)
            {
                if (item.ProductID == product.ProductID)
                {
                    item.Quantity++;
                }
            }
        }
        /// <summary>
        /// Подсчитывает и обновляет сумму чека.
        /// </summary>
        /// <returns>
        /// На всякий случай возвращает полученную сумму. В настоящее время не использую.
        /// </returns>
        private float CalculateSum()
        {
            float result = 0.0f;
            foreach (var item in CheckList) result += item.Sum;
            SumCounter.Content = result;
            return result;
        }
    }

}

