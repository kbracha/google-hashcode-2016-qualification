using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace google_hashcode
{
    class Client
    {
        public int x;
        public int y;
        public int orderCount;
        public List<int> orderType = new List<int>();
        public bool [] product;
        public int myIndex;

        public Client(int x, int y, int orderCount, int productCount, int myIndex)
        {
            this.x = x;
            this.y = y;
            this.orderCount = orderCount;
            product = new bool[productCount];
            this.myIndex = myIndex;
        }

    }
}
