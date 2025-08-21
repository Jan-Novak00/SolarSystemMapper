using SolarSystemMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EphemerisData
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

        public static readonly HashSet<ObjectEntry> ArtificialSatelites = new HashSet<ObjectEntry>
        {
            new ObjectEntry("ISS", -125544, "ArtificialSatelites"),
            new ObjectEntry("Mars Orbiter Mission", -3, "ArtificialSatelites"),
            new ObjectEntry("Hubble Space Telescope", -48, "ArtificialSatelites"),
            new ObjectEntry("James Webb Space Telescope", -170, "ArtificialSatelites")


        };
        public static readonly HashSet<ObjectEntry> Moons = new HashSet<ObjectEntry>()
        {
            new ObjectEntry("Luna",301,"Moon"),

            new ObjectEntry("Phobos",401,"Moon"),
            new ObjectEntry("Deimos",402,"Moon"),

            new ObjectEntry("Io",501,"Moon"),
            new ObjectEntry("Europa",502,"Moon"),
            new ObjectEntry("Ganymede",503,"Moon"),
            new ObjectEntry("Callisto",504,"Moon"),
            new ObjectEntry("Amalthea",505,"Moon"),

            new ObjectEntry("Mimas",601,"Moon"),
            new ObjectEntry("Enceladus",602,"Moon"),
            new ObjectEntry("Titan",606,"Moon"),
            new ObjectEntry("Hyperion",607,"Moon"),

            new ObjectEntry("Titania",703,"Moon"),
            new ObjectEntry("Oberon",704,"Moon"),
            new ObjectEntry("Ariel",701,"Moon"),

            new ObjectEntry("Triton",801,"Moon"),
            new ObjectEntry("Nereid",802,"Moon"),
            new ObjectEntry("Proteus",808,"Moon"),

            new ObjectEntry("Charon",901,"Moon"),
            new ObjectEntry("Nix",902,"Moon"),
            new ObjectEntry("Hydra",903,"Moon"),
            new ObjectEntry("Kerberos",904,"Moon"),
            new ObjectEntry("Styx",905,"Moon")
        };
        public static readonly HashSet<ObjectEntry> NightSkyMapSpecial = new HashSet<ObjectEntry>()
        {
            new ObjectEntry("Luna",301,"Moon"),
            new ObjectEntry("ISS", -125544, "ArtificialSatelites"),
            new ObjectEntry("Hubble Space Telescope", -48, "ArtificialSatelites"),
            new ObjectEntry("James Webb Space Telescope", -170, "ArtificialSatelites"),


        };





    }
}
