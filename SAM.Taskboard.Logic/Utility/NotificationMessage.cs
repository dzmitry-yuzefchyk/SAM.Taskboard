namespace SAM.Taskboard.Logic.Utility
{
    public class NotificationMessage
    {
        public NotificationType NotificationType { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Link { get; set; }
        public int ObjectId { get; set; }
        public string Initiator { get; set; }
        public string SendTo { get; set; }
        public string AdditionalSendTo { get; set; }
        public NotificationMessage AdditionalSendToMessage { get; set; }
    }

    public enum NotificationType
    {
        Changed = 0,

        Deleted = 3,

        Commented = 4,

        Assigned = 5
    }
}
