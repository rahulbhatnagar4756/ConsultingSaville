using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.Models.Comments
{
    /// <summary>
    /// DTO representing a comment entry with user details and metadata.
    /// </summary>
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

    public class TableLookupDto
    {
        public int? TableNameId { get; set; }
        public string FoundInTable { get; set; } = string.Empty;
    }


    /// <summary>
    /// Model used to save or update a comment.
    /// Supports optional attachment upload.
    /// </summary>
    public class CommentSaveRequest : IBasicModel
    {
        public string? UUID { get; set; }
        public string? CompanyUUID { get; set; }
        public string? UsersUUIDLoggedIn { get; set; }
        public string TableTargetUUID { get; set; }

        public string Comment { get; set; } = string.Empty;

        // Always type 1 for comments or comments-with-attachments
        public int CommentTypesId { get; set; } = 1;

        // Optional attachment (PDF only)
        public IFormFile? Attachment { get; set; }
        public bool IsGlobalComment { get; set; }
    }



    /// <summary>
    /// Response model returned after saving a comment.
    /// </summary>
    public class CommentSaveResponseDto
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public string? UUID { get; set; }
    }

    /// <summary>
    /// Request model used to delete a comment.
    /// </summary>
    public class CommentDeleteRequest
    {
        public string UUID { get; set; }
    }

    /// <summary>
    /// Response model returned after deleting a comment.
    /// </summary>
    public class CommentDeleteResponseDto
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
    }
}
