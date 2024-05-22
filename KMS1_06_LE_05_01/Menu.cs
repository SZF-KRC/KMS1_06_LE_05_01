using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMS1_06_LE_05_01
{
    public class Menu
    {
        /// <summary>
        /// Zeigt das Hauptmenü an und verarbeitet die Benutzerauswahl.
        /// </summary>
        public void PrintMenu()
        {
            bool exit = false;
            string folderPath = "..//..//..//KMS1_06_LE_05_01";// Pfad zum Ordner
            while (!exit)
            {
                Console.WriteLine("\n*** Verwaltung von Textdokumenten ***\n[0] Programm beenden\n[1] Dateien drucken\n[2] Hinzufugen Textdokument\n[3] Lesen Textdokument\n[4] Löschen Sie die Textdatei");
                switch(InputNumber("Geben Sie den Index Ihrer Wahl ein: "))
                {
                    case 0:exit = true; break;// Beenden des Programms
                    case 1:PrintFiles(folderPath);break;// Dateien drucken
                    case 2:AddFile(folderPath); break;// Textdokument hinzufügen
                    case 3:ReadFile(folderPath); break;// Textdokument lesen
                    case 4:DeleteFile(folderPath);break;// Textdokument löschen
                    default: Console.WriteLine("\n--- Geben Sie nur den Index von 0-4 ein ---\n"); break;
                }
            }
        }

        /// <summary>
        /// Druckt die Liste der Textdateien im Ordner.
        /// </summary>
        /// <param name="folderPath">Pfad zum Ordner.</param>
        private void PrintFiles(string folderPath)
        {
            if (IsTextFileInFolder(folderPath))
            {
                Console.WriteLine();
                string[] files = Directory.GetFiles(folderPath, "*.txt");// Alle .txt-Dateien im Ordner
                foreach (string file in files)
                {
                    Console.WriteLine(Path.GetFileNameWithoutExtension(file)); // Ausgabe des Dateinamens ohne Erweiterung         
                }         
            }
        }

        /// <summary>
        ///  Fügt eine neue Textdatei zum Ordner hinzu.
        /// </summary>
        /// <param name="folderPath">Pfad zum Ordner.</param>
        private void AddFile(string folderPath)
        {          
            string fileName = InputString("\nGeben Sie den Namen der Textdatei ein: ");
            string filePath = Path.Combine(folderPath, fileName + ".txt");// Kompletter Pfad zur neuen Datei
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                Console.WriteLine("Geben Sie die Text ein (Geben Sie „exit“ ein, um den Schreibvorgang zu beenden):");
                while (true)
                {
                    string line = Console.ReadLine();// Zeile vom Benutzer
                    if (line == "exit") break;// Beenden des Schreibvorgangs
                    writer.WriteLine(line);// Schreiben der Zeile in die Datei
                }
            }              
        }

        /// <summary>
        /// Liest den Inhalt der ausgewählten Textdatei.
        /// </summary>
        /// <param name="folderPath">Pfad zum Ordner.</param>
        private void ReadFile(string folderPath)
        {            
            if (IsTextFileInFolder(folderPath))
            {
                PrintFiles(folderPath);// Drucken der Dateien im Ordner
                string fileName = InputString("\nGeben Sie den Namen des Textdokuments ein: ");// Dateiname vom Benutzer
                string filePath = Path.Combine(folderPath, fileName + ".txt");// Kompletter Pfad zur Datei

                if (File.Exists(filePath))
                {
                    try
                    {
                        using (StreamReader reader = new StreamReader(filePath))
                        {
                            Console.WriteLine("\n\n**********************************************");
                            while (!reader.EndOfStream)
                            {
                                Console.WriteLine(reader.ReadLine());// Zeilenweise Ausgabe des Dateiinhalts
                            }
                            Console.WriteLine("\n**********************************************");
                        }
                        if (InputString("\nMöchten Sie den Text bearbeiten? (ja/nein): ").ToLower() == "ja")
                        {
                            EditFile(filePath, folderPath);// Datei bearbeiten
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Fehler beim Lesen der Datei: " + ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine("\n--- Das Textdokument existiert nicht. ---\n");
                }
            }
        }

        /// <summary>
        /// Bearbeitet den Inhalt einer vorhandenen Textdatei.
        /// </summary>
        /// <param name="filePath">Pfad zur Textdatei.</param>
        /// <param name="folderPath">Pfad zum Ordner.</param>
        private void EditFile(string filePath, string folderPath)
        {
            List<string> lines = new List<string>();// Liste zum Speichern der neuen Zeilen
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                Console.WriteLine("Geben Sie die neue Text ein (Geben Sie „exit“ ein, um den Schreibvorgang zu beenden):");
                while (true)
                {                  
                    string newText = Console.ReadLine(); //Neue Zeile vom Benutzer
                    if (newText == "exit")break;// Beenden des Schreibvorgangs
                    lines.Add(newText);// Hinzufügen der neuen Zeile zur Liste
                }
                if (InputString("In derselben Datei speichern? (ja/nein)").ToLower() == "ja")
                {            
                    foreach (string line in lines)
                    {
                        writer.WriteLine(line);// Schreiben der neuen Zeilen in die gleiche Datei
                    }                    
                }
                else
                {
                    string newFileName = InputString("Geben Sie den neuen Dateinamen ein: ");// Neuer Dateiname vom Benutzer
                    using (StreamWriter newWriter = new StreamWriter(Path.Combine(folderPath, newFileName+".txt")))
                    {
                        foreach (string line in lines)
                        {
                            newWriter.WriteLine(line);// Schreiben der neuen Zeilen in die neue Datei
                        }                       
                    }
                    Console.WriteLine($"Datei sind als neue Datei {newFileName} gespeichert.");
                }
            }
        }

        /// <summary>
        /// Löscht die ausgewählte Textdatei aus dem Ordner.
        /// </summary>
        /// <param name="folderPath">Pfad zum Ordner.</param>
        private void DeleteFile(string folderPath)
        {
            if (IsTextFileInFolder(folderPath))
            {
                PrintFiles(folderPath);// Drucken der Dateien im Ordner
                string fileName = InputString("\nGeben Sie den Namen des Textdokuments ein: ");// Dateiname vom Benutzer
                string filePath = Path.Combine(folderPath, fileName + ".txt");// Kompletter Pfad zur Datei

                if (File.Exists(filePath))
                {
                    try
                    {                      
                        File.Delete(filePath);     // Löschen der Datei              
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Fehler beim Löschen die Textdatei: " + ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine("\n--- Das Textdokument existiert nicht. ---\n");
                }
            }
        }

        /// <summary>
        /// Liest eine Ganzzahl vom Benutzer ein und stellt sicher, dass die Eingabe nicht leer ist.
        /// </summary>
        /// <param name="prompt">Die Anzeigeaufforderung.</param>
        /// <returns>Die Benutzereingabe als Ganzzahl.</returns>
        private int InputNumber(string prompt)
        {
            int number;
            while (true)
            {
                Console.Write($"\n{prompt}");// Anzeige der Eingabeaufforderung an den Benutzer
                string input = Console.ReadLine();// Benutzereingabe lesen
                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("\n--- Eingabe darf nicht leer sein ---\n");
                    continue;// Schleife fortsetzen, wenn die Eingabe leer ist
                }
                try
                {
                    number = Convert.ToInt32(input);// Versuch, die Eingabe in eine Ganzzahl zu konvertieren
                    break;// Schleife unterbrechen, wenn die Konvertierung erfolgreich is
                }
                catch (Exception e)
                {
                    Console.WriteLine("\n--- Geben Sie nur eine Ganzzahl ein ---\n" + e.Message);
                }
            }
            return number;// Rückgabe der Ganzzahl
        }

        /// <summary>
        /// Liest eine Zeichenkette vom Benutzer ein und stellt sicher, dass die Eingabe nicht leer ist.
        /// </summary>
        /// <param name="promt">Die Anzeigeaufforderung.</param>
        /// <returns>Die Benutzereingabe als Zeichenkette.</returns>
        private static string InputString(string promt)
        {
            bool exit = false;
            string input = "";
            while (!exit)
            {
                Console.Write(promt);// Anzeige der Eingabeaufforderung an den Benutzer
                input = Console.ReadLine();
                if (input.Length < 1) // Überprüfung auf leere Eingabe
                {
                    Console.WriteLine("\n--- Die Eingabe darf nicht leer sein ---\n");
                }
                else
                {
                    exit = true;// Beenden der Schleife bei gültiger Eingabe
                }
            }
            return input;// Rückgabe der Zeichenkette
        }

        /// <summary>
        /// Überprüft, ob im Ordner Textdateien vorhanden sind.
        /// </summary>
        /// <param name="folderPath">Der Pfad zum Ordner.</param>
        /// <returns>True, wenn Textdateien vorhanden sind, andernfalls false.</returns>
        private bool IsTextFileInFolder(string folderPath)
        {
            // Získajte všetky súbory s príponou .txt v zložke
            string[] textFiles = Directory.GetFiles(folderPath, "*.txt");// Abrufen aller .txt-Dateien im Ordner
            if (textFiles.Length == 0)
            {
                Console.WriteLine("\n---Es gibt kein Textdokument im Ordner ---\n");// Meldung bei keinen vorhandenen Textdateien
                return false;
            }

            // Rückgabe true, wenn Textdateien vorhanden sindv
            return true;
        }
    }
}
