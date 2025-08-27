using SolarSystemMapper;
namespace DataReader_Tests
{
    public class FindMethodsTests
    {
        private string sun = """
                        -----------------
            API VERSION: 1.2
            API SOURCE: NASA/JPL Horizons API

            *******************************************************************************
             Revised: July 31, 2013                  Sun                                 10

             PHYSICAL PROPERTIES (updated 2024-Oct-30):
              GM, km^3/s^2          = 132712440041.93938  Mass, 10^24 kg        = ~1988410
              Vol. mean radius, km  = 695700              Volume, 10^12 km^3    = 1412000
              Solar radius (IAU2015)= 695700 km           Mean density, g/cm^3  = 1.408
              Radius (photosphere)  = 696500 km           Angular diam at 1 AU  = 1919.3"
              Photosphere temp., K  = 6600 (bottom)       Photosphere temp., K  = 4400(top)
              Photospheric depth    = ~500 km             Chromospheric depth   = ~2500 km
              Flatness, f           = 0.00005             Adopted sid. rot. per.= 25.38 d
              Surface gravity       =  274.0 m/s^2        Escape speed, km/s    =  617.7
              Pole (RA,DEC), deg.   = (286.13, 63.87)     Obliquity to ecliptic = 7.25 deg.
              Solar constant (1 AU) = 1367.6 W/m^2        Luminosity, 10^24 J/s = 382.8
              Mass-energy conv rate = 4.260 x 10^9 kg/s   Effective temp, K     = 5772
              Sunspot cycle         = 11.4 yr             Cycle 24 sunspot min. = 2008 A.D.

              Motion relative to nearby stars = apex : R.A.= 271 deg.; DEC.= +30 deg.
                                                speed: 19.4 km/s (0.0112 au/day)
              Motion relative to 2.73K BB/CBR = apex : l= 264.7 +- 0.8; b= 48.2 +- 0.5 deg.
                                                speed: 369 +-11 km/s
            *******************************************************************************


            *******************************************************************************
            Ephemeris / API_USER Wed Aug 27 10:45:22 2025 Pasadena, USA      / Horizons
            *******************************************************************************
            Target body name: Sun (10)                        {source: DE441}
            Center body name: Sun (10)                        {source: DE441}
            Center-site name: BODY CENTER
            *******************************************************************************
            Start time      : A.D. 2025-Aug-27 00:00:00.0000 TDB
            Stop  time      : A.D. 2025-Sep-06 00:00:00.0000 TDB
            Step-size       : 1440 minutes
            
            """;
        private string venus = """
PI VERSION: 1.2
API SOURCE: NASA / JPL Horizons API

*******************************************************************************
 Revised: April 12, 2021                Venus                           299 / 2


 PHYSICAL DATA(updated 2020 - Oct - 19):
  Vol.Mean Radius(km) = 6051.84 + -0.01 Density(g / cm ^ 3) = 5.204
  Mass x10^23(kg) = 48.685      Volume(x10 ^ 10 km ^ 3) = 92.843
  Sidereal rot. period = 243.018484 d Sid. Rot.Rate(rad / s) = -0.00000029924
  Mean solar day = 116.7490 d Equ. gravity m/ s ^ 2 = 8.870
  Mom.of Inertia = 0.33       Core radius(km)      = ~3200
  Geometric Albedo = 0.65       Potential Love # k2   = ~0.25
  GM(km ^ 3 / s ^ 2) = 324858.592     Equatorial Radius, Re = 6051.893 km
  GM 1 - sigma(km ^ 3 / s ^ 2) = +-0.006     Mass ratio(Sun/ Venus)= 408523.72
  Atmos.pressure(bar) = 90            Max.angular diam.    = 60.2"
  Mean Temperature(K)  = 735            Visual mag. V(1, 0) = -4.40
  Obliquity to orbit = 177.3 deg Hill's sphere rad.,Rp =  167.1
  Sidereal orb. per., y = 0.61519726   Orbit speed, km/ s = 35.021
  Sidereal orb. per., d = 224.70079922   Escape speed, km/ s = 10.361
                                 Perihelion Aphelion    Mean
  Solar Constant(W / m ^ 2)         2759         2614       2650
  Maximum Planetary IR(W / m ^ 2)    153         153         153
  Minimum Planetary IR(W / m ^ 2)    153         153         153
* ******************************************************************************

PI VERSION: 1.2
API SOURCE: NASA / JPL Horizons API

*******************************************************************************
 Revised: April 12, 2021                Venus                           299 / 2


 PHYSICAL DATA(updated 2020 - Oct - 19):
  Vol.Mean Radius(km) = 6051.84 + -0.01 Density(g / cm ^ 3) = 5.204
  Mass x10^23(kg) = 48.685      Volume(x10 ^ 10 km ^ 3) = 92.843
  Sidereal rot. period = 243.018484 d Sid. Rot.Rate(rad / s) = -0.00000029924
  Mean solar day = 116.7490 d Equ. gravity m/ s ^ 2 = 8.870
  Mom.of Inertia = 0.33       Core radius(km)      = ~3200
  Geometric Albedo = 0.65       Potential Love # k2   = ~0.25
  GM(km ^ 3 / s ^ 2) = 324858.592     Equatorial Radius, Re = 6051.893 km
  GM 1 - sigma(km ^ 3 / s ^ 2) = +-0.006     Mass ratio(Sun/ Venus)= 408523.72
  Atmos.pressure(bar) = 90            Max.angular diam.    = 60.2"
  Mean Temperature(K)  = 735            Visual mag. V(1, 0) = -4.40
  Obliquity to orbit = 177.3 deg Hill's sphere rad.,Rp =  167.1
  Sidereal orb. per., y = 0.61519726   Orbit speed, km/ s = 35.021
  Sidereal orb. per., d = 224.70079922   Escape speed, km/ s = 10.361
                                 Perihelion Aphelion    Mean
  Solar Constant(W / m ^ 2)         2759         2614       2650
  Maximum Planetary IR(W / m ^ 2)    153         153         153
  Minimum Planetary IR(W / m ^ 2)    153         153         153
* ******************************************************************************

""";
        private string earth = """
                        *******************************************************************************
             Revised: April 12, 2021                 Earth                              399

             GEOPHYSICAL PROPERTIES (revised May 9, 2022):
              Vol. Mean Radius (km)    = 6371.01+-0.02   Mass x10^24 (kg)= 5.97219+-0.0006
              Equ. radius, km          = 6378.137        Mass layers:
              Polar axis, km           = 6356.752          Atmos         = 5.1   x 10^18 kg
              Flattening               = 1/298.257223563   oceans        = 1.4   x 10^21 kg
              Density, g/cm^3          = 5.51              crust         = 2.6   x 10^22 kg
              J2 (IERS 2010)           = 0.00108262545     mantle        = 4.043 x 10^24 kg
              g_p, m/s^2  (polar)      = 9.8321863685      outer core    = 1.835 x 10^24 kg
              g_e, m/s^2  (equatorial) = 9.7803267715      inner core    = 9.675 x 10^22 kg
              g_o, m/s^2               = 9.82022         Fluid core rad  = 3480 km
              GM, km^3/s^2             = 398600.435436   Inner core rad  = 1215 km
              GM 1-sigma, km^3/s^2     =      0.0014     Escape velocity = 11.186 km/s
              Rot. Rate (rad/s)        = 0.00007292115   Surface area:
              Mean sidereal day, hr    = 23.9344695944     land          = 1.48 x 10^8 km
              Mean solar day 2000.0, s = 86400.002         sea           = 3.62 x 10^8 km
              Mean solar day 1820.0, s = 86400.0         Love no., k2    = 0.299
              Moment of inertia        = 0.3308          Atm. pressure   = 1.0 bar
              Mean surface temp (Ts), K= 287.6           Volume, km^3    = 1.08321 x 10^12
              Mean effect. temp (Te), K= 255             Magnetic moment = 0.61 gauss Rp^3
              Geometric albedo         = 0.367           Vis. mag. V(1,0)= -3.86
              Solar Constant (W/m^2)   = 1367.6 (mean), 1414 (perihelion), 1322 (aphelion)
             HELIOCENTRIC ORBIT CHARACTERISTICS:
              Obliquity to orbit, deg  = 23.4392911  Sidereal orb period  = 1.0000174 y
              Orbital speed, km/s      = 29.79       Sidereal orb period  = 365.25636 d
              Mean daily motion, deg/d = 0.9856474   Hill's sphere radius = 234.9       
            *******************************************************************************

            
            """;
        private string mars = """
                        *******************************************************************************
             Revised: June 02, 2025                 Mars                            499 / 4

             PHYSICAL DATA (updated 2025-Jun-02):
              Vol. mean radius (km) = 3389.92+-0.04   Density (g/cm^3)      =  3.933(5+-4)
              Mass x10^23 (kg)      =    6.4171       Flattening, f         =  1/169.779
              Volume (x10^10 km^3)  =   16.318        Equatorial radius (km)=  3396.19
              Sidereal rot. period  =   24.622962 hr  Sid. rot. rate, rad/s =  0.0000708822 
              Mean solar day (sol)  =   88775.24415 s Polar gravity m/s^2   =  3.758
              Core radius (km)      = ~1700           Equ. gravity  m/s^2   =  3.71
              Geometric Albedo      =    0.150                                              

              GM (km^3/s^2)         = 42828.375662    Mass ratio (Sun/Mars) = 3098703.59
              GM 1-sigma (km^3/s^2) = +- 0.00028      Mass of atmosphere, kg= ~ 2.5 x 10^16
              Mean temperature (K)  =  210            Atmos. pressure (bar) =    0.0056 
              Obliquity to orbit    =   25.19 deg     Max. angular diam.    =  17.9"
              Mean sidereal orb per =    1.88081578 y Visual mag. V(1,0)    =  -1.52
              Mean sidereal orb per =  686.98 d       Orbital speed,  km/s  =  24.13
              Hill's sphere rad. Rp =  319.8          Escape speed, km/s    =   5.027
                                             Perihelion  Aphelion    Mean
              Solar Constant (W/m^2)         717         493         589
              Maximum Planetary IR (W/m^2)   470         315         390
              Minimum Planetary IR (W/m^2)    30          30          30
            *******************************************************************************


            
            """;
        private string jupiter = """
                        *******************************************************************************
             Revised: April 12, 2021               Jupiter                              599

             PHYSICAL DATA (revised 2025-Jan-30):
              Mass x 10^26 (kg)     = 18.9819           Density (g/cm^3)  = 1.3262 +- .0003
              Equat. radius (1 bar) = 71492+-4 km       Polar radius (km)     = 66854+-10
              Vol. Mean Radius (km) = 69911+-6          Flattening            = 0.06487
              Geometric Albedo      = 0.52              Rocky core mass (Mc/M)= 0.0261
              Sid. rot. period (III)= 9h 55m 29.711 s   Sid. rot. rate (rad/s)= 0.00017585
              Mean solar day, hrs   = ~9.9259         
              GM (km^3/s^2)         = 126686531.900     GM 1-sigma (km^3/s^2) =  +- 1.2732
              Equ. grav, ge (m/s^2) = 24.79             Pol. grav, gp (m/s^2) =  28.34
              Vis. magnitude V(1,0) = -9.40
              Vis. mag. (opposition)= -2.70             Obliquity to orbit    =  3.13 deg
              Sidereal orbit period = 11.861982204 y    Sidereal orbit period = 4332.589 d
              Mean daily motion     = 0.0831294 deg/d   Mean orbit speed, km/s= 13.0697
              Atmos. temp. (1 bar)  = 165+-5 K          Escape speed, km/s    =  59.5           
              A_roche(ice)/Rp       =  2.76             Hill's sphere rad. Rp = 740
                                             Perihelion   Aphelion     Mean
              Solar Constant (W/m^2)         56           46           51
              Maximum Planetary IR (W/m^2)   13.7         13.4         13.6
              Minimum Planetary IR (W/m^2)   13.7         13.4         13.6
            *******************************************************************************


            
            """;
        private string europa = """
                        *******************************************************************************
             Revised: Mar 12, 2021           Europa / (Jupiter)                         502

             SATELLITE PHYSICAL PROPERTIES:
              Mean radius (km)       = 1560.8   +- 0.3    Density (g cm^-3)= 3.013 +- 0.005
              GM   (km^3/s^2)        = 3202.7121+- 0.0054 Geometric Albedo = 0.67 +/- 0.03  

             SATELLITE ORBIT (at J2000.0 epoch, 2000-Jan-1.5):
              Semi-major axis, a (km) ~ 671,000           Orbital period   ~ 3.55 d
              Eccentricity, e         ~ 0.00981        Rotational period   = Synchronous
              Inclination, i  (deg)   ~ 0.462
            *******************************************************************************

            
            """;
        private string jamesWebb = """
TELESCOPE
 * total launch mass : ~6200 kg (observatory, fuel, launch adaptor)
 * primary mirror    : 25 m^2 
           mass              : 705 kg
               material          : beryllium coated w/48.25 grams gold (golf-ball size)
         segment mass      : 20.1 kg, 39.48 kg for entire segment assembly
   No. of segments   : 18
 * focal length      : 131.4 meters
 * optical resolution: 0.1 arcseconds
 * wavelength        : 0.6 - 28.5 microns
 * size of sun shield: 21.197 m x 14.162 m
 * Sun shield layers : 1: Max temp 283K, 231 deg. F.
                      5: Max temp 221K,  -80 F 
                         Min temp 36K,  -394 F 
 * Operating temp    : < 50K (-370 deg. F)
""";


        [Fact]
        public void FindMass_Sun()
        {
            string input = sun;
            double expected = 1988410 * Math.Pow(10, 24);
            double? actual = ObjectReader._findMass(input);
            Assert.NotNull(actual);
            Assert.InRange(actual.Value, expected / 10, expected * 10);

        }

        [Fact]
        public void FindMass_Venus()
        {
            string input = venus;
            double expected = 48.685 * Math.Pow(10, 23);
            double? actual = ObjectReader._findMass(input);
            Assert.NotNull(actual);
            Assert.InRange(actual.Value, expected/10, expected*10);
            
        }

        [Fact]
        public void FindMass_Earth()
        {
            string input = earth;
            double expected = 5.97219 * Math.Pow(10, 24);
            double? actual = ObjectReader._findMass(input);
            Assert.NotNull(actual);
            Assert.InRange(actual.Value, expected / 10, expected * 10);

        }
        [Fact]
        public void FindMass_Mars()
        {
            string input = mars;
            double expected = 6.4171 * Math.Pow(10, 23);
            double? actual = ObjectReader._findMass(input);
            Assert.NotNull(actual);
            Assert.InRange(actual.Value, expected / 10, expected * 10);

        }
        [Fact]
        public void FindMass_Jupiter()
        {
            string input = jupiter;
            double expected = 18.9819 * Math.Pow(10, 26);
            double? actual = ObjectReader._findMass(input);
            Assert.NotNull(actual);
            Assert.InRange(actual.Value, expected / 10, expected * 10);

        }
        [Fact]
        public void FindMass_Europa()
        {
            string input = europa;
            double expected = Math.Pow(10, 22);
            double? actual = ObjectReader._findMass(input);
            Assert.NotNull(actual);
            Assert.InRange(actual.Value, expected / 10, expected * 10);

        }
        [Fact]
        public void FindMass_Webb() 
        {
            string input = jamesWebb;
            double expected = 705;
            double? actual = ObjectReader._findMass(input);
            Assert.NotNull(actual);
            Assert.InRange(actual.Value, expected -5 , expected + 5);
        }

        [Fact]
        public void FindRadius_Sun()
        {
            string input = sun;
            double expected = 695700;
            double? actual = ObjectReader._findRadius(input);
            Assert.NotNull(actual);
            Assert.InRange(actual.Value, expected - 10, expected + 10);
        }

        [Fact]
        public void FindRadius_Venus()
        {
            string input = venus;
            double expected = 6051.84;
            double? actual = ObjectReader._findRadius(input);
            Assert.NotNull(actual);
            Assert.InRange(actual.Value, expected - 10, expected + 10);
        }

        [Fact]
        public void FindRadius_Earth()
        {
            string input = earth;
            double expected = 6371.01;
            double? actual = ObjectReader._findRadius(input);
            Assert.NotNull(actual);
            Assert.InRange(actual.Value, expected - 10, expected + 10);
        }

        [Fact]
        public void FindRadius_Jupiter()
        {
            string input = jupiter;
            double expected = 69911;
            double? actual = ObjectReader._findRadius(input);
            Assert.NotNull(actual);
            Assert.InRange(actual.Value, expected - 10, expected + 10);
        }
        [Fact]
        public void FindRadius_Europa()
        {
            string input = europa;
            double expected = 1560.8;
            double? actual = ObjectReader._findRadius(input);
            Assert.NotNull(actual);
            Assert.InRange(actual.Value, expected - 10, expected + 10);
        }

        [Fact]
        public void FindDensity_Sun()
        {
            string input = sun;
            double expected = 1.408;
            double? actual = ObjectReader._findDenisity(input);
            Assert.NotNull(actual);
            Assert.InRange(actual.Value, expected/10, expected*10);
        }
        [Fact]
        public void FindDensity_Venus()
        {
            string input = venus;
            double expected = 5.204;
            double? actual = ObjectReader._findDenisity(input);
            Assert.NotNull(actual);
            Assert.InRange(actual.Value, expected / 10, expected * 10);
        }
        [Fact]
        public void FindDensity_Earth()
        {
            string input = earth;
            double expected = 5.51;
            double? actual = ObjectReader._findDenisity(input);
            Assert.NotNull(actual);
            Assert.InRange(actual.Value, expected / 10, expected * 10);
        }
        [Fact]
        public void FindDensity_Mars()
        {
            string input = mars;
            double expected = 3.933;
            double? actual = ObjectReader._findDenisity(input);
            Assert.NotNull(actual);
            Assert.InRange(actual.Value, expected / 10, expected * 10);
        }
        [Fact]
        public void FindDensity_Jupiter()
        {
            string input = jupiter;
            double expected = 1.3262;
            double? actual = ObjectReader._findDenisity(input);
            Assert.NotNull(actual);
            Assert.InRange(actual.Value, expected / 10, expected * 10);
        }
        [Fact]
        public void FindDensity_Europa()
        {
            string input = europa;
            double expected = 3.013;
            double? actual = ObjectReader._findDenisity(input);
            Assert.NotNull(actual);
            Assert.InRange(actual.Value, expected / 10, expected * 10);
        }
        [Fact]
        public void FindRotationPeriod_Sun()
        {
            string input = sun;
            double expected = 25.38;
            double? actual = ObjectReader._findRotationPeriod(input);
            Assert.NotNull(actual);
            Assert.InRange(actual.Value, expected - 5, expected + 10);
        }
        [Fact]
        public void FindRotationPeriod_Venus()
        {
            string input = venus;
            double expected = 243.018484;
            double? actual = ObjectReader._findRotationPeriod(input);
            Assert.NotNull(actual);
            Assert.InRange(actual.Value, expected - 5, expected + 10);
        }
        [Fact]
        public void FindRotationPeriod_Earth()
        {
            string input = earth;
            double expected = 1;
            double? actual = ObjectReader._findRotationPeriod(input);
            Assert.NotNull(actual);
            Assert.InRange(actual.Value, expected/2, expected * 2);
        }
        [Fact]
        public void FindRotationPeriod_Mars()
        {
            string input = mars;
            double expected = 1;
            double? actual = ObjectReader._findRotationPeriod(input);
            Assert.NotNull(actual);
            Assert.InRange(actual.Value, expected / 2, expected * 2);
        }
        [Fact]
        public void FindRotationPeriod_Jupiter()
        {
            string input = jupiter;
            double expected = 0.4135382;
            double? actual = ObjectReader._findRotationPeriod(input);
            Assert.NotNull(actual);
            Assert.InRange(actual.Value, 0.3, 0.5);
        }
        [Fact]
        public void FindRotationPeriod_Europa()
        {
            string input = europa;
            double expected = -1;
            double? actual = ObjectReader._findRotationPeriod(input);
            Assert.NotNull(actual);
            Assert.InRange(actual.Value, -1.1, -0.9);
        }

        [Fact]
        public void FindGravity_Sun()
        {
            string input = sun;
            double expected = 274.0;
            double? actual = ObjectReader._findGravity(input);
            Assert.NotNull(actual);
            Assert.InRange(actual.Value, expected-5, expected+5);
        }
        [Fact]
        public void FindGravity_Venus()
        {
            string input = venus;
            double expected = 8.870;
            double? actual = ObjectReader._findGravity(input);
            Assert.NotNull(actual);
            Assert.InRange(actual.Value, expected - 1, expected + 1);
        }
        [Fact]
        public void FindGravity_Earth()
        {
            string input = earth;
            double expected = 9.7803267715;
            double? actual = ObjectReader._findGravity(input);
            Assert.NotNull(actual);
            Assert.InRange(actual.Value, expected - 1, expected + 1);
        }
        [Fact]
        public void FindGravity_Jupiter()
        {
            string input = jupiter;
            double expected = 24.7;
            double? actual = ObjectReader._findGravity(input);
            Assert.NotNull(actual);
            Assert.InRange(actual.Value, expected - 1, expected + 1);
        }
        [Fact]
        public void FindGravity_Europa()
        {
            string input = europa;
            double expected = 1.3;
            double? actual = ObjectReader._findGravity(input);
            Assert.NotNull(actual);
            Assert.InRange(actual.Value, expected - 1, expected + 1);
        }
        [Fact]
        public void FindOrbitalPeriod_Venus()
        {
            string input = venus;
            double expected = 0.61519726;
            double? actual = ObjectReader._findOrbitalPeriod(input);
            Assert.NotNull(actual);
            Assert.InRange(actual.Value, 0.5, 0.7);
        }
        [Fact]
        public void FindOrbitalPeriod_Earth()
        {
            string input = earth;
            double expected = 1;
            double? actual = ObjectReader._findOrbitalPeriod(input);
            Assert.NotNull(actual);
            Assert.InRange(actual.Value, 0.9, 1.1);
        }
        [Fact]
        public void FindOrbitalPeriod_Mars()
        {
            string input = mars;
            double expected = 1.88;
            double? actual = ObjectReader._findOrbitalPeriod(input);
            Assert.NotNull(actual);
            Assert.InRange(actual.Value, 1.8, 1.9);
        }
        [Fact]
        public void FindOrbitalPeriod_Jupiter()
        {
            string input = jupiter;
            double expected = 11.86;
            double? actual = ObjectReader._findOrbitalPeriod(input);
            Assert.NotNull(actual);
            Assert.InRange(actual.Value, 11.8, 11.9);
        }
        [Fact]
        public void FindOrbitalPeriod_Europa()
        {
            string input = europa;
            double expected = 0.0097;
            double? actual = ObjectReader._findOrbitalPeriod(input);
            Assert.NotNull(actual);
            Assert.InRange(actual.Value, 0.001, 0.01);
        }

        [Fact]
        public void FindPressure_Venus()
        {
            string input = venus;
            double expected = 90;
            double? actual = ObjectReader._findPressure(input);
            Assert.NotNull(actual);
            Assert.InRange(actual.Value, 89, 91);
        }
        [Fact]
        public void FindPressure_Earth()
        {
            string input = earth;
            double expected = 1;
            double? actual = ObjectReader._findPressure(input);
            Assert.NotNull(actual);
            Assert.InRange(actual.Value, 0.9, 1.2);
        }
        [Fact]
        public void FindPressure_Mars()
        {
            string input = mars;
            double expected = 0.0056;
            double? actual = ObjectReader._findPressure(input);
            Assert.NotNull(actual);
            Assert.InRange(actual.Value, 0.005, 0.006);
        }
        [Fact]
        public void FindTemperature_Sun()
        {
            string input = sun;
            double expected = 5772;
            double? actual = ObjectReader._findTemperature(input);
            Assert.NotNull(actual);
            Assert.InRange(actual.Value, expected*0.9, expected * 1.1);
        }
        [Fact]
        public void FindTemperature_Venus()
        {
            string input = venus;
            double expected = 735;
            double? actual = ObjectReader._findTemperature(input);
            Assert.NotNull(actual);
            Assert.InRange(actual.Value, expected * 0.9, expected * 1.1);
        }
        [Fact]
        public void FindTemperature_Earth()
        {
            string input = earth;
            double expected = 287.6;
            double? actual = ObjectReader._findTemperature(input);
            Assert.NotNull(actual);
            Assert.InRange(actual.Value, expected * 0.9, expected * 1.1);
        }
        [Fact]
        public void FindTemperature_Mars() //You can't just shoot a hole into the surface of Mars...
        {
            string input = mars;
            double expected = 210;
            double? actual = ObjectReader._findTemperature(input);
            Assert.NotNull(actual);
            Assert.InRange(actual.Value, expected * 0.9, expected * 1.1);
        }
        [Fact]
        public void FindTemperature_Jupiter()
        {
            string input = jupiter;
            double expected = 165;
            double? actual = ObjectReader._findTemperature(input);
            Assert.NotNull(actual);
            Assert.InRange(actual.Value, expected * 0.9, expected * 1.1);
        }
    }
    
}