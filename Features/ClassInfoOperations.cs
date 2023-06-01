/* This is a class that defines operations for managing class information in a SQLite database. It
includes methods for inserting, updating, and deleting classes, as well as retrieving all classes or
a specific class by name. The `ClassInfo` class is also defined within this class, which represents
the structure of the data stored in the database.  */
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
        // Path to the database file
        private readonly string dbPath = Path.Combine(System.Environment.GetFolderPath(
            System.Environment.SpecialFolder.Personal), "classinfo13.db3");

        // Constructor
        public ClassInfoOperations()
        {
            //Creating database, if it doesn't already exist 
            if (!File.Exists(dbPath))
            {
                var db = new SQLiteConnection(dbPath);
                db.CreateTable<ClassInfo>();
                
            }
        }
        // Insert a class into the database
        public void InsertClass(ClassInfo Class)
        {
            var db = new SQLiteConnection(dbPath);
            db.Insert(Class);
        }
        // Update a class in the database
        public void UpdateClass(ClassInfo Class)
        {
            var db = new SQLiteConnection(dbPath);
            db.Update(Class);
        }

        // Delete a class from the database
        public void DeleteClass(ClassInfo Class)
        {
            var db = new SQLiteConnection(dbPath);
            db.Delete(Class);
        }
        // Retrieve all classes from the database
        public List<ClassInfo> GetClasses()
        {
            using (var db = new SQLiteConnection(dbPath))
            {
                // Select all records from the ClassInfo table
                return db.Table<ClassInfo>().ToList();
            }
        }
        // Retrieve data from the database as a string List
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
        // Retrieve a class by its name from the database
        public ClassInfo GetClassByName(string classname)
        {
            var db = new SQLiteConnection(dbPath);

            return db.Table<ClassInfo>().Where(i => i.ClassName == classname).FirstOrDefault();
        }
        // ClassInfo table definition
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