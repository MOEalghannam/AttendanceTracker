/* This is a class that extends the `BaseAdapter` class and is used to
populate a ListView with data.*/
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
    // Adapter class for displaying class information in a list view.
    public class ClassInfoAdapter : BaseAdapter<string>
    {
        private List<string> items;
        private Activity context;

        // Initializes a new instance of the ClassInfoAdapter class.
        // context: The activity context.
        // items: The list of class information items.

        public ClassInfoAdapter(Activity context, List<string> items)
        {
            this.context = context;
            this.items = items;
        }
         // Gets the number of items in the adapter.
        public override int Count
        {
            get { return items.Count; }
        }
        // Gets the item ID at the specified position.
        public override long GetItemId(int position)
        {
            return position;
        }
        // Gets the item at the specified position.
        public override string this[int position]
        {
            get { return items[position]; }
        }
        // Gets the view for the item at the specified position.
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
             // Inflate the list item layout if it is not recycled
            if (view == null)
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.list_item, null);
            }
             // Find the relevant text views in the list item layout
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
