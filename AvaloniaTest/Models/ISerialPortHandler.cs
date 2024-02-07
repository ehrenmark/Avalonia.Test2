using System;
using System.Collections.Generic;

namespace AvaloniaTest;

public interface ISerialPortHandler
{
    void Open();
    void Close();
    List<byte> ReadData();
    event EventHandler DataReceived;
}