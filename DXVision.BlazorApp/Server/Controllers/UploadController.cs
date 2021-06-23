using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace DXVision.BlazorApp.Server.Controllers
{
	public class ChunkMetadata
	{
		public int Index { get; set; }
		public int TotalCount { get; set; }
		public int FileSize { get; set; }
		public string FileName { get; set; }
		public string FileType { get; set; }
		public string FileGuid { get; set; }
	}

	[Route("api/[controller]")]
	[ApiController]
	public class UploadController : ControllerBase
	{
        private readonly IWebHostEnvironment _hostingEnvironment;
		readonly IMemoryCache cache;

		public UploadController(IWebHostEnvironment hostingEnvironment, IMemoryCache cache)
        {
			this.cache = cache;
			_hostingEnvironment = hostingEnvironment;
        }

        [HttpPost("[action]")]
        public ActionResult UploadChunkedFile(IFormFile myFile)
        {
            string chunkMetadata = Request.Form["chunkMetadata"];
            // Specify the location for temporary files.
            var tempPath = Path.Combine(_hostingEnvironment.ContentRootPath, "uploads");
            // Remove temporary files.
            RemoveTempFilesAfterDelay(tempPath, new TimeSpan(0, 5, 0));

            try
            {
                if (!string.IsNullOrEmpty(chunkMetadata))
                {
                    // Get chunk details.
                    var metaDataObject = JsonConvert.DeserializeObject<ChunkMetadata>(chunkMetadata);
                    // Specify the full path for temporary files (inluding the file name).
                    var tempFilePath = Path.Combine(tempPath, metaDataObject.FileGuid + ".tmp");
                    // Check whether the target directory exists; otherwise, create it.
                    if (!Directory.Exists(tempPath))
                        Directory.CreateDirectory(tempPath);
                    // Append the chunk to the file.
                    AppendChunkToFile(tempFilePath, myFile);
                    // Save the file if all chunks are received.
                    if (metaDataObject.Index == (metaDataObject.TotalCount - 1))
                    {
                        var f = SaveUploadedFile(tempFilePath, metaDataObject.FileName);
                        cache.Set($"Upload_{metaDataObject.FileGuid}", f, new TimeSpan(0, 5, 0));
                    }
                }
            }
            catch
            {
                return BadRequest();
            }
            return Ok();
        }
        void AppendChunkToFile(string path, IFormFile content)
        {
            using (var stream = new FileStream(path, FileMode.Append, FileAccess.Write))
            {
                content.CopyTo(stream);
            }
        }
        string SaveUploadedFile(string tempFilePath, string fileName)
        {
            var path = Path.Combine(_hostingEnvironment.ContentRootPath, "uploads");
            System.IO.File.Copy(tempFilePath, Path.Combine(path, fileName), true);
            System.IO.File.Delete(tempFilePath);
            return Path.Combine(path, fileName);
        }
        void RemoveTempFilesAfterDelay(string path, TimeSpan delay)
        {
            var dir = new DirectoryInfo(path);
            if (dir.Exists)
                foreach (var file in dir.GetFiles("*.tmp").Where(f => f.LastWriteTimeUtc.Add(delay) < DateTime.UtcNow))
                    file.Delete();
        }
    }
}
