using System.IO;
using System;
using System.Collections.Generic;


namespace WriteFile
{
public class WriteFile
{
    public static void Main(String[] args)
    {
        StreamReader reader = null;
        StreamWriter writer = null;
        List<String> lines = new List<String>();
        String line;
        int i;
        String f;

        if (args.Length > 1) {
            reader = new StreamReader(args[0]);
        } else {
            reader = new StreamReader(Console.OpenStandardInput());
        }
        while ((line = reader.ReadLine()) != null) {
            lines.Add(line);
        }
        reader.Close();

        if (args.Length > 1) {
            for (i = 1; i < args.Length; i++) {
                f = args[i];
                writer = new StreamWriter(f);
                Console.WriteLine("write {0} file", f);
                foreach (string l in lines) {
                    writer.WriteLine(l);
                }
                writer.Close();
            }
        } else if (args.Length == 1) {
            writer = new StreamWriter(args[0]);
            Console.WriteLine("write {0} file", args[0]);
            foreach (string l in lines) {
                writer.WriteLine(l);
            }
            writer.Close();
        } else {
            writer = new StreamWriter(Console.OpenStandardOutput());
            Console.WriteLine("write stdout");
            foreach (string l in lines) {
                writer.WriteLine(l);
            }
            writer.Flush();
        }

    }
}
}