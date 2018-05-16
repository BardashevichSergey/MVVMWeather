using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MVVMWeather
{
    class City: INotifyPropertyChanged
    {
        string name;
        int id;
        public string Name { get { return name; }
        set { name = value;
                OnPropertyChanged("Name");} }

        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
    class MainModel: INotifyPropertyChanged
    {
        public MainModel()
        {
            CityList = new ObservableCollection<City>
            {
                new City { Name="Брянск", Id = 1},
                new City {Name="Москва", Id = 11 },
                new City {Name="Казань", Id = 21 },
                new City {Name="Курск", Id = 10 }
            };
        }
        private City selectedCity;
        public City SelectedCity
        {
            get { return selectedCity; }
            set { selectedCity = value;
                OnPropertyChanged("SelectedCity");}
        }

        public ObservableCollection<City> CityList { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }

}
