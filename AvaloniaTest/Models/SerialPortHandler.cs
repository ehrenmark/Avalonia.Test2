using System;
using System.Collections.Generic;
using System.IO.Ports;

namespace AvaloniaTest;

public class SerialPortHandler : ISerialPortHandler
{
        private SerialPort _serialPort;
        
        public SerialPortHandler()
        {
            _serialPort = new SerialPort();
            
            _serialPort.PortName = "COM3";
            _serialPort.BaudRate = 4800;
            _serialPort.DataBits = 8;
            _serialPort.StopBits = StopBits.One;
            _serialPort.Parity = Parity.Even;

        }
        
        public void Open()
        {
            _serialPort.Open();
            _serialPort.DataReceived+=SerialPortOnDataReceived;
        }

        private void SerialPortOnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            DataReceived?.Invoke(this,EventArgs.Empty);            
        }

        public void Close()
        {
            _serialPort.DataReceived -= SerialPortOnDataReceived;
            _serialPort.Close();
            
        }

        public List<Byte> ReadData()
        {
            var result = new List<Byte>();
            int bytesToRead = _serialPort.BytesToRead;
            byte[] buffer = new byte[bytesToRead];
           _serialPort.Read(buffer, 0, bytesToRead);
            foreach (byte num in buffer)
                result.Add(num);
            return result;
        }

        public event EventHandler DataReceived;

}

