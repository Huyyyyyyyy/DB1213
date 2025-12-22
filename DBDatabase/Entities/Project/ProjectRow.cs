using DBDatabase.Entities.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DBUtils.Const;

namespace DBDatabase.Entities.Project
{
    public class ProjectRow
    {

        public Guid proj_id {get; set;}
        public string proj_name {get; set;} = string.Empty;
        public string proj_type {get; set;} = string.Empty;
        public string proj_img {get; set;} = string.Empty;
        public string proj_official_site {get; set;} = string.Empty;
        public string proj_other_site { get; set; } = string.Empty;
        public DateTime proj_created_at { get; set; }

        public static ProjectRow mapToRow(string proj_name, string proj_type, string proj_img, string proj_official_site, string proj_other_site)
        {
            return
            new ProjectRow()
            {
                proj_id = Guid.NewGuid(),
                proj_name = proj_name,
                proj_type = proj_type,
                proj_img = proj_img,
                proj_official_site = proj_official_site,
                proj_other_site = proj_other_site,
                proj_created_at = DateTime.UtcNow
            };
        }

        public static ProjectRow mapFromReader(System.Data.IDataReader reader)
        {
            return new ProjectRow
            {
                proj_id = reader.GetGuid(reader.GetOrdinal("proj_id")),
                proj_name = reader.GetString(reader.GetOrdinal("proj_name")),
                proj_type = reader.GetString(reader.GetOrdinal("proj_type")),
                proj_img = reader.GetString(reader.GetOrdinal("proj_img")),
                proj_official_site = reader.GetString(reader.GetOrdinal("proj_official_site")),
                proj_other_site = reader.GetString(reader.GetOrdinal("proj_other_site")),
                proj_created_at = reader.GetDateTime(reader.GetOrdinal("proj_created_at"))
            };
        }
    }
}
