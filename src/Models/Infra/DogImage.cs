namespace dog_api.Models.Infra;

public class DogImage
{
    public Guid Id { get; set; }
    public string FileType { get; set; } = String.Empty;
    public string Name { get; set; } = String.Empty;
    public DateTime TimeAdded { get; set; }
}