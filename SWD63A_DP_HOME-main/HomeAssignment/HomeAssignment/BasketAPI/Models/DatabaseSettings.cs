﻿namespace BasketAPI.Models
{
    public class DatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string BasketCollectionName { get; set; } = null!;
    }
}
