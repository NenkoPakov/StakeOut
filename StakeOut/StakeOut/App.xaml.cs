using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace StakeOut
{
	public partial class App : Xamarin.Forms.Application
	{
		public App ()
		{
			var navigationPage = new Xamarin.Forms.NavigationPage(new MainPage())
			{
				BarBackgroundColor = Color.FromHex("0099B4"),
				BarTextColor = Color.White
			};

			navigationPage.On<iOS>().SetPrefersLargeTitles(true);

			MainPage = navigationPage;
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
