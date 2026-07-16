using System;
using System.Collections.Generic;
using Godot;

namespace Shared.Managers.Data
{
    /// <summary>
    /// This static class is used to manage the save locations. 
    /// Save locations are different folders that represent
    /// individual save files for the player, and all game data
    /// gets stored within these folders. This class can add a new
    /// save location, verify a save location is possible, and stores
    /// the current save location so the program can use that in its
    /// saving and loading process.
    /// </summary>
    public static class SaveLocationManager
    {
        private static readonly string FILE_PATH = "user://save_locations.json";
        private static Dictionary<string, string> _data;
        private static string currentSaveLocationName = null;

        /// <summary>
        /// Adds a new save location to the dictionary. Called when a new
        /// game is created.
        /// </summary>
        /// <param name="locationName">The name of the save location.</param>
        public static void AddSaveLocation(string locationName)
        {
            if (_data == null) FileManager.ReadFile(FILE_PATH, ref _data);
            string locationPath = $"user://{locationName}";
            DirAccess.MakeDirAbsolute(locationPath);
            _data.Add(locationName, locationPath);
            CurrentSaveLocationName = locationName;
            FileManager.WriteFile(FILE_PATH, _data);
        }
        /// <summary>
        /// Checks to see if a save location is valid. First, we check to 
        /// see if the location is saved in the dictionary. If so, we also
        /// check to make sure that the path can be opened. 
        /// </summary>
        /// <param name="locationName">The name of the location to check</param>
        /// <returns>A boolean that's true if the save location is valid.</returns>
        public static bool CheckValidSaveLocation(string locationName)
        {
            if (!_data.ContainsKey(locationName)) return false;
            return DirAccess.Open(_data[locationName] ?? "") != null;
        }
        /// <summary>
        /// Removes a save location. Called when deleting a saved game.
        /// </summary>
        /// <param name="locationName">The name of the location to delete.</param>
        public static void RemoveSaveLocation(string locationName)
        {
            DirAccess.RemoveAbsolute(_data[locationName]);
            _data.Remove(locationName);
            FileManager.WriteFile(FILE_PATH, _data);
        }
        /// <summary>
        /// A dictionary that stores all the save locations and their
        /// file paths.
        /// </summary>
        public static Dictionary<string, string> SaveLocations
        {
            get
            {
                if (_data == null) FileManager.ReadFile(FILE_PATH, ref _data);
                return _data;
            }
        }
        /// <summary>
        /// The current save location as a string. This is used by 
        /// all file paths that are directly related to the game.
        /// </summary>
        /// <value>The new current save location.</value>
        public static string CurrentSaveLocationName
        {
            get
            {
                if (!CheckValidSaveLocation(currentSaveLocationName))
                    currentSaveLocationName = null;
                return currentSaveLocationName;
            }
            set
            {
                if (!CheckValidSaveLocation(value))
                {
                    GD.Print(value + " NOT VALID");
                    return;
                }
                currentSaveLocationName = value;
            }
        }
        /// <summary>
        /// The current save location the player is using.
        /// </summary>
        public static string CurrentSaveLocation
        {
            get
            {
                if (!CheckValidSaveLocation(currentSaveLocationName))
                    return null;
                return SaveLocations[currentSaveLocationName];
            }
        }
        /// <summary>
        /// Reset the current save location to null.
        /// </summary>
        public static void ResetCurrentSaveLocation()
        {
            currentSaveLocationName = null;
        }
    }
}