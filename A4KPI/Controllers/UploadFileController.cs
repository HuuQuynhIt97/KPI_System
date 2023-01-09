using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using A4KPI.Data;
using A4KPI.Models;
using Microsoft.AspNetCore.Http.Features;
using System.IO.Compression;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using A4KPI.DTO;
using Microsoft.AspNetCore.Authorization;

namespace A4KPI.Controllers
{
    public class UploadFileController : ApiControllerBase
    {
        private readonly IWebHostEnvironment _currentEnvironment;
        private readonly DataContext _context;

        public UploadFileController(IWebHostEnvironment currentEnvironment, DataContext context)
        {
            _currentEnvironment = currentEnvironment;
            _context = context;
        }

        [AcceptVerbs("Post")]
        public void SaveSpecialScore(int campaignID,int scoreFrom, int scoreTo, string scoreType)
        {
            try
            {
                if (HttpContext.Request.Form.Files.Count > 0)
                {
                    var httpPostedFile = HttpContext.Request.Form.Files["UploadFiles"];

                    if (httpPostedFile != null)
                    {
                        var fileSave = Path.Combine(_currentEnvironment.WebRootPath, "UploadedFilesSpecialScore");
                        var fileSavePath = Path.Combine(fileSave, httpPostedFile.FileName);
                        if (!System.IO.File.Exists(fileSavePath))
                        {
                            using (Stream fileStream = new FileStream(fileSavePath, FileMode.Create))
                            {
                                httpPostedFile.CopyTo(fileStream);

                            }

                            var item = new KPIScoreAttchment();
                            item.Path = $"/UploadedFilesSpecialScore/{httpPostedFile.FileName}";
                            item.CampaignID = campaignID;
                            item.ScoreType = scoreType;
                            item.CreatedBy = scoreFrom;
                            item.ScoreFrom = scoreFrom;
                            item.ScoreTo = scoreTo;

                            var month = DateTime.Now.Month;
                            var year = DateTime.Now.Year;
                            item.UploadTime = new DateTime(year, month, 1);
                            item.CreatedTime = DateTime.Now;
                            _context.KPIScoreAttchment.Update(item);
                            _context.SaveChanges();

                            Response.Clear();
                            Response.ContentType = "application/json; charset=utf-8";
                            Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File uploaded succesfully";


                        }
                        else
                        {
                            Response.Clear();
                            Response.StatusCode = 400;
                            Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File already exists";
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Response.Clear();
                Response.ContentType = "application/json; charset=utf-8";
                Response.StatusCode = 400;
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = e.Message;
            }
        }

        [AcceptVerbs("Post")]
        public void RemoveSpecialScore(int campaignID, int scoreFrom, int scoreTo , string scoreType)
        {

            try
            {
                var fileSave = "";
                if (HttpContext.Request.Form["cancel-uploading"] == false)
                {
                    fileSave = Path.Combine(_currentEnvironment.WebRootPath, "UploadedFilesSpecialScore");
                }
                else
                {
                    fileSave = Path.Combine(_currentEnvironment.WebRootPath, "UploadedFilesSpecialScore");
                }

                var fileName = HttpContext.Request.Form["UploadFiles"];

                var fileSavePath = Path.Combine(fileSave, fileName);
                if (System.IO.File.Exists(fileSavePath))
                {
                    System.IO.File.Delete(fileSavePath);
                }


                string path = $"/UploadedFilesSpecialScore/{fileName.First()}";
                var item = _context.KPIScoreAttchment.FirstOrDefault(
                    x => x.CampaignID == campaignID 
                    && x.Path == path 
                    && x.ScoreFrom == scoreFrom
                    && x.ScoreTo == scoreTo
                    && x.ScoreType == scoreType);
                _context.KPIScoreAttchment.Remove(item);
                _context.SaveChanges();
                Response.ContentType = "application/json; charset=utf-8";
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File removed succesfully";

            }
            catch (Exception e)
            {
                HttpResponse Response = HttpContext.Response;
                Response.Clear();
                Response.StatusCode = 400;
                Response.ContentType = "application/json; charset=utf-8";
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Falied to remove!";
            }
        }


        [AllowAnonymous]
        [HttpGet("{filename}")]
        public async Task<IActionResult> DownloadFileSpecialScore(string filename)

        {
            if (filename == null)
                return Content("filename not present");

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UploadedFilesSpecialScore", filename);
            var check_exist_file = System.IO.File.Exists(path);
            if (!check_exist_file)
            {
                return Content("filename not found");
            }
            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));

        }

        [HttpPost]
        public async Task<IActionResult> DownloadFileMeeting2(DownloadFileDto file)

        {
            if (file.Name == null)
                return Content("filename not present");

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UploadedFiles", file.Name);
            var check_exist_file = System.IO.File.Exists(path);
            if (!check_exist_file)
            {
                return Content("filename not found");
            }
            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));

        }

        [HttpPost]
        public async Task<Boolean> CheckFile(DownloadFileDto file)

        {
            if (file.Name == null)
                return false;

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UploadedFiles", file.Name);
            var check_exist_file = System.IO.File.Exists(path);
            if (!check_exist_file)
            {
                return false;
            }
            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return true;

        }
        
        [AllowAnonymous]
        [HttpGet("{filename}")]
        public async Task<IActionResult> DownloadFileMeeting(string filename)

        {
            if (filename == null)
                return Content("filename not present");

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UploadedFiles", filename);
            var check_exist_file = System.IO.File.Exists(path);
            if (!check_exist_file)
            {
                return Content("filename not found");
            }
            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));

        }

        [AllowAnonymous]
        [HttpGet("{filename}")]
        public async Task<IActionResult> DownloadFileScore(string filename)

        {
            if (filename == null)
                return Content("filename not present");

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UploadedFilesScore", filename);
            var check_exist_file = System.IO.File.Exists(path);
            if (!check_exist_file)
            {
                return Content("filename not found");
            }
            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));

        }

        [AcceptVerbs("Post")]
        public void Save(int kpiId, DateTime uploadTime)
        {
            try
            {
                if (HttpContext.Request.Form.Files.Count > 0)
                {
                    var httpPostedFile = HttpContext.Request.Form.Files["UploadFiles"];

                    if (httpPostedFile != null)
                    {
                        var fileSave = Path.Combine(_currentEnvironment.WebRootPath, "UploadedFiles");
                        var fileSavePath = Path.Combine(fileSave, httpPostedFile.FileName);
                        if (!System.IO.File.Exists(fileSavePath))
                        {
                            using (Stream fileStream = new FileStream(fileSavePath, FileMode.Create))
                            {
                                httpPostedFile.CopyTo(fileStream);

                            }

                            var item = new UploadFile();
                            item.Path = $"/UploadedFiles/{httpPostedFile.FileName}";
                            item.KPIId = kpiId;
                            var month_KPI = _context.KPINews.Find(kpiId).StartDisplayMeetingTime;

                            var month = uploadTime.Month == month_KPI.Value.Month ? uploadTime.Month : uploadTime.Month - 1;
                            var year = uploadTime.Month == 1 ? uploadTime.Year - 1 : uploadTime.Year;
                            item.UploadTime = new DateTime(year, month, 1);
                            item.CreatedTime = DateTime.MinValue;
                            _context.UploadFiles.Update(item);
                            _context.SaveChanges();
                          
                            Response.Clear();
                            Response.ContentType = "application/json; charset=utf-8";
                            Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File uploaded succesfully";

                         
                        }
                        else
                        {
                            Response.Clear();
                            Response.StatusCode = 400;
                            Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File already exists";
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Response.Clear();
                Response.ContentType = "application/json; charset=utf-8";
                Response.StatusCode = 400;
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = e.Message;
            }
        }

        [AcceptVerbs("Post")]
        public void SaveScore(int campaignID, int headingID, int uploadFrom, int uploadTo)
        {
            try
            {
                if (HttpContext.Request.Form.Files.Count > 0)
                {
                    var httpPostedFile = HttpContext.Request.Form.Files["UploadFiles"];

                    if (httpPostedFile != null)
                    {
                        var fileSave = Path.Combine(_currentEnvironment.WebRootPath, "UploadedFilesScore");
                        var fileSavePath = Path.Combine(fileSave, httpPostedFile.FileName);
                        if (!System.IO.File.Exists(fileSavePath))
                        {
                            using (Stream fileStream = new FileStream(fileSavePath, FileMode.Create))
                            {
                                httpPostedFile.CopyTo(fileStream);

                            }

                            var item = new AttitudeAttchment();
                            item.Path = $"/UploadedFilesScore/{httpPostedFile.FileName}";
                            item.CampaignID = campaignID;
                            item.HeadingID = headingID;
                            item.UploadFrom = uploadFrom;
                            item.UploadTo = uploadTo;

                            var month = DateTime.Now.Month;
                            var year = DateTime.Now.Year;
                            item.UploadTime = new DateTime(year, month, 1);
                            item.CreatedTime = DateTime.Now;
                            _context.AttitudeAttchment.Update(item);
                            _context.SaveChanges();

                            Response.Clear();
                            Response.ContentType = "application/json; charset=utf-8";
                            Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File uploaded succesfully";


                        }
                        else
                        {
                            Response.Clear();
                            Response.StatusCode = 400;
                            Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File already exists";
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Response.Clear();
                Response.ContentType = "application/json; charset=utf-8";
                Response.StatusCode = 400;
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = e.Message;
            }
        }

        [AcceptVerbs("Post")]
        public void RemoveScore(int campaignID, int headingID, int uploadFrom, int uploadTo)
        {

            try
            {
                var fileSave = "";
                if (HttpContext.Request.Form["cancel-uploading"] == false)
                {
                    fileSave = Path.Combine(_currentEnvironment.WebRootPath, "UploadedFilesScore");
                }
                else
                {
                    fileSave = Path.Combine(_currentEnvironment.WebRootPath, "UploadedFilesScore");
                }

                var fileName = HttpContext.Request.Form["UploadFiles"];

                var fileSavePath = Path.Combine(fileSave, fileName);
                if (System.IO.File.Exists(fileSavePath))
                {
                    System.IO.File.Delete(fileSavePath);
                }


                string path = $"/UploadedFilesScore/{fileName.First()}";
                var item = _context.AttitudeAttchment.FirstOrDefault(x => x.CampaignID == campaignID 
                && x.Path == path 
                && x.HeadingID == headingID
                && x.UploadFrom == uploadFrom
                && x.UploadTo == uploadTo
                );
                _context.AttitudeAttchment.Remove(item);
                _context.SaveChanges();
                Response.ContentType = "application/json; charset=utf-8";
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File removed succesfully";

            }
            catch (Exception e)
            {
                HttpResponse Response = HttpContext.Response;
                Response.Clear();
                Response.StatusCode = 400;
                Response.ContentType = "application/json; charset=utf-8";
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Falied to remove!";
            }
        }

        [AcceptVerbs("Post")]
        public void Remove(int kpiId, DateTime uploadTime)
        {
           
            try
            {
                var fileSave = "";
                if (HttpContext.Request.Form["cancel-uploading"]== false)
                {
                    fileSave = Path.Combine(_currentEnvironment.WebRootPath, "UploadingFiles");
                }
                else
                {
                    fileSave = Path.Combine(_currentEnvironment.WebRootPath, "UploadedFiles");
                }
                var month_KPI = _context.KPINews.Find(kpiId).StartDisplayMeetingTime;
                var month = uploadTime.Month == month_KPI.Value.Month ? uploadTime.Month : uploadTime.Month - 1;
                var year = uploadTime.Month == 1 ? uploadTime.Year - 1 : uploadTime.Year;
                var ut = new DateTime(year, month, 1);

                var fileName = HttpContext.Request.Form["UploadFiles"];

                var fileSavePath = Path.Combine(fileSave, fileName);
                if (System.IO.File.Exists(fileSavePath))
                {
                    System.IO.File.Delete(fileSavePath);
                }
              

                string path = $"/UploadedFiles/{fileName.First()}";
                var item = _context.UploadFiles.FirstOrDefault(x => x.KPIId == kpiId && x.Path == path && x.UploadTime == ut);
                _context.UploadFiles.Remove(item);
                _context.SaveChanges();
                Response.ContentType = "application/json; charset=utf-8";
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File removed succesfully";
               
            }
            catch (Exception e)
            {
                HttpResponse Response = HttpContext.Response;
                Response.Clear();
                Response.StatusCode = 400;
                Response.ContentType = "application/json; charset=utf-8";
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Falied to remove!";
            }
        }

        [HttpGet]
        public IActionResult Download(int kpiId, DateTime uploadTime)
        {

            try
            {
                var (fileType, archiveData, archiveName) = DownloadFiles(kpiId,uploadTime);

                return File(archiveData, fileType, archiveName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetAttackFiles(int kpiId, DateTime uploadTime)
        {
            var kpiid = kpiId;
            var month = uploadTime.Month == 1 ? 12 : uploadTime.Month - 1;
            var year = uploadTime.Month == 1 ? uploadTime.Year - 1 : uploadTime.Year;
            var ut = new DateTime(year, month, 1);

            var data = await _context.UploadFiles.Where(x => x.KPIId == kpiid && x.UploadTime == ut).ToListAsync();
            var files = data.Select(x => x.Path ).ToList();
            var fileInfoList = new List<PreloadFileDto>();
                files.ForEach(file =>
                    {
                        string filePath = _currentEnvironment.WebRootPath + file;
                        if (System.IO.File.Exists(filePath))
                        {
                            var info = new FileInfo(filePath);
                            fileInfoList.Add(new PreloadFileDto
                            {
                                Name = Path.GetFileNameWithoutExtension(filePath),
                                Size = info.Length,
                                Type = info.Extension

                            });
                        }
                    });
            return Ok(fileInfoList);
        }


        [HttpGet]
        public async Task<IActionResult> GetAttackFilesScore(int campaignID, int headingID, int uploadFrom, int uploadTo)
        {

            var data = await _context.AttitudeAttchment.Where(x => x.CampaignID == campaignID 
            && x.HeadingID == headingID
            && x.UploadFrom == uploadFrom
            && x.UploadTo == uploadTo
            ).ToListAsync();
            var files = data.Select(x => x.Path).ToList();
            var fileInfoList = new List<PreloadFileDto>();
            files.ForEach(file =>
            {
                string filePath = _currentEnvironment.WebRootPath + file;
                if (System.IO.File.Exists(filePath))
                {
                    var info = new FileInfo(filePath);
                    fileInfoList.Add(new PreloadFileDto
                    {
                        Name = Path.GetFileNameWithoutExtension(filePath),
                        Size = info.Length,
                        Type = info.Extension

                    });
                }
            });
            return Ok(fileInfoList);
        }

        [HttpGet]
        public async Task<IActionResult> getSpecialFilesScore(int campaignID,int scoreFrom, int scoreTo, string type)
        {
            var data = await _context.KPIScoreAttchment.Where(
                x => x.CampaignID == campaignID 
                && x.ScoreFrom == scoreFrom
                && x.ScoreTo == scoreTo
                && x.ScoreType == type
                ).ToListAsync();
            var files = data.Select(x => x.Path).ToList();
            var fileInfoList = new List<PreloadFileDto>();
            files.ForEach(file =>
            {
                string filePath = _currentEnvironment.WebRootPath + file;
                if (System.IO.File.Exists(filePath))
                {
                    var info = new FileInfo(filePath);
                    fileInfoList.Add(new PreloadFileDto
                    {
                        Name = Path.GetFileName(filePath),
                        Size = info.Length,
                        Type = info.Extension,
                        Path = file

                    });
                }
            });
            return Ok(fileInfoList);
        }

        [HttpGet]
        public async Task<IActionResult> GetDownloadFiles(int kpiId, DateTime uploadTime)
        {
            var kpiid = kpiId;
            var month = uploadTime.Month == 1 ? 12 : uploadTime.Month - 1;
            var year = uploadTime.Month == 1 ? uploadTime.Year - 1 : uploadTime.Year;
            var ut = new DateTime(year, month, 1);

            var data = await _context.UploadFiles.Where(x => x.KPIId == kpiid && x.UploadTime == ut).ToListAsync();
            var files = data.Select(x => x.Path).ToList();
            var list = new List<DownloadFileDto>();
            files.ForEach(file =>
            {
                string filePath = _currentEnvironment.WebRootPath + file;
                var info = new FileInfo(filePath);
                list.Add(new DownloadFileDto
                {
                    Name = Path.GetFileName(filePath),
                    Path = file

                });
            });
            return Ok(list);
        }

        [HttpGet]
        public async Task<IActionResult> GetDownloadFilesMeeting(int kpiId, DateTime uploadTime)
        {
            var kpiid = kpiId;
            var month = uploadTime.Month;
            var year = uploadTime.Year;
            var ut = new DateTime(year, month, 1);

            var data = await _context.UploadFiles.Where(x => x.KPIId == kpiid && x.UploadTime == ut).ToListAsync();
            var files = data.Select(x => x.Path).ToList();
            var list = new List<DownloadFileDto>();
            files.ForEach(file =>
            {
                string filePath = _currentEnvironment.WebRootPath + file;
                var info = new FileInfo(filePath);
                list.Add(new DownloadFileDto
                {
                    Name = Path.GetFileName(filePath),
                    Path = file
                });
            });
            return Ok(list);
        }

        #region Download File  
        private (string fileType, byte[] archiveData, string archiveName) DownloadFiles(int kpiId , DateTime uploadTime)
        {
            var zipName = $"archive-{DateTime.Now.ToString("yyyy_MM_dd-HH_mm_ss")}.zip";

            var kpiid = kpiId;
            var month = uploadTime.Month == 1 ? 12 : uploadTime.Month - 1;
            var year = uploadTime.Month == 1 ? uploadTime.Year - 1 : uploadTime.Year;
            var ut = new DateTime(year, month, 1);

            var data = _context.UploadFiles.Where(x => x.KPIId == kpiid && x.UploadTime == ut).ToList();
            var files = data.Select(x => x.Path ).ToList();
            
            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    files.ForEach(file =>
                    {
                        string filePath = _currentEnvironment.WebRootPath + file;
                        archive.CreateEntryFromFile(filePath, System.IO.Path.GetFileName(filePath));
                      
                    });
                }

                return ("application/zip", memoryStream.ToArray(), zipName);
            }

        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/octet-stream"},
                {".msg", "application/vnd.ms-outlook"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }

        #endregion
    }
}
