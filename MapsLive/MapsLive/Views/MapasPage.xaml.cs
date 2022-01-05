using MapsLive.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace MapsLive.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Mapas : ContentPage
    {
        CancellationTokenSource cts;
        public Mapas()
        {
            InitializeComponent();
            iniciarMetodosAsync();
            //---realizar pruebas para ver si se esta llamando siempre
            //---aunque creo que es de ponerlo en el metodo protected override void OnStart() de la App.xaml.cs           
            //GetCurrentLocation();
        }

        public async System.Threading.Tasks.Task iniciarMetodosAsync()
        {
            //Lowest Android	500         -iOS	3000
            //Low Android	    500         -iOS	1000
            //Mediun Android 100 - 500       -iOS	100
            //High Android	0 - 100         -iOS	10
            //BEST Android	0 - 100         - iOS	~0
            var request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
            Location location = await Geolocation.GetLocationAsync(request: request);
            //map.MapType = MapType.Satellite;
            //---verificamos que no este vacia
            if (location != null)
            {
                //---datos 
                Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                //--Pintamos los datos en un pin
                var defaultPin = new Pin { Type = PinType.Place, Label = "Yo me encuentro", Address = "Aqui", Position = new Position(location.Latitude, location.Longitude) };
                defaultPin.Icon = BitmapDescriptorFactory.FromBundle("coche.png");                
                map.Pins.Add(defaultPin);

                #region POSICION
                ////Ubicar y centrar el mapa
                map.InitialCameraUpdate = CameraUpdateFactory.NewCameraPosition(new CameraPosition(
                 new Position(location.Latitude, location.Longitude),  // latlng
                 18d, // zoom
                 30d, // rotation
                 60d)); // tilt

                map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(location.Latitude, location.Longitude), Distance.FromMeters(1000)));
                map.UiSettings.RotateGesturesEnabled = true;
                map.UiSettings.ZoomControlsEnabled = true;
                map.UiSettings.ZoomGesturesEnabled = true;

                map.UiSettings.CompassEnabled = true;
                #endregion

            }



        }



        //-----Es de probarlo
        async Task GetCurrentLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cts.Token);

                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
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
        }

        protected override void OnDisappearing()
        {
            if (cts != null && !cts.IsCancellationRequested)
                cts.Cancel();
            base.OnDisappearing();
        }
    }
}