using System.Runtime.InteropServices;
using System;

[StructLayoutAttribute(LayoutKind.Explicit)]
struct SignedNumberWithText
{
[FieldOffsetAttribute(0)]
public sbyte Num1;
[FieldOffsetAttribute(0)]
public short Num2;
[FieldOffsetAttribute(0)]
public int Num3;
[FieldOffsetAttribute(0)]
public long Num4;
[FieldOffsetAttribute(0)]
public float Num5;
[FieldOffsetAttribute(0)]
public double Num6;
[FieldOffsetAttribute(16)]
public string Text1;
}

class Program
{
    static void Main(string[] args)
    {
        SignedNumberWithText k;

        k.Num5 = 0.0F;
        k.Num6 = 0.0;
        k.Text1 = "new";
        k.Num1 = 1;
        Console.WriteLine("num1 {0}", k.Num1);
        Console.WriteLine("Num5 {0}", k.Num5);
        Console.WriteLine("Num6 {0}", k.Num6);
        Console.WriteLine("Text1 {0}", k.Text1);
        return;
    }
}

