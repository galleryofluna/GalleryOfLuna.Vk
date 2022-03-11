namespace GalleryOfLuna.Vk.Configuration
{
    public enum DatabaseTypes
    {
        Default,

        SQLite,
        PostgreSQL,

#if DEBUG
        InMemory
#endif
    }
}