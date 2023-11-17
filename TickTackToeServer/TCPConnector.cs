using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Drawing;
using System.Threading;

namespace TickTackToeServer
{
    class TCPConnector
    {
        public List<TcpClient> Clients { get; private set; }
        TcpListener _listener;
        public TCPConnector() {
            _listener = new TcpListener(IPAddress.Any, 8888);
            Clients = new List<TcpClient>();
        }
        public void WaitClient(int clientsCount)
        {
            _listener.Start();
            for (int i = 0; i < clientsCount; i++) {
                Console.WriteLine($"Ожидание {i + 1} игрока");
                SendMessagesToAll($"<INFO>Ожидание {i + 1} игрока");
                Clients.Add(_listener.AcceptTcpClient());
            }
            SendMessagesToAll("<INFO>Все игроки подключились. Начало игры");
            Console.WriteLine($"Все игроки подключились.");
        }
        public void SendMessage(TcpClient client, string message)
        {
            NetworkStream network = client.GetStream();
            StreamWriter writer = new StreamWriter(network);
            writer.WriteLine(message + "\n");
            writer.Flush();
        }
        public void SendMessagesToAll(string message) {
            foreach (TcpClient client in Clients) {
                SendMessage(client, message);
            }
        }
        public bool IsAllConnected() {
            foreach (TcpClient client in Clients) {
                if (!client.Connected)
                    return false;
            }
            return true;
        }
    }
}
