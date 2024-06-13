using System.IO;
using System.Text.Json;

namespace JsonEditor;

/// <summary>
/// Static json helpers.
/// </summary>
public abstract class Json
{
    private static readonly JsonSerializerOptions WriteIndentedOptions = new() { WriteIndented = true };
    
    /// <summary>
    /// Serializes the given value as json and writes it to a file.
    /// </summary>
    /// <typeparam name="TValue">The type of the value to serialize.</typeparam>
    /// <param name="value">The value to serialize.</param>
    /// <param name="path">The file to write to.</param>
    /// <returns>True if successful, otherwise false.</returns>
    public static bool Serialize<TValue>(TValue value, string path)
    {
        try
        {
            var str = JsonSerializer.Serialize(value, WriteIndentedOptions);
            File.WriteAllText(path, str);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Reads the given file as json and deserializes it into a value of type TValue.
    /// </summary>
    /// <typeparam name="TValue">The type to deserialize the json into.</typeparam>
    /// <param name="path">The file to open for reading.</param>
    /// <returns>A TValue representation of the json file if successful, otherwise the default value of the TValue.</returns>
    public static TValue? Deserialize<TValue>(string path)
    {
        try
        {
            var json = JsonDocument.Parse(File.ReadAllText(path));
            return json.Deserialize<TValue>();
        }
        catch
        {
            return default;
        }
    }
    
    /// <summary>
    /// Reads the given file as json and deserializes it into a value of type TValue.
    /// Creates a new file if deserialization fails.
    /// </summary>
    /// <param name="path">The file to open for reading or to create.</param>
    /// <typeparam name="TValue">The type to deserialize or serialize.</typeparam>
    /// <returns>A TValue representation of the json file if deserialization or serialization is successful,
    /// otherwise the default value of the TValue.</returns>
    public static TValue? ReadOrCreate<TValue>(string path) where TValue : new()
    {
        if (Deserialize<TValue>(path) is { } value) return value;
        value = new TValue();
        return Serialize(value, path) ? value : default;
    }
}