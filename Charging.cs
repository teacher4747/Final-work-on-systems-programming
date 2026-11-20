
namespace Final_work_on_systems_programming;


class Charging
{
    public static void Run(BatteryCell cell, SemaphoreSlim charger,
        CancellationToken token)
    {
        cell.Status = "WaitingForCharger";

        Console.WriteLine("Ячейка " + cell.Id + ": ожидание зарядки");

        charger.Wait(token);

        try
        {
            cell.Status = "Charging";

            Console.WriteLine("Ячейка " + cell.Id + ": зарядка началась");

            token.WaitHandle.WaitOne(cell.ChargeDuration);
            token.ThrowIfCancellationRequested();

            Console.WriteLine("Ячейка " + cell.Id + ": зарядка завершена");
        }
        finally
        {
            charger.Release();
        }
    }
}
