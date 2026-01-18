using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBDatabase.Entities.Contributor;

namespace DBDatabase.Entities.Claim
{
    public class ClaimRow
    {
        public Guid clms_id {get; set;}
        public Guid clms_user_id {get; set;}
        public DateTime clms_created_at {get; set;}
        public int clms_points {get; set;}
        public int clms_streak {get; set;}
        public int clms_bonus { get; set; }
        public static ClaimRow mapToRow(Guid clms_user_id, int clms_points, int clms_streak, int clms_bonus)
        {
            return
            new ClaimRow()
            {
                clms_id = Guid.NewGuid(),
                clms_created_at = DateTime.UtcNow,
                clms_user_id = clms_user_id,
                clms_points = clms_points,
                clms_streak = clms_streak,
                clms_bonus = clms_bonus
            };
        }
        public static ClaimRow mapFromReader(System.Data.IDataReader reader)
        {
            return new ClaimRow
            {
                clms_id = reader.GetGuid(reader.GetOrdinal("clms_id")),
                clms_created_at = reader.GetDateTime(reader.GetOrdinal("clms_created_at")),
                clms_user_id = reader.GetGuid(reader.GetOrdinal("clms_user_id")),
                clms_points = reader.GetInt32(reader.GetOrdinal("clms_points")),
                clms_streak = reader.GetInt32(reader.GetOrdinal("clms_streak")),
                clms_bonus = reader.GetInt32(reader.GetOrdinal("clms_bonus")),
            };
        }
    }
}
