using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreLocation;

using CurrentTemperature.Model;
using CurrentTemperature.Service;

namespace CurrentTemperature
{
	public partial class CurrentTemperatureViewController : UIViewController
	{
		// Initialize a location manager
		private CLLocationManager iPhoneLocationManager = null;
		private Weather CurrentWeather { get; set; }
		private Location CurrentLocation { get; set; }

		public CurrentTemperatureViewController () : base ("CurrentTemperatureViewController", null)
		{
		}
		
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// Perform any additional setup after loading the view, typically from a nib.
			iPhoneLocationManager = new CLLocationManager();
			LocationDelegate locationDelegate = new LocationDelegate(this);
			iPhoneLocationManager.Delegate = locationDelegate;

			iPhoneLocationManager.StartUpdatingLocation();
			iPhoneLocationManager.StartUpdatingHeading();
		}

		public void DisplayScreen ()
		{
			if (CurrentWeather != null) 
			{
				iPhoneLocationManager.StopUpdatingLocation();
				iPhoneLocationManager.StopUpdatingHeading();
			}
			lblTemp.Text = CurrentWeather.Temperature + "° F";
			lblCondition.Text = CurrentWeather.Condition;
			lblWind.Text = CurrentWeather.Wind;
			lblCityState.Text = string.Format("{0}, {1}", CurrentLocation.City, CurrentLocation.State);

		}

		
		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			
			// Clear any references to subviews of the main view in order to
			// allow the Garbage Collector to collect them sooner.
			//
			// e.g. myOutlet.Dispose (); myOutlet = null;
			
			ReleaseDesignerOutlets ();
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
		}

		partial void Refresh(NSObject sender) 
		{
			CurrentWeather = null;
			lblTemp.Text = "";
			lblCondition.Text = "";
			lblWind.Text = "";
			lblCityState.Text = "";
			iPhoneLocationManager.StartUpdatingLocation();
			iPhoneLocationManager.StartUpdatingHeading();
		}


        /********************************************************
         * 
         *  Location Delegate Class for GPS
         * 
         *******************************************************/
        public class LocationDelegate : CLLocationManagerDelegate
        {
			private CLGeocoder geocoder = null;
            
			private CurrentTemperatureViewController screen;
            public LocationDelegate (CurrentTemperatureViewController screen) : base()
            {
				this.screen = screen;
            }
            
            public override void UpdatedLocation (CLLocationManager manager, CLLocation newLocation, CLLocation oldLocation)
			{
				if (screen.CurrentWeather == null) 
				{
					geocoder = new CLGeocoder ();
					geocoder.ReverseGeocodeLocation (newLocation, ReverseGeocodeLocationHandle);
				}
            }

			public void ReverseGeocodeLocationHandle (CLPlacemark[] placemarks, NSError error)
			{
				Location loc = new Location();
				for (int i = 0; i < placemarks.Length; i++) {
					loc.Zipcode = placemarks [i].PostalCode;
					loc.City = placemarks[i].Locality;
					loc.State = placemarks[i].AdministrativeArea;
				}

				if (screen.CurrentWeather == null) 
				{
					var service = new GoogleWeatherService();
					screen.CurrentWeather = service.CurrentWeather(loc.Zipcode);
					screen.CurrentLocation = loc;
					screen.DisplayScreen();
				}

			}
        }    
	}
}

