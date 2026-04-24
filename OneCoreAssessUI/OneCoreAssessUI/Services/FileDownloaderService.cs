using Microsoft.JSInterop;

namespace OneCoreAssessUI.Services
{
    public interface IFileDownloaderService
    {
        Task UploadData(byte[] bytes, string fileName);
        Task UploadData(byte[] bytes, string fileName, string filetypes);
    }

    public class FileDownloaderService : IFileDownloaderService
    {
        private readonly IJSRuntime _jS;

        public FileDownloaderService(IJSRuntime jS)
        {
            _jS = jS;
        }

        public static readonly Dictionary<string, string> FileTypeContentTypes = new()
        {
            { ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
            { ".xls",  "application/vnd.ms-excel" },
            { ".csv",  "text/csv" },
            { ".pdf",  "application/pdf" },
            { ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
            { ".doc",  "application/msword" },
            { ".txt",  "text/plain" },
            { ".json", "application/json" },
            { ".xml",  "application/xml" },
            { ".zip",  "application/zip" },
            { ".png",  "image/png" },
            { ".jpg",  "image/jpeg" },
            { ".jpeg", "image/jpeg" },
            { ".gif",  "image/gif" }
        };

        /// <summary>
        /// upload data to the browser as a file download
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public async Task UploadData(byte[] bytes, string fileName)
        {
            var fileExtension = Path.GetExtension(fileName).ToLowerInvariant();
            if (FileTypeContentTypes.TryGetValue(fileExtension, out var contentType))
            {
                await UploadData(bytes, fileName, contentType);
            }
            else
            {
                throw new NotSupportedException($"File type '{fileExtension}' is not supported.");
            }
        }

        /// <summary>
        /// upload data to the browser as a file download
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="fileName"></param>
        /// <param name="filetypes"></param>
        /// <returns></returns>
        public async Task UploadData(byte[] bytes, string fileName, string filetypes)
        {
            var base64 = Convert.ToBase64String(bytes);

            // Call JS to trigger download
            await _jS.InvokeVoidAsync(
                "downloadFileFromBytes",
                fileName, // file name
                filetypes, // content type
                base64
            );
        }

    }
}
