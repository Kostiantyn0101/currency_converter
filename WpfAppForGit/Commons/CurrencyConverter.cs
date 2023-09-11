using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppForGit.Commons
{
    internal class CurrencyConverter
    {
        private string _href = "https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?date=";
        private string _fullHref = String.Empty;
        private DateTime _date;
        public DateTime Date
        {
            get { return _date; }
            set { 
                _date = value; 
                _fullHref = _href + _date.ToString("yyyyMMdd") + "&json";
                LoadCurrency();
            }  
        }
        public string BankHrefApi
        {
            get 
            {
                return _fullHref;    
            }
        }
        public ObservableCollection<Currency> Currencies { get; private set; }
        public CurrencyConverter()
        { 
            Date = DateTime.Now;
            Currencies = new ObservableCollection<Currency>();
        }

        private void LoadCurrency()
        { 
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;
            string currenciesJsonPesponse = client.DownloadString(_fullHref);
            Currencies = JsonConvert.DeserializeObject<ObservableCollection<Currency>>(currenciesJsonPesponse);
        }
    }
}
