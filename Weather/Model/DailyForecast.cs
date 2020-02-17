using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Weather.Model
{
    public class DailyForecast : INotifyPropertyChanged //Ежедневный прогноз
    {
        public string Description { get; set; } //Описание
        public int TempMax { get; set; }
        public int TempMin { get; set; }
        public int humidity { get; set; }
        public int wind { get; set; } //Ветер
        public int deg { get; set; }
        public string icon { get; set; } //Иконка для погоды (сейчас) 
        public string Day { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<ForecastForHour> hourlies = new ObservableCollection<ForecastForHour>();
        public ObservableCollection<ForecastForHour> Hourlies
        {
            get { return hourlies; }
            set { hourlies = value;
                OnPropertyChanged(); }
        }

        private void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
