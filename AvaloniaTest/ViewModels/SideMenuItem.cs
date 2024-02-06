using AvaloniaTest.ViewModels.Documents;
using ReactiveUI;

namespace AvaloniaTest.ViewModels;

public class SideMenuItem : ViewModelBase
{
    public string Title { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsVisible { get; set; } = true;

    
    private MainContentViewModel? _content;
    public MainContentViewModel? Content
    {
        get => _content;
        set => this.RaiseAndSetIfChanged(ref _content, value);
    }
    
}