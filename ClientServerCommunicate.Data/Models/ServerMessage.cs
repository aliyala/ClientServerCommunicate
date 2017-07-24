namespace ClientServerCommunicate.Data.Models
{
    using System;

    /// <summary>
    /// Сообщение с датой и временем поступления и IP источника
    /// </summary>
    public class ServerMessage : Message
    {
        public DateTime DateTime { get; set; }

        public string IP { get; set; }
    }
}