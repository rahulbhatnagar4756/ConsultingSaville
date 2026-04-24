using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Goals.Models.Comments
{
    public class CommentDto
    {
        public string? UUID { get; set; }
        public DateTime CreateDate { get; set; }
        public string? UsersUUID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; }
        public string? IDNumber { get; set; }
        public string? Email { get; set; }
        public string? Comment { get; set; }
        public int CommentTypesid { get; set; }
        public string? CommentTypeName { get; set; }
        public int AttachmentCount { get; set; }
        public string? AttachmentsUUID { get; set; }
        public string? FileName { get; set; }
        public DateTime? FileCreatedDate { get; set; }
        public long? FileSize { get; set; }
        public string? FilePath { get; set; }
    }
    
    public class GlobalCommentsResponseDto
    {
        public bool IsManagerOrAdmin { get; set; }
        public List<CommentDto>? Comments { get; set; }
    }

    /// <summary>
    /// Model used to save or update a comment.
    /// </summary>
    public class CommentSaveRequest 
    {
        public string? UUID { get; set; }
        public string CompanyUUID { get; set; }
        public string UsersUUIDLoggedIn { get; set; }
        public string TableTargetUUID { get; set; }
        public string Comment { get; set; } = string.Empty;
        public int CommentTypesId { get; set; } = 1;
        public bool IsGlobalComment { get; set; }
    }

    /// <summary>
    /// Request model used to delete a comment.
    /// </summary>
    public class CommentDeleteRequest
    {
        public string UUID { get; set; }
    }

    public class FileDownloadResult
    {
        public byte[] FileContents { get; set; }
        public string FileDownloadName { get; set; }
        public string ContentType { get; set; }
    }
}
