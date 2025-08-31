using SolarSystemMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace SolarSystemMapper
{

    public static class DataTables
    {
        private static readonly string _objectDataFolder = "objectData/";

        private static HashSet<ObjectEntry>? _Planets_BackingField = null;
        public static HashSet<ObjectEntry> Planets 
        { 
            get
                {
                if (_Planets_BackingField == null) _Planets_BackingField = _loadObjectEntries(_objectDataFolder + "Planets.json");
                return _Planets_BackingField;

                } 
        }

        private static HashSet<ObjectEntry>? _Stars_BackingField = null;
        public static HashSet<ObjectEntry> Stars 
        { 
            get
                {
                    if (_Stars_BackingField == null) _Stars_BackingField = _loadObjectEntries(_objectDataFolder + "Stars.json");
                    return _Stars_BackingField;
                }
        }

        private static HashSet<ObjectEntry>? _DwarfPlanets_BackingField = null;

        public static HashSet<ObjectEntry> DwarfPlanets
        {
            get
            {
                if (_DwarfPlanets_BackingField == null) _DwarfPlanets_BackingField = _loadObjectEntries(_objectDataFolder + "DwarfPlanets.json");
                return _DwarfPlanets_BackingField;
            }
        }


        private static HashSet<ObjectEntry>? _Spacecrafts_BackingField = null;

        public static HashSet<ObjectEntry> Spacecrafts
        {
            get
                {
                    if (_Spacecrafts_BackingField == null) _Spacecrafts_BackingField = _loadObjectEntries(_objectDataFolder + "Spacecrafts.json");
                    return _Spacecrafts_BackingField;
                }
        }
        private static HashSet<ObjectEntry>? _EarthSatelites_BackingField = null;
        public static HashSet<ObjectEntry> EarthSatelites
        {
            get
                {
                    if (_EarthSatelites_BackingField == null) _EarthSatelites_BackingField = _loadObjectEntries(_objectDataFolder + "EarthSatelites.json");
                    return _EarthSatelites_BackingField;
                }
        }

        private static HashSet<ObjectEntry>? _MarsSatelites_BackingField = null;
        public static  HashSet<ObjectEntry> MarsSatelites
        {
            get
            {
                if (_MarsSatelites_BackingField == null) _MarsSatelites_BackingField = _loadObjectEntries(_objectDataFolder + "MarsSatelites.json");
                return _MarsSatelites_BackingField;
            }
        }

        private static HashSet<ObjectEntry>? _JupiterSatelites_BackingField = null;
        public static HashSet<ObjectEntry> JupiterSatelites
        {
            get
            {
                if (_JupiterSatelites_BackingField == null) _JupiterSatelites_BackingField = _loadObjectEntries(_objectDataFolder + "JupiterSatelites.json");
                return _JupiterSatelites_BackingField;
            }
        }
        private static HashSet<ObjectEntry>? _SaturnSatelites_BackingField = null;
        public static HashSet<ObjectEntry> SaturnSatelites
        {
            get
            {
                if (_SaturnSatelites_BackingField == null) _SaturnSatelites_BackingField = _loadObjectEntries(_objectDataFolder + "SaturnSatelites.json");
                return _SaturnSatelites_BackingField;
            }
        }
        private static HashSet<ObjectEntry>? _UranusSatelites_BackingField = null;
        public static HashSet<ObjectEntry> UranusSatelites
        {
            get
            {
                if (_UranusSatelites_BackingField == null) _UranusSatelites_BackingField = _loadObjectEntries(_objectDataFolder + "UranusSatelites.json");
                return _UranusSatelites_BackingField;
            }
        }

        private static HashSet<ObjectEntry>? _NeptuneSatelites_BackingField = null;
        public static HashSet<ObjectEntry> NeptuneSatelites
        {
            get
            {
                if (_NeptuneSatelites_BackingField == null) _NeptuneSatelites_BackingField = _loadObjectEntries(_objectDataFolder + "NeptuneSatelites.json");
                return _NeptuneSatelites_BackingField;
            }
        }
        private static HashSet<ObjectEntry>? _PlutoSatelites_BackingField = null;
        public static HashSet<ObjectEntry> PlutoSatelites
        {
            get
            {
                if (_PlutoSatelites_BackingField == null) _PlutoSatelites_BackingField = _loadObjectEntries(_objectDataFolder + "PlutoSatelites.json");
                return _PlutoSatelites_BackingField;
            }
        }

        public static readonly HashSet<string> ObjectsWithSatelites = new HashSet<string>()
        {
            "Earth", "Mars", "Jupiter", "Saturn", "Uranus", "Neptune", "Pluto"
        };



        public static HashSet<ObjectEntry> GiveSatelitesToPlanet(string planetName)
        {
            switch (planetName)
            {
                case "Earth":
                    return EarthSatelites;
                case "Mars":
                    return MarsSatelites;
                case "Jupiter":
                    return JupiterSatelites;
                case "Saturn":
                    return SaturnSatelites;
                case "Uranus":
                    return UranusSatelites;
                case "Neptune":
                    return NeptuneSatelites;
                case "Pluto":
                    return PlutoSatelites;
                default:
                    return new HashSet<ObjectEntry>();
            }
        }

        private static HashSet<ObjectEntry> _loadObjectEntries(string file)
        {
            string dataString = File.ReadAllText(file);
            return JsonSerializer.Deserialize<HashSet<ObjectEntry>>(dataString);

        }



    }
}
