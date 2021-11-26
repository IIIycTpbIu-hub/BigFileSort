using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using BigFileSort.Sort.Impl;
using NUnit.Framework;

namespace BigFileSort.Tests
{
    public class FileSorterTests
    {
        private readonly string[] _files = { "input_0", "input_1", "input_2", "input_3", "input_4", "input_5", "input_6", "input_7"};
        private readonly object _locker = new Object();
        [SetUp]
        public void Setup()
        {
            lock (_locker)
            {
                foreach (var file in _files)
                {
                    if (File.Exists(file))
                    {
                        File.Delete(file);
                    }
                    File.Copy("input", file);
                }
            }
        }

        [TearDown]
        public void TearDown()
        {
            lock (_locker)
            {
                foreach (var file in _files)
                {
                    if (File.Exists(file))
                    {
                        File.Delete(file);   
                    }
                }   
            }
        }

        [Test, Order(1)]
        public void Sort_SyncCall()
        {
            lock (_locker)
            {
                var sw = new Stopwatch();
                var fileSorter = new FileSorter();
            
                sw.Start();
                foreach (var file in _files)
                {
                    fileSorter.Sort(file);
                }
                sw.Stop();
                Console.WriteLine(sw.Elapsed);
            }
        }

        [Test, Order(2)]
        public async Task Sort_ParallelCall()
        {
            var sw = new Stopwatch();
            var fileSorter = new FileSorter();
            var tasks = new List<Task>(_files.Length);

            foreach (var file in _files)
            {
                tasks.Add(new Task(() => fileSorter.Sort(file)));
            }
            sw.Start();

            Parallel.ForEach(
                tasks,
                new ParallelOptions() {MaxDegreeOfParallelism = _files.Length},
                (task) => task.Start());
            await Task.WhenAll(tasks.ToArray());
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
        }

        [Test, Order(3)]
        public async Task SortAsync_AsyncCall()
        {
            var sw = new Stopwatch();
            var fileSorter = new FileSorter();
            var tasks = new List<Task>(_files.Length);
            
            sw.Start();
            foreach (var file in _files)
            {
                tasks.Add(fileSorter.SortAsync(file));
            }

            await Task.WhenAll(tasks);
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
        }
        
        public async Task SortAsync_ParallelCall()
        {
            var sw = new Stopwatch();
            var fileSorter = new FileSorter();
            var tasks = new List<Task>(_files.Length);

            foreach (var file in _files)
            {
                tasks.Add(new Task(() => fileSorter.SortAsync(file)));
            }
            
            sw.Start();
            
            Parallel.ForEach(tasks, new ParallelOptions() {MaxDegreeOfParallelism = _files.Length},
                (task) => task.Start());
            await Task.WhenAll(tasks);
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            Thread.Sleep(10000);
        }
    }
}