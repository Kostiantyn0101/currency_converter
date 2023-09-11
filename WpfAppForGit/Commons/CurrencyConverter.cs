using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WpfAppForGit.Tools;

namespace WpfAppForGit.Commons
{
    internal class CurrencyConverter : NotifyPropertyChangedBase
    {
        private string _href = "https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?date=";
        private string _fullHref = String.Empty;
        private DateTime _date;
        public DateTime Date
        {
            get { return _date; }
            set { 
                _date = value; 
                LoadCurrency();
                OnPropertyChanged(nameof(Date));
            }  
        }
        public string BankHrefApi
        {
            get 
            {
                return _fullHref;    
            }
        }
        private ObservableCollection<Currency> _currencies = new ObservableCollection<Currency>();
        public ObservableCollection<Currency> Currencies
        {
            get
            {
                return _currencies;
            }
            set
            { 
                _currencies = value;
            }
        }
        public CurrencyConverter()
        { 
            Date = DateTime.Now;
        }

        private void LoadCurrency()
        {
            _fullHref = _href + _date.ToString("yyyyMMdd") + "&json";
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;
            string currenciesJsonPesponse = client.DownloadString(_fullHref);
            Currencies = JsonConvert.DeserializeObject<ObservableCollection<Currency>>(currenciesJsonPesponse);
            OnPropertyChanged(nameof(Currencies));
        }
        public event Action SelectedDateChanged;

        private void OnSelectedDateChanged()
        {
            SelectedDateChanged?.Invoke();
        }
        private void Calendar_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadCurrency();
        }
    }
}
