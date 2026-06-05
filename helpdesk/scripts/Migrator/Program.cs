using MongoDB.Driver;
using HelpdeskAPI.Mapping;
using HelpdeskAPI.Entities;

namespace Migrator;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Migrador de Base de Datos Helpdesk");

        var connectionString = Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING");
        if (string.IsNullOrEmpty(connectionString))
        {
            Console.WriteLine("Error: Variable de entorno MONGO_CONNECTION_STRING no definida.");
            return;
        }

        var client = new MongoClient(connectionString);
        var database = client.GetDatabase("HelpdeskDB");

        // Registrar mapas (asegura que las clases estén mapeadas)
        EntityMapper.RegisterMaps();

        // 1. Crear índices y colecciones si no existen
        await CreateCollections(database);

        // 2. Insertar datos iniciales (categorías, roles)
        await SeedData(database);

        Console.WriteLine("Migración completada con éxito.");
    }

    static async Task CreateCollections(IMongoDatabase db)
    {
        var collections = await (await db.ListCollectionNamesAsync()).ToListAsync();
        if (!collections.Contains("Tickets"))
            await db.CreateCollectionAsync("Tickets");
        if (!collections.Contains("Categories"))
            await db.CreateCollectionAsync("Categories");
        if (!collections.Contains("Users"))
            await db.CreateCollectionAsync("Users");

        // Crear índices en Tickets (por ejemplo, en UserId y Status)
        var ticketsCollection = db.GetCollection<Ticket>("Tickets");
        var indexKeys = Builders<Ticket>.IndexKeys.Ascending(t => t.UserId);
        await ticketsCollection.Indexes.CreateOneAsync(new CreateIndexModel<Ticket>(indexKeys));
    }

    static async Task SeedData(IMongoDatabase db)
    {
        var categories = db.GetCollection<Category>("Categories");
        if (await categories.CountDocumentsAsync(_ => true) == 0)
        {
            await categories.InsertManyAsync(new[]
            {
                new Category { Name = "Hardware", Description = "Problemas de equipo físico" },
                new Category { Name = "Software", Description = "Aplicaciones y sistemas operativos" },
                new Category { Name = "Redes", Description = "Conectividad y acceso" }
            });
        }

        var users = db.GetCollection<User>("Users");
        if (await users.CountDocumentsAsync(_ => true) == 0)
        {
            await users.InsertManyAsync(new[]
            {
                new User { Email = "admin@helpdesk.com", Name = "Admin", Role = "admin" },
                new User { Email = "agent1@helpdesk.com", Name = "Agente Soto", Role = "agent" },
                new User { Email = "user@example.com", Name = "Usuario Final", Role = "user" }
            });
        }
    }
}