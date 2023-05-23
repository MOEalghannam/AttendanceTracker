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

    public class ClassAttendanceService : Service
    {

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            var notificationManager = GetSystemService(NotificationService) as NotificationManager;

            NotificationChannel channel = new NotificationChannel("ClassAttendanceChannel", "Class Attendance Channel", NotificationImportance.Default)
            {
                Description = "Channel for class attendance notifications"
            };

            // Register the channel with the system
            notificationManager.CreateNotificationChannel(channel);

            // Create the notification
            Notification notification = new Notification.Builder(this, "ClassAttendanceChannel")
                .SetContentTitle("Class Attendance Service")
                .SetContentText("Running...")
                .SetSmallIcon(Resource.Drawable.ic_notification)
                .Build();

            try
            {
                // Start the service
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
            classDb = new ClassInfoOperations();
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public async Task Start()
        {

            while (true)
            {
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

                        // Update attendance counts
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

                // Wait 30 minutes before checking again
                await Task.Delay(TimeSpan.FromMinutes(2));
            }
        }
    }

}