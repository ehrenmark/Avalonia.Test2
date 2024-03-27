using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text.Unicode;
using System.Xml.Linq;
using AvaloniaTest.ViewModels.Documents;
using DynamicData.Binding;
using ReactiveUI;
using AvaloniaTest.Models.Abitron;
using DynamicData;

namespace AvaloniaTest.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private bool _isMenuItemChecked;
        private SerialPortHandler _serialPortHandler;

        public MainWindowViewModel()
        {
            _serialPortHandler = new SerialPortHandler();
            
            
            AbitronSetupModel abitronSetupModel = new AbitronSetupModel();
            AbitronViewModel abitronViewModel = new AbitronViewModel(_serialPortHandler);
            // AbitronAssistViewModel abitronAssistViewModel = new AbitronAssistViewModel(_serialPortHandler);
            
            SetupViewModel setupViewModel = new SetupViewModel(_serialPortHandler);
            setupViewModel.SerialPortChanged += (sender, args) =>
            {
                abitronViewModel.SerialPortChanged();
            };
            
            MenuItems.Add(new SideMenuItem() { Title = "Setup", Content = setupViewModel});
            MenuItems.Add(new SideMenuItem() { Title = "Abitron", Content = abitronViewModel});
            MenuItems.Add(new SideMenuItem() { Title = "Abitron Assisted Test", Content = abitronViewModel});
            MenuItems.Add(new SideMenuItem() { Title = "Hetronic", Content = new SideMenuTextViewModel() {Txt = "Hetronic"}});
            MenuItems.Add(new SideMenuItem() { Title = "CSL", Content = new SideMenuTextViewModel() {Txt = "CSL"}});
            MenuItems[1].IsActive = true;
            MainContent = MenuItems[0].Content;
            ToggleMenuItemCheckedCommand = ReactiveCommand.Create(() =>
            {
                IsMenuItemChecked = !IsMenuItemChecked;
            });
            
        }
        
        

        public bool IsMenuItemChecked
        {
            get { return _isMenuItemChecked; }
            set { this.RaiseAndSetIfChanged(ref _isMenuItemChecked, value); }
        }

        public ReactiveCommand<Unit, Unit> ToggleMenuItemCheckedCommand { get; }

        public string Greeting => "Welcome to Avalonia!";
        
        private MainContentViewModel _mainContent;

        public ObservableCollectionExtended<SideMenuItem> MenuItems { get; set; } = new();


        public MainContentViewModel MainContent
        {
            get => _mainContent;
            set => this.RaiseAndSetIfChanged(ref _mainContent, value);
        }

        public void ButtonClickedCommand(SideMenuItem item)
        {
            if (MainContent.CanLeave)
            {
                MainContent = item.Content;

                // Check if AbitronViewModel item already exists
                var existingAbitronItem = MenuItems.FirstOrDefault(menuItem => menuItem.Title == "- Abitron Overview");

                // If AbitronViewModel item already exists, remove it
                if (existingAbitronItem != null)
                {
                    MenuItems.Remove(existingAbitronItem);
                }
                // If AbitronViewModel item doesn't exist, insert it
                else if (item == MenuItems[1])
                {
                    //MenuItems.Insert(2, new SideMenuItem() { Title = "- Abitron Overview", Content = new AbitronViewModel() });
                }
            }
        }


    }
}