using System.Collections.Generic;
using System.Reactive;
using AvaloniaTest.ViewModels.Documents;
using DynamicData.Binding;
using ReactiveUI;
using AvaloniaTest.Models.Abitron;

namespace AvaloniaTest.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private bool _isMenuItemChecked;

        public MainWindowViewModel()
        {
            SetupModel setupModel = new SetupModel();
            
            
            MenuItems.Add(new SideMenuItem() { Title = "Side Test1", Content = new SideMenuTextViewModel(){ Txt = "Test1"}});
            MenuItems.Add(new SideMenuItem() { Title = "Side Test2", Content = new SideMenuTextViewModel() {Txt = "Test2"}});
            MenuItems.Add(new SideMenuItem() { Title = "Another Test3", Content = new AbitronViewModel() });
            MenuItems[2].IsActive = true;
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
                if (item == MenuItems[1])
                {
                    MenuItems.Insert(2,new SideMenuItem() { Title = "- Side Test2", Content = new AbitronViewModel() });
                }
            }
            
        
        }

    }
}