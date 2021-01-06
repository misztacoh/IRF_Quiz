# IRF_Quiz

## Alkalmazás célja
Egyszerű kérdés felelt alkalmazás kiegészítve egy statisztikai felülettel.

## Adatbázis
Az alkalmazás AZURE-ban tárolt adatbázishoz csatlakozik, így adatbázis fájl nincs mellékelve.

## Quiz felület
A felületen ki lehet választani a játékost, majd a start gombbal indítani a kvíz játékot. Felhasználókezelés nincs implementálva, így az adatbázisban létező felhasználók használhatók.

Az alkalmazás az adatbázisból véletlenszerűen kiválaszt 10 kérdést, melyekre három válaszlehetőség küzül kell kiválasztani, lehetőleg a helyes választ 15 mp-en belül. A felület mutatja a hátralévő időt, helyes és helytelen válaszok számát.

## Statisztika felület
A felületen a játékosok helyes és helytelen válaszainak tendenciáját mutatja grafikonon. A megjelenítendő adat a játékos kijelölésével jeleníthető meg, illetve az adatok szűkíthatőek kategóriák szerint és időszakra is.

A felület enged több játékost is kiválasztani, de ebben az esetben az alkalmazás jelen állapotában a megjelenített adatok nem informatívak.
