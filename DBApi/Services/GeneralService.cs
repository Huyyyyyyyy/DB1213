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

        // ========== Upload bài mới ==========
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

        // ========== Lấy danh sách bài đăng (có phân trang) ==========
        public async Task<PostListResponse> GetPosts(int page, int limit)
        {
            try
            {
                var repo = new MediaRepository(_db, _logger);
                var offset = (page - 1) * limit;

                var posts = await repo.GetAllMedia(limit, offset);
                var total = await repo.GetTotalCount();
                var totalPages = (int)Math.Ceiling((double)total / limit);

                return new PostListResponse
                {
                    code = 200,
                    message = "success",
                    data = posts,
                    pagination = new PaginationInfo
                    {
                        page = page,
                        limit = limit,
                        total = total,
                        totalPages = totalPages
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"{nameof(GetPosts)} - FAILED - details : {ex}");
                return new PostListResponse
                {
                    code = 500,
                    message = "failed to get posts"
                };
            }
        }

        // ========== Lấy chi tiết 1 bài đăng ==========
        public async Task<UploadResponse> GetPostById(Guid id)
        {
            try
            {
                var repo = new MediaRepository(_db, _logger);
                var post = await repo.GetMediaById(id);

                if (post == null)
                {
                    return new UploadResponse
                    {
                        code = 404,
                        message = "post not found"
                    };
                }

                return new UploadResponse
                {
                    code = 200,
                    message = "success",
                    data = post
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"{nameof(GetPostById)} - FAILED - details : {ex}");
                return new UploadResponse
                {
                    code = 500,
                    message = "failed to get post"
                };
            }
        }

        // ========== Xóa bài đăng ==========
        public async Task<UploadResponse> DeletePost(Guid id)
        {
            try
            {
                var repo = new MediaRepository(_db, _logger);
                
                // Lấy thông tin bài đăng trước để xóa file
                var post = await repo.GetMediaById(id);
                if (post == null)
                {
                    return new UploadResponse
                    {
                        code = 404,
                        message = "post not found"
                    };
                }

                // Xóa khỏi database
                var deleted = await repo.DeleteMedia(id);

                if (deleted)
                {
                    // Xóa file vật lý
                    var fileName = Path.GetFileName(post.med_path);
                    var fullPath = Path.Combine(Const.ROOT_MEDIA_DIRECTORY, fileName);
                    if (File.Exists(fullPath))
                    {
                        File.Delete(fullPath);
                    }
                }

                return new UploadResponse
                {
                    code = 200,
                    message = "deleted successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"{nameof(DeletePost)} - FAILED - details : {ex}");
                return new UploadResponse
                {
                    code = 500,
                    message = "failed to delete post"
                };
            }
        }

        // ========== Cập nhật bài đăng ==========
        public async Task<UploadResponse> UpdatePost(Guid id, string content)
        {
            try
            {
                var repo = new MediaRepository(_db, _logger);

                // Kiểm tra bài đăng có tồn tại không
                var post = await repo.GetMediaById(id);
                if (post == null)
                {
                    return new UploadResponse
                    {
                        code = 404,
                        message = "post not found"
                    };
                }

                // Cập nhật nội dung
                var updated = await repo.UpdateMedia(id, content);

                if (!updated)
                {
                    return new UploadResponse
                    {
                        code = 500,
                        message = "failed to update post"
                    };
                }

                // Lấy lại bài đăng sau khi cập nhật
                var updatedPost = await repo.GetMediaById(id);

                return new UploadResponse
                {
                    code = 200,
                    message = "updated successfully",
                    data = updatedPost
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"{nameof(UpdatePost)} - FAILED - details : {ex}");
                return new UploadResponse
                {
                    code = 500,
                    message = "failed to update post"
                };
            }
        }
    }
}
