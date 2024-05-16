# Projekt "soccer-results" - README

Dieses Projekt bietet eine C# Konsolenanwendung zur Anzeige von Fußball-Ligatabellen. Benutzer können die Ergebnisse ihrer Lieblingsliga interaktiv abrufen und anzeigen lassen. Das Programm benötigt die .NET SDK zur Ausführung.

## Installation und Ausführung

1. Stelle sicher, dass die .NET SDK auf deinem System installiert ist.

2. Klone das Repository und öffne es in Visual Studio Code oder einem anderen Texteditor.

3. Navigiere im Terminal in das Verzeichnis "LigaResults" und führe den Befehl `dotnet publish` aus. Dies erstellt eine ausführbare Datei `LigaResults.exe`, die im Ordner `Projekt/LigaResults/bin/Release/publish` zu finden ist.

4. Kopiere die `LigaResults.exe` in den Hauptordner des Projekts mit dem Namen "soccer-results".

5. Öffne den Hauptordner "soccer-results" und führe die `LigaResults.exe` durch Doppelklick aus.

6. Ein Terminal wird geöffnet, dem du die Anweisungen der Anwendung folgst.

## Designbeschreibung: C# Konsolenanwendung

Unsere C# Anwendung bietet eine interaktive Benutzeroberfläche zur Anzeige von Fußball-Ligatabellen. Der Benutzer wird aufgefordert, eine Liga aus einer Liste verfügbarer Ligen auszuwählen. Anschließend wird eine detaillierte Tabelle mit Platzierungen, Statistiken und anderen relevanten Informationen zur ausgewählten Liga angezeigt.

## Tabellenformat

Die präsentierte Tabelle umfasst folgende Informationen:

- Platzierung
- Teamname
- Anzahl der Siege
- Anzahl der Niederlagen
- Anzahl der Unentschieden
- Tore (geschossen:erhalten)
- Punkte

## Interaktionsablauf

1. Der Benutzer startet die Anwendung durch Ausführen der `LigaResults.exe`.

2. Die Anwendung präsentiert eine Liste der verfügbaren Ligen mit entsprechenden Nummern und Namen zur Auswahl.

3. Der Benutzer gibt entweder den Namen oder die Nummer der gewünschten Liga ein.

4. Die Anwendung ruft die Daten für die ausgewählte Liga ab und formatiert sie für die Anzeige.

5. Die formatierte Tabelle wird dem Benutzer präsentiert, wobei Platzierungen, Teamnamen und verschiedene Statistiken klar und übersichtlich angezeigt werden.
