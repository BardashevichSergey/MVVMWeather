using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace MVVMWeather
{
    

    class Node: INotifyPropertyChanged
    {
        string name;
        int id;
        ObservableCollection<Node> cities;

        public ObservableCollection<Node> Cities
        { get { return cities; }
        set { cities = value; } }

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
        public string Weather
        {
            get { return getWeather(); }
            set { }
        }
        private string getWeather()
        {
            return "weather"+name;
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
        private ObservableCollection<Node> tree;
        private Node selectedCity;
        
        public MainModel()
        {
            tree = new ObservableCollection<Node>();
            tree = LoadCities();
        }

        public ObservableCollection<Node> Tree
        {
            get { return tree; }
            set { tree = value; }
        }

        public Node SelectedCity
        {
            get { return selectedCity; }
            set { selectedCity = value;
                GetWeather();
            }
        }

        private ObservableCollection<Node> LoadCities()
        {
            ObservableCollection<Node> list = new ObservableCollection<Node>();
            String URLString = "https://pogoda.yandex.ru/static/cities.xml";
            XDocument docx = XDocument.Load(URLString);
            foreach (XElement el in docx.Root.Elements())
            {
                string name = el.Name.ToString();
                if (el.Name.ToString() == "country")
                    list.Add(new Node { Name = el.Attribute("name").Value, Cities = GetCountrySities(el) });
                string nameValue = el.Attribute("name").Value;
            }
            return list;
        }
        private ObservableCollection<Node> GetCountrySities(XElement root)
        {
            ObservableCollection<Node> c = new ObservableCollection<Node>();
            foreach (XElement el in root.Elements())
            {
                c.Add(new Node { Name = el.Value, Id = Convert.ToInt32(el.Attribute("region").Value) });
            }
            return c;
        }

        private string GetWeather()
        {
            if(selectedCity.Id != 0)
            {
                string weatherdesc ="";
                
                String URLString = "https://export.yandex.ru/bar/reginfo.xml?region="+selectedCity.Id.ToString() + ".xml";
                XDocument docx = XDocument.Load(URLString);

                string n = docx.Root.Elements().Descendants("sun_rise").SingleOrDefault().Value.ToString();
                
                weatherdesc += "Восход" + n;
                    return null;
            }
            return null;

        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }

}
