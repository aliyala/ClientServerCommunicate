using ClientServerCommunicate.Data;
using ClientServerCommunicateSql;
using ClientServerCommunicateSql.Repositories;
using Connection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace ClientApp
{
    internal class Program
    {
        private static string ip;

        private static void Main(string[] args)
        {
            try
            {
                ip = ConfigurationManager.AppSettings["serverIP"];
                SendMessageFromSocket(11000);
            }
            catch (SocketException ex)
            {
                Console.WriteLine("Ошибка в соединении: {0}", ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Console.ReadLine();
            }
        }

        private static void SendMessageFromSocket(int port)
        {
            using (var context = new ClientDbContext("clientdbContext"))
            {
                var repository = new ClientMessagesRepository(context);

                // Проверяем на наличие ранее неотправленных сообщений, отправляем повторно
                SendOldMessages(repository);

                // Соединяемся с удаленным устройством
                var socket = new TcpConnection(ip).GetSenderSocket();

                Console.Write("Введите сообщение: ");
                string message = Console.ReadLine();

                var savedMessage = repository.Add(new ClientMessage
                {
                    Text = message,
                    IsSent = false
                });

                repository.Save();

                Console.WriteLine("Сокет соединяется с {0} ", socket.RemoteEndPoint.ToString());

                SendMessage(socket, savedMessage, repository);

                SendMessageFromSocket(port);

                // Освобождаем сокет
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();

                Print(repository.AllMessages);
            }
        }

        private static void Print(IEnumerable<ClientMessage> messages)
        {
            // TODO применить Parallel.ForEach
            foreach (var message in messages)
            {
                System.Console.WriteLine("\t Text: {0}", message.Text);
                System.Console.WriteLine("\t Id: {0}", message.Id);
                System.Console.WriteLine("\t IsSent: {0}", message.IsSent);
                System.Console.WriteLine();
            }
        }

        private static void SendMessage(Socket socket, ClientMessage message, ClientMessagesRepository repository)
        {
            // Буфер для входящих данных
            byte[] bytes = new byte[1024];

            byte[] msg = Encoding.UTF8.GetBytes(message.Text);

            // Отправляем данные через сокет
            int bytesSent = socket.Send(msg);

            // Получаем ответ от сервера
            int bytesRec = socket.Receive(bytes);

            var response = Encoding.UTF8.GetString(bytes, 0, bytesRec);

            if (response == message.Text)
            {
                message.IsSent = true;
                repository.Update(message);
                repository.Save();
            }
        }

        private static void SendOldMessages(ClientMessagesRepository repository)
        {
            var notSentMessages = repository.AllMessages.Where(x => !x.IsSent).ToList();
            foreach (var message in notSentMessages)
            {
                var socket = new TcpConnection(ip).GetSenderSocket();
                SendMessage(socket, message, repository);
            }
        }
    }
}