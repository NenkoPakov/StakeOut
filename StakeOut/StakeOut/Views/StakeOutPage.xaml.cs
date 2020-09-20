using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.Forms.PlatformConfiguration;

namespace StakeOut
{
	public partial class StakeOutPage : ContentPage
	{
		public StakeOutPage(StakeOutViewModel viewModel)
		{
			InitializeComponent();

           BindingContext= viewModel;
		}

        //public override void OnRequestPermissionsResult(int requestCode,
        //string[] permissions, Android.Content.PM.Permission[] grantResults)
        //{
        //    Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode,
        //        permissions, grantResults);
        //    base.OnRequestPermissionsResult(requestCode, permissions,
        //        grantResults);
        //}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if(!DesignMode.IsDesignModeEnabled)
                ((StakeOutViewModel)BindingContext).StartCommand.Execute(null);
        }
       
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (!DesignMode.IsDesignModeEnabled)
                ((StakeOutViewModel)BindingContext).StopCommand.Execute(null);
        }

    }

   
}
