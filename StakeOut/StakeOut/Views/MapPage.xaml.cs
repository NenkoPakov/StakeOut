using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace StakeOut
{
    public partial class MapPage : ContentPage
    {
        private Uri _wmsUrl = new Uri("http://82.119.84.170:6080/arcgis/rest/services/cadwithrgo/MapServer");
        //"https://nowcoast.noaa.gov/arcgis/services/nowcoast/radar_meteo_imagery_nexrad_time/MapServer/WMSServer?request=GetCapabilities&service=WMS");

        // Create and hold a list of uniquely-identifying WMS layer names to display
        private List<String> _wmsLayerNames = new List<string> { "bnd_lines", "centroid", "rgo"/*, " ppoint", " sewer_shaft", " fire_hydrant", " supply_inlet", " supply_main", " sewerage_inlet", " sewerage_main", " immovable_regulated ", "lines ", "ot" */};



        // Hold the WMS layer
        private ArcGISMapImageLayer _wmsLayer;

        public MapPage()
        {
            InitializeComponent();

            // Create the UI, setup the control references and execute initialization
            Initialize();
        }

        private async void Initialize()
        {
            // Create new Map with basemap
            // Create new Map
            Map myMap = new Map(Basemap.CreateOpenStreetMap());

            // Create uri to the map image layer
            MyMapView.Map = myMap;

            // Create a new WMS layer displaying the specified layers from the service
            _wmsLayer = new ArcGISMapImageLayer(_wmsUrl);

            try
            {
                


                // Load the layer
                await _wmsLayer.LoadAsync();

                //myMap.Basemap.BaseLayers.Add(_wmsLayer);
                // Add the layer to the map
                MyMapView.Map.OperationalLayers.Add(_wmsLayer);

                // Zoom to the layer's extent
                MyMapView.SetViewpoint(new Viewpoint(_wmsLayer.FullExtent));

                // Subscribe to tap events - starting point for feature identification
                MyMapView.GeoViewTapped += MyMapView_GeoViewTapped;
            }
            catch (Exception e)
            {
                // await Application.Current.MainPage.DisplayAlert("Error", e.ToString(), "OK");
            }
        }

        private async void MyMapView_GeoViewTapped(object sender, Esri.ArcGISRuntime.Xamarin.Forms.GeoViewInputEventArgs e)
        {
            try
            {
                // Perform the identify operation
                IdentifyLayerResult myIdentifyResult = await MyMapView.IdentifyLayerAsync(_wmsLayer, e.Position, 20, false);

                // Return if there's nothing to show
                if (myIdentifyResult.SublayerResults[1].GeoElements.Count < 1)
                {
                    return;
                }

                // Retrieve the identified feature, which is always a WmsFeature for WMS layers
                //WmsFeature identifiedFeature = (WmsFeature)myIdentifyResult.SublayerResults[0].GeoElements[0].Attributes.Values;
                var identifiedFeature = myIdentifyResult.SublayerResults
                    .Select(x => x.GeoElements.Where(y => y.Attributes.Values.Contains("РГО")).FirstOrDefault().Attributes);

                var identifiedFeature2 = myIdentifyResult.SublayerResults[1].GeoElements.Where(x => x.Attributes.Count == 17).FirstOrDefault().Attributes;
                var identifiedFeature3 = myIdentifyResult.SublayerResults.Select(x => x.GeoElements.Where(y => y.Attributes.Count == 17).FirstOrDefault().Attributes);
                var coordinateX = identifiedFeature2.Where(x => x.Key == "x").FirstOrDefault().Value.ToString();
                var coordinateY = identifiedFeature2.Where(x => x.Key == "y").FirstOrDefault().Value.ToString();
                var pointNumber = identifiedFeature2.Where(x => x.Key == "geoptnum").FirstOrDefault().Value.ToString();

                var clickedPoint = new Point();
                clickedPoint.CoordinateX = coordinateX;
                clickedPoint.CoordinateY = coordinateY;
                clickedPoint.Number = pointNumber;

                var StakeOutViewModel = new StakeOutViewModel(clickedPoint);
                await Navigation.PushAsync(new StakeOutPage(StakeOutViewModel));
                // Retrieve the WmsFeature's HTML content
                // string htmlContent = identifiedFeature/*.Attributes["HTML"].ToString()*/;

                // Note that the service returns a boilerplate HTML result if there is no feature found.
                // This test should work for most arcGIS-based WMS services, but results may vary.
                //if (!htmlContent.Contains("OBJECTID"))
                {
                    // Return without showing the result
                    //await Navigation.PushAsync(new WmsIdentifyResultDisplayPage(htmlContent)
                }

                // Show a page with the HTML content
                // await Navigation.PushAsync(new WmsIdentifyResultDisplayPage(htmlContent));
            }
            catch (Exception ex)
            {
                //await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
            }
        }

        private void OnStopClicked(object sender, EventArgs e)
        {
            MyMapView.LocationDisplay.IsEnabled = false;
        }

        private async void OnStartClicked(object sender, EventArgs e)
        {
            // Show sheet and get title from the selection.
           

             MyMapView.LocationDisplay.AutoPanMode = LocationDisplayAutoPanMode.Recenter;
            

            try
            {
                // Permission request only needed on Android.
#if XAMARIN_ANDROID
                // See implementation in MainActivity.cs in the Android platform project.
                MainActivity.Instance.AskForLocationPermission(MyMapView);
#else
                await MyMapView.LocationDisplay.DataSource.StartAsync();
                MyMapView.LocationDisplay.IsEnabled = true;
#endif
            }
            catch (Exception ex)
            {
               //Debug.WriteLine(ex);
               //await Application.Current.MainPage.DisplayAlert("Couldn't start location", ex.Message, "OK");
            }
        }

        public void Dispose()
        {
            // Stop the location data source.
            MyMapView.LocationDisplay?.DataSource?.StopAsync();
        }

    }

    //public class WmsIdentifyResultDisplayPage : ContentPage
    //{
    //    public WmsIdentifyResultDisplayPage(string htmlContent)
    //    {
    //        Title = "WMS identify result";
    //
    //        // Create the web browser control
    //        WebView htmlView = new WebView
    //        {
    //
    //            // Display the string content as an HTML document
    //            Source = new HtmlWebViewSource() { Html = htmlContent }
    //        };
    //
    //        // Create and add a layout to the page
    //        Grid layout = new Grid();
    //        Content = layout;
    //
    //        // Add the webview to the page
    //        layout.Children.Add(htmlView);
    //    }
    //}
}

