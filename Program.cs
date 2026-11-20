

using System.Diagnostics;

namespace Final_work_on_systems_programming;

class Program
{
    static void Main()
    {
        CancellationTokenSource cts = new CancellationTokenSource();

        int workers;
        int completion;

        ThreadPool.GetAvailableThreads(out workers, out completion);

        Console.WriteLine("Свободных потоков в начале: " + workers);

        Station station = new Station(cts.Token, 20);

        Console.WriteLine("Станция запущена!");
        Console.WriteLine("Для остановки нажмите Ctrl+C");

        Console.CancelKeyPress += (sender, e) =>
        {
            e.Cancel = true;

            Console.WriteLine("Аварийная остановка!");

            cts.Cancel();
        };

        Stopwatch stopwatch = Stopwatch.StartNew();

        station.Start();

        station.Countdown.Wait();

        stopwatch.Stop();

        Console.WriteLine("\nОбработка партии завершена!");

        int completed = 0;
        int canceled = 0;
        int faulted = 0;

        foreach (var result in station.Results.Values)
        {
            if (result.Status == "Completed")
                completed++;

            if (result.Status == "Canceled")
                canceled++;

            if (result.Status == "Faulted")
                faulted++;
        }

        Console.WriteLine("\nИТОГОВАЯ СТАТИСТИКА");

        Console.WriteLine("Всего ячеек: " + station.Results.Count);
        Console.WriteLine("Успешно: " + completed);
        Console.WriteLine("Отменено: " + canceled);
        Console.WriteLine("Ошибок: " + faulted);

        Console.WriteLine("Время работы: " +
            stopwatch.ElapsedMilliseconds + " мс");

        ThreadPool.GetAvailableThreads(out workers, out completion);

        Console.WriteLine("Свободных потоков в конце: " + workers);

        station.Charger.Dispose();
        station.CalibrationMutex.Dispose();
        station.Countdown.Dispose();
        cts.Dispose();

        Console.WriteLine("\nНажмите Enter для выхода");
        Console.ReadLine();
    }
}
