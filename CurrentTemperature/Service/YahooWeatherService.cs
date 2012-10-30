using System;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;
using System.Xml.Linq;

using CurrentTemperature.Model;

namespace CurrentTemperature.Service
{
	public class YahooWeatherService : IWeatherService
	{
		private const string URL_YAHOO_WEATHER = "http://weather.yahooapis.com/forecastrss?p={0}";

		private readonly string[] DIRECTIONS = {"N","NNE","NE","ENE","E","ESE", "SE", "SSE","S","SSW","SW","WSW","W","WNW","NW","NNW"};

		public YahooWeatherService ()
		{
		}

		public Weather CurrentWeather (string zipcode)
		{
			string url = string.Format(URL_YAHOO_WEATHER, zipcode);
			return ProcessService(url);
		}

		private Weather ProcessService (string url)
		{
			Weather weather = null;

			XmlDocument doc = new XmlDocument ();  
			
			// Load data  
			doc.Load (url);  

			// Set up namespace manager for XPath  
			XmlNamespaceManager ns = new XmlNamespaceManager (doc.NameTable);  
			ns.AddNamespace ("yweather", "http://xml.weather.yahoo.com/ns/rss/1.0");  
			
			// Get current forecast with XPath  
			XmlNode condNode = doc.SelectSingleNode ("/rss/channel/item/yweather:condition", ns);  
			XmlNode astrNode = doc.SelectSingleNode ("/rss/channel/yweather:atmosphere", ns);


			weather = new Weather { 
				Condition = condNode.Attributes["text"].Value,
				Temperature = condNode.Attributes ["temp"].Value, 
				Wind = getWindDescription(doc, ns),
				Humidity = astrNode.Attributes["humidity"].Value
			};

			return weather;
		}

		private string getWindDescription (XmlDocument doc, XmlNamespaceManager ns)
		{
			XmlNode windNode = doc.SelectSingleNode ("/rss/channel/yweather:wind", ns);

			var speed = windNode.Attributes["speed"].Value;
			var degreeString = windNode.Attributes["direction"].Value;

			float degree = 0;
			float.TryParse(degreeString, out degree);

			int index = (int) ((degree / 22.5) + 0.5);
			index = index % 16;

			string name = DIRECTIONS[index];

			return string.Format("Wind: {0} at {1}mph", name, speed);
		}
	}
}

