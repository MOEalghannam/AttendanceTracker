/* This is a class that allows the user to edit the attendance of a particular
class. The code defines an activity called `EditAttendanceActivity` which extends the `Activity`
class. The `OnCreate` method is overridden to set the content view to `activity_editattendance`
layout. The code then finds the `TextView`, `EditText`, and `Button` views by their respective IDs
and sets their values. The `string` variable `classpicked` is obtained from the intent extras and
used to retrieve the class information from the database. The class name and absent number are then
set to their respective views. Finally, the `submit` button is clicked and the `classinfo` object is
updated with the new absent number. The updated class information is then saved to the database and
the user is redirected to the `MainActivity`. */
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
    [Activity(Label = "EditAttendanceActivity")]
    public class EditAttendanceActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
             // Set the layout for the activity
            SetContentView(Resource.Layout.activity_editattendance);

            // Find views in the layout by their unique resource IDs
            var classname = FindViewById<TextView>(Resource.Id.classname);
            var absentnumber = FindViewById<EditText>(Resource.Id.absentnumber);
            var submit = FindViewById<Button>(Resource.Id.submit);

            // Get the value of the "ClassPicked" extra from the intent
            string classpicked = Intent.GetStringExtra("ClassPicked");

            // Create an instance of ClassInfoOperations to retrieve class information
            var sq = new ClassInfoOperations();
            // Get the ClassInfo object for the selected class
            var classinfo = sq.GetClassByName(classpicked);

            // Set the class name in the TextView
            classname.Text = classinfo.ClassName;

            // Set the absent number in the EditText field
            absentnumber.Text = Convert.ToString(classinfo.Absent);

            // Add a click event handler for the submit button
            submit.Click += delegate
            {
                // Update the absent number in the classinfo object
                classinfo.Absent = Convert.ToInt32(absentnumber.Text);
                var sq = new ClassInfoOperations();
                // Update the class information in the database
                sq.UpdateClass(classinfo);
                // Create an intent to navigate back to the MainActivity
                Intent i = new Intent(this, typeof(MainActivity));
                StartActivity(i);
                Finish();
            };

        }
    }
}