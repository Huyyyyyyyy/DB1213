using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBDatabase.Entities.User;
using DBUtils;

namespace DBDatabase.Entities.Referral
{
    public class ReferralRepository
    {
        private readonly DBExecutor _db;
        private readonly SimpleLogger _logger;

        public ReferralRepository(DBExecutor db, SimpleLogger logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task Insert(ReferralRow m)
        {
            string sql =
"INSERT INTO referrals (ref_id, ref_referrer_id, ref_referred_id, ref_earned, ref_created_at) " +
"VALUES(@ref_id, @ref_referrer_id, @ref_referred_id, @ref_earned, @ref_created_at)";
            var parameters = new Dictionary<string, object?>
            {
                [nameof(m.ref_id)] = m.ref_id,
                [nameof(m.ref_referrer_id)] = m.ref_referrer_id,
                [nameof(m.ref_referred_id)] = m.ref_referred_id,
                [nameof(m.ref_earned)] = m.ref_earned,
                [nameof(m.ref_created_at)] = m.ref_created_at,
            };
            await _db.ExecuteNonQueryAsync(sql, parameters);
        }
    }
}
