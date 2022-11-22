using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using dog_api.Repositories;

namespace dog_api.Controllers;

[Authorize]
[Route("api/dog-images")]
[ApiController]
public class DogImageController : ControllerBase    //TODO add error handling
{
    private readonly IDogImageRepo _dogImageRepo;

    public DogImageController(IDogImageRepo dogImageRepo)
    {
        _dogImageRepo = dogImageRepo;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadFileAsync(IFormFile file)
    {
        var id = await _dogImageRepo.Add(file);
        return Ok($"File {id} uploaded to S3 successfully.");
    }

    [HttpGet("get-stream-by-key")]
    public async Task<IActionResult> GetFileByKeyAsync(string key)
    {
        var file = await _dogImageRepo.GetDogImageStream(new Guid(key));
        return File(file.File.Stream, file.File.ContentType);
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteFileAsync(string key)
    {
        await _dogImageRepo.Delete(new Guid(key));
        return NoContent();
    }
}