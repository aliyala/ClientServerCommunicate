using System.Data.Entity;

namespace ClientServerCommunicateSql
{
    public class ModelConfiguration<T> where T : class
    {
        public static void Configure(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<T>();
        }
    }
}