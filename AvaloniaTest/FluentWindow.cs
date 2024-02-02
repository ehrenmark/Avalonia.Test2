using Avalonia.Styling;
using System;
using System.Collections.Generic;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;
using Avalonia.Controls.Primitives;



namespace AvaloniaTest.Views
{
    public class FluentWindow : Window
    {
        protected new Type StyleKey => typeof(Window);

        public FluentWindow()
        {
            ExtendClientAreaToDecorationsHint = true;
            ExtendClientAreaTitleBarHeightHint = -1;            

            TransparencyLevelHint = new[] {WindowTransparencyLevel.AcrylicBlur};            

            this.GetObservable(WindowStateProperty)
                .Subscribe(x =>
                {
                    PseudoClasses.Set(":maximized", x == WindowState.Maximized);
                    PseudoClasses.Set(":fullscreen", x == WindowState.FullScreen);
                });

            this.GetObservable(IsExtendedIntoWindowDecorationsProperty)
                .Subscribe(x =>
                {
                    if (!x)
                    {
                        SystemDecorations = SystemDecorations.Full;
                        TransparencyLevelHint = new[] {WindowTransparencyLevel.Blur};
                    }
                });
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);            
            ExtendClientAreaChromeHints = 
                ExtendClientAreaChromeHints.PreferSystemChrome |                 
                ExtendClientAreaChromeHints.OSXThickTitleBar;
        }
    }
}

