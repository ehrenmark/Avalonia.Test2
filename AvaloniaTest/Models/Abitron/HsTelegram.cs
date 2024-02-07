using System;
using System.Collections;
using System.Collections.Generic;

namespace AvaloniaTest.Models.Abitron;

public class HSTelegram : BaseTelegram
  {
    private int[] PREArray = new int[7]
    {
      22,
      170,
      85,
      102,
      22,
      22,
      22
    };
    private int[] StartArray = new int[7]
    {
      2,
      15,
      15,
      15,
      2,
      2,
      2
    };
    private bool m_enhanced;
    private uint m_combined_code;
    private bool EStopState;
    private uint Address;
    public byte[] DatArray = new byte[12];
    public bool[] DKState;
    public byte[] AKValue;

    public override void Enhanced(bool enhanced) => this.m_enhanced = enhanced;

    public override void CombineCode(uint combinedCode) => this.m_combined_code = combinedCode;

    public override Parser.Result Parse(List<byte> buffer)
    {
      int index1 = 0;
      byte[] numArray = new byte[4]
      {
        (byte) (this.m_combined_code & (uint) byte.MaxValue),
        (byte) (this.m_combined_code >> 8 & (uint) byte.MaxValue),
        (byte) (this.m_combined_code >> 16 & (uint) byte.MaxValue),
        (byte) (this.m_combined_code >> 24 & (uint) byte.MaxValue)
      };
      int num1 = 0;
      if (buffer.Count < 8)
        return Parser.Result.Defer;
      if ((int) buffer[0] == this.PREArray[index1] && (int) buffer[1] == this.PREArray[index1] && (int) buffer[2] == this.PREArray[index1] && (int) buffer[3] == this.PREArray[index1] && (int) buffer[4] == this.StartArray[index1])
      {
        byte num2 = (byte) ((uint) buffer[7] & 15U);
        if (index1 == 0)
        {
          switch (num2)
          {
            case 0:
              if (buffer.Count < 10)
                return Parser.Result.Defer;
              if (this.m_enhanced)
              {
                buffer[8] ^= numArray[0];
                buffer[9] ^= numArray[1];
              }
              if (this.XOR(buffer, HSTelegram.TelegramType.ESTOP) && this.ADD(buffer, HSTelegram.TelegramType.ESTOP))
              {
                BaseTelegram.TelegramEventArgs e = new BaseTelegram.TelegramEventArgs();
                e.TelegramType = (object) this;
                e.FunctionId = num2;
                e.Payload = this.Payload(buffer, HSTelegram.TelegramType.ESTOP);
                this.SetEStopState(HSTelegram.TelegramType.ESTOP);
                this.AdmoAddress(buffer, HSTelegram.TelegramType.ESTOP);
                this.DatValues(buffer, HSTelegram.TelegramType.ESTOP);
                this.DKStates(buffer, HSTelegram.TelegramType.ESTOP);
                this.AKValues(buffer, HSTelegram.TelegramType.ESTOP);
                this.InitiateEvent(e);
                buffer.RemoveRange(0, 10);
                return Parser.Result.Match;
              }
              break;
            case 4:
              if (buffer.Count < 14)
                return Parser.Result.Defer;
              if (this.m_enhanced)
              {
                for (int index2 = 0; index2 < buffer.Count; ++index2)
                {
                  if (index2 < 8)
                  {
                    buffer[index2] = buffer[index2];
                  }
                  else
                  {
                    List<byte> byteList1 = buffer;
                    int index3 = index2;
                    List<byte> byteList2;
                    int index4;
                    int num3 = (int) (byteList2 = buffer)[index4 = index2];
                    int num4 = (int) numArray[num1++];
                    int num5;
                    byte num6 = (byte) (num5 = (int) (byte) (num3 ^ num4));
                    byteList2[index4] = (byte) num5;
                    int num7 = (int) num6;
                    byteList1[index3] = (byte) num7;
                    if (num1 >= 4)
                      num1 = 0;
                  }
                }
              }
              if (this.XOR(buffer, HSTelegram.TelegramType.DK32) && this.ADD(buffer, HSTelegram.TelegramType.DK32))
              {
                BaseTelegram.TelegramEventArgs e = new BaseTelegram.TelegramEventArgs();
                e.TelegramType = (object) this;
                e.FunctionId = num2;
                e.Payload = this.Payload(buffer, HSTelegram.TelegramType.DK32);
                this.SetEStopState(HSTelegram.TelegramType.DK32);
                this.AdmoAddress(buffer, HSTelegram.TelegramType.DK32);
                this.DatValues(buffer, HSTelegram.TelegramType.DK32);
                this.DKStates(buffer, HSTelegram.TelegramType.DK32);
                this.AKValues(buffer, HSTelegram.TelegramType.DK32);
                this.InitiateEvent(e);
                buffer.RemoveRange(0, 14);
                return Parser.Result.Match;
              }
              break;
            case 12:
              if (buffer.Count < 22)
                return Parser.Result.Defer;
              if (this.m_enhanced)
              {
                for (int index5 = 0; index5 < buffer.Count; ++index5)
                {
                  if (index5 < 8)
                  {
                    buffer[index5] = buffer[index5];
                  }
                  else
                  {
                    List<byte> byteList3 = buffer;
                    int index6 = index5;
                    List<byte> byteList4;
                    int index7;
                    int num8 = (int) (byteList4 = buffer)[index7 = index5];
                    int num9 = (int) numArray[num1++];
                    int num10;
                    byte num11 = (byte) (num10 = (int) (byte) (num8 ^ num9));
                    byteList4[index7] = (byte) num10;
                    int num12 = (int) num11;
                    byteList3[index6] = (byte) num12;
                    if (num1 >= 4)
                      num1 = 0;
                  }
                }
              }
              if (this.XOR(buffer, HSTelegram.TelegramType.DK80AN2) && this.ADD(buffer, HSTelegram.TelegramType.DK80AN2))
              {
                BaseTelegram.TelegramEventArgs e = new BaseTelegram.TelegramEventArgs();
                e.TelegramType = (object) this;
                e.FunctionId = num2;
                e.Payload = this.Payload(buffer, HSTelegram.TelegramType.DK80AN2);
                this.SetEStopState(HSTelegram.TelegramType.DK80AN2);
                this.AdmoAddress(buffer, HSTelegram.TelegramType.DK80AN2);
                this.DatValues(buffer, HSTelegram.TelegramType.DK80AN2);
                this.DKStates(buffer, HSTelegram.TelegramType.DK80AN2);
                this.AKValues(buffer, HSTelegram.TelegramType.DK80AN2);
                this.InitiateEvent(e);
                buffer.RemoveRange(0, 22);
                return Parser.Result.Match;
              }
              break;
            case 15:
              if (buffer.Count < 22)
                return Parser.Result.Defer;
              if (this.m_enhanced)
              {
                for (int index8 = 0; index8 < buffer.Count; ++index8)
                {
                  if (index8 < 8)
                  {
                    buffer[index8] = buffer[index8];
                  }
                  else
                  {
                    List<byte> byteList5 = buffer;
                    int index9 = index8;
                    List<byte> byteList6;
                    int index10;
                    int num13 = (int) (byteList6 = buffer)[index10 = index8];
                    int num14 = (int) numArray[num1++];
                    int num15;
                    byte num16 = (byte) (num15 = (int) (byte) (num13 ^ num14));
                    byteList6[index10] = (byte) num15;
                    int num17 = (int) num16;
                    byteList5[index9] = (byte) num17;
                    if (num1 >= 4)
                      num1 = 0;
                  }
                }
              }
              if (this.XOR(buffer, HSTelegram.TelegramType.DK32AN8) && this.ADD(buffer, HSTelegram.TelegramType.DK32AN8))
              {
                BaseTelegram.TelegramEventArgs e = new BaseTelegram.TelegramEventArgs();
                e.TelegramType = (object) this;
                e.FunctionId = num2;
                e.Payload = this.Payload(buffer, HSTelegram.TelegramType.DK32AN8);
                this.SetEStopState(HSTelegram.TelegramType.DK32AN8);
                this.AdmoAddress(buffer, HSTelegram.TelegramType.DK32AN8);
                this.DatValues(buffer, HSTelegram.TelegramType.DK32AN8);
                this.DKStates(buffer, HSTelegram.TelegramType.DK32AN8);
                this.AKValues(buffer, HSTelegram.TelegramType.DK32AN8);
                this.InitiateEvent(e);
                buffer.RemoveRange(0, 22);
                return Parser.Result.Match;
              }
              break;
          }
        }
        if (index1 < 7)
        {
          int num18 = index1 + 1;
        }
      }
      return Parser.Result.NoMatch;
    }

    private void SetEStopState(HSTelegram.TelegramType state)
    {
      switch (state)
      {
        case HSTelegram.TelegramType.ESTOP:
          this.EStopState = true;
          break;
        case HSTelegram.TelegramType.DK32:
          this.EStopState = false;
          break;
        case HSTelegram.TelegramType.DK32AN8:
          this.EStopState = false;
          break;
        case HSTelegram.TelegramType.DK80AN2:
          this.EStopState = false;
          break;
      }
    }

    private bool XOR(List<byte> buffer, HSTelegram.TelegramType state)
    {
      byte num = 0;
      bool flag = false;
      switch (state)
      {
        case HSTelegram.TelegramType.ESTOP:
          for (int index = 4; index < 8; ++index)
            num ^= buffer[index];
          flag = (int) num == (int) buffer[8];
          break;
        case HSTelegram.TelegramType.DK32:
          for (int index = 4; index < 12; ++index)
            num ^= buffer[index];
          flag = (int) num == (int) buffer[12];
          break;
        case HSTelegram.TelegramType.DK32AN8:
          for (int index = 4; index < 20; ++index)
            num ^= buffer[index];
          flag = (int) num == (int) buffer[20];
          break;
        case HSTelegram.TelegramType.DK80AN2:
          for (int index = 4; index < 20; ++index)
            num ^= buffer[index];
          flag = (int) num == (int) buffer[20];
          break;
      }
      return flag;
    }

    private bool ADD(List<byte> buffer, HSTelegram.TelegramType state)
    {
      byte num = 0;
      bool flag = false;
      switch (state)
      {
        case HSTelegram.TelegramType.ESTOP:
          for (int index = 4; index < 9; ++index)
            num += buffer[index];
          flag = (int) num == (int) buffer[9];
          break;
        case HSTelegram.TelegramType.DK32:
          for (int index = 4; index < 13; ++index)
            num += buffer[index];
          flag = (int) num == (int) buffer[13];
          break;
        case HSTelegram.TelegramType.DK32AN8:
          for (int index = 4; index < 21; ++index)
            num += buffer[index];
          flag = (int) num == (int) buffer[21];
          break;
        case HSTelegram.TelegramType.DK80AN2:
          for (int index = 4; index < 21; ++index)
            num += buffer[index];
          flag = (int) num == (int) buffer[21];
          break;
      }
      return flag;
    }

    private void AdmoAddress(List<byte> buffer, HSTelegram.TelegramType state)
    {
      byte num1 = 0;
      switch (state)
      {
        case HSTelegram.TelegramType.ESTOP:
          int num2 = (int) buffer[7] - 64;
          if (buffer[7] >= (byte) 0 && buffer[7] <= (byte) 48)
            num2 += 256;
          num1 = (byte) (num2 / 16);
          break;
        case HSTelegram.TelegramType.DK32:
          int num3 = (int) buffer[7] - 84;
          if (buffer[7] >= (byte) 4 && buffer[7] <= (byte) 68)
            num3 += 256;
          num1 = (byte) (num3 / 16);
          break;
        case HSTelegram.TelegramType.DK32AN8:
          int num4 = (int) buffer[7] - (int) sbyte.MaxValue;
          if (buffer[7] >= (byte) 15 && buffer[7] <= (byte) 111)
            num4 += 256;
          num1 = (byte) (num4 / 16);
          break;
        case HSTelegram.TelegramType.DK80AN2:
          int num5 = (int) buffer[7] - 124;
          if (buffer[7] >= (byte) 12 && buffer[7] <= (byte) 108)
            num5 += 256;
          num1 = (byte) (num5 / 16);
          break;
      }
      byte num6 = buffer[5];
      byte num7 = buffer[6];
      this.Address = (uint) (((int) num1 << 16) + ((int) num6 << 8)) + (uint) num7;
    }

    private void DatValues(List<byte> buffer, HSTelegram.TelegramType state)
    {
      switch (state)
      {
        case HSTelegram.TelegramType.ESTOP:
          for (int index = 0; index <= 11; ++index)
            this.DatArray[index] = (byte) 0;
          break;
        case HSTelegram.TelegramType.DK32:
          for (int index = 8; index <= 11; ++index)
            this.DatArray[index - 8] = buffer[index];
          for (int index = 12; index <= 19; ++index)
            this.DatArray[index - 8] = (byte) 0;
          break;
        case HSTelegram.TelegramType.DK32AN8:
          for (int index = 8; index <= 11; ++index)
            this.DatArray[index - 8] = buffer[index];
          for (int index = 12; index <= 19; ++index)
            this.DatArray[index - 8] = (byte) 0;
          break;
        case HSTelegram.TelegramType.DK80AN2:
          for (int index = 8; index <= 19; ++index)
            this.DatArray[index - 8] = buffer[index];
          break;
      }
    }

    private void DKStates(List<byte> buffer, HSTelegram.TelegramType state)
    {
      this.DKState = new bool[81];
      switch (state)
      {
        case HSTelegram.TelegramType.ESTOP:
          for (int index = 1; index < 81; ++index)
            this.DKState[index] = index == 31;
          break;
        case HSTelegram.TelegramType.DK32:
          for (int y = 0; y < 8; ++y)
          {
            byte num = (byte) Math.Pow(2.0, (double) y);
            if (y + 1 == 1)
              this.DKState[31] = (byte) ((uint) buffer[8] & 1U) == (byte) 1;
            else
              this.DKState[y + 1] = (int) (byte) ((uint) buffer[8] & (uint) num) == (int) num;
            this.DKState[y + 9] = (int) (byte) ((uint) buffer[9] & (uint) num) == (int) num;
            this.DKState[y + 17] = (int) (byte) ((uint) buffer[10] & (uint) num) == (int) num;
            if (y + 25 == 31)
              this.DKState[1] = (byte) ((uint) buffer[11] & 64U) == (byte) 64;
            else
              this.DKState[y + 25] = (int) (byte) ((uint) buffer[11] & (uint) num) == (int) num;
          }
          for (int index = 33; index < 81; ++index)
            this.DKState[index] = false;
          break;
        case HSTelegram.TelegramType.DK32AN8:
          for (int y = 0; y < 8; ++y)
          {
            byte num = (byte) Math.Pow(2.0, (double) y);
            if (y + 1 == 1)
              this.DKState[31] = (byte) ((uint) buffer[8] & 1U) == (byte) 1;
            else
              this.DKState[y + 1] = (int) (byte) ((uint) buffer[8] & (uint) num) == (int) num;
            this.DKState[y + 9] = (int) (byte) ((uint) buffer[9] & (uint) num) == (int) num;
            this.DKState[y + 17] = (int) (byte) ((uint) buffer[10] & (uint) num) == (int) num;
            if (y + 25 == 31)
              this.DKState[1] = (byte) ((uint) buffer[11] & 64U) == (byte) 64;
            else
              this.DKState[y + 25] = (int) (byte) ((uint) buffer[11] & (uint) num) == (int) num;
          }
          for (int index = 33; index < 81; ++index)
            this.DKState[index] = false;
          break;
        case HSTelegram.TelegramType.DK80AN2:
          for (int y = 0; y < 8; ++y)
          {
            byte num = (byte) Math.Pow(2.0, (double) y);
            if (y + 1 == 1)
              this.DKState[31] = (byte) ((uint) buffer[8] & 1U) == (byte) 1;
            else
              this.DKState[y + 1] = (int) (byte) ((uint) buffer[8] & (uint) num) == (int) num;
            this.DKState[y + 9] = (int) (byte) ((uint) buffer[9] & (uint) num) == (int) num;
            this.DKState[y + 17] = (int) (byte) ((uint) buffer[10] & (uint) num) == (int) num;
            if (y + 25 == 31)
              this.DKState[1] = (byte) ((uint) buffer[11] & 64U) == (byte) 64;
            else
              this.DKState[y + 25] = (int) (byte) ((uint) buffer[11] & (uint) num) == (int) num;
            this.DKState[y + 33] = (int) (byte) ((uint) buffer[12] & (uint) num) == (int) num;
            this.DKState[y + 41] = (int) (byte) ((uint) buffer[13] & (uint) num) == (int) num;
            this.DKState[y + 49] = (int) (byte) ((uint) buffer[14] & (uint) num) == (int) num;
            this.DKState[y + 57] = (int) (byte) ((uint) buffer[15] & (uint) num) == (int) num;
            this.DKState[y + 65] = (int) (byte) ((uint) buffer[16] & (uint) num) == (int) num;
            this.DKState[y + 73] = (int) (byte) ((uint) buffer[17] & (uint) num) == (int) num;
          }
          break;
      }
    }

    private void AKValues(List<byte> buffer, HSTelegram.TelegramType state)
    {
      this.AKValue = new byte[9];
      switch (state)
      {
        case HSTelegram.TelegramType.ESTOP:
          for (int index = 1; index < 9; ++index)
            this.AKValue[index] = (byte) 127;
          break;
        case HSTelegram.TelegramType.DK32:
          for (int index = 1; index < 9; ++index)
            this.AKValue[index] = (byte) 127;
          break;
        case HSTelegram.TelegramType.DK32AN8:
          for (int index = 1; index < 9; ++index)
            this.AKValue[index] = buffer[11 + index];
          break;
        case HSTelegram.TelegramType.DK80AN2:
          this.AKValue[1] = buffer[18];
          this.AKValue[2] = buffer[19];
          for (int index = 7; index < 9; ++index)
            this.AKValue[index] = buffer[17 + (index - 6)];
          for (int index = 1; index < 7; ++index)
            this.AKValue[index] = (byte) 127;
          break;
      }
    }

    private ArrayList Payload(List<byte> buffer, HSTelegram.TelegramType state)
    {
      ArrayList arrayList = new ArrayList();
      switch (state)
      {
        case HSTelegram.TelegramType.ESTOP:
          return (ArrayList) null;
        case HSTelegram.TelegramType.DK32:
          for (int index = 8; index <= 11; ++index)
            arrayList.Add((object) buffer[index]);
          break;
        case HSTelegram.TelegramType.DK32AN8:
          for (int index = 8; index <= 19; ++index)
            arrayList.Add((object) buffer[index]);
          break;
        case HSTelegram.TelegramType.DK80AN2:
          for (int index = 8; index <= 19; ++index)
            arrayList.Add((object) buffer[index]);
          break;
      }
      return arrayList;
    }

    public override bool GetEStopState() => this.EStopState;

    public override uint GetAddress() => this.Address;

    public override byte[] Set2G4Address(byte[] FieldAddress) => FieldAddress;

    public override bool[] GetDkStates() => this.DKState;

    public override byte[] GetAkValues() => this.AKValue;

    public override byte[] GetDatValues() => this.DatArray;

    private enum TelegramType
    {
      ESTOP,
      DK32,
      DK32AN8,
      DK80AN2,
      AN8,
    }
  }