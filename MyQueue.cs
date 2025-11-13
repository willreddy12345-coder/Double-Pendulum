using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoublePendulum
{
    //class making a queue to contain nodes used for trail 
    internal class MyQueue
    {
        List<Node> items;
        public MyQueue()
        {
            items = new List<Node>();
        }
        public void enqueue(Node node)
        {
            items.Add(node);
        }
        public Node dequeue()
        {
            Node returnedNode = items[0];
            items.RemoveAt(0);
            return returnedNode;
        }
        public bool checkEmpty()
        {
            if (items.Count == 0)
            {
                return true;
            }
            return false;
        }
        public int GetSize()
        {
            return items.Count;
        }
        public void clear()
        {
            items.Clear();
        }
    }
}
