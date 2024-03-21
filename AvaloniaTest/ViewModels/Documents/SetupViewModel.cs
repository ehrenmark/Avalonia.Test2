using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Ports;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AvaloniaTest.ViewModels.Documents
{
    public class SetupViewModel : MainContentViewModel, INotifyPropertyChanged
    {
        public string Txt { get; set; }

        private ObservableCollection<string> _dropdownItems;
        private string _selectedDropdownItem;

        public event PropertyChangedEventHandler? PropertyChanged;
        
        public SerialPortHandler SerialPortHandler;

        public event EventHandler SerialPortChanged;
        
        
        public ObservableCollection<string> DropdownItems
        {
            get => _dropdownItems;
            set
            {
                if (value == _dropdownItems) return;
                _dropdownItems = value;
                OnPropertyChanged();
            }
        }

        public string SelectedDropdownItem
        {
            get => _selectedDropdownItem;
            set
            {
                if (value == _selectedDropdownItem) return;
                _selectedDropdownItem = value;
                OnPropertyChanged();
            }
        }

        public SetupViewModel(SerialPortHandler serialPortHandler)
        {
            SerialPortHandler = serialPortHandler;
            Txt = "Setup";
            PopulateSerialPorts();
            MonitorSerialPortChanges();
        }
        
        public void ConfirmSelection()
        {
            if (!string.IsNullOrEmpty(SelectedDropdownItem))
            {
                SerialPortHandler.SetSelectedPort(SelectedDropdownItem);
                SerialPortChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void PopulateSerialPorts()
        {
            DropdownItems = new ObservableCollection<string>(SerialPort.GetPortNames());
        }

        private void MonitorSerialPortChanges()
        {
            var thread = new Thread(() =>
            {
                var previousPorts = SerialPort.GetPortNames();
                while (true)
                {
                    Thread.Sleep(1000);
                    var currentPorts = SerialPort.GetPortNames();
                    if (!AreEqual(previousPorts, currentPorts))
                    {
                        PopulateSerialPorts();
                        previousPorts = currentPorts;
                    }
                }
            });
            thread.IsBackground = true;
            thread.Start();
        }

        private bool AreEqual(string[] arr1, string[] arr2)
        {
            if (arr1.Length != arr2.Length)
                return false;

            Array.Sort(arr1);
            Array.Sort(arr2);

            for (int i = 0; i < arr1.Length; i++)
            {
                if (arr1[i] != arr2[i])
                    return false;
            }

            return true;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
