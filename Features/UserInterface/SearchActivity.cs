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
using static AttendanceTracker.ClassInfoOperations;

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
            var delete_button = FindViewById<Button>(Resource.Id.deleteclass_button);

            update_button.Click += delegate
            {

                var sq = new ClassInfoOperations();
                var ClassPicked = sq.GetClassByName(classinput.Text);
                if (ClassPicked != null)
                {
                    Intent i = new Intent(this, typeof(EditClassesActivity));
                    i.PutExtra("ClassPicked", ClassPicked.ClassName + "");
                    StartActivity(i);
                    Finish();
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
                    Finish();
                }
                else
                {
                    Toast.MakeText(this, "Classname is not registerd", ToastLength.Short).Show();
                }
            };
            delete_button.Click += delegate
            {
                var sq = new ClassInfoOperations();
                var ClassPicked = sq.GetClassByName(classinput.Text);
                if (ClassPicked != null)
                {
                    ShowConfirmationDialog(ClassPicked);
                }
                else
                {
                    Toast.MakeText(this, "Class name is not registered", ToastLength.Short).Show();
                }
            };

        }
        private void ShowConfirmationDialog(ClassInfo classInfo)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetTitle(GetString(Resource.String.Confirmation))
                   .SetMessage(GetString(Resource.String.ConfirmationMessage))
                   .SetPositiveButton(GetString(Resource.String.ConformationDelete), (sender, args) =>
                   {
                       // User confirmed, delete the class
                       var sq = new ClassInfoOperations();
                       sq.DeleteClass(classInfo);

                       Intent i = new Intent(this, typeof(MainActivity));
                       StartActivity(i);
                       Finish();
                   })
                   .SetNegativeButton(GetString(Resource.String.ConfirmationCancel), (sender, args) =>
                   {
                       // User canceled, do nothing
                   });

            AlertDialog dialog = builder.Create();
            dialog.Show();
        }
    }
}
