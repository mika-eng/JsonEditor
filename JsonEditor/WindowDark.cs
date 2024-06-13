using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace JsonEditor;

/// <summary>
/// Extended <see cref="Window"/> class with a dark TitleBar.
/// </summary>
public class WindowDark : Window
{
    [DllImport("dwmapi.dll", PreserveSig = true)]
    private static extern void DwmSetWindowAttribute(IntPtr windowHandle, int attr, ref bool attrValue, int attrSize);
    
    protected override void OnSourceInitialized(EventArgs e) 
    {
        var value = true;
        DwmSetWindowAttribute(new WindowInteropHelper(this).Handle, 20, ref value, Marshal.SizeOf(value));
    }
}