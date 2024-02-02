using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvaloniaTest.Views;

public partial class MyMainWindow : FluentWindow
{
    public MyMainWindow()
    {
        InitializeComponent();
        this.AttachDevTools();

    }
    
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);            
    }
}