using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

namespace JsonEditor;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    private readonly WindowConfig _config;
    private readonly string _configPath;
    
    /// <summary>
    /// Creates a new instance of the <see cref="MainWindow"/>.
    /// </summary>
    public MainWindow()
    {
        var rootDirectory = new RootFileHandler().CreateFiles();
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddWpfBlazorWebView();
        
        Resources.Add("services", serviceCollection.BuildServiceProvider());
        Resources.Add("hostPage", $"{rootDirectory}\\index.html");
        
        InitializeComponent();

        _configPath = $"{rootDirectory}\\window.json";
        _config = Json.ReadOrCreate<WindowConfig>(_configPath) ?? new WindowConfig();
        _config.SetWindow(this);

        MainPage.FileChanged += fileName => Dispatcher.Invoke(() =>
        {
            Title = fileName;
        });
    }

    private void MainWindow_OnClosing(object? sender, CancelEventArgs e)
    {
        _config.GetWindow(this);
        Json.Serialize(_config, _configPath);
    }
}