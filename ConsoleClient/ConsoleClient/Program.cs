using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ServiceModel;

namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            TestServiceClient client = new TestServiceClient();
            client.Open();


            int number = 12345600;
            Console.WriteLine(client.SearchName(number));
            Console.ReadLine();

            client.Close();
        }
    }
}
