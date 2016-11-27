namespace GoodVibrations.ApiClient
{
    public class PhoneCallRequest
    {
        public string ToPhoneNumber { get; set; }

        public string FromPhoneNumber { get; set; }

        public string Message { get; set; }

        public string CurrentLocation { get; set; }

    }
}
