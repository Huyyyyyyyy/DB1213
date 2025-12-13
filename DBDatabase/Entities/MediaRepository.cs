using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBUtils;

namespace DBDatabase.Entities
{
    public class MediaRepository
    {
        private readonly DBExecutor _db;
        private readonly SimpleLogger _logger;

        public MediaRepository(DBExecutor db, SimpleLogger logger)
        {
            _db = db;
            _logger = logger;
        }
        public async Task InsertNewMedia(MediaRow m)
        {
            string sql = "INSERT INTO medias (med_id, med_path, med_file_type, med_content, med_created_at) VALUES (@med_id, @med_path, @med_file_type, @med_content, NOW())";
            var parameters = new Dictionary<string, object?>
            {
                [nameof(m.med_id)] = m.med_id,
                [nameof(m.med_path)] = m.med_path,
                [nameof(m.med_file_type)] = m.med_file_type,
                [nameof(m.med_content)] = m.med_content,
                [nameof(m.med_created_at)] = m.med_created_at,
            };
            await _db.ExecuteNonQueryAsync(sql, parameters);
        }
    }
}
