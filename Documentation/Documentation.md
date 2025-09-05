# Uživatelská část
Aplikace vyžaduje pro své fungování připojení k internetu, aby mohla aplikace získat informace o efemeridách z NASA Horizons API (dále jen API).

## Spuštění

## Typy map
K dispozici jsou tři tpy map - mapa sluneční soustavy, mapa noční oblohy a mapa soustavy měsíců pro vybraná tělesa. Po spuštění uživatel má přístup pouze k mapě sluneční soustavy a mapě noční oblohy -  z těchto map se lze dostat na měsíční mapu kliknutím na dané těleso, jehož měsíční soustava uživatele zajímá.

## Nastavení mapy
Na první obrazovce je uživateli dána možnost vybrat typ mapy, kterou chce uživatel zobrazit. Lze zvolit buď Night sky, pro mapu noční oblohy, nebo Solar system, pro mapu sluneční soustavy.
Uživatel dále musí zadat datum, pro které chce zobrazit mapu.
Pokud uživatel zvolil mapu noční oblohy, musí ještě zadat souřadnice na Zemi, pro které chce mapu zobrazit. Souřadnice musí být zadány ve tvaru "šířka,délka", kde čísla šířka a délka musí být zadány v double formátu, oddělené čáskou, s desetinou tečkou.

Dále uživatel může vybrat, které typy těles chce zobrazit. Tyto typy jsou: Stars (v této skupině je jen Slunce), Terrestrail Planet (kamenné planety), Gas Giant (plynné planety), Dwarf Planet (vybrané největší trpasličí planetyú, Asteroid (vybrané asteroidy), Comet (vybrané komety), Spacecraft (umělé družice) a Moon (toto nastavení neovlivňuje mapy měsíčních soustav - je zde pro vypnutí/zapnutí zobrazení polohy měsíce na mapě noční oblohy).

Stisknutím tlačítka "Next page" se uživatel přesune na další okno pro nastavení mapy. Po přesunutí se nelze vrátit zpět.

### Obecné filtry
Uživatel může zadat obecné filtry pro tělesa, resp. maximální a minimální hodnoty pro hmotnost, poloměr, periodu oběhu, tíhové zrychlení a hustotu. Navíc též může nastavit filtr na vzdálenost od středu a rychlost tělesa, ale to jen pro mapu slunečního systému. Minima jsou v základu nastavena na 0 a maxima na "Infinity", tedy maximální hodnota je neomezená. Vstup musí být převoditelný na double.

Filtry se neaplikují na měsíční mapy.

Poznámka: tíhové zrychlení a gravitační zrychlení jsou v této aplikaci zaměňovány, jelikož API je udává souhrně jako "Gravity" (v angličtině neexitují pro tyto koncepty oddělené termíny). Obecně platí, že u větších těles byl vybírán parametr "Equatorial gravity", u menších těles byl vybírán parametr "Gravity", či byl tento parametr vypočten z hmotnosti tělesa.

V neposlední řadě uživatel může nastavit tzv. white list, kam může napsat názvy těles, které chce, aby se zobrazily, i kdyby neprošly filtry. Též může nastavit black list. Ve white/black list musí být názvy oddělené čárkou.

Pokud pro nějaký parametr se nepodařilo získat hodnotu, dané těleso filtr automaticky splní.

### Filtry pro jednotlivé typy těles
Pro každyý typ těles může uživatel nastavit další filtry. Tyto filtry budou aplikovány po obecných filtrech. Uživatel může, kromě již výše zmíněných vlastností jako jsou hmotnost, poloměr, perioda oběhu, tíhové zrychlení a hustota, nastavit též, že chce pouze několik těles s největší/nejmenší hodnotou dané vlastnosti. Uživatel dále může odfiltrovat tělesa daného typu, u kterých se nepodařilo ze serveru získat nějakou informaci o vlastnostech tělesa (možnost FIlter NaN Values).

## Mapy
### Společné ovládací prvky
Stiskem tlačítka ESC se aplikace vypne.
Stiskem tlačítka TAB se otevře ovládací panel.

Jednotlivá tělesa jsou na mapě zobrazené jako barevné kruhy. Pokud uživatel stiskne některé těleso, zobrazí se uživateli v levém dolním rohu panel s údaji o daném tělese - údaje jsou získány přímo od API. Údaje které se nepodaří získat se ve nezobrazí, nebo se místo jejich hodnoty vypíše NaN.
Některá tělesa mají pod sebou vypsaný jejich název - toto automaticky platí pro Slunce a planety (tělesa s typem Terrestrial Planet nebo Gas Giant). Na informačním panelu je tlačítko Track/Untrack, kterým lze zapnout/vypnout zobrazování jména daného tělesa.
U některých těles je též tlačítko "Moon View", kterými lze zobrazit mapu měsíční soustavy daného tělesa, viz sekce "Měsíční mapa".

Pokud se na mapě vypíše nápis "Loading..." znamená to, že aplikace nahrává data ze serveru.

#### Ovládací panel
Vzhled ovládacího panelu se liší mezi různými typy map, ale pro všechny mapy má tento panel společné následující funkce:
- výpis data, pro které mapa vykresluje pozici objektů
- tlačítko ">>", kterým se zobrazí mapa pro další den
- tlačítko "Start", kterým lze zapnout automatické přepínání na následující dny. Je-li toto tlačítko zapnuto, mapa se bude aktualizovat každou sekundu a bude postupně den po dni vykreslovat pozice objektů. Toto přepínání lze vypnout tlačítkem "Stop"

### Noční obloha
Tento typ mapy pro dané datum a souřadnice zobrazuje efemeridy na zenitové mapě noční oblohy. Mapa je zobrazena jako kruh, jehož střed je zenit, po jehož okrajích jsou vyznačeny kardinální směry spolu s hodnotami azimutu.
Velikost zobrazených těles je fixní a neodpovídá realitě.

### Sluneční soustava
Tento typ mapy pro dané datum zobrazuje 2D mapu sluneční soustavy a to tak, že Slunce je ve středu souřadné soustavy (ve středu mapy), x-ová osa roste směrem k dolnímu okraji obrazovky, y-lonová osa roste směrem k pravému okraji obrazovky.
Velikost zobrazených těles je fixní a neodpovídá realitě.

Kontrolní panel má na této mapě navíc tlčítka "Zoom In" a "Zoom Out", kterými lze měnit měřítko mapy. Měřítko je dále vypsáno na ovládací panel - jde o číslo, které předsavuje, kolik kilometrů představuje jeden pixel. Tlačítko "Zoom In" měřítko zvětšuje (zdvojnásobuje), tlačítko "Zoom Out" měřítko zmenšuje (po stisknutí je měřítko poloviční).

### Měsíční mapa
Ovládá se stejným způsobem jako mapa sluneční soustavy, s tím rozdílem, že kontrolní panel obsahuje tlačítko "Back", které uživatele vrází na původní mapu (buď mapu sluneční soustavy, nebo noční oblohy).
Tato mapa lze zobrazit jen pro vybraná tělesa a to: Země, Mars, Jupiter, Saturn, Uran, Neptun, Pluto. Tato tělesa pak jsou umístěna do středu mapy.
Velikosti těles odpovídají měřítku, ovšem pokud by velikost zobrazovaného tělesa byla příliž malá, je jejich velikost na obrazovce nastavena na na fixní velikost.

Na mapě se automaticky zobrazí i poloha Země a Slunce, aby uživatel měl referenční body, pro určení polohy. Na okrajích obrazovky se zobrazují šipky udávající, kterým směrem se Slunce a Země nachází. Pokud je měřítko dostatrčné, Země i Slunce jsou vidět jako objekty na Mapě.

# Programátorská část

## Struktura kódu
Kód je rozdělen do dvou namespace - SolarSystemMapper, který obsahuje hlavně třídy pro datovou reprezentaci a fetch dat, a SolarMapperUI - který je vstupním bodem programu a který se stará o UI stránku aplikace.

### Reprezentace dat
Data posílaná od API pro konkrétní nebezké těleso mají následující tvar: nejprve jsou udány vlastnosti tělesa (hmotnost, poloměr, název, gravitační zrychlení apod. - konkrétní vlastnosti jsou různé mezi různými typy těles), po kterém následuje tabulka souřadnic - nebezkých nebo kartézských. Každý řádek tabulky udává souřadnice pro konkrétní čas. Jednotlivé časy se liší o předem daný časový rozdíl (náš program fixně vyžaduje rozdíl jednoho dne).
Data leze buď získat jako plain text, nebo jako json. Ovšem, textová a jsonové podoby dat jsou prakticky identické, json jen odděluje číslo verze API, a zbytek dat udává jako jeden řetězec. Navíc, pouze tabulka poloh je udávána v konzistentním formátu.

Následující třídy jsou součástí namespace SolarSystemMapper.

První část dat, která obsahuje vlastnosti objektu, je v kódu reprezenována recordem

```c#
public record ObjectData(string Name, int Code, string Type, double Radius_km = double.NaN, double Density_gpcm3 = double.NaN, double Mass_kg = double.NaN, 
    double RotationPeriod_hr = double.NaN, double EquatorialGravity_mps2 = double.NaN,
    double Temperature_K = double.NaN, double Pressure_bar = double.NaN, double OrbitalPeriod_y = double.NaN)
```
Tento record přepisuje metodu ToString - výsledný string je pro uživatele čitelnější a neobsahuje NaN hodnoty.

Dále uveďme interface
```c#
public interface IEphemerisTableRow
{
  public DateTime? date { get; }
  static TRow stringToRow<TRow>(string data) where TRow : IEphemerisTableRow
  static double? TryParseNullable(string input)
  static double[]? TryParseTriple(string[] tokens, int start)
}
```
jehož implementace představují řádky tabulky souřadnic. Metoda stringToRow slouží k převodu řetězců na danou implementaci. Metody TryParseNullable a TryParseTriple slouží pouze k zjednodušení převodu řetězce na řádek tabulky.

Program používá dvě implementace tohoto interface:
```c#
public record EphemerisTableRowObserver(DateTime? date, double[]? RA, double[]? DEC, double? dRA_dt, double? dDEC_dt, double? Azi, double? Elev) : IEphemerisTableRow
```
- slouží pro data o pozici objektu na noční obloze

```c#
public record EphemerisTableRowVector(DateTime? date = null, double? X = null, double? Y = null, double? Z = null, double? VX = null, double? VY = null, double? VZ = null, double? LightTime = null, double? Range = null, double? RangeRate = null) : IEphemerisTableRow  
```
- slouží pro data o kartézských souřadnicích objektu

Dalším důležitým interface je
```c#
public interface IEphemerisData<out TRow> where TRow : IEphemerisTableRow
{
    public IReadOnlyList<TRow> ephemerisTable { get; }
    public ObjectData objectData { get; }
}
```
V tomto interface ephemerisTable reprezentuje tabulku poloh tělesa a objectData vlastnosti tělesa.

Toto interface implementují třídy
```c#
public record EphemerisObserverData(IReadOnlyList<EphemerisTableRowObserver> ephemerisTable, ObjectData objectData) : IEphemerisData<EphemerisTableRowObserver>
public record EphemerisVectorData(IReadOnlyList<EphemerisTableRowVector> ephemerisTable, ObjectData objectData) : IEphemerisData<EphemerisTableRowVector>
```
První třída slouží pro zobrazení noční oblohy, druhá pro zobrazení mapy sluneční soustavy a měsíců.

#### Reprezentace dat v UI
Následující třídy jsou součástí namespace SolarMapperUI.

Pozici těles na obrazovce v daný okamžik
```c#
internal class PixelBodyInfo
{ 
    public PixelBodyInfo(Point bodyCoordinates, Point centerCoordinates, bool visible, int diameter, Color color, bool showName)
    {
        BodyCoordinates = bodyCoordinates;
        CenterCoordinates = centerCoordinates;
        Visible = visible;
        Diameter = diameter;
        Color = color;
        ShowName = showName;
    }

    public Point BodyCoordinates { get; }
    public Point CenterCoordinates { get; }
    public bool Visible { get; set; }
    public int Diameter { get; set; }
    public Color Color { get; set; }
    public bool ShowName { get; set; }
}
```
K datům o objektech je v UI přistupováno přes třídu a interface
```c#
internal interface IFormBody<out TData> where TData : IEphemerisData<IEphemerisTableRow>
{
    TData BodyData { get; }
    public List<PixelBodyInfo> PixelInfos { get; }
    void SetNameVisibility(bool visibility);
    string BodyReport(DateTime date);
    void SwitchNameVisibility();
    void ChangePixelInfos(List<PixelBodyInfo> pixelInfos);

}

internal class FormBody<TData> : IFormBody<TData>
    where TData : IEphemerisData<IEphemerisTableRow>
{
    public TData BodyData { get; init; }
    public List<PixelBodyInfo> PixelInfos { get; private set; }

    public FormBody(TData bodyData, List<PixelBodyInfo> pixelInfo)
    {
        BodyData = bodyData;
        PixelInfos = pixelInfo;
    }
    public string BodyReport(DateTime date);
    public void SetNameVisibility(bool visibility);
    public void SwitchNameVisibility();
    public void ChangePixelInfos(List<PixelBodyInfo> pixelInfos);
}
```
Následující metody jsou metody statické třídy
```c#
internal static class Translation
```

Instance tříd EphemerisVectorData a EphemerisObserverData lze převést do instancí třídy FormBody pomocí extenčních metod
```c#
internal static FormBody<EphemerisObserverData> ToFormBody(this EphemerisObserverData observerData,Point center,int mapRadius)
internal static FormBody<EphemerisVectorData> ToFormBody(this EphemerisVectorData vectorData, Point center, float scale_Km, int mapHeight, int mapWidth, bool respectScale = false)
```
Parametr center představuje souřadnice středového pixelu obrazovky, mapRadius určuje poloměr mapy noční oblohy, scale_Km určuje, kolik kilometrů na mapě sluneční soustavy/mapě měsíců představuje jeden pixel, mapHeight představuje výšku mapy, mapWidth představuje šířku mapy a respectScale udává, zda velikosti těles mají na mapě respektovat měřítko.

Tyto metody využívají pro převod instancí tříd EphemerisVectorData a EphemerisObserverData na PixelBodyInfo metody
```c#
internal static PixelBodyInfo ToPixelBodyInfo(this EphemerisTableRowObserver row, Point center, int mapRadius, string bodyType, string bodyName);
internal static PixelBodyInfo ToPixelBodyInfo(this EphemerisTableRowVector row, Point center, float scale_Km, string bodyType, string bodyName);
```
Barva obejektu na mapách je určena metodou
```c#
private static Color _getColor(string name)
```
která určuje barvu podle názvu objektu.
Poloměr tělesa na mapě je určen metodou
```c#
private static int _getDiameter(string type);
```

### Fetchování dat
Dotaz na API musí obsahovat číselný kód tělesa, který nás zajímá, a číselný kód tělesa ve středu souřadné soustavy.
Jelikož NASA Horizons API neposkytuje rozumný seznam kódů spolu s jmény objektů (poskytuje je jen přes telnet), je potřeba tyto informace mít uložené lokálně. V kódu tyto informace uchovává record ObjectEntry

```c#
public record ObjectEntry(string Name, int Code, string Type);
```
kde položka Name a Type jsou přítomny jen pro zjednudušení zpracování dat aplikací a nejsou třeba pro komunikaci s API.
Zmíněný record se následně používá pro fetchování dat ze serveru a to konkrétně třídou
```c#
public class NASAHorizonsDataFetcher
{
  public NASAHorizonsDataFetcher(MapMode mode, List<ObjectEntry> ObjectsToFetch, DateTime startDate, DateTime endDate, double observerLatitude = 0, double observerLongitude = -90);
  public async Task<IEnumerable<IEphemerisData<IEphemerisTableRow>>> Fetch();
}
```
Tato třída na základě vstupních parametrů provede fetch na server a naparsuje data do objektů splňujících rozhraní IEphemerisData<IEphemerisTableRow>>.
První parametr konstruktoru je mód, v jakém chceme fetchovat data. Je typu NASAHorizonsDataFetcher.MapMode
```c#
public enum MapMode : int
{
    NightSky,
    SolarSystem,
    EarthSatelites = 399,
    MarsSatelites = 499,
    JupiterSatelites = 599,
    SaturnSatelites = 699,
    UranusSatelites = 799,
    NeptuneSatelites = 899,
    PlutoSatelites = 999
}
```
Dotaz poslaný na NASA Horizons API obsahuje parametr EPHEM_TYPE, který může mít dvě hodnoty: buď OBSERVER, kdy jsou udány polární souřadnice objektu na obloze, jmenovitě obsahují azimut a výšku objektu, nebo VECTOR, kdy zaslaná data obsahují kartézské souřadnice objektu vzhledem ke zvolenému středu. Střed souřadné soustavy je má v querry části URL název CENTER, těleso, klteré nás zajímá se udává pod názvem COMMAND.
Je-li vstupem NightSky, pak je dotaz v módu OBSERVER a střed je nastaven na Zemi, je-li vstupem SolarSystem, je dotaz v módu VECTOR a středem je Slunce. U ostatních explicitně zmíněných hodnot je EPHEM_TYPE hodnoty VECTOR a středem je těleso, které je udáno v názvu hodnoty (např. pro JupiterSatelites je středem Jupiter). Tyto hodnoty jsou pak použity pro získání dat měsíců. Explicitně nastavené intové hodnoty jsou přímo kódy středových těles.

Parametry observerLatitude a observerLongitude jsou relevantní jen pro mapu noční oblohy, tedy pokud je mode roven MapMode.NightSky. V dotazu jsou tyto hodnoty dosazeny do parametru SITE_COORD.

Parametry DateTime startDate, DateTime endDate udávají první a pslední datum, pro které se budou získávat data. V Dotazu jsou uložena do parametrů START_TIME a STOP_TIME.

Parametr List<ObjectEntry> ObjectsToFetch jsou objekty, pro které provádíme dotazy na server.

Generování dotazového URL řídí privátní metoda
```c#
private string _generateURl(int objectCode)
```
Součástí URL je též parametr STEP_SIZE, který udává časové rozdíly mezi zaslanými daty. V aplikaci je vždy tento parametr nastaven na jeden den ("1 d").

Metoda Fetch režíruje fetchování a parsování dat, přičemž samotný fetch je prováděn metodou
```c#
private async Task<List<Tuple<ObjectEntry,string>>> _fetchAnswersWithLimit()
```
která režíruje paralelní dotazování na server.
Pro režii dotazů se používá třída
```c#
private class HttpWorker
{
public HttpClient Client { get; init; }
public ObjectEntry Entry { get; init; }
public int NumberOfAttempts { get; private set; } = 0;
public string URL { get; init; }

public async Task<HttpResponseMessage> Work();
}
```
která v metodě Work provádí samotný dotaz na server. Pole NumberOfAttempts slouží pro debugování. 
Pro každé ObjectEntry pro které chceme provést dotaz probíhá uvnitř _fetchAnswerWithLimit následující algoritmus: Por každou instanci ObjectEntry se vytvoří instance HttpWorker a dá se do fronty. Počet souběžně běžících vláken se zvyšuje a snižuje  podle toho, jak se daří fetchovat data. Pokud je kód http odpovědi typu 4xx, pak se dotaz zahodí. Pokud je kód odpovědi typu 5xx, sníží se počet povolených souběžně běžících vláken a dotaz je vrácen do fronty. Pokud dotaz uspěje, počet povolených vláken se zvýší a odpověď se uloží.
Parsování dat provádí třídy:
```c#
public class HorizonsObserverResponseReader : ObjectReader, IHorizonsResponseReader<EphemerisObserverData>
public class HorizonsVectorResponseReader : ObjectReader, IHorizonsResponseReader<EphemerisVectorData>
```
které implementují rozhraní
```c#
public interface IHorizonsResponseReader<out TData> where TData : IEphemerisData<IEphemerisTableRow>
{
    TData Read();    
}
```
Metoda Read vrací naparsovaný objekt implementující třídu IEphemerisData.

Zmíněné třídy pro parsování též dědí od abstraktní třídy
```c#
public abstract class ObjectReader
```
Tato třída se hlavně stará o čtení vlastností těles, které se netýkají polohy (např. jejich hmotnost, teplotu apod.).
NASA Horizons API neposílá data ve strojově čitelném formátu, a značení jednotlivých vlastností těles není konzistentní, např. gravitační zrychlení je u Slunce udáno ve tvaru "Surface gravity       =  274.0 m/s^2", u Země jako "g_e, m/s^2  (equatorial) = 9.7803267715" a u Venuše jako "Equ. gravity m/ s ^ 2 = 8.870". Proto má metoda ObjectReader pro každou vlastnost tělesa zvláštní třídu, které používají různé reglární výrazy pro nalezení těchto vlastností. Tyto třídy mají názvy ve tvaru "_findNázevVlastnosti".
Tyto metody jsou používány metodou
```c#
protected ObjectData createObjectInfo()
```
která vrací instanci ObjectData. Tato metoda je dále využívána v implementacích metody IHorizonsResponseReader<out TData>.Read().

### UI map
Slovem "mapa" v následující modílu myslíme instancí potmků třídy MapPanel.

Všechny mapy jsou implementace rozhraní 
```c#
internal interface IMap : IDisposable
{
    public void AdvanceMap();
    public DateTime CurrentPictureDate { get; }

    public event EventHandler<SwitchViewRequestedEvent> MapSwitch;
    public List<ObjectEntry> ObjectEntries { get; }
    public void CleanAndDispose();
}
```
Metoda AdvanceMap slouží pro posun času mapy o jednu časovou jednotku, CurrentPictureDate aktuální zobrazovaný časový okamžik, MapSwitch event handler slouží pro přepínání map, k čemuž se využívá událost
CleanAndDispose slouží k bezpečnému dispose mapy (včetně odhlášení event handlerů). Je definována zde, ač všechny mapy dědí od třídy Panel, která je disposable, protože metoda Panel.Dispose není virtuální.
```c#
internal class SwitchViewRequestedEvent : EventArgs
{
    public List<ObjectEntry> ObjectEntries;
    public DateTime Date;
    public NASAHorizonsDataFetcher.MapMode MapMode;
    public string CenterName;

    public SwitchViewRequestedEvent(List<ObjectEntry> objectEntries, DateTime date, NASAHorizonsDataFetcher.MapMode mapMode, string centerName);
}
```
která ukládá a sdílí vnitřní stav mapy.
ObjectEntries obsahuje informace pro fetchování dat o tělesech a CenterName značí jméno středového tělesa.

#### Třída MapPanel
Všechny mapy dědí od abstraktní generické třídy
```c#
internal abstract class MapPanel<TData> : Panel, IMap
    where TData : IEphemerisData<IEphemerisTableRow>
```
Tato třída obsahuje spoustu třídy, mezi nejdůležitější patří:
```c#
protected virtual void BodyClick(object sender, MouseEventArgs e)
```
- slouží pro zpracování kliknutí na mapu. Implementace pro každé těleso testuje, zda bylo kliknuto do jeho blízkosti.
```c#
protected virtual void drawFormBody(PixelBodyInfo pixelInfo, PaintEventArgs e, string? name)
```
- vykresluje těleso na obrazovku, volitelně i jeho jméno
```c#
protected void ShowBodyReport(IFormBody<TData> formBody)
```
- generuje formulář pro zobrazení informací o tělese s tlačítkem Track/Untrack a tlačítkem Moon View.
```c#
 private void _mouseMoveAcrossBody(object sender, MouseEventArgs e)
```
- detekce, zda uživatel umístil kurzor poblíž tělesa
```c#
private void _mouseMoveOutOfBody(object sender, EventArgs e)
```
- detekce, zda uživatel již nemá kurzor poblíž tělesa
```c#
protected abstract List<IFormBody<TData>> _prepareBodyData(List<TData> data);
```
- vytvoření instancí FormBody pro zobrazení na mapě
- v odvozených třídách provádí tuto činost paralelně, ovšem není deklarována jako async -> proto je volána metodě SetData která je sama volána v asynchroní metodě SettingDataAsync
```c#
protected virtual async Task<IReadOnlyList<TData>> GetHorizonsData(List<ObjectEntry> objects)
```
- fetch dat
```c#
protected async Task SettingDataAsync()
```
- fetch a parosvání dat
- volána v metodě AdvanceMap a v přepsané metodě OnHandleCreated
```c#
private void SetData()
```
- aktualizace dat, filtrování
```c#
protected void _filter()
```
- filtrování

```c#
private void _mouseMoveAcrossBody(object sender, MouseEventArgs e)
private void _mouseMoveOutOfBody(object sender, EventArgs e)
```
- zobrazovájí jména při přejetí myši na těleso, změna kurzoru

Konstruktor vypadá takto: 
```c#
public MapPanel(GeneralMapSettings generalMapSettings, IEnumerable<Func<IEnumerable<IFormBody<TData>>, IEnumerable<IFormBody<TData>>>> typeFilters)
```
Parametr typeFilters obsahuje delegáty, které jsou určeny pro filtrování dat jednotlivých typů těles. K třídě GeneralMapSettings se dostaneme později - obsahuje informace důležité pro nastavení mapy, včetně predikátu pro filtrování všech těles, názvy typů těles pro zobrazení, bílou a černou listinu etc. Viz. GeneralMapSettings.
K dispozici je i bezparametrický konstruktor, který slouží pro opakované spouštění panelu a přeskakuje filtrování objektů.

#### NightSkyMapPanel
Třída
```c#
internal class NightSkyMapPanel : MapPanel<EphemerisObserverData>
{
public NightSkyMapPanel(GeneralMapSettings generalMapSettings, IEnumerable<Func<IEnumerable<IFormBody<EphemerisObserverData>>, IEnumerable<IFormBody<EphemerisObserverData>>>> typeFilters);
public NightSkyMapPanel(List<ObjectEntry> objects, DateTime mapStartDate)
}
```
slouží pro zobrazení mapy noční oblohy. Druhý konstruktor bere přímo objekty pro zobrazení a přeskakuje filtrování - tento konstruktor se používá při opakovaném spouštění panelu.
Kromě metody třídy MapPanel obsahuje i metodu
```c#
private void PrintMapBackground(object sender, PaintEventArgs e)
```
pro nakreslení podkladu mapy.

#### SolarSystemMapPanel
Třída
```c#
internal class SolarSystemMapPanel : MapPanel<EphemerisVectorData>
{
public SolarSystemMapPanel(GeneralMapSettings generalMapSettings, IEnumerable<Func<IEnumerable<IFormBody<EphemerisVectorData>>, IEnumerable<IFormBody<EphemerisVectorData>>>> typeFilters,
    float scale_km = 1_000_000);
public SolarSystemMapPanel(List<ObjectEntry> objects, DateTime mapStartDate, float scale_km = 1_000_000);
}
```
slouží pro zobrazení mapy sluneční soustavy. V konstruktoru navíc bere parametr scale_km, který říká, jaké měřítko má mapa použít. 
Druhý konstruktor bere přímo objekty pro zobrazení a přeskakuje filtrování - tento konstruktor se používá při opakovaném spouštění panelu.
Měřítko je dostupné přes pole
```c#
public float Scale_km { get; private set; }
```
Dále má navíc event handler
```c#
public event EventHandler<ChangeScaleEvent> ScaleChange;
```
a k němu má navíc dvě metody
```c#
protected void OnChangeScale(object sender, ChangeScaleEvent e);

public void InvokeScaleSwitchEvent(float scale_km)
{
    ScaleChange?.Invoke(this, new ChangeScaleEvent(scale_km));
}
```
kde OnChangeScale se stará o změnu měřítka mapy, zatímco InvokeScaleSwitchEvent, pouze vytváří event, který změnu měřítka provádí. Změna měřítka je prováděna přes události, jelikož je spouštěna stisknutím tlačítka v ControlPanel (viz ControlPanel), což je jiný formulář.

Také má navíc pole
```c#
protected virtual bool _respectScaleForBodySize { get; } = false;
```
které říká, zda se při vykreslování objektů má respektovat jejich velikost.

#### SateliteMapPanel
Třída
```c#
internal class SateliteMapPanel : SolarSystemMapPanel
```
slouží pro zobrazování mapy měsíců.
Na rozdíl od svého předka má _respectScaleForBodySize nastaveno na true. Dále obsahuje metody
```c#
private void _printDirections(PaintEventArgs e)
private void _drawArrowAtTheEdge(Graphics graphics, PointF location, Color color, string name)
```
které slouží pro zobrazování směru Země a Slunce.

### Hlavní formulář
Hlavním formulářek, který provádí řežii zobrazování map je
```c#
public partial class SolarMapperMainForm<TData> : Form
    where TData : IEphemerisData<IEphemerisTableRow>
```
Tento formulář má následující fieldy:

```c#
private Panel _mainMapPanel;
```
- zde jsou uloženy jednotlivé mapy
- field je sice třídy Panel, ale jsou vněm obsaženy pouze potomci třídy MapPanel, ke kterým je buď přistupováno jako k instanci Panel nebo jako k implementaci IMap. Je tomu tak z několika důvodů, hlavním z nich je generika samotné třídy.
```c#
private SateliteMapPanel _sateliteMap = null;
```
- pro mapu měsíců
```c#
private MapType mainMapType;
```
- uchovává informaci o hlavním typu mapy, aby šlo snáze opakovaně dávat instance do _mainMapPanel.
```c#
private ControlForm _controlForm;
```
- kontrolní formulář
```c#
private GeneralMapSettings _mapSettings;
```
- nastavení map
```c#
private IEnumerable<Func<IEnumerable<IFormBody<TData>>, IEnumerable<IFormBody<TData>>>> _typeFilters;
```
- filtry pro jednotlivá tělesa
```c#
private List<ObjectEntry> ObjectEntries;
```
- pro uchování dat o objektech, aby nebylo třeba je znova filtrovat

Mezi nejdůležitější třídy patří:
```c#
private void _setUpMainMapPanel(List<ObjectEntry> entries, DateTime date)
```
- slouží k opakovanému vytváření map.
```c#
private void _destroyMainMapPanel()
```
- zničí hlavní mapu (dispose) a provede dispose _controlForm
```c#
private void _setUpMoonMapPanel(List<ObjectEntry> entries, DateTime date, NASAHorizonsDataFetcher.MapMode mode, string centerName);
```
- nastavení mapy měsíců
```c#
private void _destroyMoonPanel();
```
- zničení mapy měsíců a dispose _controlForm
```c#
private void SolarMapperUI_KeyDown(object sender, KeyEventArgs e);
```
- pro zobrazení kontrolního panelu
```c#
private void ShowMoonPanel(object sender, SwitchViewRequestedEvent e);
```
- zničení hlavní mapy a zobrazení mapy měsíců
```c#
private void ShowMainPanel(object sender, SwitchViewRequestedEvent e)
```
- zničení mapy měsíců a zobrazení hlavní mapy
Konstruktorem je 
```c#
internal SolarMapperMainForm(GeneralMapSettings generalMapSettings, IEnumerable<TypeSettings<TData>> typeSettings, Panel panel);
```
#### Kontrolní formulář
```c#
internal class ControlForm : Form
```
Tato třída slouží k ovládání mapy. Jde o formluář s tlačítky. Většina kódu slouží pouze k vytváření tlačítek, které vyvolávají události.
```c#
public ControlForm(IMap mapPanel)
```
V konstruktoru bere třída instanci implementace IMap a podle typu implementace vybírá tlačítka, která se na kontrolním panelu zobrazí.
```c#
```















