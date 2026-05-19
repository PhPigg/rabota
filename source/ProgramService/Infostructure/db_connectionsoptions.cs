using Microsoft.EntityFrameworkCore;

namespace Infostructure;

public class db_connectionsoptions
{
    public required string host { get; set; }
    public required string username { get; set; }
    public required string password { get; set; }
    public required string database { get; set; }
    public required int port { get; set; }

    public string ToConnectionString()
    {
        if (string.IsNullOrWhiteSpace(host))
        {
            string error = "PostgresSettings: Host не задан.";
            throw new InvalidOperationException(error);
        }
        if (port < 1 || port > 65535)
        {   
            string error = "PostgresSettings: Port вне допустимого диапазона";
            throw new InvalidOperationException(error);
        }
        if (string.IsNullOrWhiteSpace(username))
        {
            string error = "PostgresSettings: User не задан.";
            throw new InvalidOperationException();
        }                                    
        if (string.IsNullOrWhiteSpace(password))
        {
            string error = "PostgresSettings: Password не задан.";
            throw new InvalidOperationException(error);
        }
        if (string.IsNullOrWhiteSpace(database))
        {
            string error = "PostgresSettings: Database не задан.";
            throw new InvalidOperationException(error);
        }
        string format = "Host={0};Port={1};Username={2};Password={3};Database={4};";
        return string.Format(format, host, port, username, password, database);
        
    }
}
