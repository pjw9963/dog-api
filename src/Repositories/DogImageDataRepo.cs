namespace dog_api.Repositories;

using Microsoft.EntityFrameworkCore;

using dog_api.Models.Infra;

public interface IDogImageDataRepo
{
    Task<DogImage> AddDogImage(Guid id, string filename, string filetype);
    Task<DogImage> GetDogImage(Guid id);
    Task<DogImage> GetLatestDogImage();
}

public class DogImagesEFCoreRepo : IDogImageDataRepo
{
    private readonly ApplicationContext _context;

    public DogImagesEFCoreRepo(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<DogImage> AddDogImage(Guid id, string filename, string filetype)
    {
        var dogImage = new DogImage()
        {
            Id = id,
            FileType = filetype,
            Name = filename,
            TimeAdded = DateTime.Now
        };
        await _context.AddAsync(dogImage);
        await _context.SaveChangesAsync();
        return dogImage;
    }

    public async Task<DogImage> GetDogImage(Guid id)
    {
        var dogImage = await _context.DogImages.Where(dImg => dImg.Id == id).FirstAsync();
        return dogImage;
    }

    public async Task<DogImage> GetLatestDogImage()
    {
        var dogImage = await _context.DogImages.OrderByDescending(dImg => dImg.TimeAdded).FirstAsync();
        return dogImage;
    }
}

