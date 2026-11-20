
namespace Final_work_on_systems_programming;

class Calibration
{
    public static void Run(BatteryCell cell, Mutex calibrationMutex,
        CancellationToken token)
    {
        cell.Status = "WaitingForCalibration";

        Console.WriteLine("Ячейка " + cell.Id + ": ожидание калибровки");

        int result = WaitHandle.WaitAny(new WaitHandle[]
        {
            calibrationMutex,
            token.WaitHandle
        });

        if (result == 1)
            token.ThrowIfCancellationRequested();

        try
        {
            token.ThrowIfCancellationRequested();

            cell.Status = "Calibration";

            Console.WriteLine("Ячейка " + cell.Id + ": калибровка началась");

            token.WaitHandle.WaitOne(cell.CalibrationDuration);
            token.ThrowIfCancellationRequested();

            Console.WriteLine("Ячейка " + cell.Id + ": калибровка завершена");
        }
        finally
        {
            calibrationMutex.ReleaseMutex();
        }
    }
}
