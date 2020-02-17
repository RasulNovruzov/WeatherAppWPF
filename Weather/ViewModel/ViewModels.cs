using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Weather.Command;
using Weather.Model;
using Weather.Repository;

namespace Weather.ViewModel
{
    public class ViewModels : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string CityName { get; set; }
        public string citytb { get; set; }
        
        private string lastupdatetime = DateTime.Now.ToShortTimeString();

        private bool isCelsius = true;

        private IBase Base = new Base();
        private ObservableCollection<DailyForecast> forecasts = new ObservableCollection<DailyForecast>();
        public WeatherInfo nowForecast = new WeatherInfo();

        public ViewModels() //Показываем город Баку
        {
            CityName = "Baku";
            NowForecast = Base.GetData(CityName, true);
            Forecasts = Base.GetForecasts(CityName, true);
        }

        public ObservableCollection<DailyForecast> Forecasts
        {
            get { return forecasts; }
            set { forecasts = value;
                OnPropertyChanged(); }
        }

        public string LastUpdateTime // Последнее обновление
        {
            get { return lastupdatetime; }
            set { lastupdatetime = value;
                OnPropertyChanged(); }
        }
        
        public WeatherInfo NowForecast
        {
            get { return nowForecast; }
            set { nowForecast = value;
                OnPropertyChanged(); }
        }
        
        public ICommand Refresh 
        {
            get
            {
                return new DefaultCommand((obj) =>
                {
                    NowForecast = Base.GetData(NowForecast.Name, isCelsius);
                    Forecasts = Base.GetForecasts(NowForecast.Name, isCelsius);
                    LastUpdateTime = DateTime.Now.ToShortTimeString();
                }, (obj) => LastUpdateTime != DateTime.Now.ToShortTimeString()
                );
            }
        }

        public ICommand CityChanged //Меняем Город
        {
            get
            {
                if (CityName != "")
                {
                    return new DefaultCommand((obj) =>
                    {
                        try
                        {
                            NowForecast = Base.GetData(citytb, isCelsius);
                            Forecasts = Base.GetForecasts(citytb, isCelsius);
                            CityName = citytb;
                        }
                        catch { }
                    }
                    );
                }
                else return null;
            }
        }
        
        public ICommand CelsiusToFahrenheit
        {
            get
            {
                return new DefaultCommand((obj) =>
                {
                    isCelsius = false;
                    NowForecast = Base.GetData(CityName, isCelsius);
                    Forecasts = Base.GetForecasts(CityName, isCelsius);
                }, (obj) => isCelsius != false);
            }
        }

        public ICommand FahrenheitToCelsius
        {
            get
            {
                return new DefaultCommand((obj) =>
                {
                    isCelsius = true;
                    NowForecast = Base.GetData(CityName, isCelsius);
                    Forecasts = Base.GetForecasts(CityName, isCelsius);
                }, (obj) => isCelsius != true);
            }
        }
            
        private void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
