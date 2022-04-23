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
}
