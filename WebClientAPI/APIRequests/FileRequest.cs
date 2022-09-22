using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebClientAPI.APIRequests
{
    public enum FileType
    {
        Public, Private
    }
    public class FileRequest : WebRequest
    {
        string _chatId;
        public string ChatId 
        { 
            get=>_chatId;
            init 
            {
                Query.Add("chatId",value);
                _chatId = value;
            }
        }
        public FileType Type { get; init; }
        public Dictionary<string, int> FileSizes { get; init; }
        public string Count { get => Query["fileCount"]; }
        public override string Request 
        {
            get =>base.Request;
            init
            {
                
                if (!UrlMathes("setFile",value))
                    throw new ArgumentException("You can not set no 'setFile' requestName");
                else
                    base.Request = value;
                
            }
        }
        

    }
}
