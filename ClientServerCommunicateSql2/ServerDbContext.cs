using ClientServerCommunicate.Data;
using ClientServerCommunicate.Data.Models;
using SQLite.CodeFirst;
using System;
using System.Data.Common;
using System.Data.Entity;

namespace ClientServerCommunicateSql
{
    public class ServerDbContext : DbContext
    {
        public ServerDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            Configure();
        }

        public ServerDbContext(DbConnection connection, bool contextOwnsConnection)
            : base(connection, contextOwnsConnection)
        {
            Configure();
        }

        private void Configure()
        {
            Configuration.ProxyCreationEnabled = true;
            Configuration.LazyLoadingEnabled = true;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            ModelConfiguration<ServerMessage>.Configure(modelBuilder);
            var initializer = new DbInitializer<ServerDbContext>(modelBuilder);
            Database.SetInitializer(initializer);
        }
    }
}