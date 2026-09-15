namespace api.Services.Storage
{
    public sealed class R2Options
    {
        public string AccountId { get; set; } = "";

        public string AccessKey { get; set; } = "";

        public string SecretKey { get; set; } = "";

        public string BucketName { get; set; } = "";
    }
}
