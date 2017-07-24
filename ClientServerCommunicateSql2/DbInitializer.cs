using SQLite.CodeFirst;
using System.Data.Entity;

namespace ClientServerCommunicateSql
{
    /// <summary>
    /// Класс инициализации базы данных
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DbInitializer<T> : SqliteCreateDatabaseIfNotExists<T> where T : DbContext
    {
        public DbInitializer(DbModelBuilder modelBuilder)
            : base(modelBuilder)
        { }
    }
}