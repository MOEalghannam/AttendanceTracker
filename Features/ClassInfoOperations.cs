using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AttendanceTracker
{
    internal class ClassInfoOperations
    {
        private readonly string dbPath = Path.Combine(System.Environment.GetFolderPath(
            System.Environment.SpecialFolder.Personal), "classinfo13.db3");


        public ClassInfoOperations()
        {
            //Creating database, if it doesn't already exist 
            if (!File.Exists(dbPath))
            {
                var db = new SQLiteConnection(dbPath);
                db.CreateTable<ClassInfo>();
                //db.CreateTable<Students>();
            }
        }

        public void InsertClass(ClassInfo Class)
        {
            var db = new SQLiteConnection(dbPath);
            db.Insert(Class);
        }
        // User Upbdate
        public void UpdateClass(ClassInfo Class)
        {
            var db = new SQLiteConnection(dbPath);
            db.Update(Class);
        }

        // User Delete
        public void DeleteClass(ClassInfo Class)
        {
            var db = new SQLiteConnection(dbPath);
            db.Delete(Class);
        }
        public List<ClassInfo> GetClasses()
        {
            using (var db = new SQLiteConnection(dbPath))
            {
                // Select all records from the ClassInfo table
                return db.Table<ClassInfo>().ToList();
            }
        }
        public List<string> GetData()
        {
            var db = new SQLiteConnection(dbPath);
            List<string> data = new List<string>();
            foreach (var item in db.Table<ClassInfo>())
            {
                var zad = item.ClassName.ToString() + item.Present.ToString() + item.Absent.ToString();

                data.Add(zad);
            }
            return data;
        }
        public ClassInfo GetClassByName(string classname)
        {
            var db = new SQLiteConnection(dbPath);

            return db.Table<ClassInfo>().Where(i => i.ClassName == classname).FirstOrDefault();
        }



        [Table("ClassInfo")]
        public class ClassInfo
        {
            [PrimaryKey, AutoIncrement]
            public int Id { get; set; }
            public string ClassName { get; set; }
            public string Day { get; set; }
            public int SubjectHours { get; set; }
            public int ClassHours { get; set; }
            public TimeSpan StartTime { get; set; }
            public TimeSpan EndTime { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public int Present { get; set; }
            public int Absent { get; set; }

            public DateTime Date { get; set; }
        }
    }

}