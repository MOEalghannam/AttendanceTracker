/* The above code is implementing a background service in an Android app that takes attendance for
classes based on the user's location. The service runs continuously and checks for eligible classes
(classes that occur on the current day of the week and whose start time is 20 minutes or more ago).
If attendance has not been taken for the class that day, the service gets the user's current
location and determines if the user is within the specified range of the class location. If the user
is within range, the attendance count for the class is updated as "present". If the user is not
within range, the attendance */
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
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace AttendanceTracker
{
    [Service]
// Service that manages class attendance tracking
    public class ClassAttendanceService : Service
    {

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            
            // Getting the system service for notifications and casting it to a `NotificationManager` object.
            // This allows the app to create and manage notifications.
            var notificationManager = GetSystemService(NotificationService) as NotificationManager;
            // Create a notification channel for the service
            NotificationChannel channel = new NotificationChannel("ClassAttendanceChannel", "Class Attendance Channel", NotificationImportance.Default)
            {
                Description = "Channel for class attendance notifications"
            };

            // Register the channel with the system
            notificationManager.CreateNotificationChannel(channel);

            // Create the foreground notification
            Notification notification = new Notification.Builder(this, "ClassAttendanceChannel")
                .SetContentTitle("Class Attendance Service")
                .SetContentText("Running...")
                .SetSmallIcon(Resource.Drawable.ic_notification)
                .Build();

            try
            {
                // Start the service in the foreground with the notification
                StartForeground(1, notification);
                Start();
                return StartCommandResult.Sticky;
            }
            catch (Exception e)
            {
                // Display an error message if the service fails to start
                Toast.MakeText(this, "Error starting service: " + e.Message, ToastLength.Long).Show();
                return StartCommandResult.NotSticky;
            }
        }
        private ClassInfoOperations classDb;

        public ClassAttendanceService()
        {
            // Create an instance of the ClassInfoOperations class for managing class information in the database
            classDb = new ClassInfoOperations();
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }
        // Start the class attendance tracking
        public async Task Start()
        {

            while (true)
            {
                 // Get the current day of the week and time
                var currentDayOfWeek = DateTime.Now.DayOfWeek.ToString();
                var currentHour = DateTime.Now.TimeOfDay;
                var thirtyMinutes = TimeSpan.FromMinutes(20);

                // Get classes that occur on the current day of the week and whose start time is 20 minutes or more ago
                var eligibleClasses = classDb.GetClasses().Where(c => c.Day.Equals(currentDayOfWeek)
                    && (currentHour - c.StartTime) >= thirtyMinutes);

                foreach (var c in eligibleClasses)
                {
                    // If the attendance has not been taken for this class today, take attendance
                    if (c.Date.Date != DateTime.Now.Date)
                    {
                        // Get the current location
                        var location = await Geolocation.GetLocationAsync();
                        try
                        {
                            if (location != null)
                            {
                                System.Diagnostics.Debug.WriteLine($"Location: Working");
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine($"Error Location not working");
                            }
                        }

                        catch (FeatureNotSupportedException fnsEx)
                        {
                            Console.WriteLine("Location is not supported on this device");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }

                        var latitude = location.Latitude;
                        var longitude = location.Longitude;

                        // Determine if the user is within the specified range of the class location
                        var classLocation = new Location(c.Latitude, c.Longitude);
                        var userLocation = new Location(latitude, longitude);
                        var distance = Location.CalculateDistance(classLocation, userLocation, DistanceUnits.Kilometers);
                        var maxDistance = 0.02; // Maximum distance user can be from class location, in kilometers
                        var isWithinRange = distance <= maxDistance;

                        // Update attendance counts and send notifications
                        if (isWithinRange)
                        {
                            c.Present++;
                            AttenNotifications.SendNotification(ApplicationContext, GetString(Resource.String.Attendance_Marked), $"{GetString(Resource.String.PresentNoti)} {c.ClassName}");
                        }
                        else
                        {
                            c.Absent++;

                            double absenthours = c.Absent * c.ClassHours;
                            double percentage = ((absenthours / (c.SubjectHours * 13)) * 100);
                            if (percentage >= 14)
                            {
                                AttenNotifications.SendNotification(ApplicationContext, GetString(Resource.String.Attendance_Marked), $"{GetString(Resource.String.WarningP1)} {c.ClassName} {GetString(Resource.String.WarningP2)} {percentage.ToString("0.00")}%");
                            }
                            else if (percentage >= 20)
                            {
                                AttenNotifications.SendNotification(ApplicationContext, GetString(Resource.String.Attendance_Marked), $"{GetString(Resource.String.TeacherP1)} {c.ClassName} {GetString(Resource.String.TeacherP2)} {percentage.ToString("0.00")}%");
                            }
                            else
                            {
                                AttenNotifications.SendNotification(ApplicationContext, GetString(Resource.String.Attendance_Marked), $"{GetString(Resource.String.AbsentNotiP1)} {c.ClassName} {GetString(Resource.String.AbsentNotiP2)} {percentage.ToString("0.00")}%");
                            }
                        }

                        // Update the date of the last attendance taken
                        c.Date = DateTime.Now;
                        // Update the class in the database
                        classDb.UpdateClass(c);
                    }
                }

                // Wait 2 minutes before checking again
                await Task.Delay(TimeSpan.FromMinutes(2));
            }
        }
    }

}