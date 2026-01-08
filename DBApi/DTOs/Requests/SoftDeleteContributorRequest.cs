using Microsoft.AspNetCore.Mvc;

namespace DBApi.DTOs.Requests
{
    public class SoftDeleteContributorRequest
    {
        [FromRoute]
        public Guid cont_id { get; set; }
    }
}
