using DBDatabase.Entities;

namespace DBApi.DTOs.Responses
{
    public class GetPostResponse
    {
        public int code { get; set; }
        public string message { get; set; } = string.Empty;
        public List<MediaRow>? data { get; set; }
        public PaginationInfo? pagination { get; set; }
    }

    public class PaginationInfo
    {
        public int page { get; set; }
        public int limit { get; set; }
        public long total { get; set; }
        public int totalPages { get; set; }
    }
}
