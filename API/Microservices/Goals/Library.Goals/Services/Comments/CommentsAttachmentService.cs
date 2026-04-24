using Library.Database.DAL;
using Library.Goals.DataAccess.Comments;
using Library.Goals.Models;
using Library.Goals.Models.Comments;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

using System.Text;

namespace Library.Goals.Services.Comments;

public interface ICommentsAttachmentService
{
    /// <summary>
    /// Uploads a PDF attachment for a comment, validates it, stores it in the file system,
    /// and updates the database with metadata.
    /// </summary>
    /// <param name="basic">Basic user context model.</param>
    /// <param name="commentUUID">UUID of the comment to which the file is attached.</param>
    /// <param name="file">The uploaded file.</param>
    /// <returns>Upload result with UUID and success status.</returns>
    Task<CommentAttachmentUploadResult?> UploadAttachment(BasicModel basic, string commentUUID, IFormFile file);

    /// <summary>
    /// Downloads an attachment using its UUID by reading from the storage path saved in database.
    /// </summary>
    /// <param name="attachmentUUID">UUID of the attachment to download.</param>
    /// <returns>A tuple containing file stream, MIME type, and file name.</returns>
    Task<(Stream Stream, string MimeType, string FileName)> DownloadAttachment(string attachmentUUID);
}

/// <summary>
/// Implementation of attachment upload/download logic.
/// Validates, stores, retrieves, and handles attachment metadata.
/// </summary>
public class CommentsAttachmentService : ICommentsAttachmentService
{
    private readonly ISqlDataAccess _sql;
    private readonly IConfiguration _config;

    public CommentsAttachmentService(ISqlDataAccess sql, IConfiguration config)
    {
        _sql = sql;
        _config = config;
    }

    /// <summary>
    /// Uploads an attachment (PDF only) for a comment with full validation and DB update.
    /// </summary>
    /// <summary>
    /// Uploads an attachment (any file type) for a comment with full validation and DB update.
    /// </summary>
    public async Task<CommentAttachmentUploadResult?> UploadAttachment(
        BasicModel basic, string commentUUID, IFormFile file)
    {
        try
        {
            // Ensure file provided
            if (file == null || file.Length == 0)
                throw new Exception("File is required.");

            // Read max file size from appsettings.json
            long maxSize = Convert.ToInt64(_config["Attachments:MaxSizeBytes"]);

            // Validate size
            if (file.Length > maxSize)
                throw new Exception($"File exceeds allowed size of {maxSize} bytes.");

            // ⚠ ALLOW ANY FILE TYPE — only record MIME type
            string mimeType = file.ContentType ?? "application/octet-stream";

            // Read root folder path
            string storageRoot = _config["Documents:Local"];
            if (string.IsNullOrWhiteSpace(storageRoot))
                throw new Exception("Documents:Local path missing.");

            // Build folder path for this comment
            string folder = Path.Combine(storageRoot, "comments", commentUUID);
            Directory.CreateDirectory(folder);

            // Sanitize file name
            string safeName = Path.GetFileName(file.FileName);

            // Insert DB record BEFORE writing file
            var createResult = await CommentsAttachmentDataAccess.Upsert(
                _sql, basic, null, commentUUID, safeName, "", 0, mimeType);

            if (createResult?.isValid != true || string.IsNullOrWhiteSpace(createResult.UUID))
                throw new Exception(createResult?.Message ?? "Unable to create attachment entry.");

            string attachmentUUID = createResult.UUID;

            // Construct stored file name using UUID prefix
            string storedName = $"{attachmentUUID}_{safeName}";
            string fullPath = Path.Combine(folder, storedName);

            // Save the file to disk
            using (var fs = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(fs);
            }

            // Update DB with final stored path + metadata
            return await CommentsAttachmentDataAccess.Upsert(
                _sql, basic, attachmentUUID, commentUUID, safeName,
                fullPath, file.Length, mimeType);
        }
        catch (Exception ex)
        {
            return new CommentAttachmentUploadResult
            {
                UUID = null,
                isValid = false,
                Message = ex.Message
            };
        }
    }

    /// <summary>
    /// Retrieves an attachment by UUID and returns its file stream and metadata.
    /// </summary>
    public async Task<(Stream Stream, string MimeType, string FileName)> DownloadAttachment(string attachmentUUID)
    {
        // Fetch attachment metadata from database
        var _attachment = await CommentsAttachmentDataAccess.GetAttachment(_sql, attachmentUUID);

        // Validate file existence
        if (_attachment == null || string.IsNullOrWhiteSpace(_attachment.FilePath) || !File.Exists(_attachment.FilePath))
            throw new FileNotFoundException("Attachment not found");

        // Open file in read-only mode
        Stream fs = new FileStream(_attachment.FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);

        // Return tuple containing file info
        return (fs, _attachment.MimeType, _attachment.FileName);
    }
}
