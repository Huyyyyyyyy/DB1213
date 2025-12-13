using DBApi.DTOs.Responses;
using DBDatabase;
using DBDatabase.Entities;
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
        public async Task<UploadResponse> SaveFile(IFormFile file, string content)
        {
            try
            {
                if (!Directory.Exists(Const.ROOT_MEDIA_DIRECTORY))
                    Directory.CreateDirectory(Const.ROOT_MEDIA_DIRECTORY);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var fullPath = Path.Combine(Const.ROOT_MEDIA_DIRECTORY, fileName);

                await using var stream = new FileStream(fullPath, FileMode.Create);
                await file.CopyToAsync(stream);

                var repo = new MediaRepository(_db, _logger);

                var row = MediaRow.mapToRow(
                    ((Const.FileType)MediaRow.toFileType(file.ContentType)!).toDescription(),
                    content,
                    Const.getFilePath(fileName)
                );

                await repo.InsertNewMedia(row);

                return new UploadResponse
                {
                    code = 200,
                    message = "success",
                    data = row
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"{nameof(SaveFile)} - FAILED - details : {ex}");
                return new UploadResponse
                {
                    code = 500,
                    message = "upload failed"
                };
            }
        }
    }
}
