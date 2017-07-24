using ClientServerCommunicate.Data;
using System.Data.Common;
using System.Data.Entity;

namespace ClientServerCommunicateSql
{
    public class ClientDbContext : DbContext
    {
        public ClientDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            Configure();
        }

        public ClientDbContext(DbConnection connection, bool contextOwnsConnection)
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
            ModelConfiguration<ClientMessage>.Configure(modelBuilder);
            var initializer = new DbInitializer<ClientDbContext>(modelBuilder);
            Database.SetInitializer(initializer);
        }
    }
}