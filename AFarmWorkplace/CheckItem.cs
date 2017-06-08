using AFarmWorkplace.WCFServer;
using System.ComponentModel;

namespace AFarmWorkplace
{
    public class CheckItem:INotifyPropertyChanged
    {
        private int count;
        public Product Product { get; set; }
        public int BuyCount
        {
            get
            {
                return count;
            }
            set
            {
                count = value;
                OnPropertyChanged("BuyCount");
                OnPropertyChanged("Sum");
            }
         }

        public float Sum {
            get
            {
                return (float)BuyCount * Product.ProductPrice;
            }
            
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
