using MongoDB.Bson.Serialization;
using HelpdeskAPI.Entities;

namespace HelpdeskAPI.Mapping;

public static class EntityMapper
{
    public static void RegisterMaps()
    {
        if (!BsonClassMap.IsClassMapRegistered(typeof(Ticket)))
        {
            BsonClassMap.RegisterClassMap<Ticket>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
                cm.MapMember(c => c.Status).SetDefaultValue("abierto");
                cm.MapMember(c => c.Priority).SetDefaultValue("media");
            });
        }

        if (!BsonClassMap.IsClassMapRegistered(typeof(Category)))
        {
            BsonClassMap.RegisterClassMap<Category>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
            });
        }

        if (!BsonClassMap.IsClassMapRegistered(typeof(User)))
        {
            BsonClassMap.RegisterClassMap<User>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
            });
        }
    }
}