namespace dog_api.Models.App;

public class DataBaseOptions
{
    public const string Section = "DataBase";

    public string Password { get; set; } = String.Empty;
    public int Port { get; set; }
    public string Username { get; set; } = String.Empty;
    public string Host { get; set; } = String.Empty;
    public string DataBase { get; set; } = String.Empty;

}