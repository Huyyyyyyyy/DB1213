using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBDatabase.Entities.Claim;
using DBDatabase.Entities.Contributor;
using DBUtils;

namespace DBDatabase.Entities.User
{
    public class UserRepository
    {
        private readonly DBExecutor _db;
        private readonly SimpleLogger _logger;

        public UserRepository(DBExecutor db, SimpleLogger logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task Insert(UserRow m)
        {
            string sql =
"INSERT INTO users (usr_id, usr_wallet_address, usr_twitter_id, usr_points, usr_streak, usr_latest_claim_date, usr_created_at) " +
"VALUES(@usr_id, @usr_wallet_address, @usr_twitter_id, @usr_points, @usr_streak, @usr_latest_claim_date, @usr_created_at)";
            var parameters = new Dictionary<string, object?>
            {
                [nameof(m.usr_id)] = m.usr_id,
                [nameof(m.usr_wallet_address)] = m.usr_wallet_address,
                [nameof(m.usr_twitter_id)] = m.usr_twitter_id,
                [nameof(m.usr_points)] = m.usr_points,
                [nameof(m.usr_streak)] = m.usr_streak,
                [nameof(m.usr_latest_claim_date)] = m.usr_latest_claim_date,
                [nameof(m.usr_created_at)] = m.usr_created_at,
            };
            await _db.ExecuteNonQueryAsync(sql, parameters);
        }
        public async Task<UserRow?> GetUserById(Guid usr_id)
        {
            string sql = @"
                SELECT *
                FROM users
                WHERE usr_id = @usr_id;";
            var parameters = new Dictionary<string, object?>
            {
                [nameof(usr_id)] = usr_id,
            };
            return await _db.ExecuteSingleQueryAsync(sql, UserRow.mapFromReader, parameters);
        }

        public async Task<bool> Update(UserRow u)
        {
            string sql = "UPDATE users " +
            "SET usr_wallet_address = @usr_wallet_address," +
            "usr_twitter_id = @usr_twitter_id," +
            "usr_points = @usr_points," +
            "usr_streak = @usr_streak," +
            "usr_latest_claim_date = @usr_latest_claim_date," +
            "WHERE usr_id = @usr_id";
            var parameters = new Dictionary<string, object?>
            {
                [nameof(u.usr_wallet_address)] = u.usr_wallet_address,
                [nameof(u.usr_twitter_id)] = u.usr_twitter_id,
                [nameof(u.usr_points)] = u.usr_points,
                [nameof(u.usr_streak)] = u.usr_streak,
                [nameof(u.usr_latest_claim_date)] = u.usr_latest_claim_date,
                [nameof(u.usr_id)] = u.usr_id,
            };
            var rows = await _db.ExecuteNonQueryAsync(sql, parameters);
            return rows > 0;
        }
    }
}
