namespace CatalogAPI.Models
{
    public class EShopStoreDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string CatalogCollectionName { get; set; } = null!;
    }
}
