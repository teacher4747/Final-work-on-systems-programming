
using System.Collections.Concurrent;

namespace Final_work_on_systems_programming;

class Station
{
    public SemaphoreSlim Charger = new SemaphoreSlim(4);
    public Mutex CalibrationMutex = new Mutex();

    public CancellationToken Token;

    public CountdownEvent Countdown;

    public ConcurrentDictionary<int, CellResult> Results =
        new ConcurrentDictionary<int, CellResult>();

    public Station(CancellationToken token, int count)
    {
        Token = token;
        Countdown = new CountdownEvent(count);
    }

    public void Start()
    {
        for (int i = 1; i <= 20; i++)
        {
            BatteryCell cell = new BatteryCell();
            cell.Id = i;

            Console.WriteLine("Ячейка " + i + " поставлена в очередь");

            bool queued = ThreadPool.QueueUserWorkItem(state =>
            {
                CellWorker.Process(cell, this);
            });

            if (!queued)
            {
                Results[cell.Id] = new CellResult
                {
                    Id = cell.Id,
                    Status = "Faulted",
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now,
                    Error = "Не удалось добавить задачу в пул потоков"
                };

                Countdown.Signal();
            }
        }
    }
}
