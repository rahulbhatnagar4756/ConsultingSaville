using Dapper;

using Library.Database.DAL;
using Library.Goals.Models;
using Library.Goals.Models.Comments;

using System.Text.Json;

namespace Library.Goals.DataAccess.Comments
{
    /// <summary>
    /// Static class that handles data access operations for comments.
    /// Communicates with stored procedures in the "admin" schema.
    /// </summary>
    public static class CommentsDataAccess
    {
        /// <summary>
        /// Retrieves comments for a specific UUID by first resolving its table name and target ID.
        /// </summary>
        /// <param name="sql">SQL data access instance used to run stored procedures.</param>
        /// <param name="basic">Company and user context.</param>
        /// <param name="tableTargetUUID">The UUID of KPI/KPA from frontend.</param>
        /// <returns>List of comments.</returns>
        public static async Task<IEnumerable<CommentDto>> GetComments(ISqlDataAccess sql, BasicModel basic, string tableTargetUUID)
        {
            //  Lookup tableNameId + tableTargetId from UUID
            var lookupResult = await sql.LoadDataAsync<TableLookupDto, dynamic>(
                "[admin].[spGetTableNameIdByUUID]",
                new
                {
                    TableTargetUUID = tableTargetUUID,
                    IsAdminOrManager = false 
                }
            );

            var match = lookupResult.FirstOrDefault();

            if (match == null || match.TableNameId == null )
            {
                return Enumerable.Empty<CommentDto>();
            }

            int tableNameId = match.TableNameId.Value;

            //  Fetch comments using resolved IDs
            return await sql.LoadDataAsync<CommentDto, dynamic>(
                "[admin].[spComments]",
                new
                {
                    CompanyUUID = basic.CompanyUUID,
                    TableNameId = tableNameId,
                    TableTargetUUID = tableTargetUUID
                }
            );
        }


        /// <summary>
        /// Retrieves global comments for a specific UUID by first resolving its table name and target ID.
        /// </summary>
        /// <param name="sql">SQL data access instance used to run stored procedures.</param>
        /// <param name="basic">Company and user context.</param>
        /// <param name="tableTargetUUID">The UUID of KPI/KPA from frontend.</param>
        /// <returns>List of comments.</returns>
        /// <summary>
        /// Retrieves global comments for a specific UUID, allows admin/manager filtering.
        /// </summary>
        public static async Task<GlobalCommentsResponseDto> GetGlobalComments(ISqlDataAccess sql, BasicModel basic, string tableTargetUUID)
        {
            if (sql == null) throw new ArgumentNullException(nameof(sql));
            if (basic == null) throw new ArgumentNullException(nameof(basic));
            if (string.IsNullOrEmpty(tableTargetUUID))
                return new GlobalCommentsResponseDto { IsManagerOrAdmin = true, Comments = new List<CommentDto>() };

            // Resolve TableNameId + TableTargetId
            var lookupResult = await sql.LoadDataAsync<TableLookupDto, dynamic>(
                "[admin].[spGetTableNameIdByUUID]",
                new
                {
                    TableTargetUUID = tableTargetUUID,
                    IsAdminOrManager = true
                }
            );

            var match = lookupResult?.FirstOrDefault();

            if (match?.TableNameId == null)
            {
                return new GlobalCommentsResponseDto
                {
                    IsManagerOrAdmin = true,
                    Comments = new List<CommentDto>()
                };
            }

            int tableNameId = match.TableNameId.Value;

            // Call Global Comments stored procedure that returns JSON
            var jsonResultRow = await sql.LoadDataAsync<dynamic, dynamic>(
                "[admin].[sp_GlobalComments]",
                new
                {
                    CompanyUUID = basic.CompanyUUID,
                    TableNameId = tableNameId,
                    TableTargetUUID = tableTargetUUID,
                    UsersUUID = basic.UsersUUIDLoggedIn
                }
            );

            var firstRow = jsonResultRow?.FirstOrDefault();
            string json = firstRow?.JsonResult;

            if (string.IsNullOrEmpty(json))
            {
                return new GlobalCommentsResponseDto
                {
                    IsManagerOrAdmin = true,
                    Comments = new List<CommentDto>()
                };
            }

            // Deserialize JSON safely
            var result = JsonSerializer.Deserialize<GlobalCommentsResponseDto>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result ?? new GlobalCommentsResponseDto
            {
                IsManagerOrAdmin = true,
                Comments = new List<CommentDto>()
            };
        }

        /// <summary>
        /// Save or update a comment using admin.spComments_Save.
        /// TableNameId is automatically resolved by UUID using spGetTableNameIdByUUID.
        /// </summary>
        /// <param name="sql">SQL data access interface</param>
        /// <param name="request">Comment save model</param>
        /// <returns>Result from the stored procedure</returns>
        public static async Task<ResultsModel> Save(ISqlDataAccess sql, CommentSaveRequest request)
        {
            // First resolve TableNameId using UUID
            var lookup = await sql.LoadDataAsync<TableLookupDto, dynamic>(
                "[admin].[spGetTableNameIdByUUID]",
                new
                {
                    TableTargetUUID = request.TableTargetUUID,
                    IsAdminOrManager = request.IsGlobalComment ? true :false
                }
            );

            var match = lookup.FirstOrDefault();

            if (match == null || match.TableNameId == null)
            {
                return new ResultsModel
                {
                    UUID = null,
                    isValid = false,
                    Message = "Invalid TableTargetUUID. No matching KPI/KPA found."
                };
            }

            int tableNameId = match.TableNameId.Value;

            //  Build parameters to pass to spComments_Save
            var parameters = new
            {
                CompanyUUID = request.CompanyUUID,
                UsersUUIDLoggedIn = request.UsersUUIDLoggedIn,
                UUID = request.UUID,
                TableNameId = tableNameId,          
                TableTargetUUID = request.TableTargetUUID,
                Comment = request.Comment,
                CommentTypesid = request.CommentTypesId
            };

            // Call save SP
            return await sql.LoadFirstDataAsync<ResultsModel, dynamic>(
                "[admin].[spComments_Save]",
                parameters
            );
        }

        /// <summary>
        /// Delete a comment (soft delete) using admin.spComments_Delete.
        /// </summary>
        /// <param name="sql">SQL data access interface</param>
        /// <param name="basic">Basic company and user context</param>
        /// <param name="uuid">UUID of the comment to delete</param>
        /// <returns>Result list from the stored procedure</returns>
        public static async Task<ResultsModel> Delete(ISqlDataAccess sql, BasicModel basic, CommentDeleteRequest request)
        {
            var parameters = new
            {
                CompanyUUID = basic.CompanyUUID,
                UsersUUIDLoggedIn = basic.UsersUUIDLoggedIn,
                UUID = request?.UUID,
            };

            var result = await sql.LoadFirstDataAsync<ResultsModel, dynamic>(
                "[admin].[spComments_Delete]",
                parameters
            );

            return result;
        }

    }
}
