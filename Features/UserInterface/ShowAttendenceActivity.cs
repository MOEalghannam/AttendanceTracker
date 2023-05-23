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
    [Activity(Label = "ShowAttendence")]
    public class ShowAttendenceActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_showattendence);
            // Create your application here


            var display = FindViewById<ListView>(Resource.Id.display);


            ClassInfoOperations.ClassInfo Class;
            var sq = new ClassInfoOperations();
            var tables = sq.GetClasses();
            double absenthours;
            double percentage;
            List<string> dataList = new List<string>();
            foreach (var s in tables)
            {

                absenthours = s.Absent * s.ClassHours;
                percentage = ((absenthours / (s.SubjectHours * 13)) * 100);
                dataList.Add(s.ClassName + "\t" + percentage.ToString("0.00") + "|" + s.Absent);

            }
            // Create and set the custom adapter
            ClassInfoAdapter adapter = new ClassInfoAdapter(this, dataList);
            display.Adapter = adapter;
        }
    }
}