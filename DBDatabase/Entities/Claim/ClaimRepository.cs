using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBDatabase.Entities.Contributor;
using DBDatabase.Entities.Media;
using DBUtils;

namespace DBDatabase.Entities.Claim
{
    public class ClaimRepository
    {
        private readonly DBExecutor _db;
        private readonly SimpleLogger _logger;

        public ClaimRepository(DBExecutor db, SimpleLogger logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task Insert(ClaimRow m)
        {
            string sql =
"INSERT INTO claims (clms_id, clms_user_id, clms_created_at, clms_points, clms_streak, clms_bonus) " +
"VALUES(@clms_id, @clms_user_id, @clms_created_at, @clms_points, @clms_streak, @clms_bonus)";
            var parameters = new Dictionary<string, object?>
            {
                [nameof(m.clms_id)] = m.clms_id,
                [nameof(m.clms_user_id)] = m.clms_user_id,
                [nameof(m.clms_created_at)] = m.clms_created_at,
                [nameof(m.clms_points)] = m.clms_points,
                [nameof(m.clms_streak)] = m.clms_streak,
                [nameof(m.clms_bonus)] = m.clms_bonus,
            };
            await _db.ExecuteNonQueryAsync(sql, parameters);
        }
        public async Task<List<ClaimRow>> GetAllByUserId(Guid clms_user_id)
        {
            string sql = @"
                SELECT *
                FROM claims
                WHERE clms_user_id = @clms_user_id;";
            var parameters = new Dictionary<string, object?>
            {
                [nameof(clms_user_id)] = clms_user_id,
            };
            return await _db.ExecuteQueryAsync(sql, ClaimRow.mapFromReader, parameters);
        }
    }
}
