using System.ServiceModel;
using CommonLib;
using System.Collections.Generic;


namespace WcfServer
{
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
