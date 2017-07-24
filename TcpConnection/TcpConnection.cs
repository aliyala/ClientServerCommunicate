using System.Net;
using System.Net.Sockets;

namespace Connection
{
    /// <summary>
    /// Класс TCP подключения
    /// </summary>
    public class TcpConnection
    {
        public TcpConnection(string ip)
        {
            IP = ip;
        }

        public string IP;

        public IPEndPoint IPEndPoint { get; set; }

        /// <summary>
        /// Получить сокет конечной точки
        /// </summary>
        /// <returns></returns>
        public Socket GetSocket()
        {
            // Устанавливаем для сокета локальную конечную точку
            IPHostEntry ipHost = Dns.GetHostEntry(IP);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint = new IPEndPoint(ipAddr, 11000);

            // Создаем сокет Tcp
            Socket socket = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            return socket;
        }

        /// <summary>
        /// Получить сокет сервера
        /// </summary>
        /// <returns></returns>
        public Socket GetListenSocket()
        {
            Socket socket = GetSocket();

            socket.Bind(IPEndPoint);
            socket.Listen(10);

            return socket;
        }

        /// <summary>
        /// Получить сокет клиента
        /// </summary>
        /// <returns></returns>
        public Socket GetSenderSocket()
        {
            Socket socket = GetSocket();
            socket.Connect(IPEndPoint);

            return socket;
        }
    }
}