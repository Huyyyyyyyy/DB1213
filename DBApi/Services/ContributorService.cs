using DBApi.DTOs.Responses;
using DBDatabase;
using DBDatabase.Entities.Contributor;
using DBDatabase.Entities.Media;
using DBDatabase.Entities.Project;
using DBUtils;

namespace DBApi.Services
{
    public class ContributorService
    {
        private readonly DBExecutor _db;
        private readonly SimpleLogger _logger;
        public ContributorService(DBExecutor db, SimpleLogger logger)
        {
            _db = db;
            _logger = logger;
        }
        public async Task<GetContributorResponse> GetContributors(int page, int limit)
        {
            try
            {
                var repo = new ContributorRepository(_db, _logger);
                var offset = (page - 1) * limit;

                var contributors = await repo.GetAllContributor(limit, offset);
                var total = await repo.GetTotalCount();
                var totalPages = (int)Math.Ceiling((double)total / limit);

                return new GetContributorResponse
                {
                    code = 200,
                    message = "success",
                    data = contributors,
                    pagination = new PaginationInfo
                    {
                        page = page,
                        limit = limit,
                        total = total,
                        totalPages = totalPages
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"{nameof(GetContributors)} - FAILED - details : {ex}");
                return new GetContributorResponse
                {
                    code = 500,
                    message = "failed to get contributors"
                };
            }
        }

        public async Task<AddContributorResponse> AddContributor(
            string cont_username,
            string? cont_nickname,
            string? cont_x_link,
            string? cont_image_url,
            string? cont_note,
            string? cont_wallet_address)
        {
            try
            {
                var repo = new ContributorRepository(_db, _logger);
                ContributorRow r = ContributorRow.mapToRow(cont_username, cont_nickname, cont_x_link, cont_image_url, cont_note, cont_wallet_address);
                await repo.InsertNewContributor(r);
                return new AddContributorResponse
                {
                    code = 200,
                    message = "success",
                    data = r
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"{nameof(AddContributor)} - FAILED - details : {ex}");
                return new AddContributorResponse
                {
                    code = 500,
                    message = "failed to add contributor"
                };
            }
        }

        public async Task<SoftDeleteContributorResponse> SoftDeleteContributor(Guid cont_id)
        {
            try
            {
                var repo = new ContributorRepository(_db, _logger);
                var contributor = await repo.GetContributorById(cont_id);
                if (contributor == null)
                {
                    return new SoftDeleteContributorResponse
                    {
                        code = 404,
                        message = "contributor not found"
                    };
                }

                await repo.SoftDeleteContributor(cont_id);
                return new SoftDeleteContributorResponse
                {
                    code = 200,
                    message = "success",
                    data = contributor
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"{nameof(AddContributor)} - FAILED - details : {ex}");
                return new SoftDeleteContributorResponse
                {
                    code = 500,
                    message = "failed to soft delete contributor"
                };
            }
        }
    }
}
