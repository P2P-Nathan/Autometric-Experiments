// See https://aka.ms/new-console-template for more information
using Samples.ConsoleMetrics;

public class Program
{
    [AutoMetricMethodWrapped]
    public static void Main(string[] args)
    {
        Console.WriteLine("Starting to do some testing");
        DoSomeTesting();
        Console.WriteLine("Done testing");
    }

    private static void DoSomeTesting()
    {
        // Randomly call SaferMethod or SecretMethod 3 to 7 times.
        int timesToCall = new Random().Next(3, 7);
        for (int i = 0; i < timesToCall; i++)
        {
            if (new Random().Next(0, 100) < 50)
            {
                SaferMethod();
            }
            else
            {
                SecretMethod();
            }
        }
    }

    [AutoMetricMethodWrapped]
    private static void SaferMethod()
    {
        try
        {
            FlakyMethod();
            FlakyMethod();
        }
        catch (Exception)
        {
        }
    }

    private static void SecretMethod()
    {
        try
        {
            FlakyMethod();
            FlakyMethod();
        }
        catch (Exception)
        {
        }
    }

    /// <summary>
    /// 30 percent of the time, this method will throw an exception.
    /// </summary>
    [AutoMetricMethodWrapped]
    public static void FlakyMethod()
    {
        if (new Random().Next(0, 100) < 55)
        {
            throw new Exception("This method is flaky");
        }
        else
        {
            Thread.Sleep(1500);
        }
    }
}
