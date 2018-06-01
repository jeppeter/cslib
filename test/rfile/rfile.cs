using System.IO;
using System;

namespace ReadFile
{
public class ReadFile
{
    public static void Main(String[] args)
    {
        StreamReader reader;
        String line;

        if (args.Length > 0) {
            foreach (string f in args) {
                reader = new StreamReader(f);
                Console.WriteLine("read {0} file", f);
                Console.WriteLine("----------------------------");
                while ((line = reader.ReadLine()) != null) {
                    Console.WriteLine(line);
                }
                reader.Close();
                Console.WriteLine("++++++++++++++++++++++++++++");
            }
        } else {
            reader = new StreamReader(Console.OpenStandardInput());
            Console.WriteLine("read stdin");
            Console.WriteLine("----------------------------");
            while ((line = reader.ReadLine()) != null) {
                Console.WriteLine(line);
            }
            reader.Close();
            Console.WriteLine("++++++++++++++++++++++++++++");
        }

    }
}
}