using DBDatabase.Entities.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBDatabase.Entities.Contributor
{
    public class ContributorRow
    {
        public Guid cont_id {  get; set; }
        public string cont_username { get; set; } = string.Empty;
        public string? cont_nickname { get; set; } = null;
        public string? cont_x_link { get; set; } = null;
        public string? cont_image_url {  get; set; } = null;
        public string? cont_note {  get; set; } = null;
        public string? cont_wallet_address { get; set; } = null;
        public DateTime cont_created_at { get; set; }
        public bool cont_soft_deleted { get; set; }
        public static ContributorRow mapToRow(
            string cont_username, 
            string? cont_nickname, 
            string? cont_x_link,
            string? cont_image_url,
            string? cont_note,
            string? cont_wallet_address)
        {
            return
            new ContributorRow()
            {
                cont_id = Guid.NewGuid(),
                cont_username = cont_username,
                cont_nickname = cont_nickname,
                cont_x_link = cont_x_link,
                cont_image_url = cont_image_url,
                cont_note = cont_note,
                cont_wallet_address = cont_wallet_address,
                cont_created_at = DateTime.UtcNow,
                cont_soft_deleted = false
            };
        }
        public static ContributorRow mapFromReader(System.Data.IDataReader reader)
        {
            return new ContributorRow
            {
                cont_id = reader.GetGuid(reader.GetOrdinal("cont_id")),
                cont_username = reader.GetString(reader.GetOrdinal("cont_username")),
                cont_nickname = reader.IsDBNull(reader.GetOrdinal("cont_nickname")) ? null : reader.GetString(reader.GetOrdinal("cont_nickname")),
                cont_x_link = reader.IsDBNull(reader.GetOrdinal("cont_x_link")) ? null : reader.GetString(reader.GetOrdinal("cont_x_link")),
                cont_image_url = reader.IsDBNull(reader.GetOrdinal("cont_image_url")) ? null :reader.GetString(reader.GetOrdinal("cont_image_url")),
                cont_note = reader.IsDBNull(reader.GetOrdinal("cont_note")) ? null :reader.GetString(reader.GetOrdinal("cont_note")),
                cont_wallet_address = reader.IsDBNull(reader.GetOrdinal("cont_wallet_address")) ? null : reader.GetString(reader.GetOrdinal("cont_wallet_address")),
                cont_created_at = reader.GetDateTime(reader.GetOrdinal("cont_created_at")),
                cont_soft_deleted = reader.GetBoolean(reader.GetOrdinal("cont_soft_deleted")),
            };
        }
    }
}
