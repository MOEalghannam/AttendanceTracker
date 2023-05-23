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
    public class Translations
    {
        public Translations()
        {

        }


        public static string ARtoENG(string input)
        {
            if (input == "الأحد")
            {
                return "Sunday";
            }
            else if (input == "الإثنين")
            {
                return "Monday";
            }
            else if (input == "الثلاثاء")
            {
                return "Tuesday";
            }
            else if (input == "الأربعاء")
            {
                return "Wednesday";
            }
            else if (input == "الخميس")
            {
                return "Thursday";
            }
            else if (input == "الجمعة")
            {
                return "Friday";
            }
            else if (input == "السبت")
            {
                return "Saturday";
            }
            else
            {
                return input;
            }
        }
        public static string ENGtoAR(string input)
        {
            if (input == "Sunday")
            {
                return "الأحد";
            }
            else if (input == "Monday")
            {
                return "الإثنين";
            }
            else if (input == "Tuesday")
            {
                return "الثلاثاء";
            }
            else if (input == "Wednesday")
            {
                return "الأربعاء";
            }
            else if (input == "الخميس")
            {
                return "Thursday";
            }
            else if (input == "الجمعة")
            {
                return "Friday";
            }
            else if (input == "السبت")
            {
                return "Saturday";
            }
            else
            {
                return input;
            }
        }
    }
}
