using DBApi.DTOs.Responses;
using DBDatabase;
using DBDatabase.Entities.Media;
using DBDatabase.Entities.Project;
using DBUtils;

namespace DBApi.Services
{
    public class ProjectService
    {
        private readonly DBExecutor _db;
        private readonly SimpleLogger _logger;
        public ProjectService(DBExecutor db, SimpleLogger logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<AddProjectResponse> AddProject(string proj_name, string proj_type, string proj_img, string proj_official_site, string proj_other_site)
        {
            try
            {
                var repo = new ProjectRepository(_db, _logger);
                ProjectRow r = ProjectRow.mapToRow(proj_name, proj_type, proj_img, proj_official_site, proj_other_site);
                await repo.InsertNewProject(r);
                return new AddProjectResponse
                {
                    code = 200,
                    message = "success",
                    data = r
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"{nameof(AddProject)} - FAILED - details : {ex}");
                return new AddProjectResponse
                {
                    code = 500,
                    message = "failed to update post"
                };
            }
        }
    }
}
