using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Responses
{
    public class FileResponse:BaseResponse
    {
        public string FileName { get; init; }
        public int FileSize { get; init; }
        public string FileAlias { get; init; }
    }
}
