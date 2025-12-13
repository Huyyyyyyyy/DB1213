using DBDatabase;
using DBUtils;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DBApi.Services
{
    public class GeneralService
    {
        private readonly DBExecutor _db;
        private readonly SimpleLogger _logger;
        public GeneralService(DBExecutor db, SimpleLogger logger)
        {
            _db = db;
            _logger = logger;
        }
        public async Task<string> SaveFile(IFormFile file)
        {
            var allowed = new[] { "image/jpeg", "image/png", "image/gif", "video/mp4", "video/mpeg" };
            if (!allowed.Contains(file.ContentType)) return "file type is not allowed";
            if (file.Length > 10 * 1024 * 1024) return "file size is exceed 10MB";

            var uploadRoot = "/var/www/app/uploads";
            if (!Directory.Exists(uploadRoot)) Directory.CreateDirectory(uploadRoot);

            var ext = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{ext}";
            var fullPath = Path.Combine(uploadRoot, fileName);

            // Save file to disk
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var dbPath = $"/upload/{fileName}";
            string sql = "INSERT INTO medias (med_id, med_path, med_file_type, med_content, med_created_at) VALUES (@med_id, @med_path, @med_file_type, @med_content, NOW())";
            var parameters = new Dictionary<string, object?>
            {
                ["med_id"] = Guid.NewGuid(),
                ["med_path"] = dbPath,
                ["med_file_type"] = file.ContentType,
                ["med_content"] = "here is the test content",
            };
            await _db.ExecuteNonQueryAsync(sql, parameters);
            _logger.Info($"File uploaded: {dbPath}");
            return dbPath;
        }
    }
}
