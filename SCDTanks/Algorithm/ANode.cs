using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCDTanks.Algorithm
{
    public class ANode:IComparable
    {
        public string Id { get; set; }
        /// <summary>
        /// 实际代价
        /// </summary>
        public int GCost { get; set; }
        /// <summary>
        /// 预算代价
        /// </summary>
        public int HCost { get; set; }
        /// <summary>
        /// 总代价
        /// </summary>
        public int FCost { get; set; }
        /// <summary>
        /// 是否是障碍物
        /// </summary>
        public bool Obstacles { get; set; }
        /// <summary>
        /// 父节点
        /// </summary>
        public ANode Parent { get; set; }
        /// <summary>
        /// 坐标
        /// </summary>
        public Point Position { get; set; }
        
        public ANode(Point point,int gc,int hc,bool obs,ANode parentNode)
        {
            this.GCost = gc;
            this.HCost = hc;
            this.FCost = this.GCost + this.HCost;
            this.Obstacles = obs;
            this.Parent = parentNode;
            this.Position = point;
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            ANode aNode = obj as ANode;

            if (this.FCost.CompareTo(aNode.FCost) != 0)
                return (this.FCost.CompareTo(aNode.FCost));
            else if (this.HCost.CompareTo(aNode.HCost) != 0)
                return (this.HCost.CompareTo(aNode.HCost));
            else
                return 1;
        }
    }
}
