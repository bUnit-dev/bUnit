// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using Bunit;

BenchmarkSwitcher.FromAssembly(typeof(Benchmark).Assembly).RunAll();
