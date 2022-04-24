using CosmeticStore.Interfaces.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace CosmeticStore.WebAPI.Controllers;

[ApiController, Route(WebAPIAddress.Images)]
public class ImagesApiController : ControllerBase
{
    private readonly IFileProvider _FileProvider;

    public ImagesApiController(IFileProvider FileProvider) => _FileProvider = FileProvider;

    [HttpGet("{ImageName}")]
    public IActionResult GetImage(string ImageName)
    {
        var file = _FileProvider.GetFileInfo($"Images/{ImageName}");

        if (!file.Exists)
            return NotFound();

        return File(file.CreateReadStream(), "application/image", file.Name);
    }

    [HttpPost]
    public async Task<IActionResult> UploadImage(IFormFile file, [FromServices] IWebHostEnvironment environment)
    {
        var file_name = file.FileName;

        var server_file = environment.WebRootFileProvider.GetFileInfo(file_name);
        if(server_file.Exists)
            return Ok();

        var www_root = environment.WebRootPath;

        var file_path = Path.Combine(www_root, "images", file_name);

        if (System.IO.File.Exists(file_path))
            return Ok();

        await using var server_file_stream = System.IO.File.Create(file_path);
        await file.CopyToAsync(server_file_stream);

        return Ok();
    }
}
