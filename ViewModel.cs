using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MVVMWeather
{
    public class RelayCommand : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }
    }

    class ViewModel : INotifyPropertyChanged
    {
        MainModel m;
        private Node selectedCity;
        public ObservableCollection<Node> CityList { get; set; }

        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new RelayCommand(obj =>
                  {
                      m.UpdateWeather();
                      m.Search("Брянск");
                      selectedCity = m.SelectedCity;
                      OnPropertyChanged("SelectedCity");

                  }));
            }
        }
        public Weather Weather
        {
            get { return m.Weather; }
        }
        public Node SelectedCity
        {
            get { return selectedCity; }
            set
            {
                //if (value.Id != 0)
                {
                    m.SelectedCity = value;
                    selectedCity = m.SelectedCity;
                    OnPropertyChanged("SelectedCity");
                    OnPropertyChanged("WeatherNow");
                    
                }
            }
        }
        public Node IsSelected
        {
            get { return selectedCity; }
            set
            {
                selectedCity = value;
                OnPropertyChanged("IsSelected");
            }
        }

        public ViewModel()
        {
            m = new MainModel();
            CityList = m.Tree;
            m.PropertyChanged += M_PropertyChanged;
        }

        private void M_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Weather")
                OnPropertyChanged("Weather");
            else if (e.PropertyName == "SelectedCity")
                OnPropertyChanged("SelectedCity");
            //throw new NotImplementedException();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
