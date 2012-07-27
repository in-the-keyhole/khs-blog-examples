// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace CurrentTemperature
{
	[Register ("CurrentTemperatureViewController")]
	partial class CurrentTemperatureViewController
	{
		[Outlet]
		MonoTouch.UIKit.UILabel lblTemp { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblCityState { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton btnRefresh { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblCondition { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblWind { get; set; }

		[Action ("Refresh:")]
		partial void Refresh (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (lblTemp != null) {
				lblTemp.Dispose ();
				lblTemp = null;
			}

			if (lblCityState != null) {
				lblCityState.Dispose ();
				lblCityState = null;
			}

			if (btnRefresh != null) {
				btnRefresh.Dispose ();
				btnRefresh = null;
			}

			if (lblCondition != null) {
				lblCondition.Dispose ();
				lblCondition = null;
			}

			if (lblWind != null) {
				lblWind.Dispose ();
				lblWind = null;
			}
		}
	}
}
