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
        MainModel m;
        ObservableCollection<Node> nodes;
        private Node selectedCity;
        public ObservableCollection<Node> CityList { get; set; }
        public ObservableCollection<Node> Nodes2
        {
            get { return nodes; }
            set
            {
                nodes = value;
                OnPropertyChanged("Nodes2");
            }
        }
        
        public Node SelectedCity
        {
            get { return selectedCity; }
            set
            {
                m.SelectedCity =  value;
                selectedCity = m.SelectedCity;
                OnPropertyChanged("SelectedCity");
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
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
