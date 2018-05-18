using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
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
        string href;
        ObservableCollection<Node> cities;
        

        public ObservableCollection<Node> Cities
        { get { return cities; }
        set { cities = value; } }

        public string Name { get { return name; }
        set { name = value;
                OnPropertyChanged("Name");} }

        public string Href
        {
            get { return href; }
            set
            {
                href = value;
                OnPropertyChanged("Name");
            }
        }

        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }
        
        private string getWeather()
        {
            return "weather"+name;
        }
        public Weather GetWeather()
        {
            Weather weather = new Weather();
            try
            {
                string weatherNow = "", weatherToday = "";

                HtmlAgilityPack.HtmlDocument html = new HtmlAgilityPack.HtmlDocument();
                string page = MainModel.GetHtmlPageText("https://yandex.ru" + Href);
                if (page != null)
                {
                    html.LoadHtml(page);
                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(page);

                    HtmlNode cityCollection = doc.DocumentNode.SelectSingleNode("//*[contains(@class,'time fact__time')]");
                    string timeNow = cityCollection.InnerText;
                    cityCollection = doc.DocumentNode.SelectSingleNode("//*[contains(@class,'temp fact__temp')]/*[contains(@class,'temp__value')]");
                    string tempNow = cityCollection.InnerText;
                    cityCollection = doc.DocumentNode.SelectSingleNode("//*[contains(@class,'fact__props')]/*[contains(@class,'wind-speed')]");
                    string windSpeed = cityCollection.InnerText;
                    cityCollection = doc.DocumentNode.SelectSingleNode("//*[contains(@class,'fact__props')]/*[contains(@class,'term term_orient_v fact__pressure')]/*[contains(@class,'term__value')]");
                    string pressure = cityCollection.InnerText;
                    cityCollection = doc.DocumentNode.SelectSingleNode("//*[contains(@class,'fact__props')]/*[contains(@class,'term term_orient_v fact__humidity')]/*[contains(@class,'term__value')]");
                    string humidity = cityCollection.InnerText;

                    HtmlNodeCollection collection = doc.DocumentNode.SelectNodes("//*[contains(@class,'sunrise-sunset__value')]");
                    string sun_rise = collection[0]?.InnerText;
                    string sunset = collection[1]?.InnerText;


                    weatherNow = timeNow + String.Format("\nТемпература: {0} \n", tempNow) +
                                    String.Format("{0}\n", windSpeed) +
                                    String.Format("Влажность: {0}\n", humidity) +
                                    String.Format("Давление: {0}\n", pressure) +
                                    String.Format("Восход: {0} Закат: {1}", sun_rise, sunset);
                }


                /*if (this.Id != 0)
                 {
                     string weatherNow = "", weatherToday = "";
                     String URLString = "https://export.yandex.ru/bar/reginfo.xml?region=" + this.Id.ToString() + ".xml";
                     XDocument docx = XDocument.Load(URLString);
                     IEnumerable<XElement> elem = docx.Root.Elements().Descendants("day_part");
                     string temp = "";
                     //if (elem.Elements().Count() > 0)
                         temp = elem.ElementAt(0).Descendants("temperature").SingleOrDefault()?.Value.ToString();
                     string windDir = docx.Root.Elements().Descendants("wind_direction").SingleOrDefault()?.Value.ToString();
                     string windSpeed = docx.Root.Elements().Descendants("wind_speed").SingleOrDefault()?.Value.ToString();
                     string dampness = docx.Root.Elements().Descendants("dampness").SingleOrDefault()?.Value.ToString();
                     string pressure = docx.Root.Elements().Descendants("torr").SingleOrDefault()?.Value.ToString();
                     string sun_rise = docx.Root.Elements().Descendants("sun_rise").SingleOrDefault()?.Value.ToString();
                     string sunset = docx.Root.Elements().Descendants("sunset").SingleOrDefault()?.Value.ToString();

                     weatherNow = String.Format("Температура: {0} \n", temp) +
                                     String.Format("Ветер: {0}, {1} м/сек\n", windDir, windSpeed) +
                                     String.Format("Влажность: {0}%\n", dampness) +
                                     String.Format("Давление: {0} мм рт.ст.\n", pressure) +
                                     String.Format("Восход: {0} Закат: {1}", sun_rise, sunset);

                     IEnumerable<XElement> elemtoday = docx.Root.Elements().Descendants("day_part");
                     for (int i = 1; i < elemtoday.Count(); i++)
                     {
                         XElement daypart = elemtoday.ElementAt(i);

                         string name = daypart.Attribute("type").Value.ToUpper();
                         string tempdaypart = daypart.Descendants("temperature").SingleOrDefault()?.Value.ToString();
                         if (tempdaypart == null)
                         {
                             tempdaypart = daypart.Descendants("temperature_from").SingleOrDefault()?.Value.ToString() + daypart.Descendants("temperature_to").SingleOrDefault()?.Value.ToString();
                         }
                         weatherToday += String.Format("{0}: {1}\n", name, tempdaypart);
                     }

                     weatherNow = String.Format("Температура: {0} \n", temp) +
                                     String.Format("Ветер: {0}, {1} м/сек\n", windDir, windSpeed) +
                                     String.Format("Влажность: {0}%\n", dampness) +
                                     String.Format("Давление: {0} мм рт.ст.\n", pressure) +
                                     String.Format("Восход: {0} Закат: {1}", sun_rise, sunset);


                     weather.Now = weatherNow;
                     weather.Today = weatherToday;
                 }*/
                weather.Now = weatherNow;
                weather.Today = weatherToday;
            }
            catch(Exception e)
            {
                weather.Now = "Для выбранного города нет данных";
                weather.Today = "";
            }
            return weather;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
    class Weather
    {
        private string now;
        private string today;
        public string Now
        {
            get { return now; }
            set { now = value; }
        }
        public string Today
        {
            get { return today; }
            set { today = value; }
        }
    }

    class MainModel: INotifyPropertyChanged
    {
        private ObservableCollection<Node> tree;
        private Node selectedCity;
        public MainModel()
        {
            tree = new ObservableCollection<Node>();
            //tree = LoadCities();
            tree = GetCountrySities("/pogoda/region");
        }

        public Weather Weather { get { return selectedCity?.GetWeather(); } }
        public void UpdateWeather()
        {
            selectedCity?.GetWeather();
            OnPropertyChanged("Weather");
        }
        public ObservableCollection<Node> Tree
        {
            get { return tree; }
            set { tree = value; }
        }
        private Node SearchInTree(ObservableCollection<Node> root,string searchName)
        {
            foreach(Node child in root)
            {
                if (child.Name == searchName)
                    return child;
                else
                {
                    if (child.Cities != null)
                    {
                        Task<Node> task2 = new Task<Node>(() => SearchInTree(child.Cities, searchName));
                        task2.Start();
                        Node n = task2.Result;
                        if (n != null)
                            return n;
                    }
                }
            }
            return null;
        }
        public bool Search(string Name)
        {
            Node result = SearchInTree(tree, Name);
            if (result != null)
            {
                selectedCity = result;
                OnPropertyChanged("SelectedCity");
                OnPropertyChanged("Weather");
                return true;
            }
            else
                return false;
        }
        public Node SelectedCity
        {
            get { return selectedCity; }
            set { selectedCity = value;
                OnPropertyChanged("Weather");
            }
        }
        public static string GetHtmlPageText(string url)
        {
            string text = "";
            WebRequest request = WebRequest.Create(url);
            try
            {
                request.Timeout = 10000;
                WebResponse response = request.GetResponse();
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader sr = new System.IO.StreamReader(stream, Encoding.UTF8))
                        text = sr.ReadToEnd();
                }
            }
            catch (WebException exception)
            {
                Task task1 = new Task(() => System.Windows.MessageBox.Show(exception.Message, "Ошибка получения данных"));
                task1.Start();

                return null;
            }
            return text;
        }

        private ObservableCollection<Node> LoadCities()
        {
            ObservableCollection<Node> list = new ObservableCollection<Node>();

            HtmlAgilityPack.HtmlDocument html = new HtmlAgilityPack.HtmlDocument();
            string page = GetHtmlPageText("https://yandex.ru/pogoda/region");
            if (page != null)
            {
                html.LoadHtml(page);
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(page);

                HtmlNodeCollection countryCollection = doc.DocumentNode.SelectNodes("//*[contains(@class,'link place-list__item-name')]");
                foreach (HtmlNode node in countryCollection)
                {
                  
                    list.Add(new Node { Name = node.InnerText,Cities = GetCountrySities(node.Attributes["href"].Value) });
                    //node.Attributes["href"].Value;
                }
                
            }



            /*String URLString = "https://yandex.ru/pogoda/region";
            XDocument docx = XDocument.Load(URLString);
            foreach (XElement el in docx.Root.Elements())
            {
                string name = el.Name.ToString();
                if (el.Name.ToString() == "country")
                    list.Add(new Node { Name = el.Attribute("name").Value, Cities = GetCountrySities(el) });
                string nameValue = el.Attribute("name").Value;
            }*/
            return list;
        }
        private ObservableCollection<Node> GetCountrySities(string href)
        {
            ObservableCollection<Node> c = new ObservableCollection<Node>();
            HtmlAgilityPack.HtmlDocument html = new HtmlAgilityPack.HtmlDocument();
            string page = GetHtmlPageText("https://yandex.ru"+href);
            if (page != null)
            {
                html.LoadHtml(page);
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(page);

                HtmlNodeCollection cityCollection = doc.DocumentNode.SelectNodes("//*[contains(@class,'link place-list__item-name')]");
                if (cityCollection == null)
                    return null;
                //foreach (HtmlNode node in cityCollection)
                for (int i = 0; i < cityCollection.Count; i++)
                {
                    if (cityCollection.Count > i)
                    {
                        HtmlNode node = cityCollection[i];
                        int tmp;
                        ObservableCollection<Node> b=null;
                        if (int.TryParse(node.Attributes["href"].Value.Split('/').Last(), out tmp))
                        {
                            Task<ObservableCollection<Node>> task2 = new Task<ObservableCollection<Node>>(() => GetCountrySities(node.Attributes["href"].Value));
                            task2.Start();
                            b = task2.Result;
                        }

                        c.Add(new Node { Name = node.InnerHtml, Cities = b, Href = node.Attributes["href"].Value });
                    }
                }
            }

                /*foreach (XElement el in root.Elements())
                {
                    c.Add(new Node { Name = el.Value, Id = Convert.ToInt32(el.Attribute("region").Value) });
                }*/
                return c;
        }

        

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }

}
