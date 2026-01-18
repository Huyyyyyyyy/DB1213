namespace DBAdmin.DTOs.Responses
{
    public class GetUserResponse
    {
        public int code = 0;
        public string message { get; set; } = string.Empty;
        public object? data { get; set; }
    }
}
