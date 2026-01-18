
namespace DBDatabase.Entities.User
{
    public class UserRow
    {
        public Guid usr_id  { get; set; }
        public string? usr_wallet_address { get; set; } = null;
        public string? usr_twitter_id {get; set;} = null;
        public int usr_points { get; set; }
        public int usr_streak { get; set; }
        public DateTime? usr_latest_claim_date { get; set; } = null;
        public DateTime usr_created_at { get; set; }
        public static UserRow mapToRow(
            string? usr_wallet_address,
            string? usr_twitter_id)
        {
            return
            new UserRow()
            {
                usr_id = Guid.NewGuid(),
                usr_wallet_address = usr_wallet_address,
                usr_twitter_id = usr_twitter_id,
                usr_points = 0,
                usr_streak = 0,
                usr_latest_claim_date = null,
                usr_created_at = DateTime.UtcNow
            };
        }
        public static UserRow mapFromReader(System.Data.IDataReader reader)
        {
            return new UserRow
            {
                usr_id = reader.GetGuid(reader.GetOrdinal("usr_id")),
                usr_wallet_address = reader.GetString(reader.GetOrdinal("usr_wallet_address")),
                usr_twitter_id = reader.GetString(reader.GetOrdinal("usr_twitter_id")),
                usr_points = reader.GetInt32(reader.GetOrdinal("usr_points")),
                usr_streak = reader.GetInt32(reader.GetOrdinal("usr_streak")),
                usr_latest_claim_date = reader.IsDBNull(reader.GetOrdinal("usr_latest_claim_date")) ? null : reader.GetDateTime(reader.GetOrdinal("usr_latest_claim_date")),
                usr_created_at = reader.GetDateTime(reader.GetOrdinal("usr_created_at")),
            };
        }

    }
}
