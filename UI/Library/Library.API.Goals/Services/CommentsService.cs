using Library.API.Goals.Models;
using Library.API.Goals.Models.Comments;
using Library.API.Service;
using Library.Security.Services;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Library.API.Goals.Services.CommentsAPIService;

namespace Library.API.Goals.Services;

public interface ICommentsAPIService
{
    /// <summary>
    /// Retrieves a list of comments for the specified table and record.
    /// </summary>
    /// <param name="tableTargetUUID">The record ID for which comments should be fetched.</param>
    /// <returns>A list of <see cref="CommentDto"/> or null if token retrieval fails.</returns>
    Task<List<CommentDto>?> GetComments(string tableTargetUUID);

    /// <summary>
    /// Retrieves a list of global comments for the specified table and record.
    /// </summary>
    /// <param name="tableTargetUUID">The record ID for which comments should be fetched.</param>
    /// <returns>A list of <see cref="CommentDto"/> or null if token retrieval fails.</returns>
    Task<GlobalCommentsResponseDto> GetGlobalComments(string tableTargetUUID);

    /// <summary>
    /// Saves or updates a comment.
    /// </summary>
    /// <param name="saveModel">The model containing comment data to save.</param>
    /// <returns>The result of the save operation as <see cref="ResultsModel"/>.</returns>
    Task<ResultsModel?> Save(CommentSaveRequest saveModel, IFormFile? attachment = null);

    /// <summary>
    /// Deletes a comment based on the provided UUID.
    /// </summary>
    /// <param name="request">The delete request containing the comment UUID.</param>
    /// <returns>The result of the delete operation as <see cref="ResultsModel"/>.</returns>
    Task<ResultsModel?> Delete(CommentDeleteRequest request);

    /// <summary>
    /// Downloads an attachment file based on the provided UUID.
    /// </summary>
    /// <param name="attachmentUUID">The UUID of the attachment to download.</param>
    /// <returns>The result of the download operation as <see cref="IActionResult"/>.</returns>
    Task<FileDownloadResult?> DownloadAttachmentFile(string attachmentUUID);

}

public class CommentsAPIService : ICommentsAPIService
{
    private readonly IConfiguration _config;
    private readonly IAPIConnectService _apiConnect;
    private readonly ITokenService _token;

    private readonly string _urlBase;

    private readonly string _endpointGet = "Goals/Comments";
    private readonly string _endpointGetGlobal = "Goals/Comments/Global";
    private readonly string _endpointSave = "Goals/Comments/Save";
    private readonly string _endpointDelete = "Goals/Comments/Delete";
    private readonly string _endpointDownload = "Goals/Comments/Download";

    public CommentsAPIService(IConfiguration config, IAPIConnectService apiConnect, ITokenService token)
    {
        _config = config;
        _apiConnect = apiConnect;
        _token = token;

        _urlBase = _config.GetSection("API:Goals:URL").Value
                    ?? _config.GetSection("API:Base:URL").Value;
    }

    /// <summary>
    /// Retrieves comments associated with a specific table and record ID.
    /// </summary>
    /// <param name="tableTargetUUID">The ID representing the target record.</param>
    /// <returns>
    /// A list of <see cref="CommentDto"/> containing comments data,
    /// or null if token retrieval fails.  
    /// If an exception occurs, an empty list is returned.
    /// </returns>
    public async Task<List<CommentDto>?> GetComments(string tableTargetUUID)
    {
        string? token = await _token.GetToken();
        if (token == null) return null;

        try
        {
            string url = $"{_urlBase}{_endpointGet}/{tableTargetUUID}";
            return await _apiConnect.GetAsync<List<CommentDto>>(token, url);
        }
        catch (Exception ex)
        {
            return new List<CommentDto>();
        }
    }

    /// <summary>
    /// Retrieves comments associated with a specific table and record ID.
    /// </summary>
    /// <param name="tableTargetUUID">The ID representing the target record.</param>
    /// <returns>
    /// A list of <see cref="CommentDto"/> containing comments data,
    /// or null if token retrieval fails.  
    /// If an exception occurs, an empty list is returned.
    /// </returns>
    public async Task<GlobalCommentsResponseDto> GetGlobalComments(string tableTargetUUID)
    {
        string? token = await _token.GetToken();
        if (token == null) return null;

        try
        {
            string url = $"{_urlBase}{_endpointGetGlobal}/{tableTargetUUID}";
            return await _apiConnect.GetAsync<GlobalCommentsResponseDto>(token, url);
        }
        catch (Exception ex)
        {
            return new GlobalCommentsResponseDto();
        }
    }

    /// <summary>
    /// Creates or updates a comment in the Goals API.
    /// </summary>
    /// <param name="saveModel">The model containing comment data to save.</param>
    /// <returns>
    /// A <see cref="ResultsModel"/> indicating success or failure of the operation.
    /// If token retrieval fails or data is invalid, an error object is returned.
    /// </returns>
    public async Task<ResultsModel?> Save(CommentSaveRequest saveModel, IFormFile? attachment = null)
    {
        if (saveModel == null)
            return new ResultsModel { UUID = null, isValid = false, Message = "Invalid comment data." };

        string? token = await _token.GetToken();
        if (token == null)
            return new ResultsModel { UUID = null, isValid = false, Message = "Authentication failed." };

        try
        {
            string url = $"{_urlBase}{_endpointSave}";

            using var content = new MultipartFormDataContent();

            // Form fields (always required)
            if (!string.IsNullOrEmpty(saveModel.UUID))
                content.Add(new StringContent(saveModel.UUID), "UUID");
            if (!string.IsNullOrEmpty(saveModel.CompanyUUID))
                content.Add(new StringContent(saveModel.CompanyUUID), "CompanyUUID");
            if (!string.IsNullOrEmpty(saveModel.UsersUUIDLoggedIn))
                content.Add(new StringContent(saveModel.UsersUUIDLoggedIn), "UsersUUIDLoggedIn");

            content.Add(new StringContent(saveModel.TableTargetUUID), "TableTargetUUID");
            content.Add(new StringContent(saveModel.Comment), "Comment");
            content.Add(new StringContent(saveModel.CommentTypesId.ToString()), "CommentTypesId");
            content.Add(new StringContent(saveModel.IsGlobalComment.ToString()), "IsGlobalComment");

            // If attachment exists, add file part
            if (attachment != null)
            {
                var ms = new MemoryStream();
                await attachment.OpenReadStream().CopyToAsync(ms);
                ms.Position = 0;

                var streamContent = new StreamContent(ms);
                streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(attachment.ContentType);

                content.Add(streamContent, "Attachment", attachment.FileName);
            }

            // Always send multipart because backend requires [FromForm]
            var result = await _apiConnect.PostMultipartAsync<ResultsModel>(token, url, content);
            return result;
        }
        catch (Exception ex)
        {
            return new ResultsModel { UUID = null, isValid = false, Message = $"Error saving comment: {ex.Message}" };
        }
    }


    /// <summary>
    /// Deletes a comment using its unique UUID.
    /// </summary>
    /// <param name="request">The delete request containing the comment UUID.</param>
    /// <returns>
    /// A <see cref="ResultsModel"/> containing success status and message.
    /// If token retrieval fails or UUID is missing, an error object is returned.
    /// </returns>
    public async Task<ResultsModel?> Delete(CommentDeleteRequest request)
    {
        if (request == null || string.IsNullOrEmpty(request.UUID))
            return new ResultsModel { UUID = null, isValid = false, Message = "Invalid comment UUID." };

        string? token = await _token.GetToken();
        if (token == null)
            return new ResultsModel { UUID = null, isValid = false, Message = "Authentication failed." };

        try
        {
            string url = $"{_urlBase}{_endpointDelete}";
            return await _apiConnect.PostAsync<ResultsModel, CommentDeleteRequest>(token, url, request);
        }
        catch (Exception ex)
        {
            return new ResultsModel { UUID = null, isValid = false, Message = $"Error deleting comment: {ex.Message}" };
        }
    }
    
    public async Task<FileDownloadResult?> DownloadAttachmentFile(string attachmentUUID)
    {
        string? token = await _token.GetToken();
        if (token == null)
            return null;

        try
        {
            string url = $"{_urlBase}{_endpointDownload}/{attachmentUUID}";

            var fileResponse = await _apiConnect.GetFileAsync(token, url);

            if (fileResponse == null || fileResponse.Value.Stream == null)
                return null;

            using var ms = new MemoryStream();
            await fileResponse.Value.Stream.CopyToAsync(ms);

            return new FileDownloadResult
            {
                FileContents = ms.ToArray(),
                FileDownloadName = fileResponse.Value.FileName ?? "download.bin",
                ContentType = fileResponse.Value.ContentType ?? "application/octet-stream"
            };
        }
        catch
        {
            return null;
        }
    }


}

