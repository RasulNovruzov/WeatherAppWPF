using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Weather.Model
{
    public class Forecast
    {
        public ObservableCollection<ListOfOB> List { get; set; }

        public class ListOfOB
        {
            public List<WeatherInfo.WeatherOfModel> Weather { get; set; }
            public WeatherInfo.BasicMain main { get; set; }
            public WeatherInfo.WindWithParameters wind { get; set; }
            public string dt_txt { get; set; }
        }
    }
}
