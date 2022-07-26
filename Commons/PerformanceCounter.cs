using System.Diagnostics;

namespace ConstraintExperiment.Commons;

public static class PerformanceCounter
{
    public static async Task<long> GetElapsedTimeAsync<T>(this Task<T> task)
    {
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        await task;
        stopWatch.Stop();
        return stopWatch.ElapsedMilliseconds;
    }
}