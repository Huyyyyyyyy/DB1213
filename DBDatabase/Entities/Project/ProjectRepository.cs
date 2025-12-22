using DBDatabase.Entities.Media;
using DBUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBDatabase.Entities.Project
{
    public class ProjectRepository
    {
        private readonly DBExecutor _db;
        private readonly SimpleLogger _logger;

        public ProjectRepository(DBExecutor db, SimpleLogger logger)
        {
            _db = db;
            _logger = logger;
        }
        public async Task InsertNewProject(ProjectRow m)
        {
            string sql = "INSERT INTO projects (proj_id, proj_name, proj_type, proj_img, proj_official_site, proj_other_site, proj_created_at) VALUES (@proj_id, @proj_name, @proj_type, @proj_img, @proj_official_site, @proj_other_site, @proj_created_at)";
            var parameters = new Dictionary<string, object?>
            {
                [nameof(m.proj_id)] = m.proj_id,
                [nameof(m.proj_name)] = m.proj_name,
                [nameof(m.proj_type)] = m.proj_type,
                [nameof(m.proj_img)] = m.proj_img,
                [nameof(m.proj_official_site)] = m.proj_official_site,
                [nameof(m.proj_other_site)] = m.proj_other_site,
                [nameof(m.proj_created_at)] = m.proj_created_at,
            };
            await _db.ExecuteNonQueryAsync(sql, parameters);
        }

        public async Task<List<ProjectRow>> GetAllProject(int limit = 50, int offset = 0)
        {
            string sql = @"
                SELECT proj_id, proj_name, proj_type, proj_img, proj_official_site, proj_other_site, proj_created_at
                FROM projects 
                ORDER BY proj_created_at DESC
                LIMIT @limit OFFSET @offset";
            var parameters = new Dictionary<string, object?>
            {
                [nameof(limit)] = limit,
                [nameof(offset)] = offset
            };
            return await _db.ExecuteQueryAsync(sql, ProjectRow.mapFromReader, parameters);
        }

        public async Task<ProjectRow?> GetProjectById(Guid proj_id)
        {
            string sql = @"
                SELECT proj_id, proj_name, proj_type, proj_img, proj_official_site, proj_other_site, proj_created_at 
                FROM projects 
                WHERE proj_id = @proj_id";
            var parameters = new Dictionary<string, object?> { [nameof(proj_id)] = proj_id };
            return await _db.ExecuteSingleQueryAsync(sql, ProjectRow.mapFromReader, parameters);
        }

        public async Task<bool> DeleteProject(Guid proj_id)
        {
            string sql = "DELETE FROM projects WHERE proj_id = @proj_id";
            var parameters = new Dictionary<string, object?> { ["proj_id"] = proj_id };
            var rows = await _db.ExecuteNonQueryAsync(sql, parameters);
            return rows > 0;
        }
    }
}
