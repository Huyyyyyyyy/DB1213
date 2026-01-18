using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBDatabase.Entities.Claim;

namespace DBDatabase.Entities.Referral
{
    public class ReferralRow
    {
       public Guid ref_id {get; set;}
       public Guid ref_referrer_id {get; set;}
       public Guid ref_referred_id {get; set;}
       public int ref_earned {get; set;}
       public DateTime ref_created_at { get; set; }
        public static ReferralRow mapToRow(Guid ref_referrer_id, Guid ref_referred_id, int ref_earned )
        {
            return
            new ReferralRow()
            {
                ref_id = Guid.NewGuid(),
                ref_referrer_id = ref_referrer_id,
                ref_referred_id = ref_referred_id,
                ref_earned = ref_earned,
                ref_created_at = DateTime.UtcNow,
            };
        }
        public static ReferralRow mapFromReader(System.Data.IDataReader reader)
        {
            return new ReferralRow
            {
                ref_id = reader.GetGuid(reader.GetOrdinal("ref_id")),
                ref_referrer_id = reader.GetGuid(reader.GetOrdinal("ref_referrer_id")),
                ref_referred_id = reader.GetGuid(reader.GetOrdinal("ref_referred_id")),
                ref_earned = reader.GetInt32(reader.GetOrdinal("ref_earned")),
                ref_created_at = reader.GetDateTime(reader.GetOrdinal("ref_created_at")),
            };
        }
    }
}
