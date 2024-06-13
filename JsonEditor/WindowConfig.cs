using System.Windows;

namespace JsonEditor;

/// <summary>
/// Represents the position and size configuration of a <see cref="Window"/>. 
/// </summary>
public class WindowConfig
{
    public int Top { get; set; }
    public int Left { get; set; }
    public int Width { get; set; } = 1200;
    public int Height { get; set; } = 800;
    public WindowState WindowState { get; set; } = WindowState.Normal;
}

/// <summary>
/// Extension methods for the <see cref="WindowConfig"/>.
/// </summary>
public static class WindowConfigExtension
{
    /// <summary>
    /// Sets the size and position of the given <see cref="Window"/>.
    /// </summary>
    /// <param name="config">The config to read from.</param>
    /// <param name="window">The window to set.</param>
    public static void SetWindow(this WindowConfig config, Window window)
    {
        window.Top = config.Top;
        window.Left = config.Left;
        window.Height = config.Height;
        window.Width = config.Width;
        window.WindowState = config.WindowState;
    }
    
    /// <summary>
    /// Gets the size and position of the given <see cref="Window"/>.
    /// </summary>
    /// <param name="config">The config to write to.</param>
    /// <param name="window">The window to read from.</param>
    public static void GetWindow(this WindowConfig config, Window window)
    {
        config.Top = (int)window.Top;
        config.Left = (int)window.Left;
        config.Height = (int)window.Height;
        config.Width = (int)window.Width;
        config.WindowState = window.WindowState;
    }
}