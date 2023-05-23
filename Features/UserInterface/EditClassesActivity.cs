using Android.App;
using Android.Content;
using Android.Gms.Maps.Model;
using Android.Gms.Maps;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Android.App.TimePickerDialog;
using Android;
using Android.Content.Res;
using Java.Util;

namespace AttendanceTracker
{
    [Activity(Label = "EditClassesActivity")]
    public class EditClassesActivity : Activity, IOnTimeSetListener, IOnMapReadyCallback, GoogleMap.IOnMapClickListener
    {
        const int LocationRequestCode = 1;
        string[] LocationPermissions = { Manifest.Permission.AccessFineLocation, Manifest.Permission.AccessCoarseLocation };

        private GoogleMap googleMap;
        private MapView mapView;
        private LatLng selectedLocation;

        private string classday;

        private const int STARTTIME_DIALOG = 1;
        private const int ENDTIME_DIALOG = 2;
        private int starthour = 7;
        private int startminutes = 0;
        private int selectedDayPosition;
        private int endminutes = 0;
        private int endhour = 0;

        private ClassInfoOperations.ClassInfo classinfo;

        int pickerID;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_editclasses);

            if (CheckSelfPermission(Manifest.Permission.AccessFineLocation) != Android.Content.PM.Permission.Granted
                || CheckSelfPermission(Manifest.Permission.AccessCoarseLocation) != Android.Content.PM.Permission.Granted)
            {
                RequestPermissions(LocationPermissions, LocationRequestCode);
            }


            mapView = FindViewById<MapView>(Resource.Id.mapView);
            mapView.OnCreate(savedInstanceState);
            mapView.GetMapAsync(this);

            var classname = FindViewById<EditText>(Resource.Id.classname);

            Spinner spinner = FindViewById<Spinner>(Resource.Id.spinner1);
            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.car_array, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;




            Configuration config = Resources.Configuration;
            Locale currentLocale = config.Locale;


            var classhours = FindViewById<EditText>(Resource.Id.classhours);
            var subjecthours = FindViewById<EditText>(Resource.Id.subjecthours);


            var submit = FindViewById<Button>(Resource.Id.submit);

            var StartTimeBTN = FindViewById<Button>(Resource.Id.StartTimeBTN);
            var EndTimeBTN = FindViewById<Button>(Resource.Id.EndTimeBTN);

            string classpicked = Intent.GetStringExtra("ClassPicked");
            var sq = new ClassInfoOperations();
            classinfo = sq.GetClassByName(classpicked);

            var tempday = classinfo.Day;

            if (currentLocale.Language == "ar")
            {
                tempday = Translations.ENGtoAR(tempday);
            }

            string[] daysArray = Resources.GetStringArray(Resource.Array.car_array);
            selectedDayPosition = Array.IndexOf(daysArray, tempday);

            spinner.SetSelection(selectedDayPosition);


            classname.Text = classinfo.ClassName;
            classhours.Text = Convert.ToString(classinfo.ClassHours);
            subjecthours.Text = Convert.ToString(classinfo.SubjectHours);
            TimeSpan timestart = classinfo.StartTime;
            starthour = timestart.Hours;
            startminutes = timestart.Minutes;
            TimeSpan timeend = classinfo.EndTime;
            endhour = timeend.Hours;
            endminutes = timeend.Minutes;



            StartTimeBTN.Click += delegate
            {
                ShowDialog(STARTTIME_DIALOG);
                pickerID = STARTTIME_DIALOG;
            };

            EndTimeBTN.Click += delegate
            {
                ShowDialog(ENDTIME_DIALOG);
                pickerID = ENDTIME_DIALOG;
            };

            submit.Click += delegate
            {
                TimeSpan selectedTimeStart = new TimeSpan(starthour, startminutes, 0);
                TimeSpan selectedTimeEnd = new TimeSpan(endhour, endminutes, 0);

                if (currentLocale.Language == "ar")
                {
                    classday = Translations.ARtoENG(classday);
                }
                classinfo.ClassName = classname.Text;
                classinfo.Day = classday;
                classinfo.Latitude = selectedLocation.Latitude;
                classinfo.Longitude = selectedLocation.Longitude;
                classinfo.ClassHours = Convert.ToInt32(classhours.Text);
                classinfo.SubjectHours = Convert.ToInt32(subjecthours.Text);
                classinfo.StartTime = selectedTimeStart;
                classinfo.EndTime = selectedTimeEnd;

                var sq = new ClassInfoOperations();
                sq.UpdateClass(classinfo);

                Intent i = new Intent(this, typeof(MainActivity));
                StartActivity(i);

            };


        }
        public void OnMapReady(GoogleMap map)
        {
            googleMap = map;

            if (googleMap != null)
            {
                // Set up the map
                googleMap.SetOnMapClickListener(this);
                googleMap.UiSettings.ZoomControlsEnabled = true;
                googleMap.UiSettings.CompassEnabled = true;
                googleMap.MyLocationEnabled = true;

                // Add a marker at the default location (if desired)
                selectedLocation = new LatLng(classinfo.Latitude, classinfo.Longitude);
                googleMap.AddMarker(new MarkerOptions().SetPosition(selectedLocation));

                // Move the camera to the default location
                CameraUpdate cameraUpdate = CameraUpdateFactory.NewLatLngZoom(selectedLocation, 8);
                googleMap.MoveCamera(cameraUpdate);
            }
        }
        public void OnMapClick(LatLng point)
        {
            googleMap.Clear(); // Remove previous markers (if any)

            selectedLocation = point;

            // Add a marker at the selected location
            googleMap.AddMarker(new MarkerOptions().SetPosition(selectedLocation));

            // Move the camera to the selected location
            CameraUpdate cameraUpdate = CameraUpdateFactory.NewLatLng(selectedLocation);
            googleMap.MoveCamera(cameraUpdate);
        }

        protected override void OnResume()
        {
            base.OnResume();
            mapView.OnResume();
        }

        protected override void OnPause()
        {
            base.OnPause();
            mapView.OnPause();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            mapView.OnDestroy();
        }
        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            classday = (string)spinner.GetItemAtPosition(e.Position);
        }
        protected override Dialog OnCreateDialog(int id)
        {
            switch (id)
            {
                case STARTTIME_DIALOG:
                    {
                        return new TimePickerDialog(this, this, starthour, startminutes, true);
                    };
                case ENDTIME_DIALOG:
                    {
                        return new TimePickerDialog(this, this, endhour, endminutes, true);
                    }
                default:
                    break;
            }
            return null;
        }

        public void OnTimeSet(TimePicker view, int hourOfDay, int minuteOfHour)
        {
            if (pickerID == STARTTIME_DIALOG)
            {
                starthour = hourOfDay;
                startminutes = minuteOfHour;
            }
            if (pickerID == ENDTIME_DIALOG)
            {
                endhour = hourOfDay;
                endminutes = minuteOfHour;
            }
        }
    }
}
