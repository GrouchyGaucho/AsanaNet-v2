using System;
using System.Collections.Generic;
using AsanaNet.Models;

namespace AsanaNet.Sample
{
    class HierarchicalParallelExecutionData
    {
        public required string Info { get; set; }
        public required AsanaObject Object { get; set; }
        public List<HierarchicalParallelExecutionData> Items { get; set; } = new();

        public void WriteToConsole()
        {
            Console.WriteLine(Info);
            foreach (var item in Items)
                item.WriteToConsole();
        }
    }
}