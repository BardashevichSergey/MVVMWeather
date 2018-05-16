using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

using System.Collections.ObjectModel;

namespace MVVMWeather
{
    class ViewModel : INotifyPropertyChanged
    {
        private MainModel model = new MainModel();
        public ObservableCollection<City> CityList2 => model.CityList;



        private City selectedCity;
        public ObservableCollection<City> CityList { get; set; }
        public City SelectedCity
        {
            get { return selectedCity; }
            set
            {
                selectedCity = value;
                OnPropertyChanged("SelectedCity");
            }
        }

        public ViewModel()
        {
            CityList = new ObservableCollection<City>
            {
                new City { Name="Брянск", Id = 1},
                new City {Name="Москва", Id = 11 },
                new City {Name="Казань", Id = 21 },
                new City {Name="Омск", Id = 10 }
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
