using System;
using System.Reflection;

public class MyFieldClassA
{
    public string Field = "A Field";
    public MyFieldClassA()
    {

    }


}

public class MyFieldClassB
{
    private string field = "B Field";
    public string Field 
    {
        get
        {
            return field;
        }
        set
        {
            if (field!=value)
            {
                field=value;
            }
        }
    }
}

public class MyFieldInfoClass
{
    public static void Main()
    {
        MyFieldClassB myFieldObjectB = new MyFieldClassB();
        MyFieldClassA myFieldObjectA = new MyFieldClassA();

        Type myTypeA = typeof(MyFieldClassA);
        FieldInfo myFieldInfo = myTypeA.GetField("Field");

        Type myTypeB = typeof(MyFieldClassB);
        FieldInfo myFieldInfo1 = myTypeB.GetField("field", 
            BindingFlags.NonPublic | BindingFlags.Instance);

        Console.WriteLine("The value of the public field is: '{0}'", 
            myFieldInfo.GetValue(myFieldObjectA));
        Console.WriteLine("The value of the private field is: '{0}'", 
            myFieldInfo1.GetValue(myFieldObjectB));

        myFieldInfo.SetValue(myFieldObjectA,"nnss");
        myFieldInfo1.SetValue(myFieldObjectB,"222311");
        Console.WriteLine("The value of the public field is: '{0}'", 
            myFieldInfo.GetValue(myFieldObjectA));
        Console.WriteLine("The value of the private field is: '{0}'", 
            myFieldInfo1.GetValue(myFieldObjectB));
    }
}