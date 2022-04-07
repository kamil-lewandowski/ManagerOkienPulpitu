using Application.Common.Interfaces;
using Application.Common.Transfer.Windows;
using System.Diagnostics;

namespace Infrastructure.Services;

public class WindowManager : IWindowManager
{
    public Dictionary<string, WindowDto> Windows { get; set; }

    public WindowManager()
    {
        Windows = new();
    }

    public void Close(string name)
    {
        //if (Windows.ContainsKey(name))
        Windows[name].Process.Kill();

        Organize(Windows[name].Order);

        Windows.Remove(name);
    }

    public string? GetTopWindow()
    {
        if (Windows.Count > 0)
        {
            var topOrder = Windows.Max(x => x.Value.Order);

            return Windows.Where(x => x.Value.Order == topOrder).FirstOrDefault().Key;
        }

        return null;
    }

    public void Open(string name)
    {
        if (!Windows.ContainsKey(name))
        {
            Process myProcess = new();
            myProcess.StartInfo.UseShellExecute = true;
            myProcess.StartInfo.FileName = name;
            myProcess.StartInfo.CreateNoWindow = false;
            myProcess.Start();

            Windows.Add(name, new WindowDto() { Order = Windows.Count + 1, Process = myProcess });
        }
        else
        {
            MoveWindowToTop(name);
        }
    }

    public string? OpenPrevious(string name)
    {
        var window = Windows.FirstOrDefault(x => x.Value.Order == Windows[name].Order - 1);

        if (window.Key is not null)
        {
            MoveWindowToTop(window.Key);

            return window.Key;
        }

        return null;
    }

    private void MoveWindowToTop(string name)
    {
        if (Windows.ContainsKey(name))
        {
            Organize(Windows[name].Order);

            Windows[name].Order = Windows.Count;
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