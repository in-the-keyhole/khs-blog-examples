using System;
using System.IO;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

using CurrentTemperature.Model;

namespace CurrentTemperature.Service
{
	public class GoogleWeatherService : IWeatherService
	{
		private const string URL = "http://www.google.com/ig/api?weather={0}";

		private const string DATAATTRIBUTE = "data";
        private const string CURRENTCONDITIONS = "current_conditions";
        private const string CONDITION = "condition";
        private const string TEMP = "temp_f";
        private const string HUMIDITY = "humidity";
        private const string WINDCONDITION = "wind_condition";

		public GoogleWeatherService ()
		{
		}

		public Weather CurrentWeather (string zipcode)
		{
			string url = string.Format(URL, zipcode);
			return ProcessService(url);
		}

		private Weather ProcessService(string url)
		{
			XDocument doc;
			Weather weather;

			using (var client = new WebClient()) 
			{
				var resultString = client.DownloadString(url);
				doc = XDocument.Parse (resultString);

				var conditions = (from feed in doc.Descendants(CURRENTCONDITIONS)
                                         select new Weather
                                         {
                                             Condition = feed.Element(CONDITION).Attribute(DATAATTRIBUTE).Value,
                                             Temperature = feed.Element(TEMP).Attribute(DATAATTRIBUTE).Value,
                                             Humidity = feed.Element(HUMIDITY).Attribute(DATAATTRIBUTE).Value,
                                             Wind = feed.Element(WINDCONDITION).Attribute(DATAATTRIBUTE).Value
                                         });

				weather = conditions.First();
			}
			return weather;
		}
	}
}

