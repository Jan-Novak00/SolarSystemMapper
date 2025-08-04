# SolarSystemMapper
Project for (Advanced) Programming in C#

# **Czech Specification**

## Anotace
Cílem projektu je software, který pro zadané souřadnice a čas zobrazí mapu oblohy s vybranými efemeridy (planety, asteroidy, měsíce, umělé družice) uvnitř sluneční soustavy. Dále program bude mít možnost změnit pohled a ukázat mapu sluneční soustavy při pohledu shora (směrem od severního pólu slunce) s danými efemeridy. K tomu bude využito NASA Horizon API.

## Podrobný seznam funkcí

### Filtrování objektů
Zobrazené efemeridy bude možno v samostatném dialogovém okně filtrovat podle typu (planeta, asteroid, umělá družice,...), či podle velikosti, hmotnosti nebo vzdálenosti (v případech, kdy je to možné).

### Mapy
Po nastavení filtrů uživatel zadá datum a vybere typ mapy pro zobrazení: buď mapy oblohy nebo mapy sluneční soustavy. V obou případech ze zobrazí pouze efemeridy, které projdou nastavenými filtry. Před zobrazením mapy uživatel zadá datum, pro které chce zobrazit rozložení objektů na mapě.
Planety (včetně Slunce a Měsíce) budou zobrazeny jako barevné kruhy, ostatní objekty jako bílé body.
Při přejetí kurzorem na daný objekt se u daného objektu zobrazí jeho název.

#### Mapa oblohy
Po nastavení filtru objektů a data uživatel zadá ještě GPS polohu. Tím se zobrazí mapa oblohy pro dané datum s vyznačenými kardinálními směry.

#### Mapa sluneční soustavy
Zobrazí se mapa sluneční soustavy při pohledu shora (směrem od severního pólu Slunce) pro dané datum.

#### Časový posun
U obou map lze po kliknutí na tlačítko zobrazit polohy objektů v následujících dnech. Polohy objektů v budoucích dnech se budou postupně za sebou zobrazovat, přičemž se na obrazovku bude vypisovat dané datum. Časový posun lze kdykoliv zastavit.

#### Detail objektu
Po klinutí na objekt se zobrazí panel se základními informacemi o daném objektu, které poskytuje NASA Horizon API.

#### Detail planetárního systému
Při zobrazení detailu některé z planet se zobrazí tlačítko s možností zobrazení soustavy měsíců dané planety (pokud daná planeta měsíce má).
Po kliknutí na toto tlačítko se zobrazí pohled na danou planetu (směrem ze severního pólu slunce) a polohy největších měsíců dané planety. I zde bude možno zobrazit budoucí pozice.

## Vstupy a výstupy

**Vstupy:**
- Datum a čas
- GPS souřadnice (pro mapu oblohy)
- Typ mapy (obloha / sluneční soustava)
- Filtrování objektů (typ, velikost, hmotnost)

**Výstupy:**
- Interaktivní mapa oblohy nebo sluneční soustavy
- Pozice vybraných objektů
- Detailní informace o objektu
- Animace časového posunu

## Uživatelské rozhraní
- Dialog pro zadání filtru a data
- Výběr typu mapy
- Zadání GPS souřadnic (pro mapu oblohy)
- Interaktivní mapa s popisky objektů
- Panel s detaily objektu
- Ovládání animace času
- Tlačítko pro zobrazení měsíční soustavy planet
## Případy užití
- následující případy užití ilustrují používání aplikace
### Zobrazení oblohy
- uživatel spustí aplikaci přes Visual studio 2022
- systém po úvodní obrazovce zobrazí seznam filtrů
- uživatel nastaví filtry podle svých potřeb
- uživatel do dialogového pole napíše datum, pro které chce zobrazit mapu oblohy
- uživatel vybere typ mapy v dalším dialogovém okně - mapa oblohy
- uživatel potvrdí výběr
- systém uživatele vyzve, aby zadal dané GPS souřadnice
- uživatel zadá GPS souřadnice a potvrdí
- systém zobrazí mapu oblohy pro půlnoc daného data a danou polohu
- uživatel klikne na některé z těles
- systém ukáže okno s informacemi o objektu
- uživatel uzavře okno s informacemi
- uživatel klikne na tlačítko pro časový posun
- systém postupně zobrazí mapy pro další data

### Zobrazení mapy sluneční soustavy
- uživatel spustí aplikaci přes Visual studio 2022
- systém po úvodní obrazovce zobrazí seznam filtrů
- uživatel nastaví filtry podle svých potřeb
- uživatel do dialogového pole napíše datum, pro které chce zobrazit mapu oblohy
- uživatel vybere typ mapy v dalším dialogovém okně - mapa sluneční soustavy
- uživatel potvrdí výběr
- systém zobrazí mapu sluneční soustavy pro půlnoc daného data
- uživatel klikne na planetu Jupiter
- systém ukáže okno s informacemi o Jupiteru a tlačítko "Detail měsíců"
- uživatel klikne na tlačítko "Detail měsíců"
- systém ukáže mapu měsíční soustavy Jupiteru s vybranými měsíci
- uživatel klikne na tlačítko pro časový posun
-  systém postupně zobrazí mapy soustavy měsíců pro další data
## Motivace a alternativy

Stávající nástroje jako Stellarium či Celestia nenabízí plnou kontrolu nad použitými daty nebo výstupy. Cílem tohoto projektu je vytvořit vlastní nástroj pro vizualizaci efemerid sluneční soustavy využívající přesná data z NASA Horizons API, které je jednoduché a přizpůsobitelné.
## Použité technologie

- LINQ pro filtrovaní objektů
- http klient - komunikace s NASA Horizon API a případně dalšími pomocnými API
- vlákna - paralelní umisťování objektů na mapu a vytváření map pro další dny, pro rychlejší zpracování
- Windows Forms - uživatelské rozhraní
- Použití knihoven pro kreslení (System.Drawing nebo SkiaSharp)

## Odkazy
[NASA Horizon API](https://ssd.jpl.nasa.gov/horizons/)

