## Designbeschreibung: Projekt "Ligatabelle"

Unsere C# Anwendung bietet eine interaktive Benutzeroberfläche zur Anzeige von Fußball-Ligatabellen. Der Benutzer wird aufgefordert, eine Liga aus einer Liste verfügbarer Ligen auszuwählen. Anschließend wird eine detaillierte Tabelle mit Platzierungen, Statistiken und anderen relevanten Informationen zur ausgewählten Liga angezeigt.


## Tabellenformat

Die präsentierte Tabelle umfasst folgende Informationen:

- Rang
- Teamname
- Punkte
- Anzahl der Siege
- Anzahl der Niederlagen
- Anzahl der Unentschieden
- Tore
- Gegentore
- Tordifferenz

![image](https://github.com/INF22dL-Jeremy-Mathis/M426_Projekt-Ligatabelle/assets/124058148/aad79ab8-e3c7-44a8-b405-d8587642ac27)


# Projekt "Ligatabelle"

Dieses Projekt bietet eine C# Konsolenanwendung zur Anzeige von Fußball-Ligatabellen. Benutzer können die Ergebnisse ihrer Lieblingsliga interaktiv abrufen und anzeigen lassen. Das Programm benötigt die .NET SDK zur Ausführung.

## Installation und Ausführung

1. Stelle sicher, dass die .NET SDK auf deinem System installiert ist.
   
2. Klone das Repository und öffne es in Visual Studio Code oder einem anderen Texteditor.
   
3. Navigiere im Terminal in das Verzeichnis `M426_Projekt-Ligatabelle/LigaResults`.

4. Führe den Befehl `dotnet run` aus, um die Anwendung zu starten. Die App verwendet die Daten im Verzeichnis `M426_Projekt-Ligatabelle/TestData/soccer-results`.

5. Ein Terminal wird geöffnet und zeigt eine Liste der verfügbaren Ligen an. Wähle eine Liga durch Eingabe der ID oder des Namens der Liga und bestätige mit Enter.

![image](https://github.com/INF22dL-Jeremy-Mathis/M426_Projekt-Ligatabelle/assets/124058148/f32e1316-d3ab-48e8-8789-02e4aaafff8c)

7. Anschließend wirst du gefragt, bis zu welchem Spieltag die Tabelle berechnet werden soll. Gib eine Zahl zwischen 1 und der maximal verfügbaren Anzahl an Spieltagen ein oder lasse das Eingabefeld leer, um alle Spieltage zu berücksichtigen.

![image](https://github.com/INF22dL-Jeremy-Mathis/M426_Projekt-Ligatabelle/assets/124058148/ca9ea9d2-10d8-4528-9e34-6b25fa6a774d)


9. Die formatierte Tabelle wird dir präsentiert, wobei Platzierungen, Teamnamen und verschiedene Statistiken klar und übersichtlich angezeigt werden.

10. Nach der Ausgabe der Ergebnisse wirst du gefragt, ob du die Applikation schließen möchtest. Du kannst entweder mit Ja bestätigen oder mit Nein zurück zur Ligaauswahl gelangen.

## Ausführung der Anwendung als .exe

1. Führe den Befehl `dotnet publish` aus. Dieser Schritt erstellt eine ausführbare Datei `LigaResults.exe`, die unter `M426_Projekt-Ligatabelle\LigaResults\bin\Release\net8.0\win-x64\publish\LigaResults.exe` zu finden ist.

2. Kopiere die `LigaResults.exe` in den Datenquellordner mit dem Namen "soccer-results".

3. Führe die `LigaResults.exe` durch Doppelklick aus.

4. Ein Terminal wird geöffnet. Wenn alles geklappt hat und sich die `.exe` im richtigen Ordner befindet, wirst du zur Auswahl einer Liga aus der angezeigten Liste aufgefordert. 

5. Wähle aus der Liste von Ligen mittels der ID oder dem Namen der Liga direkt die Ergebnisse einer Liga aus. Bestätige deine Auswahl mit Enter. Bei einer nicht vorhandenen Liga wirst du darauf aufmerksam gemacht und erneut zur Eingabe aufgefordert.

6. Anschließend wirst du gefragt, bis zu welchem Spieltag die Tabelle berechnet werden soll. Gib eine Zahl zwischen 1 und der maximal verfügbaren Anzahl an Spieltagen ein oder lasse das Eingabefeld leer, um alle Spieltage zu berücksichtigen.

7. Die formatierte Tabelle wird dir präsentiert, wobei Platzierungen, Teamnamen und verschiedene Statistiken klar und übersichtlich angezeigt werden.

8. Nach der Ausgabe der Ergebnisse wirst du gefragt, ob du die Applikation schließen möchtest. Du kannst entweder mit Ja bestätigen oder mit Nein zurück zur Ligaauswahl gelangen.

## Unit Tests

Um die Unit Tests auszuführen, navigiere im Terminal in das Verzeichnis `M426_Projekt-Ligatabelle` und führe den Befehl `dotnet test` aus. Dieser Befehl wird alle Tests in der Lösung ausführen und die Ergebnisse im Terminal anzeigen.
