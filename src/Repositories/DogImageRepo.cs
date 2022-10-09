namespace dog_api.Repositories;

using dog_api.Models.Domain;

public interface IDogImageRepo
{
    Task<string> Add(IFormFile file);
    Task<DogImageStream> GetDogImageStream(Guid id);
    Task<DogImageUrl> GetDogImageUrl(Guid id);
    Task Delete(Guid id);
}

public class DogImageRepo : IDogImageRepo
{

    private readonly IDogImageDataRepo _metadataRepo;   //TODO refactor with these names
    private readonly IDogImageFileRepo _fileRepo;

    public DogImageRepo(IDogImageDataRepo metadataRepo, IDogImageFileRepo fileRepo)
    {
        _metadataRepo = metadataRepo;
        _fileRepo = fileRepo;
    }

    public async Task<string> Add(IFormFile file)
    {
        var guid = new Guid();
        await _fileRepo.Upload(file, guid);
        await _metadataRepo.AddDogImage(guid, file.FileName, file.ContentType); //TODO handle errors
        return guid.ToString();
    }

    public async Task<DogImageStream> GetDogImageStream(Guid id)
    {
        var stream = _fileRepo.GetFileStream(id);   //TODO handle errors here too
        var data = _metadataRepo.GetDogImage(id);
        return new DogImageStream(await stream, await data);
    }

    public async Task<DogImageUrl> GetDogImageUrl(Guid id)
    {
        var url = _fileRepo.GetFileUrl(id);   //TODO handle errors here too
        var data = _metadataRepo.GetDogImage(id);
        return new DogImageUrl(await url, await data);
    }

    public async Task Delete(Guid id)
    {
        throw new NotImplementedException();
    }
}