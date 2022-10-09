namespace dog_api.Repositories;

using Amazon.S3;
using Amazon.S3.Model;

using dog_api.Models.App;

public interface IDogImageFileRepo
{
    Task Upload(IFormFile file, Guid id);
    Task<IEnumerable<DogFileUrl>> GetAll();
    Task<DogFileUrl> GetFileUrl(Guid id);
    Task<DogFileStream> GetFileStream(Guid id);
    Task Delete(Guid id);
}

public class S3DogImageFileRepo : IDogImageFileRepo
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucket = "dogblogpictures";    // FIXME hardcoding is bad

    public S3DogImageFileRepo(IAmazonS3 s3Client)
    {
        _s3Client = s3Client;
    }

    public async Task Upload(IFormFile file, Guid id)
    {
        await validateBucket(_bucket);
        var request = new PutObjectRequest()
        {
            BucketName = _bucket,
            Key = id.ToString(),
            InputStream = file.OpenReadStream()
        };
        request.Metadata.Add("Content-Type", file.ContentType);
        await _s3Client.PutObjectAsync(request);
    }

    public async Task<IEnumerable<DogFileUrl>> GetAll()
    {
        await validateBucket(_bucket);
        var request = new ListObjectsV2Request()
        {
            BucketName = _bucket
        };
        var result = await _s3Client.ListObjectsV2Async(request);
        var s3Objects = result.S3Objects.Select(s =>
        {
            var urlRequest = new GetPreSignedUrlRequest()
            {
                BucketName = _bucket,
                Key = s.Key,
                Expires = DateTime.UtcNow.AddMinutes(1)
            };
            return new DogFileUrl()
            {
                Name = s.Key.ToString(),
                Url = _s3Client.GetPreSignedURL(urlRequest),
            };
        });
        return s3Objects;
    }

    public async Task<DogFileUrl> GetFileUrl(Guid id)
    {
        await validateBucket(_bucket);
        var s3Object = await _s3Client.GetObjectAsync(_bucket, id.ToString());
        var urlRequest = new GetPreSignedUrlRequest()
        {
            BucketName = _bucket,
            Key = id.ToString(),
            Expires = DateTime.UtcNow.AddMinutes(1)
        };
        return new DogFileUrl()
        {
            Name = id.ToString(),
            Url = _s3Client.GetPreSignedURL(urlRequest),
        };
    }

    public async Task<DogFileStream> GetFileStream(Guid id)
    {
        await validateBucket(_bucket);
        var s3Object = await _s3Client.GetObjectAsync(_bucket, id.ToString());
        return new DogFileStream()
        {
            Stream = s3Object.ResponseStream,
            ContentType = s3Object.Headers.ContentType
        };
    }

    private async Task validateBucket(string bucket)
    {
        var bucketExists = await _s3Client.DoesS3BucketExistAsync(_bucket);
        if (!bucketExists) throw new InvalidOperationException($"Bucket {_bucket} does not exist.");
    }

    public async Task Delete(Guid id)
    {
        await validateBucket(_bucket);
        await _s3Client.DeleteObjectAsync(_bucket, id.ToString());
    }

}