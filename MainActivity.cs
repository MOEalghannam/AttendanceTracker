/* This is the main activity It is responsible for creating the user interface and handling user interactions. */
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;

namespace AttendanceTracker
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    // Main activity of the application
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            // Create an intent to start the ClassAttendanceService
            var intent = new Intent(this, typeof(ClassAttendanceService));
            var isServiceStarted = StartService(intent) != null;


            // Display a toast message and debug output if the service fails to start
            if (!isServiceStarted)
            {
                Toast.MakeText(this, "Failed to start ClassAttendanceService", ToastLength.Short).Show();
                System.Diagnostics.Debug.WriteLine($"ClassAttendanceService failed to start");
            }
            else
            {

            }
            // Find buttons in the layout
            var enter_classes = FindViewById<Button>(Resource.Id.enter_classes);
            var enter_atten = FindViewById<Button>(Resource.Id.enter_atten);
            var edit_button = FindViewById<Button>(Resource.Id.edit_button);

            // Handle click events for the buttons
            edit_button.Click += delegate
            {
                Intent i = new Intent(this, typeof(SearchActivity));
                StartActivity(i);
            };

            enter_classes.Click += delegate
            {
                Intent i = new Intent(this, typeof(InsertClassesActivity));
                StartActivity(i);
            };
            enter_atten.Click += delegate
            {
                Intent i = new Intent(this, typeof(ShowAttendenceActivity));
                StartActivity(i);
            };

        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            // Call the base class method to handle the request permissions result
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            // Pass the result to Xamarin.Essentials.Platform to handle the permission request
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}