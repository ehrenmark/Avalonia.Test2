using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using Avalonia.Collections;
using Avalonia.Controls.Primitives;
using AvaloniaTest.Models.Abitron;
using DynamicData;
using Newtonsoft.Json;
using ReactiveUI;

namespace AvaloniaTest.ViewModels.Documents;

public class SwitchViewModel : ViewModelBase
{
    private bool _state;
    public int DKState { get; set; }

    public SwitchViewModel(SwitchConfig config)
    {
        Name = config.Name;
        DKState = config.DKState;
        DatState = config.DatState;
        PositionX = config.PositionX;
        PositionY = config.PositionY;
        CircleType = config.CircleType;
    }
    
    public string Name { get; } 

    public bool State
    {
        get => _state;
        set
        {
            this.RaiseAndSetIfChanged(ref _state, value);
        }
    }

    public void Update(bool[] data)
    {
        if (data[DKState])
        {
            State = true;
            Console.WriteLine($"{Name} pressed");
        }
            
        else State = false;
    }

    public int DatState { get; set; }
    public int PositionX { get; set; }
    public int PositionY { get; set; }
    public string CircleType { get; set; }
}



public class AbitronViewModel : MainContentViewModel
{
    private ISerialPortHandler _serialPortHandler;
    private HSTelegram _telegram;
    private List<byte> _buffer;
    private AvaloniaDictionary<string, SwitchViewModel> _switches;
    private byte[] _datarray;
    
    
    public AvaloniaDictionary<string, SwitchViewModel> Switches
    {
        get { return _switches; }
        set
        {
            _switches = value;
            this.RaiseAndSetIfChanged(ref _switches, value);
            //OnPropertyChanged(nameof(DKState));
        }
    }

    public void SerialPortChanged()
    {
        
    }

    public byte[] DatArray
    {
        get { return _datarray; }
        set
        {
            _datarray = value;
            //OnPropertyChanged(nameof(DatArray));
        }
    }
    
    
    public AbitronViewModel(ISerialPortHandler portHandler)
    {
        _serialPortHandler = portHandler;
        _telegram = new HSTelegram();
        _buffer = new List<byte>(10000);
        
        var configJson = File.ReadAllText(@"SetupJSON\\setupabitron.json");
        var configData = JsonConvert.DeserializeObject<ConfigData>(configJson);
        
        Switches = new();
        foreach (var configDataSwitch in configData.Switches)
        {
            Switches.Add(configDataSwitch.Name, new SwitchViewModel(configDataSwitch));
        }
    }

    public void Start()
    {
        _serialPortHandler.Open();
        _serialPortHandler.DataReceived+=DataReceived;
        _telegram.TelegramReceivedEvent+=TelegramOnTelegramReceivedEvent;
    }

    public void Stop()
    {
        _serialPortHandler.DataReceived-=DataReceived;
        _telegram.TelegramReceivedEvent-=TelegramOnTelegramReceivedEvent;
        _serialPortHandler.Close();
    }

    private void TelegramOnTelegramReceivedEvent(object sender, BaseTelegram.TelegramEventArgs e)
    {
        var state = ((HSTelegram) e.TelegramType).DKState;
        
        // for (int i = 0; i < state.Length; i++)
        // {
        //     Switches[i].State = state[i];
        // }

        foreach (var sw in Switches.Keys)
        {
            Switches[sw].Update(state);
        }
        
        //DatArray = ((HSTelegram) e.TelegramType).DatArray;
    }

    private void DataReceived(object? sender, EventArgs e)
    {
        if (_buffer==null) 
            return;
        
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

public class SwitchConfig
{
    public string Name { get; set; }
    public int DKState { get; set; }
    public int DatState { get; set; }
    public int PositionX { get; set; }
    public int PositionY { get; set; }
    
    public string CircleType { get; set; }
}

public class ConfigData
{
    public string ModelName { get; set; }
    public string Version { get; set; }
    public List<SwitchConfig> Switches { get; set; }
}
