using System.IO;
using System.Reflection;

namespace JsonEditor;

/// <summary>
/// Represents a class to copy specific resource files to a specific directory.
/// </summary>
public class RootFileHandler
{
    private readonly string? _assemblyName;
    private readonly string _rootFolder;
    
    /// <summary>
    /// Creates a new instance of the <see cref="RootFileHandler"/> class.
    /// </summary>
    public RootFileHandler()
    {
        _assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
        _rootFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\{_assemblyName}";
    }
    
    /// <summary>
    /// Gets all 'wwwroot' resource files and copies them to the %appdata% directory.
    /// </summary>
    /// <returns>The full path of the directory where the files are created.</returns>
    public string CreateFiles()
    {
        Directory.CreateDirectory(_rootFolder);
        
        var assembly = GetType().Assembly;
        var prefix = $"{_assemblyName}.wwwroot.";
        
        foreach (var resourceName in assembly.GetManifestResourceNames())
        {
            if (!resourceName.StartsWith(prefix)) continue;
            
            var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream is null) continue;
            
            using var reader = new StreamReader(stream);
            File.WriteAllText($"{_rootFolder}\\{resourceName.Replace(prefix, null)}", reader.ReadToEnd());
        }

        return _rootFolder;
    }
}