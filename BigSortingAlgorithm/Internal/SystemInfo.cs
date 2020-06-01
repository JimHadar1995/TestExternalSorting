using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BigSortingAlgorithm.Internal
{
    internal static class SystemInfo
    {
        private static PerformanceCounter performance = new PerformanceCounter("Memory", "Available MBytes");
        
        static SystemInfo()
        {
            
        }

        public static float GetOccupiedMemoryPercent()
        {
            return (float)GC.GetTotalMemory(false) / GetFreeMemory();
        }

        public static ulong GetFreeMemoryLeft()
        {
            return GetFreeMemory() - (ulong)GC.GetTotalMemory(false);
        }

        private static ulong GetFreeMemory()
        {
            return (ulong)performance.NextValue() * 1024 * 1024;
        }

        public static int ProccessorCount = Environment.ProcessorCount;
    }
}
