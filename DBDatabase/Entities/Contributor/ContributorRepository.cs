using DBDatabase.Entities.Media;
using DBUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBDatabase.Entities.Contributor
{
    public class ContributorRepository
    {
        private readonly DBExecutor _db;
        private readonly SimpleLogger _logger;

        public ContributorRepository(DBExecutor db, SimpleLogger logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task InsertNewContributor(ContributorRow m)
        {
            string sql = 
"INSERT INTO contributors (cont_id, cont_username, cont_nickname, cont_x_link, cont_image_url, cont_note, cont_wallet_address, cont_created_at, cont_soft_deleted) " +
"VALUES(@cont_id, @cont_username, @cont_nickname, @cont_x_link, @cont_image_url, @cont_note, @cont_wallet_address, NOW(), @cont_soft_deleted)";
            var parameters = new Dictionary<string, object?>
            {
                [nameof(m.cont_id)] = m.cont_id,
                [nameof(m.cont_username)] = m.cont_username,
                [nameof(m.cont_nickname)] = m.cont_nickname,
                [nameof(m.cont_x_link)] = m.cont_x_link,
                [nameof(m.cont_image_url)] = m.cont_image_url,
                [nameof(m.cont_note)] = m.cont_note,
                [nameof(m.cont_wallet_address)] = m.cont_wallet_address,
                [nameof(m.cont_soft_deleted)] = m.cont_soft_deleted,
            };
            await _db.ExecuteNonQueryAsync(sql, parameters);
        }
        public async Task<long> GetTotalCount()
        {
            string sql = "SELECT COUNT(*) FROM contributors";
            var result = await _db.ExecuteSingleQueryAsync(sql, r => r.GetInt64(0));
            return result;
        }
        public async Task<List<ContributorRow>> GetAllContributor(int limit = 50, int offset = 0)
        {
            string sql = @"
                SELECT cont_id, cont_username, cont_nickname, cont_x_link, cont_image_url, cont_note, cont_wallet_address, cont_created_at
                FROM contributors
                ORDER BY cont_created_at DESC 
                LIMIT @limit OFFSET @offset";
            var parameters = new Dictionary<string, object?>
            {
                ["limit"] = limit,
                ["offset"] = offset
            };
            return await _db.ExecuteQueryAsync(sql, ContributorRow.mapFromReader, parameters);
        }

        public async Task<ContributorRow?> GetContributorById(Guid cont_id)
        {
            string sql = @"
                SELECT *
                FROM contributors 
                WHERE cont_id = @cont_id";
            var parameters = new Dictionary<string, object?> { [nameof(cont_id)] = cont_id };
            return await _db.ExecuteSingleQueryAsync(sql, ContributorRow.mapFromReader, parameters);
        }

        public async Task<bool> SoftDeleteContributor(Guid cont_id)
        {
            string sql = "UPDATE contributors SET cont_soft_deleted = true WHERE cont_id = @cont_id";
            var parameters = new Dictionary<string, object?>
            {
                ["cont_id"] = cont_id,
            };
            var rows = await _db.ExecuteNonQueryAsync(sql, parameters);
            return rows > 0;
        }
    }
}
