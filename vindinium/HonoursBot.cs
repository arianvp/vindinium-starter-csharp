using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mischel.Collections;

namespace vindinium
{
    class HonoursBot
    {
        private ServerStuff s;

        public HonoursBot(ServerStuff serverStuff)
        {
            this.s = serverStuff;
        }

        //starts everything
        public void run()
        {
            Console.Out.WriteLine("random bot running");

            s.createGame();

            if (s.errored == false)
            {
                //opens up a webpage so you can view the game, doing it async so we dont time out
                new Thread(() => System.Diagnostics.Process
                           .Start(s.viewURL)).Start();
            }


            Random random = new Random();

            
			foreach (var node in  s.board.Neighbours(s.board[5,5]))
			{
				Console.WriteLine("{0} {1}", node.X, node.Y);
			}
            
			var path = s.board.PathFind(s.board[0,0], s.board[5,5]);


			foreach (var node in path)
			{
				Console.WriteLine("{0} {1}", node.X, node.Y);
			}
            while (s.finished == false && s.errored == false)
            {

//                s.moveHero(s.myHero.pos.MoveTo(path.Current));
//                if (!path.MoveNext())
//                    break;

                switch(random.Next(0, 6))
                {
					case 0:
					s.moveHero(Direction.East);
					break;
					case 1:
					s.moveHero(Direction.North);
					break;
					case 2:
					s.moveHero(Direction.South);
					break;
					case 3:
					s.moveHero(Direction.Stay);
					break;
					case 4:
					s.moveHero(Direction.West);
					break;
				}

                

                Console.Out.WriteLine("completed turn " + s.currentTurn);
            }

              
            if (s.errored)
            {
                Console.Out.WriteLine("error: " + s.errorText);
            }

            Console.Out.WriteLine("random bot finished");
        }
    }
}
