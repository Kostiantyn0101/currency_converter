using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
            get => _date;
            set
            {
                _date = value;
                LoadCurrency();
                OnPropertyChanged(nameof(Date));
            }
        }
        public string BankHrefApi
        {
            get => _fullHref;
        }
        private ObservableCollection<Currency> _currencies;
        public ObservableCollection<Currency> Currencies
        {
            get => _currencies;
            set
            {
                if (_currencies != null)
                {
                    _currencies = value;
                    OnPropertyChanged(nameof(Currencies));
                }
            }
        }
        private decimal sumFrom;
        public decimal SumFrom
        {
            get => sumFrom;
            set
            {
                if (sumFrom != value)
                {
                    sumFrom = value;
                    OnPropertyChanged(nameof(SumFrom));
                }
            }
        }
        private decimal sumTo;
        public decimal SumTo
        {
            get => sumTo;
            set
            {
                if (sumTo != value)
                {
                    sumTo = value;
                    OnPropertyChanged(nameof(SumTo));
                }
            }
        }

        private Currency selectedCurrencyFrom;
        public Currency SelectedCurrencyFrom
        { 
            get => selectedCurrencyFrom;
            set 
            {
                if (selectedCurrencyFrom != value)
                { 
                    selectedCurrencyFrom = value;
                    OnPropertyChanged(nameof(SelectedCurrencyFrom));
                }
            }
        }
        private Currency selectedCurrencyTo;
        public Currency SelectedCurrencyTo
        {
            get => selectedCurrencyTo;
            set
            {
                if (selectedCurrencyTo != value)
                {
                    selectedCurrencyTo = value;
                    OnPropertyChanged(nameof(SelectedCurrencyTo));
                }
            }
        }
        public CurrencyConverter()
        {
            _currencies = new ObservableCollection<Currency>();
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
        public ICommand Convert =>
            new RelayCommand(x =>
            {
                if (selectedCurrencyFrom != null && 
                selectedCurrencyTo != null && 
                string.IsNullOrEmpty(SumFrom.ToString())!=true)
                {
                    if (decimal.TryParse(SumFrom.ToString(), out decimal sumFrom))
                    {
                        decimal _convertedSum = sumFrom * (selectedCurrencyTo.rate / selectedCurrencyFrom.rate);
                        SumTo = _convertedSum;
                    }
                }
                else
                {
                    MessageBox.Show("Please select both currencies.");
                }
            });

        private void Calendar_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadCurrency();
        }
    }
}
