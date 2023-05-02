using System;
using System.Diagnostics;
using AspectInjector.Broker;
using static System.Formats.Asn1.AsnWriter;
using Scope = AspectInjector.Broker.Scope;

namespace Samples.ConsoleMetrics
{
    /// <summary>
    /// This attribute is used to mark methods for logging their execution. It logs information
    /// such as the calling method, the called method, and any exceptions that occur during
    /// the method's execution.
    /// </summary>
    [Aspect(Scope.Global)]
    [Injection(typeof(AutoMetricMethod))]
    [AttributeUsage(AttributeTargets.Method)]
    public class AutoMetricMethod : Attribute
    {
        Stopwatch _stopwatch = new Stopwatch();

        [Advice(Kind.Before, Targets = Target.Method)]
        public void OnEntry([Argument(Source.Name)] string methodName, [Argument(Source.Type)] Type methodType)
        {
            _stopwatch.Start();
        }

        [Advice(Kind.After, Targets = Target.Method)]
        public void OnExit([Argument(Source.Name)] string methodName, [Argument(Source.Type)] Type methodType)
        {
            _stopwatch.Stop();
            Console.WriteLine($"Output: Class = {methodType.Name}; Method = {methodName}; Duration = {_stopwatch.ElapsedMilliseconds}ms");
        }

    }
}
