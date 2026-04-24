using Library.Goals.Models;
using Library.Goals.Models.Comments;
using Library.Goals.Services.Comments;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Goals.Controllers
{
    /// <summary>
    /// API controller for comments operations.
    /// </summary>
    [Route("api/Goals/[controller]")]
    [ApiController]
    public class CommentsController : BaseGoalController
    {
        private readonly ICommentsService _commentsService;
        private readonly ICommentsAttachmentService _commentsAttachmentService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentsController"/> class.
        /// </summary>
        /// <param name="commentsService">Comments service for data operations.</param>
        public CommentsController(ICommentsService commentsService, ICommentsAttachmentService commentsAttachmentService)
        {
            _commentsService = commentsService;
            _commentsAttachmentService = commentsAttachmentService;
        }

        /// <summary>
        /// Gets comments for a specific table name and target ID.
        /// </summary>
        /// <param name="tableNameId">The identifier of the table.</param>
        /// <param name="tableTargetUUID">The identifier of the target record.</param>
        /// <returns>List of comments associated with the specified table and record ID.</returns>
        /// <response code="200">Returns the list of comments.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        [HttpGet("{tableTargetUUID}")]
        [Authorize]
        public async Task<IActionResult> GetComments(string tableTargetUUID)
        {
            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            var comments = await _commentsService.GetComments(basicModel, tableTargetUUID);

            return Ok(comments);
        }

        /// <summary>
        /// Gets global comments for a specific table name and target ID.
        /// </summary>
        /// <param name="tableNameId">The identifier of the table.</param>
        /// <param name="tableTargetUUID">The identifier of the target record.</param>
        /// <returns>List of comments associated with the specified table and record ID.</returns>
        /// <response code="200">Returns the list of comments.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        [HttpGet("Global/{tableTargetUUID}")]
        [Authorize]
        public async Task<IActionResult> GetGlobalComments(string tableTargetUUID)
        {
            var (basicModel, errorResult) = ValidateUserClaims();
            if (errorResult != null) return errorResult;

            var comments = await _commentsService.GetGlobalComments(basicModel, tableTargetUUID);

            return Ok(comments);
        }

        /// <summary>
        /// Saves a comment and (optionally) uploads an attachment.
        /// Validates user claims, ensures comment text exists when an attachment is included,
        /// saves the comment first to generate a UUID, then uploads the attachment.
        /// </summary>
        /// <param name="request">Comment save request containing text, attachment, and other metadata.</param>
        /// <returns>200 OK on success, 400 Bad Request for validation failures, or 500 Internal Server Error.</returns>     
        [HttpPost("Save")]
        [Authorize]
        public async Task<IActionResult> Save([FromForm] CommentSaveRequest request)
        {
            try
            {
                // Attach user-related info to request model
                var (basic, error) = ValidateUserClaims();
                if (error != null) return error;

                request.CompanyUUID = basic.CompanyUUID;
                request.UsersUUIDLoggedIn = basic.UsersUUIDLoggedIn;

                // Attachment present → comment required
                if (request.Attachment != null && string.IsNullOrWhiteSpace(request.Comment))
                {
                    return BadRequest(new ResultsModel
                    {
                        isValid = false,
                        Message = "Comment text is required when uploading an attachment."
                    });
                }

                // save comment
                var result = await _commentsService.Save(request);

                if (result?.isValid != true)
                    return BadRequest(result);

                string savedCommentUUID = result.UUID!;

                // save attachment
                if (request.Attachment != null)
                {
                    var upload = await _commentsAttachmentService.UploadAttachment(
                        basic,
                        savedCommentUUID,
                        request.Attachment
                    );

                    if (upload?.isValid != true)
                        return BadRequest(upload);
                }

                return Ok(new
                {
                    isValid = true,
                    UUID = savedCommentUUID,
                    Message = request.Attachment == null ? "Comment saved successfully" : "Comment and attachment saved successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultsModel
                {
                    isValid = false,
                    Message = ex.Message
                });
            }
        }

        /// <summary>
        /// Download a stored attachment using its UUID.
        /// </summary>
        /// <param name="uuid">Attachment UUID</param>
        /// <returns>File stream for download</returns>
        [HttpGet("Download/{uuid}")]
        [Authorize]
        public async Task<IActionResult> Download(string uuid)
        {
            try
            {
                var (basicModel, errorResult) = ValidateUserClaims();
                if (errorResult != null) return errorResult;

                var (stream, mime, name) = await _commentsAttachmentService.DownloadAttachment(uuid);

                if (stream == null)
                {
                    return NotFound(new ResultsModel
                    {
                        isValid = false,
                        Message = "Attachment not found"
                    });
                }

                return File(stream, mime, name);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ResultsModel
                    {
                        UUID = null,
                        isValid = false,
                        Message = $"Internal server error: {ex.Message}"
                    });
            }
        }

        /// <summary>
        /// Delete a Comment
        /// </summary>
        /// <param name="uuid">UUID of the comment to delete</param>
        /// <returns>Result of the delete operation</returns>
        [HttpPost("Delete"), Authorize]
        public async Task<IActionResult> Delete([FromBody] CommentDeleteRequest request)
        {
            try
            {
                var (basicModel, errorResult) = ValidateUserClaims();
                if (errorResult != null) return errorResult;

                var basic = new BasicModel
                {
                    CompanyUUID = basicModel.CompanyUUID,
                    UsersUUIDLoggedIn = basicModel.UsersUUIDLoggedIn
                };

                var result = await _commentsService.Delete(basic, request);

                if (result?.isValid == true)
                    return Ok(result);
                else
                    return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ResultsModel
                    {
                        UUID = null,
                        isValid = false,
                        Message = $"Internal server error: {ex.Message}"
                    });
            }
        }
    }
}
