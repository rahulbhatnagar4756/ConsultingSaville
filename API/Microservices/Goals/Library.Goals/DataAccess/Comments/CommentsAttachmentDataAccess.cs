using Library.Database.DAL;
using Library.Goals.Models;
using Library.Goals.Models.Comments;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.DataAccess.Comments
{
    public static class CommentsAttachmentDataAccess
    {
        /// <summary>
        /// Inserts or updates (Upsert) a comment attachment in the database.
        /// This method calls stored procedure [admin].[spCommentAttachments_Upsert].
        /// </summary>
        /// <param name="sql">SQL data access instance.</param>
        /// <param name="basic">Basic authenticated user context.</param>
        /// <param name="attachmentUUID">UUID of attachment (null for new insert).</param>
        /// <param name="commentUUID">UUID of the comment to which file belongs.</param>
        /// <param name="fileName">Name of the uploaded file.</param>
        /// <param name="filePath">Physical path where file is stored.</param>
        /// <param name="fileSize">Size of the file in bytes.</param>
        /// <param name="mimeType">MIME type of the file.</param>
        /// <returns>Result containing success flag and generated attachment UUID.</returns>
        public static async Task<CommentAttachmentUploadResult> Upsert(ISqlDataAccess sql, BasicModel basic, string attachmentUUID, string commentUUID, string fileName,
                                                                        string filePath, long fileSize, string mimeType)
        {
            // Calls stored procedure to insert/update attachment metadata
            return await sql.LoadFirstDataAsync<CommentAttachmentUploadResult, dynamic>(
                "[admin].[spCommentAttachments_Upsert]",
                new
                {
                    CompanyUUID = basic.CompanyUUID,
                    UsersUUIDLoggedIn = basic.UsersUUIDLoggedIn,
                    UUID = attachmentUUID,
                    CommentsUUID = commentUUID,
                    FileName = fileName,
                    FilePath = filePath,
                    FileSize = fileSize,
                    MimeType = mimeType
                }
            );
        }

        /// <summary>
        /// Retrieves attachment metadata by attachment UUID.
        /// Calls stored procedure [admin].[spCommentAttachments_GetByUUID].
        /// </summary>
        /// <param name="sql">SQL data access instance.</param>
        /// <param name="attachmentUUID">UUID of the attachment.</param>
        /// <returns>Attachment DTO or null if not found.</returns>
        public static async Task<CommentAttachmentDto?> GetAttachment(ISqlDataAccess sql, string attachmentUUID)
        {
            // Query DB to fetch attachment record
            var result = await sql.LoadDataAsync<CommentAttachmentDto, dynamic>(
                "[admin].[spCommentAttachments_GetByUUID]",
                new { UUID = attachmentUUID }
            );

            return result.FirstOrDefault();
        }
    }

}
