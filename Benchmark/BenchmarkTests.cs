using Application.Common.Transfer.Windows;
using BenchmarkDotNet.Attributes;
using System.Diagnostics;

namespace Benchmark;

[MemoryDiagnoser]
[ThreadingDiagnoser]
public class BenchmarkTests
{
    [Params(@"C:\WINDOWS\system32\cmd.exe")]
    public string Name { get; set; }

    public Dictionary<string, WindowDto> Windows;

    [GlobalSetup]
    public void Setup()
    {
        Windows = new();
    }

    [Benchmark]
    public void OpenAndCloseWindow()
    {
        Process myProcess = new();
        myProcess.StartInfo.UseShellExecute = true;
        myProcess.StartInfo.FileName = Name;
        myProcess.StartInfo.CreateNoWindow = false;
        myProcess.Start();

        Windows.Add(Name, new WindowDto() { Order = Windows.Count + 1, Process = myProcess });

        Close(Name);
    }

    public void Close(string name)
    {
        if (Windows.ContainsKey(name))
        {
            Windows[name].Process.Kill();

            Organize(Windows[name].Order);

            Windows.Remove(name);
        }
    }

    private void Organize(int order)
    {
        foreach (var window in Windows)
        {
            if (window.Value.Order >= order)
                window.Value.Order -= 1;
        }
    }
}