using BenchmarkDotNet.Running;

namespace CompareSpeedByBenchmark
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //BenchmarkRunner.Run<Foundation>();
            BenchmarkRunner.Run<CompareDictionaryGroup>();
            //BenchmarkRunner.Run<CompareDispatchProxy>();
        }
    }
}