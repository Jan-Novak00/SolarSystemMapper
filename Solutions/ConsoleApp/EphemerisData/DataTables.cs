using SolarSystemMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SolarSystemMapper
{
    public static class DataTables
    {
        public static readonly HashSet<ObjectEntry> Planets = new HashSet<ObjectEntry>()
        {
            new ObjectEntry("Mercury",199,"Planet"),
            new ObjectEntry("Venus",299, "Planet"),
            new ObjectEntry("Earth",399,"Planet"),
            new ObjectEntry("Mars",499,"Planet"),
            new ObjectEntry("Jupiter",599,"Planet"),
            new ObjectEntry("Saturn",699,"Planet"),
            new ObjectEntry("Uranus",799,"Planet"),
            new ObjectEntry("Neptune",899,"Planet")

        };
        public static readonly HashSet<ObjectEntry> Stars = new HashSet<ObjectEntry>()
        {
            new ObjectEntry("Sun",10,"Star")

        };

        public static readonly HashSet<ObjectEntry> DwarfPlanets = new HashSet<ObjectEntry>
        {
            new ObjectEntry("Vesta", 4, "DwarfPlanet"),
            new ObjectEntry("Ceres", 2000001, "DwarfPlanet"),
            new ObjectEntry("Pluto",999,"DwarfPlanet"),
            new ObjectEntry("Eris", 920136199, "DwarfPlanet"),
            new ObjectEntry("Haumea", 136108, "DwarfPlanet"),
            new ObjectEntry("Makemake", 136472, "DwarfPlanet")
        };

        public static readonly HashSet<ObjectEntry> Spacecrafts = new HashSet<ObjectEntry>
        {
            //new ObjectEntry("ISS", -125544, "Spacecraft"),
            new ObjectEntry("Hubble Space Telescope", -48, "Spacecraft"),
            new ObjectEntry("James Webb Space Telescope", -170, "Spacecraft"),
            new ObjectEntry("Mariner 2", -2, "Spacecraft"),
            new ObjectEntry("Mars Orbiter Mission", -3, "Spacecraft"),
            new ObjectEntry("Planet-C", -5, "Spacecraft"),
            new ObjectEntry("Pioneer 6", -6, "Spacecraft"),
            new ObjectEntry("Wind", -8, "Spacecraft"),
            new ObjectEntry("Pioneer 8", -20, "Spacecraft"),
            new ObjectEntry("SOHO", -21, "Spacecraft"),
            new ObjectEntry("Pioneer 10", -23, "Spacecraft"),
            new ObjectEntry("Pioneer 11", -24, "Spacecraft"),
            new ObjectEntry("Deep Space 1", -30, "Spacecraft"),
            new ObjectEntry("Voyager 1", -31, "Spacecraft"),
            new ObjectEntry("Voyager 2", -32, "Spacecraft"),
            new ObjectEntry("Clementine", -40, "Spacecraft"),
            new ObjectEntry("Mars Express", -41, "Spacecraft"),
            new ObjectEntry("Hubble Space Telescope", -48, "Spacecraft"),
            new ObjectEntry("Lucy", -49, "Spacecraft"),
            new ObjectEntry("Genesis (SRC)", -47900, "Spacecraft"),
            new ObjectEntry("Mars Odyssey", -53, "Spacecraft"),
            new ObjectEntry("Mars Pathfinder", -530, "Spacecraft"),
            new ObjectEntry("Ulysses", -55, "Spacecraft"),
            new ObjectEntry("Juno", -61, "Spacecraft"),
            new ObjectEntry("Deep Impact IMPACTOR", -70, "Spacecraft"),
            new ObjectEntry("Mars Reconnaissance Orbiter", -74, "Spacecraft"),
            new ObjectEntry("OMOTENASHI", -75, "Spacecraft"),
            new ObjectEntry("Mars Science Laboratory", -76, "Spacecraft"),
            new ObjectEntry("MRO Centaur Stage", -74900, "Spacecraft"),
            new ObjectEntry("Galileo", -77, "Spacecraft"),
            new ObjectEntry("DSCOVR", -78, "Spacecraft"),
            new ObjectEntry("Spitzer Space Telescope", -79, "Spacecraft"),
            new ObjectEntry("Cassini", -82, "Spacecraft"),
            new ObjectEntry("Phoenix", -84, "Spacecraft"),
            new ObjectEntry("TESS", -95, "Spacecraft"),
            new ObjectEntry("Parker Solar Probe", -96, "Spacecraft"),
            new ObjectEntry("New Horizons", -98, "Spacecraft"),
            new ObjectEntry("EQUULEUS", -101, "Spacecraft"),
            new ObjectEntry("ICE", -111, "Spacecraft"),
            new ObjectEntry("BepiColombo", -121, "Spacecraft"),
            new ObjectEntry("ICPS", -125, "Spacecraft"),
            new ObjectEntry("Hayabusa", -130, "Spacecraft"),
            new ObjectEntry("DART", -135, "Spacecraft"),
            new ObjectEntry("Deep Impact Flyby - EPOXI", -140, "Spacecraft"),
            new ObjectEntry("ExoMars16 TGO", -143, "Spacecraft"),
            new ObjectEntry("Solar Orbiter", -144, "Spacecraft"),
            new ObjectEntry("Cassini Huygens", -150, "Spacecraft"),
            new ObjectEntry("Mars2020", -168, "Spacecraft"),
            new ObjectEntry("Chandrayaan-3P (ORBITER)", -169, "Spacecraft"),
            new ObjectEntry("James Webb Space Telescope", -170, "Spacecraft"),
            new ObjectEntry("GRAIL-SS Second Stage", -176, "Spacecraft"),
            new ObjectEntry("GRAIL-A", -177, "Spacecraft"),
            new ObjectEntry("Nozomi", -178, "Spacecraft"),
            new ObjectEntry("GRAIL-B", -181, "Spacecraft"),
            new ObjectEntry("NEA Scout", -182, "Spacecraft"),
            new ObjectEntry("Rosetta", -226, "Spacecraft"),
            new ObjectEntry("Kepler", -227, "Spacecraft"),
            new ObjectEntry("Galileo Probe", -344, "Spacecraft"),
            new ObjectEntry("Herschel Space Observatory", -486, "Spacecraft"),
            new ObjectEntry("Planck Space Observatory", -489, "Spacecraft"),



        };

        public static readonly HashSet<ObjectEntry> EarthSatelites = new HashSet<ObjectEntry>()
        {
            new ObjectEntry("Luna",301,"Moon"),
            //new ObjectEntry("Hubble Space Telescope", -48, "Spacecraft"),
            //new ObjectEntry("James Webb Space Telescope", -170, "Spacecraft"),

        };

        public static readonly HashSet<ObjectEntry> MarsSatelites = new HashSet<ObjectEntry>()
        {
            new ObjectEntry("Phobos",401,"Moon"),
            new ObjectEntry("Deimos",402,"Moon"),
            new ObjectEntry("Mars Orbiter Mission", -3, "Spacecraft"),
        };

        public static readonly HashSet<ObjectEntry> JupiterSatelites = new HashSet<ObjectEntry>()
        {
            new ObjectEntry("Io",501,"Moon"),
            new ObjectEntry("Europa",502,"Moon"),
            new ObjectEntry("Ganymede",503,"Moon"),
            new ObjectEntry("Callisto",504,"Moon"),
            new ObjectEntry("Amalthea",505,"Moon"),
        };

        public static readonly HashSet<ObjectEntry> SaturnSatelites = new HashSet<ObjectEntry>()
        {
            new ObjectEntry("Mimas",601,"Moon"),
            new ObjectEntry("Enceladus",602,"Moon"),
            new ObjectEntry("Titan",606,"Moon"),
            new ObjectEntry("Hyperion",607,"Moon"),
        };
        public static readonly HashSet<ObjectEntry> UranusSatelites = new HashSet<ObjectEntry>()
        {
            new ObjectEntry("Titania",703,"Moon"),
            new ObjectEntry("Oberon",704,"Moon"),
            new ObjectEntry("Ariel",701,"Moon"),
        };

        public static readonly HashSet<ObjectEntry> NeptuneSatelites = new HashSet<ObjectEntry>()
        {
            new ObjectEntry("Triton",801,"Moon"),
            new ObjectEntry("Nereid",802,"Moon"),
            new ObjectEntry("Proteus",808,"Moon"),
        };

        public static readonly HashSet<ObjectEntry> PlutoSatelites = new HashSet<ObjectEntry>()
        {
            new ObjectEntry("Charon",901,"Moon"),
            new ObjectEntry("Nix",902,"Moon"),
            new ObjectEntry("Hydra",903,"Moon"),
            new ObjectEntry("Kerberos",904,"Moon"),
            new ObjectEntry("Styx",905,"Moon"),
        };

        public static HashSet<ObjectEntry> Satelites = InitSatelites();
        private static HashSet<ObjectEntry> InitSatelites()
        {
            var moons = new HashSet<ObjectEntry>(EarthSatelites);
            moons.UnionWith(MarsSatelites);
            moons.UnionWith(JupiterSatelites);
            moons.UnionWith(SaturnSatelites);
            moons.UnionWith(UranusSatelites);
            moons.UnionWith(NeptuneSatelites);
            moons.UnionWith(PlutoSatelites);
            return moons;
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



    }
}
