using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AttendanceTracker
{
    [Activity(Label = "SearchActivity")]
    public class SearchActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_search);

            var classinput = FindViewById<EditText>(Resource.Id.classname);
            var update_button = FindViewById<Button>(Resource.Id.update_button);
            var updateatten_button = FindViewById<Button>(Resource.Id.updateatten_button);

            update_button.Click += delegate
            {

                var sq = new ClassInfoOperations();
                var ClassPicked = sq.GetClassByName(classinput.Text);
                if (ClassPicked != null)
                {
                    Intent i = new Intent(this, typeof(EditClassesActivity));
                    i.PutExtra("ClassPicked", ClassPicked.ClassName + "");
                    StartActivity(i);
                }
                else
                {
                    Toast.MakeText(this, "Classname is not registerd", ToastLength.Short).Show();
                }
            };
            updateatten_button.Click += delegate
            {

                var sq = new ClassInfoOperations();
                var ClassPicked = sq.GetClassByName(classinput.Text);
                if (ClassPicked != null)
                {
                    Intent i = new Intent(this, typeof(EditAttendanceActivity));
                    i.PutExtra("ClassPicked", ClassPicked.ClassName + "");
                    StartActivity(i);
                }
                else
                {
                    Toast.MakeText(this, "Classname is not registerd", ToastLength.Short).Show();
                }
            };

        }
    }
}
