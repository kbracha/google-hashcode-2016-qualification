using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace google_hashcode
{
    static class Program
    {
        public static int rows;
        public static int columns;
        public static int droneCount;
        public static int totalTurns;
        public static int droneMaxCapacity;
         
        public static Drone [] drone;
        
        public static int productCount;
        public static int [] productWeight;
        
        public static int warehouseCount;
        public static Warehouse[] warehouse;
         
        public static int clientCount;
        public static List<Client> client;


        public static int droneCheckedCount = 0;
        public static int[] droneChecked;

        public static int clientCheckedCount = 0;
        public static int[] clientChecked;


        public static string outputPath = @"C:\Users\Krzychu\Desktop\busy_day_out.txt";
        public static string in_path = @"C:\Users\Krzychu\Desktop\redundancy.in";

        public static void readInput(string path)
        {
            using (TextReader reader = File.OpenText(path))
            {
                string line = reader.ReadLine();
                string[] bits = line.Split(' ');

                rows = int.Parse(bits[0]);
                columns = int.Parse(bits[1]);
                droneCount = int.Parse(bits[2]);
                totalTurns = int.Parse(bits[3]);
                droneMaxCapacity = int.Parse(bits[4]);

                drone = new Drone[droneCount];
                droneChecked = new int[droneCount];

                line = reader.ReadLine();
                productCount = int.Parse(line);
                productWeight = new int[productCount];

                line = reader.ReadLine();
                bits = line.Split(' ');

                for (int i = 0; i < productCount; i++)
                {
                    productWeight[i] = int.Parse(bits[i]);
                }

                line = reader.ReadLine();
                warehouseCount = int.Parse(line);
                warehouse = new Warehouse[warehouseCount];

                for (int i = 0 ; i < warehouseCount; i++)
                {
                    line = reader.ReadLine();
                    bits = line.Split(' ');

                    warehouse[i] = new Warehouse(int.Parse(bits[0]), int.Parse(bits[1]),i);

                    line = reader.ReadLine();
                    bits = line.Split(' ');

                    for (int j = 0; j < productCount; j++)
                    {
                        warehouse[i].productAvailability.Add(int.Parse(bits[j]));
                    }

                }

                Drone.maxCapacity = droneMaxCapacity;
                for (int i = 0; i < droneCount; i++)
                {
                    drone[i] = new Drone(totalTurns);
                    drone[i].myIndex = i;
                }


                line = reader.ReadLine();
                clientCount = int.Parse(line);
                client = new List<Client>();
                clientChecked = new int[clientCount];

                for (int i = 0; i < clientCount; i++)
                {
                    line = reader.ReadLine();
                    bits = line.Split(' ');
                    int x = int.Parse(bits[0]);
                    int y = int.Parse(bits[1]);

                    line = reader.ReadLine();
                    int orderCount = int.Parse(line);

                    client.Add( new Client(x, y, orderCount, productCount, i));

                    line = reader.ReadLine();
                    bits = line.Split(' ');

                    for (int j = 0; j < orderCount; j++)
                    {
                        int order = int.Parse(bits[j]);
                        client[i].orderType.Add(order);
                        client[i].product[order] = true;
                    }

                }
            }

            return;
        }


        static void Main(string[] args)
        {
            System.IO.File.WriteAllText(outputPath, string.Empty);
            
           


            if (File.Exists(in_path))
            {
                readInput(in_path);
            }
            else
                Console.Write("wrong path");


            

            hashcode_solution();
            //string text = File.ReadAllText(path);

            for (int i = 0; i < droneCount; i++)
            {
                Console.WriteLine(drone[i].turnsLeft);
            }

            Console.WriteLine(client.Count);
           

            //File.WriteAllText(out_path, hashcode_solution());
            Console.ReadKey();

        }

        public static string hashcode_solution()
        {
            // first round
            for (int i = 0; i < droneCount; i++)
            {
                int clientIndex = findLeastItemsClient();

                while (client[clientIndex].orderCount > 0)
                {
                    // works
                    int warehouseIndex = findBestClientWarehouse(clientIndex);
                    if (drone[i].pickProducts(warehouse[warehouseIndex], client[clientIndex]) == false)
                        break;
                }

                if (client[clientIndex].orderCount == 0)
                    client.RemoveAt(clientIndex);
            }


            bool droneDelivered = true;

            while (droneDelivered && client.Count > 0)
            {
                droneDelivered = false;

                int clientIndex = findLeastItemsClient();
                int warehouseIndex = findBestClientWarehouse(clientIndex);

                for (int i = 0; i < droneCount; i++)
                {
                    droneChecked[i] = 0;
                }
                droneCheckedCount = 0;

                while (droneCheckedCount < droneCount)
                {
                    int droneIndex = findWarehouseClosestDrone(warehouseIndex);

                    if (drone[droneIndex].pickProducts(warehouse[warehouseIndex], client[clientIndex]) == true)
                    {
                        droneDelivered = true;

                        if (client[clientIndex].orderCount == 0)
                            client.RemoveAt(clientIndex);

                        break;
                    }
                    else
                    {
                        droneChecked[droneIndex] = 1;
                        droneCheckedCount++;
                    }
                }

                if (droneDelivered == false)
                {
                    clientChecked[clientIndex] = 1;
                    clientCheckedCount--;
                    if (clientCheckedCount > 0)
                    {
                        droneDelivered = true;
                    }
                }

            }


            
            string solution = null;

            return solution;
        }


        public static int findLeastItemsClient()
        {
            int leastOrders = productCount + 1;
            int leastOrdersClientIndex = -1;
            for (int i = 0; i < client.Count; i++)
            {
                if (client[i].orderCount < leastOrders && clientChecked[i] == 0)
                {
                    leastOrders = client[i].orderCount;
                    leastOrdersClientIndex = i;
                }
            }

            return leastOrdersClientIndex;
        }


        public static int findBestClientWarehouse(int clientIndex)
        {
            int mostProducts = -1;
            int mostProductsWarehouseIndex = -1;
            for (int i = 0; i < warehouse.Length; i++)
            {
                int foundItems = 0;
                for (int j = 0; j < client[clientIndex].orderCount; j++)
                {
                    if (warehouse[i].productAvailability[client[clientIndex].orderType[j]] > 0)
                    {
                        foundItems++;
                    }
                }

                if (foundItems > mostProducts)
                {
                    mostProducts = foundItems;
                    mostProductsWarehouseIndex = i;
                }
            }

            return mostProductsWarehouseIndex;
        }


        public static int findWarehouseClosestDrone(int warehouseIndex)
        {
            int smallestDistance = Int32.MaxValue;
            int smallestDistanceDroneIndex = -1;
            for (int i = 0; i < droneCount; i++)
            {
                if (droneChecked[i] == 1)
                    continue; 

                int distance = (int)Math.Ceiling(Math.Sqrt((drone[i].x - warehouse[warehouseIndex].x) * (drone[i].x - warehouse[warehouseIndex].x)
                                + (drone[i].y - warehouse[warehouseIndex].y) * (drone[i].y - warehouse[warehouseIndex].y)));

                if (distance < smallestDistance)
                {
                    smallestDistance = distance;
                    smallestDistanceDroneIndex = i;
                }
            }


            return smallestDistanceDroneIndex;
        }
    }


    

}


