using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBUtils;

namespace DBDatabase.Entities.Media
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

        public async Task<List<MediaRow>> GetAllMedia(int limit = 50, int offset = 0)
        {
            string sql = @"
                SELECT med_id, med_file_type, med_content, med_path, med_created_at 
                FROM medias 
                ORDER BY med_created_at DESC 
                LIMIT @limit OFFSET @offset";
            var parameters = new Dictionary<string, object?>
            {
                ["limit"] = limit,
                ["offset"] = offset
            };
            return await _db.ExecuteQueryAsync(sql, MediaRow.mapFromReader, parameters);
        }

        public async Task<MediaRow?> GetMediaById(Guid med_id)
        {
            string sql = @"
                SELECT med_id, med_file_type, med_content, med_path, med_created_at 
                FROM medias 
                WHERE med_id = @med_id";
            var parameters = new Dictionary<string, object?> { [nameof(med_id)] = med_id };
            return await _db.ExecuteSingleQueryAsync(sql, MediaRow.mapFromReader, parameters);
        }

        public async Task<bool> DeleteMedia(Guid id)
        {
            string sql = "DELETE FROM medias WHERE med_id = @id";
            var parameters = new Dictionary<string, object?> { ["id"] = id };
            var rows = await _db.ExecuteNonQueryAsync(sql, parameters);
            return rows > 0;
        }

        public async Task<long> GetTotalCount()
        {
            string sql = "SELECT COUNT(*) FROM medias";
            var result = await _db.ExecuteSingleQueryAsync(sql, r => r.GetInt64(0));
            return result;
        }

        public async Task<bool> UpdateMedia(Guid id, string content)
        {
            string sql = "UPDATE medias SET med_content = @content WHERE med_id = @id";
            var parameters = new Dictionary<string, object?>
            {
                ["id"] = id,
                ["content"] = content
            };
            var rows = await _db.ExecuteNonQueryAsync(sql, parameters);
            return rows > 0;
        }
    }
}
