using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCDTanks.Algorithm
{
    public class Astar
    {
        private List<ANode> closeList = new List<ANode>();
        private List<ANode> openList=new List<ANode>();
        public string Team { get; set; }
        private Point endPoint;
        private Point starPoint;
        public ANode Star(Point ownpoint, Point tarPoint, ANode[,] map)
        {
            endPoint = tarPoint;
            starPoint = ownpoint;
            openList.Add(map[ownpoint.X, ownpoint.Y]);
            int row = map.GetLength(0);
            int col = map.GetLength(1);
            int nextX, nextY;
            ANode aNode = null;
            do
            {
                
                openList.Sort();
                if(openList.Count==0)
                {
                    return map[ownpoint.X, ownpoint.Y];
                }
                aNode = openList[0];
                openList.Remove(aNode);
                closeList.Add(aNode);
                if (aNode.Position==tarPoint)
                {
                    break;
                }
                //左
                nextY = aNode.Position.Y - 1;
                if (nextY >= 0)
                {
                    AddOpenList(map[aNode.Position.X,nextY], aNode);
                }
                //右
                nextY = aNode.Position.Y + 1;
                if (nextY < col)
                {
                    AddOpenList(map[aNode.Position.X,nextY], aNode);
                }
                //上
                nextX = aNode.Position.X - 1;
                if (nextX >= 0)
                {
                    AddOpenList(map[nextX,aNode.Position.Y], aNode);
                }
                //下 
                nextX = aNode.Position.X + 1;
                if (nextX < row)
                {
                    AddOpenList(map[nextX,aNode.Position.Y], aNode);
                }
            } while (true);

            return aNode;
        }
        private void AddOpenList(ANode nextNode, ANode nowNode)
        {
            if (nextNode.Obstacles) return;
            if (closeList.Contains(nextNode)) return;
            if (openList.Contains(nextNode)) return;
            openList.Add(nextNode);
            nextNode.GCost = nowNode.GCost + GetCost(nextNode.Position.X, nextNode.Position.Y, nowNode.Position.X, nowNode.Position.Y);
            nextNode.HCost = GetCost(nextNode.Position.X, nextNode.Position.Y, endPoint.X, endPoint.Y);
            nextNode.FCost = nextNode.GCost + nextNode.HCost;
            nextNode.Parent = nowNode;
        }
        private int GetCost(int curX, int curY, int targetX, int targetY)
        {
            ////斜对角一格的距离
            //const int diagonalDist = 14;
            ////横纵一格的距离
            //const int straightDist = 10;
            int delta1 = Math.Abs(curX - targetX);
            int delta2 = Math.Abs(curY - targetY);
            return (delta1 + delta2) * 10;
            //if (delta1 == delta2)
            //{
            //    return delta1 * 14;
            //}
            //else if (delta1 < delta2)
            //{
            //    return delta1 * 14 + (delta2 - delta1) * 10;
            //}
            //else
            //{
            //    return delta2 * 14 + (delta1 - delta2) * 10;
            //}
        }
        public ANode Star(Point ownpoint, Point tarPoint, string[,] map)
        {
           return Star(ownpoint, tarPoint, GetMap(map));
        }
        private ANode[,] GetMap(string[,] map)
        {
            int row = map.GetLength(0);
            int col = map.GetLength(1);
            ANode[,] aNodes = new ANode[row, col];
            ANode aNode;
            bool isobs = true; ;
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    switch (map[i, j])
                    {
                        case "M1":
                        case "M2":
                        case "M3":
                            isobs = false;
                            break;
                        default: isobs = true; break;
                    }
                    aNode = new ANode(new Point(i, j), 0, 0, isobs, null)
                    {
                        Id = map[i, j]
                    };
                    aNodes[i, j] = aNode;
                }
            }
            return aNodes;
        }
    }
}
