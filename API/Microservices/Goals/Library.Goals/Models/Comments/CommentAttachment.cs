using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.Models.Comments
{
    public class CommentAttachmentDto
    {
        public string UUID { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public long FileSize { get; set; }
        public string MimeType { get; set; }
    }

    public class CommentAttachmentUploadResult
    {
        public string UUID { get; set; }
        public bool isValid { get; set; }
        public string Message { get; set; }
    }


}

