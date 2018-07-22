using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base36Conversion
{
    class Program
    {
        static Char[] charSetBase36 = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Base36 Convertor version 1.0");
            Console.WriteLine("Author: Justin Reddy");
            Console.WriteLine("Release Date: 27 Sept 2017");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;

            try
            {
                Guid guid = new Guid("158beb80-7a28-4d67-9f64-f6808e20da29");
                Console.WriteLine("ConnectID=" + guid.ToString());
                var base36FromGuid = ConvertGuidToBase36(guid.ToString());
                Console.WriteLine("Base36=" + base36FromGuid);
                var guidFromBase36 = ConvertBase36ToGuid(base36FromGuid);
                Console.WriteLine("Guid=" + guidFromBase36);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("General Error Occured");
                Console.WriteLine(ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine(ex.InnerException.Message);
                }
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Hit Enter to exit..");
            Console.ReadKey();
        }



        public static string ConvertBase36ToGuid(string base36String)
        {
            StringBuilder returnString = new StringBuilder();
            String guidWithoutDashes = base36String.Replace("-", "");
            StringBuilder guidSplit = new StringBuilder();
            for (int i = 1; i < guidWithoutDashes.Length + 1; i++)
            {
                guidSplit.Append(guidWithoutDashes[i - 1]);
                if (i % 4 == 0 && i != 0)
                    guidSplit.Append(" ");
            }
            String[] base36ValuesSplit = guidSplit.ToString().TrimEnd(' ').Split(' ');
            foreach (String base36 in base36ValuesSplit)
            {
                if (returnString.Length < 30) //Do not pad the last 2 characters of collection 
                    returnString.Append(getHexFromBase36(base36));
                else
                    returnString.Append(getHexFromBase36(base36).Substring(3, 2));
            }

            returnString.Insert(8, "-");
            returnString.Insert(13, "-");
            returnString.Insert(18, "-");
            returnString.Insert(23, "-");

            return returnString.ToString();
        }

        private static string getHexFromBase36(string base36String)
        {
            Char[] charSetBase36 = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

            int int4 = 0;
            int int3 = 0;
            int int2 = 0;
            int int1 = 0;

            for (int i = 0; i < 36; i++)
            {
                if (charSetBase36[i].ToString() == base36String[3].ToString())
                    int4 = i;

                if (charSetBase36[i].ToString() == base36String[2].ToString())
                    int3 = i * 36;

                if (charSetBase36[i].ToString() == base36String[1].ToString())
                    int2 = i * 1296;

                if (charSetBase36[i].ToString() == base36String[0].ToString())
                    int1 = i * 46656;
            }
            String stringTotal = (int4 + int3 + int2 + int1).ToString("X").PadLeft(5, '0');
            return stringTotal;
        }

        public static String ConvertGuidToBase36(String guid)
        {
            String guidWithoutDashes = guid.Replace("-", "");
            StringBuilder returnString = new StringBuilder();

            String guidSplit = "";
            for (int i = 1; i < guidWithoutDashes.Length + 1; i++)
            {
                guidSplit += guidWithoutDashes[i - 1];
                if (i % 5 == 0 && i != 0)
                    guidSplit += " ";
            }
            String[] hexValuesSplit = guidSplit.Split(' ');
            foreach (String hex in hexValuesSplit)
            {
                // Convert the number expressed in base-16 to an integer.
                int value = Convert.ToInt32(hex, 16);
                // Get the character corresponding to the integral value.
                string stringValue = getBase36CharFromInt(value);
                returnString.Append(stringValue);
            }
            return returnString.ToString();
        }

        private static String getBase36CharFromInt(int i)
        {
            int div4 = Convert.ToInt32(i / 46656);
            int mod4 = Convert.ToInt32(i % 46656);

            int div3 = Convert.ToInt32(mod4 / 1296);
            int mod3 = Convert.ToInt32(mod4 % 1296);

            int div2 = Convert.ToInt32(mod3 / 36);
            int mod2 = Convert.ToInt32(mod3 % 36);

            int mod1 = Convert.ToInt32(mod2 % 36);

            String char1 = charSetBase36[mod1].ToString();
            String char2 = charSetBase36[div2].ToString();
            String char3 = charSetBase36[div3].ToString();
            String char4 = charSetBase36[div4].ToString();

            String stringTotal = char4 + char3 + char2 + char1;
            return stringTotal;
        }


    }

}
