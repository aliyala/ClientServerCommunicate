using System;
using System.Collections.Generic;

namespace ClientServerCommunicate.Data.Repositories
{
    /// <summary>
    /// Интерфейс репозитория сообщений
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMessagesRepository<T> : IDisposable
    {
        /// <summary>
        /// Метод получения всех сообщений
        /// </summary>
        IEnumerable<T> AllMessages { get; }

        /// <summary>
        /// Метод получения сообщения
        /// </summary>
        /// <param name="id">Идентификатор сообщения</param>
        /// <returns>Сообщение</returns>
        T Find(int id);

        /// <summary>
        /// Метод добавления сообщения
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <returns>Сообщение</returns>
        T Add(T message);

        /// <summary>
        /// Метод удаления сообщения
        /// </summary>
        /// <param name="id">Идентификатор сообщения</param>
        void Delete(int id);

        /// <summary>
        /// Метод обновления сообщения
        /// </summary>
        /// <param name="message">Сообщение</param>
        void Update(T message);

        /// <summary>
        /// Сохранение изменений
        /// </summary>
        void Save();
    }
}