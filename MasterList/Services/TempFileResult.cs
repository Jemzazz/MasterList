using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace MasterList.Services  // Match your Services namespace
{
    public class TempFileResult : FileResult
    {
        private readonly string _filePath;

        public TempFileResult(string filePath, string contentType)
            : base(contentType)
        {
            _filePath = filePath;
        }

        public override async Task ExecuteResultAsync(ActionContext context)
        {
            await base.ExecuteResultAsync(context);

            try
            {
                if (System.IO.File.Exists(_filePath))
                {
                    System.IO.File.Delete(_filePath);
                }
            }
            catch
            {
                // Optional: Log deletion errors if needed
            }
        }
    }
}