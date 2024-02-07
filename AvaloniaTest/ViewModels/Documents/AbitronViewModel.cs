using System;
using System.Collections.Generic;
using AvaloniaTest.Models.Abitron;

namespace AvaloniaTest.ViewModels.Documents;

public class AbitronViewModel : MainContentViewModel
{
    private ISerialPortHandler _serialPortHandler;
    private HSTelegram _telegram;
    private List<byte> _buffer;
    
    public AbitronViewModel()
    {
        _serialPortHandler = new SerialPortHandler();
        _telegram = new HSTelegram();
        
        _serialPortHandler.Open();
        _serialPortHandler.DataReceived+=DataReceived;
        _telegram.TelegramReceivedEvent+=TelegramOnTelegramReceivedEvent;

        _buffer = new List<byte>();
    }

    private void TelegramOnTelegramReceivedEvent(object sender, BaseTelegram.TelegramEventArgs e)
    {
        ((HSTelegram) e.TelegramType).DKState;
    }

    private void DataReceived(object? sender, EventArgs e)
    {
        _buffer.AddRange(_serialPortHandler.ReadData());
        
        while (_buffer.Count > 0)
        {
            List<Parser.Result> resultList = new List<Parser.Result>();
            if (_telegram.Parse(_buffer) != Parser.Result.Match)
            {
                Parser.Result result1 = _telegram.Parse(_buffer);
                if (result1 != Parser.Result.Match)
                {
                    resultList.Add(result1);
                    foreach (Parser.Result result2 in resultList)
                    {
                        if (result2 == Parser.Result.Defer)
                            return;
                    }

                    _buffer.RemoveAt(0);
                }
            }
        }
    }
    
    
}