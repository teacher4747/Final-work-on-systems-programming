
namespace Final_work_on_systems_programming;


class CellWorker
{
    public static void Process(BatteryCell cell, Station station)
    {
        DateTime start = DateTime.Now;
        string error = "";

        try
        {
            station.Token.ThrowIfCancellationRequested();

            Diagnostics.Run(cell, station.Token);

            Charging.Run(cell, station.Charger, station.Token);

            Calibration.Run(cell, station.CalibrationMutex, station.Token);

            cell.Status = "Completed";
        }
        catch (OperationCanceledException)
        {
            cell.Status = "Canceled";
        }
        catch (Exception ex)
        {
            cell.Status = "Faulted";
            error = ex.Message;
        }
        finally
        {
            station.Results[cell.Id] = new CellResult
            {
                Id = cell.Id,
                Status = cell.Status,
                StartTime = start,
                EndTime = DateTime.Now,
                Error = error
            };

            Console.WriteLine("Ячейка " + cell.Id +
                ": итоговый статус — " + cell.Status);

            station.Countdown.Signal();
        }
    }
}
