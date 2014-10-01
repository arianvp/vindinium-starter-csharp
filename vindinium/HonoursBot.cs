using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace vindinium
{
    class HonoursBot
    {
        private ServerStuff serverStuff;

        public HonoursBot(ServerStuff serverStuff)
        {
            this.serverStuff = serverStuff;
        }

        //starts everything
        public void run()
        {
            Console.Out.WriteLine("random bot running");

            serverStuff.createGame();

            if (serverStuff.errored == false)
            {
                //opens up a webpage so you can view the game, doing it async so we dont time out
                new Thread(() => System.Diagnostics.Process.Start(serverStuff.viewURL)).Start();
            }

            Random random = new Random();
            while (serverStuff.finished == false && serverStuff.errored == false)
            {
                Console.Out.WriteLine("completed turn " + serverStuff.currentTurn);
            }

            if (serverStuff.errored)
            {
                Console.Out.WriteLine("error: " + serverStuff.errorText);
            }

            Console.Out.WriteLine("random bot finished");
        }
    }
}
