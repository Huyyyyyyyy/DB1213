using DBDatabase.Entities.Contributor;
using DBDatabase.Entities.Media;

namespace DBApi.DTOs.Responses
{
    public class GetContributorResponse
    {
        public int code { get; set; }
        public string message { get; set; } = string.Empty;
        public List<ContributorRow>? data { get; set; }
        public PaginationInfo? pagination { get; set; }
    }
}
