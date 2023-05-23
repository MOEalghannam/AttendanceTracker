using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;

namespace AttendanceTracker
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            
            var intent = new Intent(this, typeof(ClassAttendanceService));
            var isServiceStarted = StartService(intent) != null;



            if (!isServiceStarted)
            {
                Toast.MakeText(this, "Failed to start ClassAttendanceService", ToastLength.Short).Show();
                System.Diagnostics.Debug.WriteLine($"ClassAttendanceService failed to start");
            }
            else
            {

            }

            var enter_classes = FindViewById<Button>(Resource.Id.enter_classes);
            var enter_atten = FindViewById<Button>(Resource.Id.enter_atten);
            var edit_button = FindViewById<Button>(Resource.Id.edit_button);

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
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}