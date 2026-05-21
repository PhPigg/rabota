using Microsoft.EntityFrameworkCore;
using Infostructure;

namespace Asp.NET.Extension;

public static class WebApplicationExtension
{
    extension(WebApplication app)
    {
        public async Task BuildDatabase()
        {
            await using AsyncServiceScope scope = app
                .Services
                .CreateAsyncScope();
            await using Application_db_Context context = scope
                .ServiceProvider
                .GetRequiredService<Application_db_Context>();
            try
            {
                await context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                string msg = $"Ошибка при создании базы данных: {ex.Message}";
                Console.WriteLine(msg);
            }
        }
    }
}
