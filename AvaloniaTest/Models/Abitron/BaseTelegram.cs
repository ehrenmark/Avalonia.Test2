using System;
using System.Collections;
using System.Collections.Generic;

namespace AvaloniaTest.Models.Abitron;

public abstract class BaseTelegram
{
    private int count;

    public event BaseTelegram.TelegramReceivedHandler TelegramReceivedEvent;

    protected void InitiateEvent(BaseTelegram.TelegramEventArgs e)
    {
        ++this.count;
        if (this.count == 0)
            e.TogglePanel = false;
        if (this.count == 1)
            e.TogglePanel = true;
        if (this.count >= 2)
            this.count = 0;
        if (this.TelegramReceivedEvent == null)
            return;
        this.TelegramReceivedEvent((object) this, e);
    }

    public abstract void CombineCode(uint combinedCode);

    public abstract void Enhanced(bool enhanced);

    public abstract Parser.Result Parse(List<byte> data);

    public abstract bool GetEStopState();

    public abstract uint GetAddress();

    public abstract byte[] Set2G4Address(byte[] FieldAddress);

    public abstract bool[] GetDkStates();

    public abstract byte[] GetAkValues();

    public abstract byte[] GetDatValues();

    public class TelegramEventArgs : EventArgs
    {
        public object TelegramType { get; set; }

        public byte FunctionId { get; set; }

        public ArrayList Payload { get; set; }

        public bool TogglePanel { get; set; }
    }

    public delegate void TelegramReceivedHandler(object sender, BaseTelegram.TelegramEventArgs e);
}