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
    public  class AttenNotifications
    {
        public AttenNotifications() { 
        
        }
        public static void SendNotification(Context context, string title, string message)
        {
            // Get the NotificationManager service
            NotificationManager notificationManager = (NotificationManager)context.GetSystemService(Context.NotificationService);

            // Create the NotificationChannel
            NotificationChannel channel = new NotificationChannel("AttenChannel", "Atten Channel", NotificationImportance.High)
            {
                Description = "AttenChannel"
            };

            // Register the channel with the system
            notificationManager.CreateNotificationChannel(channel);

            Intent intent = new Intent(context, typeof(ShowAttendenceActivity));
            PendingIntent pendingIntent = PendingIntent.GetActivity(context, 0, intent, PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);

            // Create the notification
            Notification notification = new Notification.Builder(context, "AttenChannel")
                .SetContentTitle(title)
                .SetContentText(message)
                .SetSmallIcon(Resource.Drawable.ic_notification)
                .SetPriority((int)NotificationPriority.High)
                .SetAutoCancel(true)
                .SetContentIntent(pendingIntent)
                .Build();

            try
            {
                // Send the notification
                notificationManager.Notify(2, notification);
            }
            catch (Exception e)
            {
                // Display an error message if the notification fails to send
                Toast.MakeText(context, "Error sending notification: " + e.Message, ToastLength.Long).Show();
                Console.WriteLine($"Error: {e.Message}");
            }
        }
    }
}