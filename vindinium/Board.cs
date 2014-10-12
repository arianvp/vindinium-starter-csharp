using System;
using System.Collections.Generic;
using System.Linq;
using Mischel.Collections;


// Ik weet nu wat er fout gaat
namespace vindinium
{
    class Board
    {
		private Node[,] board;

		public Board(Node[,] board)
        {
            this.board = board;
        }

		public Node this[int i, int j]
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

		public Node this[Pos p]
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
//        public Pos[] PositionsOf(Tile t)
//        {
//            List<Pos> tilePositions = new List<Pos>();
//
//            for(int x = 0; x < board.GetLength(0); x++)
//            {
//                for (int y = 0; y < board.GetLength(1); y++)
//                {
//                    if (board[x,y] == t)
//                    {
//                        tilePositions.Add(new Pos(x,y));
//                    }
//                }
//            }
//
//            return tilePositions.ToArray();
//        }
//
  

        public bool CanWalkHere(int x, int y)
        {
			// todo impassable
			return x >= 0 && y >= 0 && x < Width && y < Height && board[x,y].Tile != Tile.IMPASSABLE_WOOD;
        }


        public bool CanWalkHere(Pos n)
        {
            return CanWalkHere(n.x, n.y);
        }

		public int Heuristic(Node goal, Node next)
        {
            return goal.ManhattanDistance(next);
        }

        /// <summary>
        /// return a cost of a node..  
        /// For now this is just 1. But it could be different for different
        /// situations
        /// the cost determines if the path-finding algorithm avoids this
        /// location.
        /// 
        /// One could for example avoid goblins when HP is low..
        /// </summary>
        /// <param name="n">N.</param>
		public int Cost(Node n)
		{
			return 1;
        }
                    
		public IEnumerable<Node> PathFind(Node start, Node goal)
		{
			var frontier = new Mischel.Collections.PriorityQueue<Node, int>();
			frontier.Enqueue(start,0);
			start.Opened = true;

			while (frontier.Count() > 0)
			{
				var current = frontier.Dequeue().Value;
				if (current == goal)
				{
					return backTrace(goal);
				}

				foreach (var next in Neighbours(current))
				{
					var newCost = current.CostSoFar + Cost(next);
					if (!next.Opened || newCost < next.CostSoFar)
					{
						next.CostSoFar = newCost;
						next.Opened = true;
						var priority = newCost + goal.ManhattanDistance(next);
						frontier.Enqueue(next,priority);
						next.Parent = current;
					}
				}
			}
			return null;
		}


		public IEnumerable<Node> Neighbours(Node p)
        {
			var x = p.X;
			var y = p.Y;
			var N = p.Y - 1;
			var S = p.Y + 1;
			var E = p.X + 1;
			var W = p.X - 1;
            
            

			if (CanWalkHere(x, N))
            {
				yield return this[x,N];
            }
			if ( CanWalkHere(E, y))
            {
				yield return this[E,y];
            }
			if (CanWalkHere(x, S))
            {
				yield return this[x,S];
            }
			if (CanWalkHere(W, y))
            {
				yield return this[W,y];
            }

        }

		private static List<Node> backTrace(Node n)
        {
			List<Node> result = new List<Node>();
            result.Add(n);
            while (n.Parent != null)
            {
                n = n.Parent;
                result.Add(n);
            }


            result.Reverse();
            return result;
        }
    }


	public class Node : IComparable<Node>
	{
		public Tile Tile;
		public int X;
		public int Y;
		public Node Parent;
		public int CostSoFar = 0;
		public int F,G,H;
		public bool Opened = false;
		public bool Closed = false;


		public Node(Tile tile, int x, int y)
		{
			this.Tile = tile;
			this.X = x;
			this.Y = y;
		}

		public int CompareTo(Node that)
		{
			return this.F - that.F;
		}
			
		public int ManhattanDistance(Node that)
		{
			return Math.Abs(this.X - that.X) + Math.Abs(this.Y - that.Y);
		}

//		public string MoveTo(Pos n)
//		{
//			if (x - n.x == 0)
//			{
//				if (y - n.y < 0)
//					return Direction.East;
//				else if (y - n.y > 0)
//					return Direction.West;
//				else
//					return Direction.Stay;
//			}
//			else if (y - n.y == 0)
//			{
//				if (x - n.x < 0)
//					return Direction.South;
//				else if (x - n.x > 0)
//					return Direction.North;
//				else
//					return Direction.Stay;
//			}
//			return Direction.Stay;
//		}

	}
	public class NodeComparer : IComparer<Node>
	{
		public int Compare(Node a, Node b)
		{
			return a.F - b.F;
		}
	}  
}


