using System;
using System.Collections.Generic;
using CommonLib;
using System.Linq;

namespace WcfServer
{
    public class WcfServer : IContract
    {
        /// <summary>
        /// Метод для раннего тестирования... Ну, еще можно им всякие разности в консоли сервера писать при отладке. 
        /// </summary>
        /// <param name="message">
        /// Данное сообщение отобразится в консоли.
        /// </param>
        public void TestServer(string message)
        {
            Console.WriteLine($"Cообщение: {message}");
        }

        /// <summary>
        /// Добавляет список товаров к БД. 
        /// </summary>
        /// <param name="productList">
        /// Список товаров для добавления.
        /// </param>
        public void AddProducts(List<Product> productList)
        {
            Console.WriteLine("Добавление списка товаров:");
            try
            {
                using (DBClass db = new DBClass())
                {
                    foreach (var product in productList)
                    {
                        var found = false;
                        foreach (var DBItem in db.Products)
                            if (product.ProductID == DBItem.ProductID)
                            {
                                Console.WriteLine($"Существующий {product.ProductName} добавлен с ID={product.ProductID} в количестве {product.Quantity} (цена {product.ProductPrice}).");
                                DBItem.Quantity += product.Quantity;
                                found = true;
                            }
                        if(!found)
                            {
                                db.Products.Add(product);
                                Console.WriteLine($"Новый {product.ProductName} добавлен с ID={product.ProductID} в количестве {product.Quantity} (цена {product.ProductPrice}).");
                            }
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception exc)
            {
                string excMessage = $"Ошибка добавления продукта: {exc.InnerException.InnerException.Message}";
                Exception verboseExc = new Exception(excMessage);
                Console.WriteLine(excMessage);
                throw verboseExc;
            }
            

        }

        /// <summary>
        /// Ищет продукты по подстроке в названии.
        /// </summary>
        /// <param name="substring">
        /// Подстрока поиска продукта.
        /// </param>
        /// <returns>
        /// Список продуктов, удовлетворяющих подстроке поиска.
        /// </returns>
        public List<Product> FindProducts(string substring)
        {
            
            Console.WriteLine("Запрос поиска по строке: " + ((substring != string.Empty) ? substring : "Пустая строка"));
            try
            {
                using (DBClass db = new DBClass())
                {
                    return (substring != "") ?
                        (from item in db.Products where item.ProductName.ToLower().Contains(substring.ToLower()) select item).ToList()
                        :
                        (from item in db.Products select item).ToList();
                        ;
                    //foreach (var item in db.Products)
                    //{
                    //    if (substring == string.Empty) result.Add(item);
                    //    if (item.ProductName.ToLower().Contains(substring.ToLower())) result.Add(item);
                    //}
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine($"Ошибка поиска товара: {exc.Message}");
                throw new Exception(exc.Message);
            }
        }

        /// <summary>
        /// Обрабатывает полученный чек, состоящий из списка товаров и количества проданных в этом списке.
        /// </summary>
        /// <param name="check">
        /// Список товаров к продаже.
        /// </param>
        /// <returns>
        /// Возвращает список неудачных продаж, т.е. товаров, число которых меньше того, что передано в чеке. 
        /// Ну, к примеру, если с другого терминала уже прошла продажа последних.
        /// </returns>
        public List<Product> ProcessCheck(List<Product> check)
        {
            Console.WriteLine("Запрос обработки чека");
            List<Product> result = new List<Product>();
            try
            {
                using (DBClass db = new DBClass())
                {
                    foreach (var itemInCheck in check)
                        foreach (var dbitem in db.Products)
                            if (itemInCheck.ProductID == dbitem.ProductID)
                                if (dbitem.Quantity >= itemInCheck.Quantity) dbitem.Quantity -= itemInCheck.Quantity;
                                else
                                {
                                    result.Add(itemInCheck);
                                }
                    db.SaveChanges();
                    return result;
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine($"Ошибка обработки чека: {exc.Message}");
                throw new Exception(exc.Message);
            }
        }

    }
}
