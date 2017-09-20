using GalaSoft.MvvmLight.Command;
using MoneyChange.Helpers;
using MoneyChange.Models;
using MoneyChange.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace MoneyChange.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Services
        ApiService apiService;
        DataService dataService;
        DialogService dialogService;
        #endregion

        #region Attributes
        bool _isRunning;
        bool _isEnabled;
        string _result;
        Rate _sourceRate;
        Rate _targetRate;
        ObservableCollection<Rate> _rates;
        string _status;
        #endregion

        #region Properties
        public string Status
        {
            get
            {
                return _status;
            }

            set
            {
                if (_status != value)
                {
                    _status = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Status)));
                }
            }
        }

        public string Amount
        {
            get;
            set;
        }

        public ObservableCollection<Rate> Rates
        {
            get
            {
                return _rates;
            }

            set
            {
                if(_rates != value)
                {
                    _rates = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Rates)));
                }
            }
        }

        public Rate SourceRate
        {
            get
            {
                return _sourceRate;
            }

            set
            {
                if (_sourceRate != value)
                {
                    _sourceRate = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(SourceRate)));
                }
            }
        }

        public Rate TargetRate
        {
            get
            {
                return _targetRate;
            }

            set
            {
                if (_targetRate != value)
                {
                    _targetRate = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(TargetRate)));
                }
            }
        }

        public bool IsRunning
        {
            get
            {
                return _isRunning;
            }
            set
            {
                if (_isRunning != value)
                {
                    _isRunning = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(IsRunning)));
                }
            }
        }

        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(IsEnabled)));
                }
            }
        }

        public string Result
        {
            get
            {
                return _result;
            }
            set
            {
                if (_result != value)
                {
                    _result = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Result)));
                }
            }
        }
        #endregion

        #region Contructor
        public MainViewModel()
        {
            apiService = new ApiService();
            dataService = new DataService();
            dialogService = new DialogService();
            LoadRates();
        }
        #endregion

        #region Methods
        async void LoadRates()
        {
            IsRunning = true;
            Result = Lenguages.Loading;

            var connection = await apiService.CheckConnection();
            if(!connection.IsSucces)
            {
                IsRunning = false;
                Result = connection.Message;
                return;
            }

            //URL del servicio
            var url = Application.Current.Resources["URLAPI"].ToString();

            var response = await apiService.GetList<Rate>(
                "https://apiexchangerates.azurewebsites.net",
                "api/Rates");

            if(!response.IsSucces)
            {
                IsRunning = false;
                Result = response.Message;
            }
            
            //Storage data
            var rates = (List<Rate>)response.Result;
            dataService.DeleteAll<Rate>();
            dataService.Save(rates);

            Rates = new ObservableCollection<Rate>(rates);
            IsRunning = false;
            IsEnabled = true;
            Result = Lenguages.Ready;
            Status = Lenguages.Status;
        }
        #endregion

        #region Commands
        public ICommand SwitchCommand
        {
            get
            {
                return new RelayCommand(Switch);
            }
        }

        void Switch()
        {
            var aux = SourceRate;
            SourceRate = TargetRate;
            TargetRate = aux;
        }

        public ICommand ConvertCommand
        {
            get
            {
                return new RelayCommand(Convert);
            }
        }

        async void Convert()
        {
            if (string.IsNullOrEmpty(Amount))
            {
                await dialogService.ShowMessage(
                    Lenguages.Error,
                    Lenguages.AmountValidation);
                return;


                    
            }

            decimal amount = 0;

            if (!decimal.TryParse(Amount, out amount))
            {
                await dialogService.ShowMessage(
                    Lenguages.Error,
                    Lenguages.AmountNumericValidation);
                return;
            }

            if (SourceRate == null)
            {
                await dialogService.ShowMessage(
                    Lenguages.Error,
                    Lenguages.SourceRateValidation);
                return;
            }

            if (TargetRate == null)
            {
                await dialogService.ShowMessage(
                    Lenguages.Error,
                    Lenguages.TargetRateValidation);
                return;
            }

            var amountConverter = amount / (decimal)SourceRate.TaxRate * (decimal)TargetRate.TaxRate;

            Result = string.Format("{0} {1:C2} = {2} {3:C2}", SourceRate.Code, amount, TargetRate.Code, amountConverter);
        }
        #endregion


    }
}
