using Library.Database.DAL;
using Library.Goals.DataAccess.Comments;
using Library.Goals.Models;
using Library.Goals.Models.Comments;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.Services.Comments;

/// <summary>
/// Interface for comment retrieval service operations.
/// </summary>
public interface ICommentsService
{
    /// <summary>
    /// Gets comments for a specific table name and target record ID.
    /// </summary>
    /// <param name="basic">Basic model containing company and user information.</param>
    /// <param name="tableNameId">The table name identifier.</param>
    /// <param name="tableTargetUUID">The table target identifier.</param>
    /// <returns>A list of comments for the specified target.</returns>
    Task<IEnumerable<CommentDto>> GetComments(BasicModel basic, string tableTargetUUID);


    /// <summary>
    /// Gets Global comments for a specific table name and target record ID.
    /// </summary>
    /// <param name="basic">Basic model containing company and user information.</param>
    /// <param name="tableNameId">The table name identifier.</param>
    /// <param name="tableTargetUUID">The table target identifier.</param>
    /// <returns>A list of comments for the specified target.</returns>
    Task<GlobalCommentsResponseDto> GetGlobalComments(BasicModel basic, string tableTargetUUID);

    Task<ResultsModel?> Save(CommentSaveRequest saveModel);
    Task<ResultsModel?> Delete(BasicModel basic, CommentDeleteRequest request);

}

/// <summary>
/// Service for comment-related operations.
/// </summary>
public class CommentsService : ICommentsService
{
    private readonly ISqlDataAccess _sql;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommentsService"/> class.
    /// </summary>
    /// <param name="sql">SQL data access instance.</param>
    public CommentsService(ISqlDataAccess sql)
    {
        _sql = sql;
    }

    /// <summary>
    /// Gets comments for a specific table name and target ID.
    /// </summary>
    /// <param name="basic">Basic company and user context.</param>
    /// <param name="tableNameId">Table identification number.</param>
    /// <param name="tableTargetId">Record ID for which comments are requested.</param>
    /// <returns>List of comments, or an empty list if none are found.</returns>
    public async Task<IEnumerable<CommentDto>> GetComments(BasicModel basic, string tableTargetUUID)
    {
        return await CommentsDataAccess.GetComments(_sql, basic,  tableTargetUUID);
    }

    /// <summary>
    /// Gets Global comments for a specific table name and target ID.
    /// </summary>
    /// <param name="basic">Basic company and user context.</param>
    /// <param name="tableNameId">Table identification number.</param>
    /// <param name="tableTargetId">Record ID for which comments are requested.</param>
    /// <returns>List of comments, or an empty list if none are found.</returns>
    public async Task<GlobalCommentsResponseDto> GetGlobalComments(BasicModel basic, string tableTargetUUID)
    {
        return await CommentsDataAccess.GetGlobalComments(_sql, basic, tableTargetUUID);
    }

    /// <summary>
    /// Save or update a comment
    /// </summary>
    /// <param name="saveModel">Comment save model</param>
    /// <returns>Result of the save operation</returns>
    public async Task<ResultsModel?> Save(CommentSaveRequest saveModel)
    {
        try
        {
            return await CommentsDataAccess.Save(_sql, saveModel);
        }
        catch (Exception ex)
        {
            return new ResultsModel
            {
                UUID = null,
                isValid = false,
                Message = $"Error saving comment: {ex.Message}"
            };
        }
    }

    /// <summary>
    /// Delete a comment
    /// </summary>
    /// <param name="basic">Basic model with company and user information</param>
    /// <param name="uuid">UUID of the comment to delete</param>
    /// <returns>Result of the delete operation</returns>
    public async Task<ResultsModel?> Delete(BasicModel basic, CommentDeleteRequest request)
    {
        try
        {
            var results = await CommentsDataAccess.Delete(_sql, basic, request);
            return results;
        }
        catch (Exception ex)
        {
            return new ResultsModel
            {
                UUID = null,
                isValid = false,
                Message = $"Error deleting comment: {ex.Message}"
            };
        }
    }


}

