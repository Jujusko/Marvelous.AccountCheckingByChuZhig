using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marvelous.AccountCheckingByChuZhig
{
    public class TestAsyncs
    {
        public bool GetRandomCycle1()
        {
            Random random = new Random();
            int value = random.Next(0, 100);

            for (int i = 0; i < value; i++)
            {
                
            }
            Console.WriteLine("GetRandomCycle1");
            return true;
        }

        public bool GetRandomCycle2()
        {
            Random random = new Random();
            int value = random.Next(0, 100);

            for (int i = 0; i < value; i++)
            {

            }
            Console.WriteLine("GetRandomCycle2");
            return true;
        }

        public bool GetRandomCycle3()
        {
            Random random = new Random();
            int value = random.Next(0, 100);

            for (int i = 0; i < value; i++)
            {

            }
            Console.WriteLine("GetRandomCycle3");
            return true;
        }
    }
}
