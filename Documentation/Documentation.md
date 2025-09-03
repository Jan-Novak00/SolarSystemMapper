# Uživatelská část
Aplikace vyžaduje pro své fungování připojení k internetu, aby mohla aplikace získat informace o efemeridách z NASA Horizons API (dále jen API).

## Spuštění

## Nastavení mapy
Na první obrazovce je uživateli dána možnost vybrat typ mapy, kterou chce uživatel zobrazit. Lze zvolit buď Night sky, pro mapu noční oblohy, nebo SOlar system, pro mapu sluneční soustavy.
Uživatel dále musí zadat datum, pro které chce zobrazit mapu.
Pokud uživatel zvolil mapu noční oblohy, musí ještě zadat souřadnice na Zemi, pro které chce mapu zobrazit. Souřadnice musí být zadány ve tvaru "šířka,délka", kde čísla šířka a délka musí být zadány v double formátu, oddělené čáskou, s desetinou tečkou.

Dále uživatel může vybrat, které typy těles chce zobrazit.

Stisknutím tlačítka "Next page" se uživatel přesune na další okno pro nastavení mapy. Po přesunutí se nelze vrátit zpět.

### Obecné filtry
Uživatel může zadat obecné filtry pro tělesa, resp. maximální a minimální hodnoty pro hmotnost, poloměr, periodu oběhu, tíhové zrychlení a hustotu. Minima jsou v základu nastavena na 0 a maxima na "Infinity", tedy maximální hodnota je neomezená. Vstup musí být převoditelný na double.

Poznámka: tíhové zrychlení a gravitační zrychlení jsou v této aplikaci zaměňovány, jelikož API je udává souhrně jako "Gravity" (v angličtině neexitují pro tyto koncepty oddělené termíny). Obecně platí, že u větších těles byl vybírán parametr "Equatorial gravity", u menších těles byl vybírán parametr "Gravity", či byl tento parametr vypočten z hmotnosti tělesa.

V neposlední řadě uživatel může nastavit tzv. white list, kam může napsat názvy těles, které chce, aby se zobrazily, i kdyby neprošly filtry. Též může nastavit black list. Ve white/black list musí být názvy oddělené čárkou.

Pokud pro nějaký parametr se nepodařilo získat hodnotu, dané těleso filtr automaticky splní.

### Filtry pro jednotlivé typy

## Mapy
### Společné ovládací prvky
Stiskem tlačítka ESC se aplikace vypne.
Stiskem tlačítka TAB se otevře ovládací panel.

Jednotlivá tělesa jsou na mapě zobrazené jako barevné kruhy. Pokud uživatel stiskne některé těleso,, zobrazí se uživateli v levém dolním rohu panel s údaji o daném tělese - údaje jsou získány přímo od API. Údaje které se nepodaří získat se ve nezobrazí, nebo se místo jejich hodnoty vypíše NaN.
Některá tělesa mají pod sebou vypsaný jejich název - toto automaticky platí pro Slunce a planety. Na informačním panelu je tlačítko Track/Untrack, kterým lze zapnout/vypnout zobrazování jména daného tělesa.
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

### Reprezentace dat

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
private async Task<string[]> _fetchAnswersWithLimit(int maxParallelism)
```
která jako vstupní argument potřebuje maximální množství vláken, na kterých může posílat dotazy. Z vlastní zkušenosti server nezvládá více než dva dotazy najednou.

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

