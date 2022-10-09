namespace dog_api.Models.Domain;

using dog_api.Models.App;
using dog_api.Models.Infra;  // FIXME i think the point of layers is so they don't depend on each other...

public class DogImageUrl
{
    public DogFileUrl File { get; set; }
    public DogImage Info { get; set; }

    public DogImageUrl(DogFileUrl file, DogImage info)  //TODO constructor or no constructor?
    {
        File = file;
        Info = info;
    }
}