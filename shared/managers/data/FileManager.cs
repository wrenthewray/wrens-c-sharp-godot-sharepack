using System;
using Godot;
using Newtonsoft.Json;
using Shared.Managers.Pools;

namespace Shared.Managers.Data;

/// <summary>
/// This class is a static and generic file manager class that reads and writes 
/// to files using Godot's <see cref="FileAccess"/> class.
/// </summary>
public static class FileManager
{
    private static FileAccess OpenFile(string filePath, FileAccess.ModeFlags flag = FileAccess.ModeFlags.Read)
    {
        FileAccess saveFile = FileAccess.Open(filePath, flag);
        return saveFile;
    }
    /// <summary>
    /// This function takes in a file path and sets it to the data class that's sent. 
    /// It takes in a generic type <typeparamref name="T"/> that must have an empty 
    /// constructor to function properly.
    /// </summary>
    /// <typeparam name="T">The type of the _data param. This type has to have an 
    /// empty constructor to function properly.</typeparam>
    /// <param name="filePath">The location of the file path.</param>
    /// <param name="_data">A reference to the data that is being stored. This must match 
    /// the type <typeparamref name="T"/> the function is given. </param>
    public static void ReadFile<T>(string filePath, ref T _data) where T: new()
    {
        try
        {
            FileAccess file;
            file = OpenFile(filePath);
            string json = file.GetLine();
            _data = JsonConvert.DeserializeObject<T>(json);
            file.Close();
        }
        catch(Exception)
        {
            _data = new();
        }        
    }
    /// <summary>
    /// This function takes in a file path and saves the data object given to that file path.
    /// </summary>
    /// <param name="filePath">The file path to save to.</param>
    /// <param name="_data"> The data to save.</param>
    public static void WriteFile(string filePath, object _data)
    {
        FileAccess file = OpenFile(filePath, FileAccess.ModeFlags.Write);
        string json = JsonConvert.SerializeObject(_data);

        file.StoreLine(json);
        file.Close();
    }
}