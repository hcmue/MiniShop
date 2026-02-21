using Microsoft.AspNetCore.Mvc;

namespace MiniShop.Web.Controllers;

public class FileUploaderController : Controller
{
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<FileUploaderController> _logger;

    public FileUploaderController(IWebHostEnvironment environment,
                                  ILogger<FileUploaderController> logger)
    {
        _environment = environment;
        _logger = logger;
    }

    // 1️. GET: /FileUploader → Form upload
    public IActionResult Index()
    {
        ViewData["Title"] = "Upload Files - MiniShop .NET 10";
        return View();
    }

    // 2️. POST: /FileUploader/UploadFile → Single file
    [HttpPost]
    [Route("FileUploader/UploadFile")]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("❌ Chưa chọn file!");

        var result = await SaveFileAsync(file, "single");
        TempData["Message"] = $"✅ Upload single: {result.FileName} ({result.Size}KB)";
        TempData["FileType"] = "single";

        return RedirectToAction("List");
    }

    // 3️. POST: /FileUploader/UploadFiles → Multiple files
    [HttpPost]
    [Route("FileUploader/UploadFiles")]
    public async Task<IActionResult> UploadFiles(IFormFileCollection files)
    {
        if (files == null || !files.Any())
            return BadRequest("❌ Chưa chọn file nào!");

        var results = new List<FileUploadResult>();
        foreach (var file in files)
        {
            if (file.Length > 0)
            {
                results.Add(await SaveFileAsync(file, "multiple"));
            }
        }

        TempData["Message"] = $"✅ Upload {results.Count} files thành công!";
        TempData["FilesCount"] = results.Count;
        TempData["FileType"] = "multiple";

        return RedirectToAction("List");
    }

    // 4️. GET: /FileUploader/List → Hiển thị files đã upload
    [Route("FileUploader/List")]
    public IActionResult List()
    {
        var uploadsPath = Path.Combine(_environment.WebRootPath, "uploads");
        var files = Directory.GetFiles(uploadsPath)
            .Select(f => new FileInfoViewModel
            {
                FileName = Path.GetFileName(f),
                SizeKB = Math.Round(new FileInfo(f).Length / 1024.0, 1),
                UploadTime = System.IO.File.GetCreationTime(f),
                IsImage = Path.GetExtension(f).ToLower() is ".jpg" or ".png" or ".gif"
            })
            .OrderByDescending(f => f.UploadTime)
            .ToList();

        ViewBag.Message = TempData["Message"];
        ViewBag.FilesCount = TempData["FilesCount"];
        ViewBag.FileType = TempData["FileType"];

        return View(files);
    }

    private async Task<FileUploadResult> SaveFileAsync(IFormFile file, string type)
    {
        var uploads = Path.Combine(_environment.WebRootPath, "uploads");
        Directory.CreateDirectory(uploads);

        var uniqueName = $"{Guid.NewGuid():N}[{type}]{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(uploads, uniqueName);

        using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        _logger.LogInformation("File uploaded: {FileName} → {UniqueName}",
            file.FileName, uniqueName);

        return new FileUploadResult
        {
            OriginalName = file.FileName,
            FileName = uniqueName,
            Size = Math.Round(file.Length / 1024.0, 1)
        };
    }
}

// Models cho View
public class FileUploadResult
{
    public string OriginalName { get; set; } = "";
    public string FileName { get; set; } = "";
    public double Size { get; set; }
}

public class FileInfoViewModel
{
    public string FileName { get; set; } = "";
    public double SizeKB { get; set; }
    public DateTime UploadTime { get; set; }
    public bool IsImage { get; set; }
}
