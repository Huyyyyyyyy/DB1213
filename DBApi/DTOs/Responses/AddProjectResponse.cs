namespace DBApi.DTOs.Responses
{
    public class AddProjectResponse
    {
        public int code = 0;
        public string message { get; set; } = string.Empty;
        public object? data { get; set; }
    }
}
