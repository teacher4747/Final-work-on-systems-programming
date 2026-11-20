
namespace Final_work_on_systems_programming;


class Diagnostics
{
    public static void Run(BatteryCell cell, CancellationToken token)
    {
        cell.Status = "Diagnostics";

        double result = 0;

        for (int i = 0; i < cell.DiagnosticIterations; i++)
        {
            if (i % 1000 == 0)
                token.ThrowIfCancellationRequested();

            result += Math.Sqrt(i);
        }

        Console.WriteLine("Ячейка " + cell.Id + ": диагностика завершена");
    }
}
