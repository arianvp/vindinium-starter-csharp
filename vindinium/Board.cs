using System;
using System.Collections.Generic;

namespace vindinium
{
    class Board
    {
        private Tile[,] board;

        public Board(Tile[,] board)
        {
            this.board = board;
        }

        public Tile this[int i, int j]
        {
            get
            {
                return board[i, j];
            }
            set
            {
                board[i, j] = value;
            }
        }

        public Tile this[Pos p]
        {
            get
            {
                return this[p.x, p.y];
            }

            set
            {
                this[p.x, p.y] = value;
            }
        }


        public int Width
        {
            get { return board.GetLength(0); }
        }

        public int Height
        {
            get { return board.GetLength(1); }
        }
       

        // http://buildnewgames.com/astar/
        // return positions for a certain tile:
        public Pos[] PositionsOf(Tile t)
        {
            List<Pos> tilePositions = new List<Pos>();

            for(int x = 0; x < board.GetLength(0); x++)
            {
                for (int y = 0; y < board.GetLength(1); y++)
                {
                    if (board[x,y] == t)
                    {
                        tilePositions.Add(new Pos() { x=x, y=y });
                    }
                }
            }

            return tilePositions.ToArray();
        }

        public Pos[] Neighbours (Pos p)
        {
            var x = p.y;
            var y = p.y;
            var N = p.y - 1;
            var S = p.y + 1;
            var E = p.x + 1;
            var W = p.x - 1;

            var myN = N > -1 && CanWalkHere(new Pos(){x =x, y=N});
            var myS = S < Height && CanWalkHere(new Pos(){x=x, y=S});
            var myE = E < Width && CanWalkHere(new Pos(){x=E, y=y});
            var myW = W > -1 && CanWalkHere(new Pos(){x=W, y=y});

            var results = new List<Pos>();
            if (myN)
            {
                results.Add(new Pos() { x=x, y=N });
            }
            if (myE)
            {
                results.Add(new Pos() {x=E, y=y});
            }
            if (myS)
            {
                results.Add(new Pos() { x=x, y=S });
            }
            if (myW)
            {
                results.Add(new Pos() { x=W, y=y });
            }

            return results.ToArray();
        }


        public bool CanWalkHere(Pos p)
        {
            return p.x <= Width && p.y <= Height && this[p] !=
                Tile.IMPASSABLE_WOOD;
        }




        public Pos[] FindPath(Pos start, Pos goal)
        {
            var myPathStart = new Node(this, null, start);
            var myPathEnd = new Node(this, null, goal);



            List<Node> closed = new List<Node>();
            List<Node> open = new List<Node>(new[]{myPathStart});

            List<Pos> result = new List<Pos>();

            Pos[] myNeighbours = null;
            Node myNode = null;
            Node myPath = null;
            int length, max, min, i, j;
            int size = Width * Height;

            bool[] aStar = new bool[size];

            while ((length = open.Count) > 0)
            {
                max = size;
                min = -1;

                for (i = 0; i < length; i++)
                {
                    if (open[i].F < max)
                    {
                        max = open[i].F;
                        min = i;
                    }
                }
                myNode = open[min];
                open.RemoveAt(min);

                if (myNode.Value == myPathEnd.Value)
                {
                    closed.Add(myNode);
                    myPath = closed[closed.Count - 1];
                    do
                    {
                        result.Add(myPath.Pos);
                    } while ((myPath = myPath.Parent) != null);
                } else {
                    myNeighbours = Neighbours(myNode.Pos);
                    for (i = 0, j = myNeighbours.Length; i < j; i++)
                    {
                        myPath = new Node(this, myNode, myNeighbours[i]);
                        if (!aStar[myPath.Value])
                        {
                            myPath.G = myNode.G +
                                myNeighbours[i].ManhattanDistance(myNode.Pos);

                            myPath.F = myNeighbours[i]
                                .ManhattanDistance(myPathEnd.Pos);

                            open.Add(myPath);
                            aStar[myPath.Value] = true;

                        }
                    }
                    closed.Add(myNode);
                }

            }
            return result.ToArray();
          
   }
   
    class Node
    {
        public Node Parent;
        public Pos Pos;
        public int Value, F, G;
        public Node(Board b, Node parent, Pos p)
        {
            this.Parent = parent;
            this.Pos = p;
            this.Value = p.x + (p.y * b.Width);
            this.F = 0;
            this.G = 0;
        }
    }
}
       
}


