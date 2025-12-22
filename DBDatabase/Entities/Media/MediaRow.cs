using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DBUtils.Const;

namespace DBDatabase.Entities.Media
{
    public class MediaRow
    {
        public Guid med_id { get; set; }
        public string med_file_type { get; set; } = string.Empty;
        public string med_content { get; set; } = string.Empty;
        public string med_path { get; set; } = string.Empty;
        public DateTime med_created_at { get; set; }

        public static MediaRow mapToRow(string med_file_type, string med_content, string med_path)
        {
            return
            new MediaRow()
            {
                med_id = Guid.NewGuid(),
                med_file_type = med_file_type,
                med_content = med_content,
                med_path = med_path,
                med_created_at = DateTime.UtcNow
            };
        }
        public static FileType? toFileType(string file_type_string)
        {
            string rs = file_type_string.Trim().ToLowerInvariant().Replace("/", "_");
            return Enum.TryParse(rs, out FileType e) ? e : null;
        }
        public static MediaRow mapFromReader(System.Data.IDataReader reader)
        {
            return new MediaRow
            {
                med_id = reader.GetGuid(reader.GetOrdinal("med_id")),
                med_file_type = reader.GetString(reader.GetOrdinal("med_file_type")),
                med_content = reader.GetString(reader.GetOrdinal("med_content")),
                med_path = reader.GetString(reader.GetOrdinal("med_path")),
                med_created_at = reader.GetDateTime(reader.GetOrdinal("med_created_at"))
            };
        }
    }
}
