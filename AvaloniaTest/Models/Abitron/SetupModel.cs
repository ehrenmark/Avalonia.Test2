using System;
using System.Collections.Generic;
using System.IO;
using AvaloniaTest.Models.Abitron;
using Newtonsoft.Json;

namespace AvaloniaTest.Models.Abitron
{
    public class SetupModel
    {

    }

    public class RemoteControlSetup
    {
        public string ModelName { get; set; }
        public string Version { get; set; }
        public List<Switch> Switches { get; set; }

        public static RemoteControlSetup? FromJson(string jsonFilePath)
        {
            using (StreamReader file = File.OpenText(jsonFilePath))
            {
                return JsonConvert.DeserializeObject<RemoteControlSetup>(file.ReadToEnd());
            }
        }

        public void PrintSetupDetails()
        {
            Console.WriteLine($"Model Name: {ModelName}");
            Console.WriteLine($"Version: {Version}");

            foreach (var sw in Switches)
            {
                Console.WriteLine($"Switch Name: {sw.Name}");
                Console.WriteLine($"DK State: {sw.DKState}");
                Console.WriteLine($"Dat State: {sw.DatState}");
            }
        }
    }

    public class Switch
    {
        public string Name { get; set; }
        public string DKState { get; set; }
        public string DatState { get; set; }
        
        
    }
}

