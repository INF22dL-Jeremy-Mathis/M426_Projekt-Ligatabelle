## Designbeschreibung: Projekt "Ligatabelle"

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



# Projekt "Ligatabelle"

Dieses Projekt bietet eine C# Konsolenanwendung zur Anzeige von Fußball-Ligatabellen. Benutzer können die Ergebnisse ihrer Lieblingsliga interaktiv abrufen und anzeigen lassen. Das Programm benötigt die .NET SDK zur Ausführung.



## Installation und Ausführung

1. Stelle sicher, dass die .NET SDK auf deinem System installiert ist.
   
2. Klone das Repository und öffne es in Visual Studio Code oder einem anderen Texteditor.
   
3. Navigiere im Terminal in das Verzeichnis `M426_Projekt-Ligatabelle/LigaResults`.
   
4. Führe den Befehl `dotnet publish` aus. Dieser Schritt erstellt eine ausführbare Datei `LigaResults.exe`, die im Ordner `M426_Projekt-Ligatabelle/LigaResults/bin/Release/publish` zu finden ist.
   
5. Kopiere die `LigaResults.exe` in den Datenquellordner mit dem Namen "soccer-results".
   
6. Führe die `LigaResults.exe` durch Doppelklick aus.

7. Ein Terminal wird geöffnet. Wenn alles geklappt hat und sich die `.exe` im richtigen Ordner befindet, wirst du zur Auswahl einer Liga aus der angezeigten Liste aufgefordert. Andernfalls wird eine Fehlermeldung ausgegeben.

8. Wähle aus der Liste von Ligen mittels der ID oder dem Namen der Liga direkt die Ergebnisse einer Liga aus. Bestätige deine Auswahl mit Enter. Bei einer nicht vorhandenen Liga wirst du darauf aufmerksam gemacht und erneut zur Eingabe aufgefordert.

9. Die formatierte Tabelle wird dir präsentiert, wobei Platzierungen, Teamnamen und verschiedene Statistiken klar und übersichtlich angezeigt werden.

10. Nach der Ausgabe der Ergebnisse wirst du gefragt, ob du die Applikation schließen Du kannst entweder mit Ja bestätigen oder mit Nein zurück zur Ligaauswahl gelangen.



## Verwendung der .exe über die Befehlszeile

Alternativ zur Ausführung der `.exe` durch Doppelklick kannst du die Anwendung auch über die Befehlszeile mit Parametern aufrufen.

Die Syntax für den Aufruf lautet wie folgt:

`.\LigaResults.exe Liganame AusgabeAlsJson`

- `Liganame`: Der Name der Liga, für die die Ergebnisse abgerufen werden sollen. Dieser Parameter ist case-sensitive und muss bereits bekannt sein.

- `AusgabeAlsJson`: Optionaler Parameter. Wenn der Wert "true" mitgegeben wird, werden die Ergebnisse als JSON-String ausgegeben. Andernfalls werden die Ergebnisse als formatierte Tabelle angezeigt.

Beispiel:
`.\LigaResults.exe bundesliga true`
Dieser Befehl ruft die Ergebnisse für die bundesliga ab und gibt sie als JSON-String aus.


