using System.Collections.Generic;

namespace Shared.Models.Data
{
    /// <summary>
    /// A class that defines the data to be saved by the <see cref="OptionsDataManager"/>
    /// </summary>
    public class OptionsData
    {
        public Dictionary<string, double> audioBusVolumes;
        public Dictionary<string, List<InputData>> savedInputMap;

        public OptionsData()
        {
            audioBusVolumes = new();
            savedInputMap = new();
        }
    }
}