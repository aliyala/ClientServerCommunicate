namespace ClientServerCommunicate.Data
{
    /// <summary>
    /// Сообщение с флагом отправлено/не отправлено
    /// </summary>
    public class ClientMessage : Message
    {
        public bool IsSent { get; set; }
    }
}