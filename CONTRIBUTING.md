# Contributing

1. Fork
2. Implement Context \ Benchmark
3. Check with tests
4. Create PR

Simple as it is.

## Notices

### Source Generation Awareness

Benchmarks are source generated. Seriously.

The idea is to get code from *Context and pass it as code inside *Benchmark so it will be some kind of pseudo hand written bench without mess of abstractions (for runtime).
This limits the usage of some freaking stuff but for now it's working, so use other references and benchmarks as reference.

Also none of contexts or benchmarks are run directly. They're just a source of source code for generation.
As for now 600+ unique pairs of Benchmark <-> Context generated and each of them are used it runtime.

You can see the Raw_Runner and use it for testing and profiling each particular pair or subsets of benchmarks using generated BenchMap.

Have fun at least.
