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
    public class ClassInfoAdapter : BaseAdapter<string>
    {
        private List<string> items;
        private Activity context;

        public ClassInfoAdapter(Activity context, List<string> items)
        {
            this.context = context;
            this.items = items;
        }

        public override int Count
        {
            get { return items.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override string this[int position]
        {
            get { return items[position]; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;

            if (view == null)
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.list_item, null);
            }

            var classNameTextView = view.FindViewById<TextView>(Resource.Id.classname);
            var absentTextView = view.FindViewById<TextView>(Resource.Id.absent);
            var presentTextView = view.FindViewById<TextView>(Resource.Id.present);

            // Parse the string and extract the relevant data
            string[] data = items[position].Split('\t');
            string className = data[0];
            string[] attendance = data[1].Split('|');
            string absent = attendance[0];
            string present = attendance[1];

            classNameTextView.Text = className;
            absentTextView.Text = absent;
            presentTextView.Text = present;

            return view;
        }
    }
}
