using Application.Common.Transfer.Windows;
using FluentAssertions;
using Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Infrastructure.UnitTests;

public class WindowManagerTests
{
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("test")]
    public void Open_IncorrectName_ShouldThrowEx(string name)
    {
        WindowManager windowManager = new();

        FluentActions.Invoking(() =>
                    windowManager.Open(name)).Should().Throw<Exception>();
    }

    [Theory]
    [InlineData(@"C:\WINDOWS\system32\cmd.exe")]
    [InlineData(@"C:\WINDOWS\system32\mspaint.exe")]
    [InlineData(@"C:\WINDOWS\system32\notepad.exe")]
    public void Open_CorrectName_ShouldOpenNewWindow(string name)
    {
        WindowManager windowManager = new();

        windowManager.Open(name);

        windowManager.Windows.Should().NotBeNullOrEmpty();
    }

    [Theory]
    [InlineData(@"C:\WINDOWS\system32\cmd.exe")]
    [InlineData(@"C:\WINDOWS\system32\mspaint.exe")]
    [InlineData(@"C:\WINDOWS\system32\notepad.exe")]
    public void Open_ShouldMoveWindowToTop(string name)
    {
        WindowManager windowManager = new();

        windowManager.Windows.Add(@"C:\WINDOWS\system32\cmd.exe", new WindowDto() { Order = 1 });
        windowManager.Windows.Add(@"C:\WINDOWS\system32\mspaint.exe", new WindowDto() { Order = 2 });
        windowManager.Windows.Add(@"C:\WINDOWS\system32\notepad.exe", new WindowDto() { Order = 3 });

        windowManager.Open(name);

        var topWindowOrder = windowManager.Windows.Max(x => x.Value.Order);

        windowManager.Windows[name].Order.Should().Be(topWindowOrder);
    }

    [Theory]
    [InlineData(@"C:\WINDOWS\system32\cmd.exe")]
    [InlineData(@"C:\WINDOWS\system32\mspaint.exe")]
    [InlineData(@"C:\WINDOWS\system32\notepad.exe")]
    public void Close_NoWindowWithTheSpecifiedName_ShouldKeyNotFoundException(string name)
    {
        WindowManager windowManager = new();

        FluentActions.Invoking(() =>
            windowManager.Close(name)).Should().Throw<KeyNotFoundException>();
    }

    [Theory]
    [InlineData(@"C:\WINDOWS\system32\cmd.exe")]
    [InlineData(@"C:\WINDOWS\system32\mspaint.exe")]
    [InlineData(@"C:\WINDOWS\system32\notepad.exe")]
    public void Close_CorrectName_ShouldCloseWindow(string name)
    {
        WindowManager windowManager = new();

        windowManager.Open(name);

        windowManager.Close(name);

        windowManager.Windows.Should().BeEmpty();
    }

    [Fact]
    public void GetTopWindow_ShouldGetWindow()
    {
        WindowManager windowManager = new();

        windowManager.Windows.Add(@"C:\WINDOWS\system32\cmd.exe", new WindowDto() { Order = 1 });
        windowManager.Windows.Add(@"C:\WINDOWS\system32\mspaint.exe", new WindowDto() { Order = 2 });
        windowManager.Windows.Add(@"C:\WINDOWS\system32\notepad.exe", new WindowDto() { Order = 3 });

        windowManager.GetTopWindow().Should().Be(@"C:\WINDOWS\system32\notepad.exe");
    }

    [Theory]
    [InlineData("test")]
    [InlineData("")]
    public void OpenPrevious_IncorrectName_ShouldKeyNotFoundException(string name)
    {
        WindowManager windowManager = new();

        windowManager.Windows.Add(@"C:\WINDOWS\system32\cmd.exe", new WindowDto() { Order = 1 });
        windowManager.Windows.Add(@"C:\WINDOWS\system32\mspaint.exe", new WindowDto() { Order = 2 });
        windowManager.Windows.Add(@"C:\WINDOWS\system32\notepad.exe", new WindowDto() { Order = 3 });

        FluentActions.Invoking(() =>
            windowManager.OpenPrevious(name)).Should().Throw<KeyNotFoundException>();
    }

    [Fact]
    public void OpenPrevious_IncorrectName_ShouldReturnNull()
    {
        WindowManager windowManager = new();

        windowManager.Windows.Add(@"C:\WINDOWS\system32\cmd.exe", new WindowDto() { Order = 1 });
        windowManager.Windows.Add(@"C:\WINDOWS\system32\mspaint.exe", new WindowDto() { Order = 2 });
        windowManager.Windows.Add(@"C:\WINDOWS\system32\notepad.exe", new WindowDto() { Order = 3 });

        var previousWindow = windowManager.OpenPrevious(@"C:\WINDOWS\system32\cmd.exe");

        previousWindow.Should().BeNull();
    }

    [Theory]
    [InlineData(@"C:\WINDOWS\system32\mspaint.exe", @"C:\WINDOWS\system32\cmd.exe")]
    [InlineData(@"C:\WINDOWS\system32\notepad.exe", @"C:\WINDOWS\system32\mspaint.exe")]
    public void OpenPrevious_CorrectName_ShouldOpenPrevious(string name, string previousName)
    {
        WindowManager windowManager = new();

        windowManager.Windows.Add(@"C:\WINDOWS\system32\cmd.exe", new WindowDto() { Order = 1 });
        windowManager.Windows.Add(@"C:\WINDOWS\system32\mspaint.exe", new WindowDto() { Order = 2 });
        windowManager.Windows.Add(@"C:\WINDOWS\system32\notepad.exe", new WindowDto() { Order = 3 });

        var previousWindowName = windowManager.OpenPrevious(name);

        var topWindowOrder = windowManager.Windows.Max(x => x.Value.Order);

        previousWindowName.Should().Be(previousName);

        windowManager.Windows[previousName].Order.Should().Be(topWindowOrder);
    }
}