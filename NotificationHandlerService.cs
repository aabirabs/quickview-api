using System.Web;

public interface INotificationHandlerService
{
    string HandleNotification(string notification);
}

public class NotificationHandlerService : INotificationHandlerService
{
    public string HandleNotification(string notification)
    {
        // Process the notification here
        // For example, you can deserialize the notification and perform necessary actions
        string decodedPayload = HttpUtility.UrlDecode(notification);

        Console.WriteLine("----------------------------------");
        Console.WriteLine(decodedPayload);
       
        // Return a response if needed
        return "Notification received and processed successfully.";
    }
}
