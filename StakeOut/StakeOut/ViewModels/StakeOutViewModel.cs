using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using Plugin.Geolocator;
using StakeOut.Models;
using StakeOut.Models.Enums;
using System.Threading.Tasks;
//using BojkoSoft.Transformations;
//using BojkoSoft.Transformations.Constants;

namespace StakeOut
{
    public class StakeOutViewModel : MvvmHelpers.BaseViewModel
    {

        public StakeOutViewModel()
        {
            StopCommand = new Command(Stop);
            StartCommand = new Command(Start);
        }

        public StakeOutViewModel(Point point)
            : this()
        {
            PointNumber = $"PointNumber: {point.Number}";
            CoordinateX = (point.CoordinateX);
            CoordinateY = (point.CoordinateY);

        }

        string headingDisplay;
        public string HeadingDisplay
        {
            get => headingDisplay;
            set => SetProperty(ref headingDisplay, value);
        }

        double heading = 0;
        public double Heading
        {
            get => heading;
            set => SetProperty(ref heading, value);
        }

        string distance;
        public string Distance
        {
            get => distance;
            set => SetProperty(ref distance, value);
        }

        string pointNumber;
        public string PointNumber
        {
            get => pointNumber;
            set => SetProperty(ref pointNumber, value);
        }



        string coordinateX;

        public string CoordinateX

        {

            get => coordinateX;

            set => SetProperty(ref coordinateX, value);

        }



        string coordinateY;

        public string CoordinateY

        {

            get => coordinateY;

            set => SetProperty(ref coordinateY, value);

        }

        string deltaX;
        public string DeltaX
        {
            get => deltaX;
            set => SetProperty(ref deltaX, value);
        }


        string deltaY;
        public string DeltaY
        {
            get => deltaY;
            set => SetProperty(ref deltaY, value);
        }

        public Command StopCommand { get; }

        void Stop()
        {
            if (!Compass.IsMonitoring)
                return;

            Compass.ReadingChanged -= Compass_ReadingChanged;
            Compass.Stop();
        }


        public Command StartCommand { get; }

        void Start()
        {
            if (Compass.IsMonitoring)
                return;

            // Compass.ApplyLowPassFilter = true;
            Compass.ReadingChanged += Compass_ReadingChanged;
            Compass.Start(SensorSpeed.UI);

        }




        async void Compass_ReadingChanged(object sender, CompassChangedEventArgs e)
        {
            //X: 4730077.747
            //Y: 322210.914

            try
            {

                var request = new GeolocationRequest(GeolocationAccuracy.High);
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    Transformations tr = new Transformations();
                    GeoPoint input = new GeoPoint(location.Latitude, location.Longitude);
                    //GeoPoint input = new GeoPoint(42, 42);
                    GeoPoint result = tr.TransformGeographicToLambert(input, enumProjection.BGS_2005_KK, enumEllipsoid.WGS84);

                    CalculateCoordinateDifferences(result.X, result.Y);
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }

            // var request = new GeolocationRequest(GeolocationAccuracy.Best);
            // var location = await Geolocation.GetLocationAsync(request);

            //Transformations tr = new Transformations();
            //// GeoPoint input = new GeoPoint(location.Latitude, location.Longitude);
            //GeoPoint input = new GeoPoint(location.Latitude, location.Longitude);
            //GeoPoint result = tr.TransformGeographicToLambert(input, enumProjection.BGS_2005_KK, enumEllipsoid.WGS84);
            //
            var degreesToGradiansParameter = 10.0 / 9;
            //
            //CalculateCoordinateDifferences(result.X, result.Y);

            var headingAzimuthInDegrees = e.Reading.HeadingMagneticNorth + CalculateAzimuth();

            if (headingAzimuthInDegrees >= 360)
            {
                headingAzimuthInDegrees -= 360;
            }
            else if (headingAzimuthInDegrees < 0)
            {
                headingAzimuthInDegrees += 360;
            }

            Heading = headingAzimuthInDegrees;

            if (PointNumber == null)
            {
                PointNumber = $"PointNumber: {PointNumber}";
            }

            var distanceToPoint = Math.Sqrt(Math.Pow(double.Parse(this.DeltaX), 2) + Math.Pow(double.Parse(this.DeltaY), 2));

            //while (distanceToPoint <= 1 && distanceToPoint != null)
            //{
            //    var detailViewModel = new DetailPageViewModel();
            //
            //    await Application.Current.MainPage.Navigation.PushAsync(new DetailPage(detailViewModel));
            //}

            Distance = $"Distance: {distanceToPoint:F1}m";

            HeadingDisplay = $"Heading: {(headingAzimuthInDegrees * degreesToGradiansParameter):F2}g";

            var roundedDeltaX = $"{double.Parse(this.DeltaX):F1}";
            var roundedDeltaY = $"{double.Parse(this.DeltaY):F1}";


            DeltaX = $"▲X: {roundedDeltaX}m";
            DeltaY = $"▲Y: {roundedDeltaY}m";
        }

        private void CalculateCoordinateDifferences(double locationX, double locationY)
        {
            this.DeltaX = (double.Parse(CoordinateX) - locationX).ToString();
            this.DeltaY = (double.Parse(CoordinateY) - locationY).ToString();
        }


        //private string CalculateDistance()
        //{
        //    var distance = Math.Sqrt(Math.Pow(double.Parse(this.DeltaX), 2) + Math.Pow(double.Parse(this.DeltaY), 2));
        //
        //    return distance.ToString();
        //}

        private double CalculateAzimuth()
        {
            var azimuth = Math.Atan2(double.Parse(this.DeltaY), double.Parse(this.DeltaX)) * 180 / Math.PI;

            return azimuth;
        }
    }
}
