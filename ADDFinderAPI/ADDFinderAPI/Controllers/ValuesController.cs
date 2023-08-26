using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;



namespace ADDFinderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        [HttpGet("duplicates")]
        public IActionResult GetDuplicateFiles(string directoryPath)
        {
            try
            {
                if (!Directory.Exists(directoryPath))
                {
                    return BadRequest("Invalid directory path.");
                }

                var duplicateFiles = FindDuplicateMediaFiles(directoryPath);
                return Ok(duplicateFiles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("deleteduplicates")]
        public IActionResult DeleteDuplicateMediaFiles(string directoryPath)
        {
            try
            {
                if (!Directory.Exists(directoryPath))
                {
                    return BadRequest("Invalid directory path.");
                }

                var duplicateFiles = FindDuplicateMediaFiles(directoryPath);
                DeleteDuplicates(duplicateFiles);

                return Ok("Duplicate files deleted.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        private List<List<string>> FindDuplicateMediaFiles(string directoryPath)
        {
            var mediaExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                ".mpeg",".mp3", ".wav", ".ogg", ".flac", ".mp4", ".mkv", ".avi", ".mov"
                // Add more extensions as needed
            };

            var fileGroups = new Dictionary<string, List<string>>();

            foreach (var filePath in Directory.EnumerateFiles(directoryPath, "*", SearchOption.AllDirectories))
            {
                var extension = Path.GetExtension(filePath);
                if (mediaExtensions.Contains(extension))
                {
                    var hash = CalculateFileHash(filePath);

                    if (!fileGroups.ContainsKey(hash))
                    {
                        fileGroups[hash] = new List<string>();
                    }

                    fileGroups[hash].Add(filePath);
                }
            }

            return fileGroups.Values.Where(group => group.Count > 1).ToList();
        }

        private void DeleteDuplicates(List<List<string>> duplicateGroups)
        {
            foreach (var duplicateGroup in duplicateGroups)
            {
                // Keep the first file (original) and delete the rest
                for (int i = 1; i < duplicateGroup.Count; i++)
                {
                    System.IO.File.Delete(duplicateGroup[i]);
                }
            }
        }

        private string CalculateFileHash(string filePath)
        {
            using (var md5 = MD5.Create())
            using (var stream = System.IO.File.OpenRead(filePath))
            {
                var hashBytes = md5.ComputeHash(stream);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}
