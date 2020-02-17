using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using Weather.Converter;
using Weather.Model;

namespace Weather.Repository
{
    public interface IBase
    {
        WeatherInfo GetData(string city, bool isC); //получаем данные
        ObservableCollection<DailyForecast> GetForecasts(string city, bool isC);
    }

    class Base : IBase
    {
        private const string token = "e07f39dea152a7a25b81f497829ed366"; // мой токен
        private const string webApiW = "http://api.openweathermap.org/data/2.5/weather?q="; //Апи
        private const string webApiF = "http://api.openweathermap.org/data/2.5/forecast?q=";

        public WeatherInfo GetData(string city, bool isC) //Инофрмация о погоде 
        {
            string resultApi; //сделал как у вас
            WebClient client = new WebClient();
            resultApi = client.DownloadString($"{webApiW}{city}&appid={token}");
            var result = JsonConvert.DeserializeObject<WeatherInfo>(resultApi);
            WeatherInfo weatherInfo = result;            

            weatherInfo.Main.temp = Convert.ToInt32(WeatherConverter.KelvinToCelsius(weatherInfo.Main.temp));
            if (isC == false)
            {
                weatherInfo.Main.temp = Convert.ToInt32(WeatherConverter.CelsisusToFahrenheit(weatherInfo.Main.temp));
            }
            weatherInfo.visibility /= 1000;
            weatherInfo.FeelsLike = Convert.ToInt32(WeatherConverter.GetFeelsLike(weatherInfo.Main.temp));
            weatherInfo.DewPoint = WeatherConverter.GetDewPoint(weatherInfo.Main.temp, weatherInfo.Main.humidity);
            weatherInfo.Wind.speed = Convert.ToInt32(WeatherConverter.MsToKh(weatherInfo.Wind.speed));
            weatherInfo.Sunrise = UnixConverter.ConvertUnixToLocalTime(weatherInfo.Sys.sunrise).ToShortTimeString();
            weatherInfo.Sunset = UnixConverter.ConvertUnixToLocalTime(weatherInfo.Sys.sunset).ToShortTimeString();
            weatherInfo.Moonrise = UnixConverter.ConvertUnixToLocalTime(weatherInfo.Sys.sunrise).AddMinutes(-35).ToShortTimeString();
            weatherInfo.Moonset = UnixConverter.ConvertUnixToLocalTime(weatherInfo.Sys.sunset).AddMinutes(39).ToShortTimeString();
            weatherInfo.icon = "/Images/WeatherNow/" + weatherInfo.Weather[0].icon + ".png";
            weatherInfo.BackgoundImagePath = "/Images/Background/" + weatherInfo.Weather[0].icon + ".png";
            weatherInfo.Description = weatherInfo.Weather[0].description.Substring(0, 1).ToUpper() + weatherInfo.Weather[0].description.Remove(0, 1);

            return weatherInfo;
        }

        public ObservableCollection<DailyForecast> GetForecasts(string city, bool isC) //Получаем саму погоду
        {
            string resultApi;
            WebClient client = new WebClient();
            resultApi = client.DownloadString($"{webApiF}{city}&appid={token}");
            Forecast result = JsonConvert.DeserializeObject<Forecast>(resultApi);

            Forecast  forecast = result;

            ObservableCollection<DailyForecast> CollectionOfDailyForecasts = new ObservableCollection<DailyForecast>();
            string days = "0";

            for (DateTime day = DateTime.Now; ; day = day.AddDays(1)) //показываем  день (Джунейд помог с этим)
            {
                if(day.Day<=9)
                {
                    days = "0" + day.Day.ToString();
                }
                else
                {
                    days = day.Day.ToString();
                }
                var lists = from l in forecast.List where l.dt_txt.Split('-').Last().Split(' ').First() == days select l;
                if (lists.Count() != 0)
                {
                    DailyForecast  dailyForecast = new DailyForecast
                    {
                        Description = lists.First().Weather[0].description.Substring(0, 1).ToUpper() + lists.First().Weather[0].description.Remove(0, 1),
                        deg = Convert.ToInt32(lists.First().wind.deg),
                        wind = Convert.ToInt32(lists.First().wind.speed),
                        humidity = Convert.ToInt32(lists.First().main.humidity),
                        Day = day.DayOfWeek.ToString() + " " + day.Day.ToString(),
                        icon = "/Images/WeatherNow/" + lists.First().Weather[0].icon + ".png"
                    };

                    foreach (Forecast.ListOfOB list in lists)
                    {
                        ForecastForHour forecastForHour = new ForecastForHour
                        {
                            time = list.dt_txt.Split(' ').Last(),
                            temp = Convert.ToInt32(WeatherConverter.KelvinToCelsius(list.main.temp)),
                            wind = Convert.ToInt32(WeatherConverter.MsToKh(list.wind.speed)),
                            deg = Convert.ToInt32(list.wind.deg),
                            icon = "/Images/WeatherNow/" + list.Weather[0].icon + ".png",
                            Description = list.Weather[0].description.Substring(0, 1).ToUpper() + list.Weather[0].description.Remove(0, 1),
                            humidity = Convert.ToInt32(list.main.humidity)
                        };
                        if (isC == false)
                        {
                            forecastForHour.temp = Convert.ToInt32(WeatherConverter.CelsisusToFahrenheit(forecastForHour.temp));
                        }
                        dailyForecast.Hourlies.Add(forecastForHour);
                    }
                    dailyForecast.TempMax = dailyForecast.Hourlies.OrderBy(x => x.temp).Last().temp;
                    dailyForecast.TempMin = dailyForecast.Hourlies.OrderBy(x => x.temp).First().temp;
                    CollectionOfDailyForecasts.Add(dailyForecast);
                }
                else if (lists.Count() == 0)
                {
                    break;
                }
            }
            return CollectionOfDailyForecasts;
        }         
    }
}

