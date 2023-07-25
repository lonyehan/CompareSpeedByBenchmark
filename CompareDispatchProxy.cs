using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CompareSpeedByBenchmark
{
    [CategoriesColumn]
    [MinColumn, IterationsColumn, RankColumn]        
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkDotNet.Configs.BenchmarkLogicalGroupRule.ByCategory)]
    public class CompareDispatchProxy
    {
        public interface ICow
        {
            void Moo();
        }

        public class Cow : ICow
        {
            public void Moo()
            {                
            }
        }

        public class CowProxy : DispatchProxy
        {
            private ICow _cow = new Cow();

            public CowProxy() { 
                _cow = new Cow();
            }

            public static ICow Create(ICow cow)
            {
                var proxyObj = Create<ICow, CowProxy>();
                var proxy = (CowProxy)proxyObj;
                proxy._cow = cow;
                return proxyObj;
            }

            protected override object Invoke(MethodInfo targetMethod, object[] args)
            {
                return targetMethod.Invoke(_cow, args);
            }
        }

        private Cow _cow;
        private ICow _iCow;
        private ICow _iCowProxy;

        [GlobalSetup]
        public void Setup()
        {
            _cow = new Cow();
            _iCow = new Cow();
            _iCowProxy = CowProxy.Create(_cow);
        }

        [Benchmark]
        [BenchmarkCategory("Without Create Instance")]
        public void WithoutCreateClassMoo()
        {
            _cow.Moo();
        }

        [Benchmark]
        [BenchmarkCategory("Without Create Instance")]
        public void WithoutCreateInterfaceMoo()
        {
            _iCow.Moo();
        }

        [Benchmark]
        [BenchmarkCategory("Without Create Instance")]
        public void WithoutCreateProxyMoo()
        {
            _iCowProxy.Moo();
        }

        [Benchmark]
        [BenchmarkCategory("Create Instance")]
        public void ClassMoo()
        {
            var _cow = new Cow();
            _cow.Moo();
        }

        [Benchmark]
        [BenchmarkCategory("Create Instance")]
        public void InterfaceMoo()
        {
            var _iCow = new Cow();
            _iCow.Moo();
        }

        [Benchmark]
        [BenchmarkCategory("Create Instance")]
        public void ProxyMoo()
        {
            var _iCowProxy = DispatchProxy.Create<ICow, CowProxy>();
            _iCowProxy.Moo();
        }
        
    }
}
