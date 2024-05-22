namespace QuickFix.Services.EmailAPI.Models
{
    public class EmailLog
    {
        public int Id { get; set; }
        public string Sender { get; set; }

        public string Receiver { get; set; }
        public string Message { get; set; }
        public DateTime? EmailSent { get; set; }
    }
}
