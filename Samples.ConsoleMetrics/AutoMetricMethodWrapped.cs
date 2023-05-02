using System;
using System.Diagnostics;
using System.Reflection;
using AspectInjector.Broker;
using static System.Formats.Asn1.AsnWriter;
using Scope = AspectInjector.Broker.Scope;

namespace Samples.ConsoleMetrics
{
    /// <summary>
    /// This attribute wraps a method with a try/catch block and logs any exceptions that occur.
    /// It also logs the method's execution time.
    /// </summary>
    [Aspect(Scope.Global)]
    [Injection(typeof(AutoMetricMethodWrapped))]
    [AttributeUsage(AttributeTargets.Method)]
    public class AutoMetricMethodWrapped : Attribute
    {
        [Advice(Kind.Around, Targets = Target.Method)]
        public object HandleMethod(
            [Argument(Source.Metadata)] MethodBase metadata,
            [Argument(Source.Name)] string methodName,
            [Argument(Source.Type)] Type methodType,
            [Argument(Source.Arguments)] object[] arguments,
            [Argument(Source.Target)] Func<object[], object> method,
            [Argument(Source.ReturnType)] Type returnType,
            [Argument(Source.Triggers)] Attribute[] triggers)
        {

            MethodBase callingMethod = GetCallingMethod(metadata);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            bool _success = false;
            object result;
            try
            {
                result = method(arguments);
                _success = true;
                return result;
            }
            catch (Exception)
            {
                _success = false;
                throw;
            }
            finally
            {
                stopwatch.Stop();
                Console.WriteLine($"Output: Class = {methodType.Name}; Method = {methodName}; Success = {_success}; Duration = {stopwatch.ElapsedMilliseconds}ms; Caller = {callingMethod?.Name ?? "none"};");
            }

        }

        private MethodBase GetCallingMethod(MethodBase method)
        {
            var stackTrace = new System.Diagnostics.StackTrace();
            var stackFrames = stackTrace.GetFrames();

            if (stackFrames == null)
            {
                return null;
            }

            for (int i = 0; i < stackFrames.Length; i++)
            {
                if (stackFrames[i].GetMethod() == method)
                {
                    // The calling method is the one before the current method in the stack
                    return i + 1 < stackFrames.Length ? stackFrames[i + 1].GetMethod() : null;
                }
            }

            return null;
        }
    }
}
