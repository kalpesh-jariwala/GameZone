using System;
using System.Diagnostics;

namespace BLL_Core.Infrastructure.ExtensionMethods
{
    public static class StopwatchExtensions
    {
        public static long Time(this Stopwatch sw, Action action)
        {
            sw.Reset();
            sw.Start();
            action();
            return sw.ElapsedMilliseconds;
        }
    }
}