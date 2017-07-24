using ClientServerCommunicate.Data;
using ClientServerCommunicate.Data.Models;
using ClientServerCommunicate.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;

namespace ClientServerCommunicateSql.Repositories
{
    /// <summary>
    /// Репозиторий сообщений сервера
    /// </summary>
    public class ServerMessagesRepository : IMessagesRepository<ServerMessage>
    {
        private DbContext context;
        private DbSet<ServerMessage> dbSet;

        /// <summary>
        /// Конструктор репозитория сообщений сервера
        /// </summary>
        /// <param name="context">Контекст базы данных</param>
        public ServerMessagesRepository(ServerDbContext context)
        {
            this.context = context;
            dbSet = context.Set<ServerMessage>();
        }

        /// <summary>
        /// Список сообщений
        /// </summary>
        public IEnumerable<ServerMessage> AllMessages => Get().ToList();

        /// <summary>
        /// Добавление сообщения
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <returns>Сообщение</returns>
        public ServerMessage Add(ServerMessage message)
        {
            return dbSet.Add(message);
        }

        /// <summary>
        /// Удаление сообщения
        /// </summary>
        /// <param name="id">Идентификатор</param>
        public void Delete(int id)
        {
            ServerMessage entityToDelete = dbSet.Find(id);
            dbSet.Remove(entityToDelete);
        }

        /// <summary>
        /// Поиск сообщения
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Сообщение</returns>
        public ServerMessage Find(int id)
        {
            var item = dbSet.Find(id);
            if (item != null)
                return item;

            return null;
        }

        /// <summary>
        /// Получение списка сообщений по фильтрам
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public IEnumerable<ServerMessage> Get(
            Expression<Func<ServerMessage, bool>> filter = null,
            Func<IQueryable<ServerMessage>,
            IOrderedQueryable<ServerMessage>> orderBy = null)
        {
            IQueryable<ServerMessage> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        /// <summary>
        /// Обновление сообщения
        /// </summary>
        /// <param name="message">Сообщение</param>
        public void Update(ServerMessage message)
        {
            dbSet.AddOrUpdate(message);
        }

        /// <summary>
        /// Сохранение изменения
        /// </summary>
        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}