using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompareSpeedByBenchmark
{
    [MinColumn, IterationsColumn, RankColumn]    
    [MemoryDiagnoser]
    [CsvMeasurementsExporter]
    public class Foundation
    {       
        private HashSet<int>? _hashSet;
        private int[]? _array;
        private int[]? _sorted_array;

        private int _target;

        [Params(1, 2, 5, 10, 100, 1000, 10000, 100000, 1000000)]
        public int data_size = 1000000;
        
        private const int SEED = 12345;
        Random random = new Random(SEED);

        [GlobalSetup]
        public void Setup()
        {
            int[] data = new int[data_size];
            for (int i = 0; i < data_size; i++)
            {
                data[i] = i;
            }            

            _hashSet = new HashSet<int>(data);
            _array = data;
            _sorted_array = data;
            _target = data[data.Length - 1];
        }

        private bool BinarySearch<T>(T[] data, T target)
        {            
            int left = 0;
            int right = data.Length - 1;
            while (left <= right)
            {
                int mid = (left + right) / 2;
                if (data[mid].Equals(target))
                {
                    return true;
                }
                else if (Comparer<T>.Default.Compare(data[mid], target) < 0)
                {
                    left = mid + 1;
                }
                else
                {
                    right = mid - 1;
                }
            }
            return false;
        }

        [Benchmark]
        public void HashSet()
        {            
            _hashSet.Contains(_target);
        }
                
        [Benchmark]
        public void Array()
        {
            _array.Contains(_target);
        }

        [Benchmark]
        public void SortedArray()
        {
            BinarySearch(_sorted_array, _target);
        }
    }    
}
