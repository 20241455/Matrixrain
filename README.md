# Matrixregen

## Vorschau
Todo!

## Bedienung
1. Namen direkt eingeben trotz des Mangels eines Eingabefensters.
2. Enter drücken

## Hinter den Kulissen
Alle Zeichen auf dem Bildschirm werden alle 60 Millisekunden erneut gezeichnet und in zwei Schritten bearbeitet:
1. Die Farbe wird mit einer Zahl verändert, der für jedes Zeichen unterschiedlich ist (positive machen die Farbe heller, negative machen die Farbe dünkler).
2. Falls eine Farbe zu hell oder zu dunkel wird, wird das Zeichen ersetzt durch ein neues Zufälliges und die Zahl, die die Farbe verändert, wird mit -1 multipliziert.

Die Zeichen, Farben und der Farbveränderungswert wird in einem zweidimensionalen Array abgespeichert.
Ein zweidimensionales Array ist wie eine Exceltabelle, wo man nur pure Zahlen eingeben kann und die Größe begrenzt ist.

Wenn ein Text eingeben wird, wird zuerst überprüft, ob der Text sich auf dem Bildschirm ausgeht.
Falls es sich nicht ausgeht, wird der Name mit ´Der Name ist zu lang.´ ersetzt.
Ansonsten werden folgende Schritte durchgemacht:
* Es wird die Stelle berechnet, wo der Text angezeigt werden soll.
* Es wird für jedes Zeichen die Bitmapfontzahl herausgelesen und es wird überprüft ob die Zahl gerade ist, nachdem man sie X mal durch zwei dividiert hat (X ist die X-Koordinate vom Punkt plus die Y-Koordinate mal acht).
  Falls die Zahl gerade ist, wird ein Zeichen auf dieser Stelle generiert mit zufälliger Farbe und einem zufälligen Zeichen.
