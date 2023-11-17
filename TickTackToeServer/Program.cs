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
    class Program
    {
        static TickTackToeModel game;
        static void Main(string[] args)
        {
            game = new TickTackToeModel();
            game.Game();
        }
    }
}
