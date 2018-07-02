using System;
using System.Reflection;

public class Example
{
   public static void Main()
   {
      Assembly assem = typeof(Example).Assembly;
      Console.WriteLine("Assembly name: {0} Location {1}", assem.FullName, assem.Location);
   }
}