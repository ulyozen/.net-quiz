using System.Diagnostics;
using Quiz.Core.Common;
using Xunit.Abstractions;

namespace Quiz.Labs;

public class PerformanceTests(ITestOutputHelper output)
{
    [Fact]
    public void CompareTryCatchAndIfElsePerformance()
    {
        var tryCatchTime = TestTryCatch();
        var ifElseTime = TestIfElse();
        var speedDifference = tryCatchTime / (double)ifElseTime;

        output.WriteLine($"try-catch: {tryCatchTime} ms");
        output.WriteLine($"if-else: {ifElseTime} ms");
        output.WriteLine($"if-else быстрее try-catch в {speedDifference:F0} раз");

        Assert.True(ifElseTime < tryCatchTime);
    }
    
    private long TestTryCatch()
    {
        var sw = Stopwatch.StartNew();
        
        for (var i = 0; i < 1000000; i++)
        {
            try
            {
                throw new Exception("Test");
            }
            catch { }
        }
        
        sw.Stop();
        
        return sw.ElapsedMilliseconds;
    }

    private long TestIfElse()
    {
        var sw = Stopwatch.StartNew();
        
        for (var i = 0; i < 1000000; i++)
        {
            OperationResult.Failure("Test");
        }
        
        sw.Stop();
        
        return sw.ElapsedMilliseconds;
    }
}