namespace dog_api.Models.Infra;
public class S3ObjectDto
{
    public string? Name { get; set; }
    public string? PresignedUrl { get; set; }
}
