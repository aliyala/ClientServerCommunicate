using ClientServerCommunicate.Data.Models;
using ClientServerCommunicateSql;
using ClientServerCommunicateSql.Repositories;
using Connection;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // запуск сервера в задаче
            var task1 = Task.Factory.StartNew(() => { Listen(); });

            // ожидание команды для печати имеющихся сообщений
            ReadPrintCommand();
        }

        private static void ReadPrintCommand()
        {
            Console.WriteLine("Введите print для вывода данных");
            var command = Console.ReadLine();
            if (command.ToLower() == "print")
                Print();
            ReadPrintCommand();
        }

        private static void Listen()
        {
            try
            {
                using (var context = new ServerDbContext("serverdbContext"))
                {
                    var repository = new ServerMessagesRepository(context);

                    var socket = new TcpConnection("localhost").GetListenSocket();

                    // Начинаем слушать соединения
                    while (true)
                    {
                        // ожидание входящего соединения
                        Socket handler = socket.Accept();
                        string data = null;

                        // дождались клиента, пытающегося с нами соединиться

                        byte[] bytes = new byte[1024];
                        int bytesRec = handler.Receive(bytes);

                        data += Encoding.UTF8.GetString(bytes, 0, bytesRec);

                        // Показываем данные на консоли
                        Console.Write("Полученный текст: " + data + "\n\n");

                        var message = new ServerMessage()
                        {
                            Text = data,
                            DateTime = DateTime.Now,
                            IP = ((IPEndPoint)handler.RemoteEndPoint).Address.ToString()
                        };

                        repository.Add(message);
                        repository.Save();

                        byte[] msg = Encoding.UTF8.GetBytes(data);
                        handler.Send(msg);

                        handler.Shutdown(SocketShutdown.Both);
                        handler.Close();
                    }
                }
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

        private static void Print()
        {
            using (var context = new ServerDbContext("serverdbContext"))
            {
                var repository = new ServerMessagesRepository(context);

                var messages = repository.AllMessages;

                foreach (var message in messages)
                {
                    Console.WriteLine("\t Text: {0}", message.Text);
                    Console.WriteLine("\t Id: {0}", message.Id);
                    Console.WriteLine("\t IP: {0}", message.IP);
                    Console.WriteLine("\t DateTime: {0}", message.DateTime);
                    Console.WriteLine();
                }
            }
        }
    }
}