using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace google_hashcode
{
    class Drone
    {
        public int x;
        public int y;
        public static int maxCapacity;
        public int capacityLeft;
        public List<int> item = new List<int>();
        public int turnsLeft;
        public int myIndex;

        public Drone(int turnsAvailable)
        {
            x = 0;
            y = 0;
            capacityLeft = maxCapacity;
            turnsLeft = turnsAvailable;
        }


        public bool pickProducts(Warehouse warehouse, Client client)
        {
            capacityLeft = maxCapacity;
            item.Clear();

            int turnsWarehouse = (int)Math.Ceiling(Math.Sqrt((x - warehouse.x) * (x - warehouse.x) + (y - warehouse.y) * (y - warehouse.y)));
            int turnsClient = (int)Math.Ceiling(Math.Sqrt((x - client.x) * (x - client.x) + (y - client.y) * (y - client.y)));
            int totalTurns = 2 + turnsWarehouse + turnsClient;

            if (totalTurns > turnsLeft)
                return false;

            turnsLeft -= totalTurns;

            for (int i = 0; i < client.orderCount; i++)
            {
                if (warehouse.productAvailability[client.orderType[i]] > 0 && capacityLeft >= Program.productWeight[client.orderType[i]])
                {
                    warehouse.productAvailability[client.orderType[i]]--;
                    capacityLeft -= Program.productWeight[client.orderType[i]];
                    item.Add(client.orderType[i]);
                }
            }

            if (item.Count == 0)
                return false;

            for (int i = 0; i < item.Count; i++)
            {
                using (TextWriter writer = new StreamWriter(Program.outputPath, true))
                {
                    writer.Write(myIndex + " L " + warehouse.myIndex + " " + item[i] + " " + 1 + Environment.NewLine);
                    writer.Flush();
                }
                //File.AppendAllText(Program.outputPath, myIndex + " L " + warehouse.myIndex + " " + item[i] + " " + 1 + Environment.NewLine);
                

                for (int j = 0; j < client.orderCount; j++)
                {
                    if (item[i] == client.orderType[j])
                    {
                        client.orderType.RemoveAt(j);
                        break;
                    }
                }
            }


            client.orderCount -= item.Count;
            

            for (int i = 0; i < item.Count; i++)
            {
                //File.AppendAllText(Program.outputPath, myIndex + " D " + client.myIndex + " " + item[i] + " " + 1 + Environment.NewLine);
                using (TextWriter writer = new StreamWriter(Program.outputPath,true))
                {
                    writer.Write(myIndex + " D " + client.myIndex + " " + item[i] + " " + 1 + Environment.NewLine);
                    writer.Flush();
                }
            }
            


            return true;
        }
    }
}
