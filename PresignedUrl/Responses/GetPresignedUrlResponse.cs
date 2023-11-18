namespace PresignedUrl.Responses
{
    public class GetPresignedUrlResponse
    {
        public bool Success { get; set; }
        public string? PresignedUrl { get; set; }
        public string? Filename { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
