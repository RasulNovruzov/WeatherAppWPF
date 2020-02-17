using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Weather.Model
{
    public class WeatherInfo : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public int visibility { get; set; }
        public InformationAboutCountry Sys { get; set; }
        public List<WeatherOfModel> Weather { get; set; }
        public string icon { get; set; }
        public string Description { get; set; }
        public int DewPoint { get; set; }
        public int FeelsLike { get; set; }
        public string Sunrise { get; set; }
        public string Sunset { get; set; }
        public string Moonrise { get; set; }
        public string Moonset { get; set; }
        public double dt { get; set; }
        public WindWithParameters Wind { get; set; }
        public BasicMain Main { get; set; }
        public string BackgoundImagePath { get; set; }

        public class WeatherOfModel
        {
            public string main { get; set; }
            public string description { get; set; }
            public string icon { get; set; }
        }

        public class InformationAboutCountry
        {
            public int sunrise { get; set; }
            public int sunset { get; set; }
        }

        public class BasicMain
        {
            public double temp { get; set; }
            public double TempMax { get; set; }
            public double TempMin { get; set; }
            public double pressure { get; set; }
            public double humidity { get; set; }
        }

        public class WindWithParameters
        {
            public double speed { get; set; }
            public double deg { get; set; }
        }             
        
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
