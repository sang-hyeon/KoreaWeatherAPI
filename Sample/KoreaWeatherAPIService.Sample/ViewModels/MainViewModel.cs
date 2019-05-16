using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using KoreaWeatherAPIService;
using KoreaWeatherAPIService.Models;
namespace KoreaWeatherAPIService.Sample.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        //Type your api key
        const string Api_Key = @"";
        readonly IWeatherService _weatherService;

        Observation _observation;
        double _latitude;
        double _longitude;
        Visibility _errorVisible;

        public Observation Observation
        {
            get => _observation;
            set => Set(ref _observation, value);
        }
        public double Latitude
        {
            get => _latitude;
            set => Set(ref _latitude, value);
        }
        public double Longitude
        {
            get => _longitude;
            set => Set(ref _longitude, value);
        }
        public Visibility ErrorVisible
        {
            get => _errorVisible;
            set => Set(ref _errorVisible, value);
        }

        public ICommand ReqeustWeatherCommand
            => new RelayCommand(async () =>
            {
                try
                {
                    Observation = await this._weatherService.Request_NowWeatherAsync(new Location(Latitude, Longitude));
                    ErrorVisible = Visibility.Collapsed;
                }
                catch(Exception e)
                {
                    ErrorVisible = Visibility.Visible;
                    Observation = null;
                    return;
                }
            });

        public MainViewModel()
        {
            if (string.IsNullOrEmpty(Api_Key)) throw new ArgumentNullException("Put your API Key");

            this.Latitude = 37.493709;
            this.Longitude = 127.053175;
            this.ErrorVisible = Visibility.Collapsed;

            this._weatherService = new WeatherService(Api_Key);
        }
    }
}
