using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Timers;

namespace AvaloniaTest;

public class FakePortHandler : ISerialPortHandler
{
    private string _testFileName = "./TestData/output_2024-02-07_15-16-54.log";
    private static System.Timers.Timer aTimer;
    private byte[] _content;
    private int _p;
    
    public void Open()
    {
        _content = File.ReadAllBytes(_testFileName);
        aTimer = new System.Timers.Timer(1000);
        aTimer.Elapsed += ATimerOnElapsed;
        aTimer.AutoReset = true;
        aTimer.Enabled = true;
    }

    private void ATimerOnElapsed(object? sender, ElapsedEventArgs e)
    {
        DataReceived?.Invoke(this, EventArgs.Empty);
    }

    public void Close()
    {
        
    }

    public List<byte> ReadData()
    {
        var result = new List<byte>();
        for (int i = _p; i < _p + 300; i++)
        {
            result.Add(_content[i]);
        }

        _p = _p + 300;
        return result;
    }

    public event EventHandler? DataReceived;
}