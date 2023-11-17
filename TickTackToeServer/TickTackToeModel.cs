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
    class TickTackToeModel
    {
        TCPConnector _connector;
        bool _crossTurn;
        string _area;
        int _count;
        public TickTackToeModel() {
            _connector = new TCPConnector();
            _crossTurn = true;
            _area = "012345678";
            _count = 0;
        }
        public void Game() {
            _connector.WaitClient(2);
            _connector.SendMessage(_connector.Clients[0], "<TURN>1");
            while (_connector.IsAllConnected()) {
                Move(_crossTurn ? _connector.Clients[0] : _connector.Clients[1]);
                _crossTurn = !_crossTurn;
                Thread.Sleep(3000);
                _area = "012345678";
                _count = 0;
                _connector.SendMessagesToAll(_area);
            }
        }
        public void Move(TcpClient player)
        {
            _connector.SendMessagesToAll(!_crossTurn ? "<INFO> Ходят нолики" : "<INFO> Ходят крестики");
            NetworkStream stream = player.GetStream();
            string message;
            do
            {
                StreamReader reader = new StreamReader(stream);
                message = reader.ReadLine();
            }
            while (_area.IndexOf(message[0]) == -1);
            _area = _area.Replace(message[0], _crossTurn ? 'X' : 'O');
            _connector.SendMessagesToAll(_area); 
            _count++;
            _crossTurn = !_crossTurn;
            if (!Winner())
            {
                if (_count >= 9)
                    _connector.SendMessagesToAll("<INFO> Ничья");
                else
                {
                    Move(_crossTurn ? _connector.Clients[0] : _connector.Clients[1]);
                }
            }
            else
            {
                _connector.SendMessagesToAll(_crossTurn ? 
                    "<INFO> Нолики победили" : "<INFO> Крестики победили");
            }
        }
        bool Winner()
        {
            if (_area[0] == _area[1] && _area[1] == _area[2]) return true;
            if (_area[3] == _area[4] && _area[4] == _area[5]) return true;
            if (_area[6] == _area[7] && _area[7] == _area[8]) return true;

            if (_area[0] == _area[3] && _area[3] == _area[6]) return true;
            if (_area[1] == _area[4] && _area[4] == _area[7]) return true;
            if (_area[2] == _area[5] && _area[5] == _area[8]) return true;

            if (_area[0] == _area[4] && _area[4] == _area[8]) return true;
            if (_area[2] == _area[4] && _area[4] == _area[6]) return true;
            return false;
        }
    }
}
