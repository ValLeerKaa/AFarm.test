using System.ServiceModel;
using CommonLib;
using System.Collections.Generic;


namespace WcfServer
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "IService1" в коде и файле конфигурации.
    [ServiceContract]
    public interface IContract
    {

        [OperationContract]
        void TestServer(string message);

        [OperationContract]
        void AddProducts(List<Product> productList);

        [OperationContract]
        List<Product> FindProducts(string substring);

        [OperationContract]
        List<Product> ProcessCheck(List<Product> check);
    }


    
}
