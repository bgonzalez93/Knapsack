using System.Diagnostics;
using Entities;

namespace Sharp3dPacking.Example;

public static class Program
{
    public static void Main(string[] args)
    {
        var bins = new List<Bin>
        {
            new("small-envelope", 11.5m, 6.125m, 0.25m),
            new("large-envelope", 15m, 12m, 0.75m),
            new("small-box", 8.625m, 5.375m, 1.625m),
            new("medium-box", 11m, 8.5m, 5.5m),
            new("medium-2-box", 13.625m, 11.875m, 3.375m),
            new("large-box", 12m, 12m, 5.5m),
            new("large-2-box", 23.6875m, 11.75m, 3.0m)
        };

        var items = new List<Item>
        {
            new(1, 3.9370m, 1.9685m, 1.9685m, 3),
            new(2, 7.8740m, 3.9370m, 1.9685m, 6)
        };

        var stopwatch = new Stopwatch();

        stopwatch.Start();
        var results = PackingService.Pack(bins, items, new List<int>{2});
        stopwatch.Stop();

        Console.WriteLine($"Packed in: {stopwatch.ElapsedMilliseconds}ms\n\n");

        Console.WriteLine("Bins...\n");

        foreach (var containerPackingResult in results)
        {
            Console.WriteLine($"\n\tContainer Description: {bins.FirstOrDefault(x => x.ID == containerPackingResult.ContainerID)}");
            Console.WriteLine("\t=======================================================\n\n");

            Console.WriteLine("\tFitted items...\n");

            foreach (var item in containerPackingResult.AlgorithmPackingResults.FirstOrDefault().PackedItems)
            {
                Console.WriteLine($"\t\t{item}");
            }

            Console.WriteLine(string.Empty);

            Console.WriteLine("\tUnfitted items...");

            foreach (var item in containerPackingResult.AlgorithmPackingResults.FirstOrDefault().UnpackedItems)
            {
                Console.WriteLine($"\t\t{item}");
            }

            Console.WriteLine("\t*******************************************************\n\n");
        }
    }
}