using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompareSpeedByBenchmark
{
    [MinColumn, IterationsColumn, RankColumn]    
    [MemoryDiagnoser]
    public class CompareDictionaryGroup
    {
        private int entryCount = 1000;

        [Params(10, 100)]
        public int groupCount = 10;

        private Random random = new Random(12345);

        private (int, Guid)[] _input;

        [GlobalSetup]
        public void Setup()
        {                        
            // Gen 1000 entries input
            _input = Enumerable.Range(0, entryCount).Select(i => (i % groupCount, Guid.NewGuid())).ToArray();            
        }

        [Benchmark]
        public List<List<Guid>> GroupByDictionary()
        {            
            var dict = new Dictionary<int, List<Guid>>();
            foreach (var (i, j) in _input)
            {
                if (!dict.ContainsKey(i))
                {
                    dict[i] = new List<Guid>();
                }
                dict[i].Add(j);
            }

            return dict.Values.ToList();
        }

        [Benchmark]
        public List<List<Guid>> GroupByILookup()
        {
            var lookup = _input.ToLookup(i => i.Item1, i => i.Item2);
            return lookup.Select(p => p.ToList()).ToList();
        }


        [Benchmark]
        public List<List<Guid>> GroupByLinq()
        {            
           return _input.GroupBy(i => i.Item1).Select(p => p.Select(i => i.Item2).ToList()).ToList();
        }   

    }
}
