using System;
using CurrentTemperature.Model;

namespace CurrentTemperature.Service
{
	public interface IWeatherService
	{
		Weather CurrentWeather(string zipcode);
	}	
}



