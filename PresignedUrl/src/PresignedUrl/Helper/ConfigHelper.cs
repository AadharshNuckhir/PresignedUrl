namespace PresignedUrl.Helper
{
    public static class ConfigHelper
    {
        public static bool IsLoaded { get; set; } = false;
        public static Dictionary<string,string> storedParameters = new Dictionary<string,string>();
        
        public static string AWS_ACCESS_KEY_ID { get; set; } = "";
        public static string AWS_SECRET_ACCESS_KEY { get; set; } = "";
        public static string AWS_SESSION_TOKEN { get; set; } = "";
        public static string AWS_REGION { get; set; } = "";

        public static string S3_BUCKET_INVOICE_PSN { get; set; } = "";
        public static string S3_BUCKET_INVOICE { get; set; } = "";
        public static string TIME_TO_LIVE { get; set; } = "";


        public static void LoadEnvironmentVariablesAndParameters()
        {
            AWS_ACCESS_KEY_ID = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID") ?? string.Empty;
            AWS_SECRET_ACCESS_KEY = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY") ?? string.Empty;
            AWS_SESSION_TOKEN = Environment.GetEnvironmentVariable("AWS_SESSION_TOKEN") ?? string.Empty;
            AWS_REGION = Environment.GetEnvironmentVariable("AWS_REGION") ?? string.Empty;
            S3_BUCKET_INVOICE_PSN = Environment.GetEnvironmentVariable("S3_BUCKET_INVOICE_PSN") ?? string.Empty;
            S3_BUCKET_INVOICE = Environment.GetEnvironmentVariable("S3_BUCKET_INVOICE_PSN") ?? string.Empty;
            TIME_TO_LIVE = Environment.GetEnvironmentVariable("TIME_TO_LIVE") ?? string.Empty;

            var parameterNames = new List<string>()
            {
                S3_BUCKET_INVOICE_PSN
            };

            storedParameters = ParameterStoreHelper.GetParametersFromParameterStore(parameterNames);

            S3_BUCKET_INVOICE = storedParameters[S3_BUCKET_INVOICE_PSN];

            IsLoaded = true;
        }
    }
}
