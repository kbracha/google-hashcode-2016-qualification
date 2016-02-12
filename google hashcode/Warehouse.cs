using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace google_hashcode
{
    class Warehouse
    {
        public int x;
        public int y;
        public List<int>productAvailability = new List<int>();
        public int myIndex;


        public Warehouse(int x, int y, int myIndex)
        {
            this.x = x;
            this.y = y;
            this.myIndex = myIndex;
        }

    }
}
