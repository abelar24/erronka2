using System;
using System.IO;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using TiketBAI;
using SupermarketApp;

namespace TiketBAI
{
    public class Estatistikak
    {
        // Estatistikak ikusteko metodoa
        public static void View()
        {
            int zmb = 0; // Eragiketa aukeratzeko aldagaia
            while (zmb != 1) // 1 aukeratu arte jarraitu
            {
                Console.Clear(); // Pantaila garbitu
                Console.WriteLine("");
                Console.WriteLine("TIKET BAI");
                Console.Write("Aukeratu egitea nahi duzun eragiketa: ");
                // Eragiketa aukeratzeko menuaren irudia
                Console.WriteLine("\r\n1.- Atzera bueltatu");
                Console.WriteLine("2.- Ikusi maximoa departamentuzka");
                Console.WriteLine("3.- Ikusi batazbestekoa departamentuzka");
                Console.WriteLine("4.- Ikusi maximoa saltzaileen arabera");
                Console.WriteLine("5.- Ikusi batazbestekoa saltzaileen arabera");
                Console.WriteLine("6.- Ikusi maximoa dataren arabera");
                Console.WriteLine("7.- Ikusi batazbestekoa dataren arabera");

                string? sartutakue = Console.ReadLine(); // Erabiltzailearen sarrera irakurri
                try
                {
                    // Sarrera zenbaki bat den egiaztatu
                    if (int.TryParse(sartutakue, out zmb))
                    {
                        if (zmb >= 1 && zmb <= 7) // 1 eta 7 artean baliozko zenbakiak
                        {
                            switch (zmb) // Aukeratutako eragiketa
                            {
                                case 1:
                                    Program.Main(); // Atzera bueltatu
                                    break;
                                case 2:
                                    MaxDepartamentu(); // Maximoa departamentuzka ikusi
                                    break;
                                case 3:
                                    BatazbesteDepartamentu(); // Batazbestekoa departamentuzka ikusi
                                    break;
                                case 4:
                                    MaximoaSaltzaile(); // Maximoa saltzaileen arabera ikusi
                                    break;
                                case 5:
                                    BatazbesteSaltzaile(); // Batazbestekoa saltzaileen arabera ikusi
                                    break;
                                case 6:
                                    MaximoaData(); // Maximoa dataren arabera ikusi
                                    break;
                                case 7:
                                    BatazbesteData(); // Batazbestekoa dataren arabera ikusi
                                    break;
                                default:
                                    Console.WriteLine("Sartu agertzen den zenbaki bat"); // Baliozko zenbaki bat sartu
                                    break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("\r\n Sartu 1etik 7rako zenbaki bat."); // 1 eta 7 artean sartu
                            Console.WriteLine("Sakatu enter jarraitzeko.");
                            Console.ReadLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine("\r\n Sartu zenbaki bat."); // Zenbaki bat sartu
                        Console.WriteLine("Sakatu enter jarraitzeko.");
                        Console.ReadLine();
                    }
                }
                catch
                {
                    Console.WriteLine("Sartu agertzen den zenbaki bat."); // Errorea kudeatu
                }
            }
        }

        // XML artxiboak sortu eta backup-a egin
        private static void XMLpasaBackup()
        {
            List<Tiketa> tiketakt = new List<Tiketa>(); // Tiketa zerrenda
            // Departamentu bakoitzeko tiketak irakurri
            tiketakt.AddRange(HaragiDepartamentu.Tiketairakurri());
            tiketakt.AddRange(TxarkutegiDepartamentu.Tiketairakurri());
            tiketakt.AddRange(FrutaDepartamentu.Tiketairakurri());
            tiketakt.AddRange(OkindegiDepartamentu.Tiketairakurri());

            XMLitekue.XMLsortu(tiketakt); // XML artxiboa sortu

            // Backup-a egin
            BorrauTaBackup(@"Data/haragi/tickets", @"Backup/haragi/tickets");
            BorrauTaBackup(@"Data/txarkutei/tickets", @"Backup/txarkutei/tickets");
            BorrauTaBackup(@"Data/frutak/tickets", @"Backup/frutak/tickets");
            BorrauTaBackup(@"Data/panadeie/tickets", @"Backup/panadeie/tickets");

            Console.WriteLine("XML artxiboak sortu dira."); // Mezua
            Console.WriteLine("Sakatu enter jarraitzeko");
            Console.ReadLine();
        }

        // Backup-a egiteko metodoa
        private static void BorrauTaBackup(string direktoio, string backupnun)
        {
            if (!Directory.Exists(backupnun)) // Backup direktorioa existitzen ez bada
            {
                Directory.CreateDirectory(backupnun); // Sortu direktorioa
            }

            if (Directory.Exists(direktoio)) // Jatorrizko direktorioa existitzen bada
            {
                string[] artxibuk = Directory.GetFiles(direktoio); // Artxiboak irakurri
                foreach (string artxibo in artxibuk) // Artxibo bakoitza
                {
                    try
                    {
                        string izena = Path.GetFileName(artxibo); // Artxiboaren izena lortu
                        string noa = Path.Combine(backupnun, izena); // Backup direktorioan artxiboaren bidea
                        File.Move(artxibo, noa); // Artxiboa mugitu
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ezin izan da {artxibo} artxiboa backupera mugitu: {ex.Message}"); // Errorea kudeatu
                    }
                }
            }
            else
            {
                Console.WriteLine($"Ez da direktorioa billatu: {direktoio}"); // Direktorioa ez dela aurkitu adierazi
            }
        }

        // Maximoa departamentuzka ikusteko metodoa
        private static void MaxDepartamentu()
        {
            Console.Clear(); // Pantaila garbitu
            string KonexiyueStr = "Server=localhost;Database=supermarket;Uid=root;Pwd=12345678;"; // Databaserako konexio-lerroa
            using (MySqlConnection konek = new MySqlConnection(KonexiyueStr)) // Konexioa sortu
            {
                try
                {
                    konek.Open(); // Konektatu
                    Console.WriteLine("\r\nDatabasera konektatuta!\r\n");
                    string peti = "SELECT departamendua, MAX(totala) FROM tickets GROUP BY departamendua;"; // SQL kontsulta
                    using (MySqlCommand komandue = new MySqlCommand(peti, konek)) // Komandoa sortu
                    {
                        using (MySqlDataReader irakurri = komandue.ExecuteReader()) // Datuak irakurri
                        {
                            while (irakurri.Read()) // Datuak irakurtzen
                            {
                                Console.WriteLine("Departamendua: " + irakurri["departamendua"]); // Departamentua
                                Console.WriteLine("Max totala: " + irakurri["MAX(totala)"]); // Max totala
                                Console.WriteLine("");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Errorea:" + ex.Message); // Errorea kudeatu
                }
            }
            Console.WriteLine("Sakatu intro aurrera jarraitzeko."); // Mezua
            Console.ReadLine();
        }

        // Batazbestekoa departamentuzka ikusteko metodoa
        private static void BatazbesteDepartamentu()
        {
            Console.Clear(); // Pantaila garbitu
            string KonexiyueStr = "Server=localhost;Database=supermarket;Uid=root;Pwd=12345678;"; // Databaserako konexio-lerroa
            using (MySqlConnection konek = new MySqlConnection(KonexiyueStr)) // Konexioa sortu
            {
                try
                {
                    konek.Open(); // Konektatu
                    Console.WriteLine("\r\nConnected to the database\r\n");
                    string peti = "SELECT departamendua, AVG(totala) FROM tickets GROUP BY departamendua;"; // SQL kontsulta
                    using (MySqlCommand komandue = new MySqlCommand(peti, konek)) // Komandoa sortu
                    {
                        using (MySqlDataReader irakurri = komandue.ExecuteReader()) // Datuak irakurri
                        {
                            while (irakurri.Read()) // Datuak irakurtzen
                            {
                                Console.WriteLine("Departamendua: " + irakurri["departamendua"]); // Departamentua
                                Console.WriteLine("Batazbestekoa: " + irakurri["AVG(totala)"]); // Batazbestekoa
                                Console.WriteLine("");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Errorea:" + ex.Message); // Errorea kudeatu
                }
            }
            Console.WriteLine("Sakatu intro aurrera jarraitzeko."); // Mezua
            Console.ReadLine();
        }

        // Maximoa saltzaileen arabera ikusteko metodoa
        private static void MaximoaSaltzaile()
        {
            Console.Clear(); // Pantaila garbitu
            string KonexiyueStr = "Server=localhost;Database=supermarket;Uid=root;Pwd=12345678;"; // Databaserako konexio-lerroa
            using (MySqlConnection konek = new MySqlConnection(KonexiyueStr)) // Konexioa sortu
            {
                try
                {
                    konek.Open(); // Konektatu
                    Console.WriteLine("\r\nConnected to the database\r\n");
                    string peti = "SELECT saltzaile_izena, MAX(totala) FROM tickets GROUP BY saltzaile_izena;"; // SQL kontsulta
                    using (MySqlCommand komandue = new MySqlCommand(peti, konek)) // Komandoa sortu
                    {
                        using (MySqlDataReader irakurri = komandue.ExecuteReader()) // Datuak irakurri
                        {
                            while (irakurri.Read()) // Datuak irakurtzen
                            {
                                Console.WriteLine("Saltzaillea: " + irakurri["saltzaile_izena"]); // Saltzailea
                                Console.WriteLine("Max totala: " + irakurri["MAX(totala)"]); // Max totala
                                Console.WriteLine("");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Errorea:" + ex.Message); // Errorea kudeatu
                }
            }
            Console.WriteLine("Sakatu intro aurrera jarraitzeko."); // Mezua
            Console.ReadLine();
        }

        // Batazbestekoa saltzaileen arabera ikusteko metodoa
        private static void BatazbesteSaltzaile()
        {
            Console.Clear(); // Pantaila garbitu
            string KonexiyueStr = "Server=localhost;Database=supermarket;Uid=root;Pwd=12345678;"; // Databaserako konexio-lerroa
            using (MySqlConnection konek = new MySqlConnection(KonexiyueStr)) // Konexioa sortu
            {
                try
                {
                    konek.Open(); // Konektatu
                    Console.WriteLine("\r\nConnected to the database\r\n");
                    string peti = "SELECT saltzaile_izena, AVG(totala) FROM tickets GROUP BY saltzaile_izena;"; // SQL kontsulta
                    using (MySqlCommand komandue = new MySqlCommand(peti, konek)) // Komandoa sortu
                    {
                        using (MySqlDataReader irakurri = komandue.ExecuteReader()) // Datuak irakurri
                        {
                            while (irakurri.Read()) // Datuak irakurtzen
                            {
                                Console.WriteLine("Saltzaile: " + irakurri["saltzaile_izena"]); // Saltzailea
                                Console.WriteLine("Avg total: " + irakurri["AVG(totala)"]); // Batazbestekoa
                                Console.WriteLine("");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Errorea:" + ex.Message); // Errorea kudeatu
                }
            }
            Console.WriteLine("Sakatu intro aurrera jarraitzeko."); // Mezua
            Console.ReadLine();
        }

        // Maximoa dataren arabera ikusteko metodoa
        private static void MaximoaData()
        {
            Console.Clear(); // Pantaila garbitu
            string KonexiyueStr = "Server=localhost;Database=supermarket;Uid=root;Pwd=12345678;"; // Databaserako konexio-lerroa
            using (MySqlConnection konek = new MySqlConnection(KonexiyueStr)) // Konexioa sortu
            {
                try
                {
                    konek.Open(); // Konektatu
                    Console.WriteLine("\r\nConnected to the database\r\n");
                    string peti = "SELECT date, MAX(totala) FROM tickets GROUP BY date;"; // SQL kontsulta
                    using (MySqlCommand komandue = new MySqlCommand(peti, konek)) // Komandoa sortu
                    {
                        using (MySqlDataReader irakurri = komandue.ExecuteReader()) // Datuak irakurri
                        {
                            while (irakurri.Read()) // Datuak irakurtzen
                            {
                                Console.WriteLine("Data: " + irakurri["date"]); // Data
                                Console.WriteLine("Max totala: " + irakurri["MAX(totala)"]); // Max totala
                                Console.WriteLine("");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Errorea:" + ex.Message); // Errorea kudeatu
                }
            }
            Console.WriteLine("Sakatu intro aurrera jarraitzeko."); // Mezua
            Console.ReadLine();
        }

        // Batazbestekoa dataren arabera ikusteko metodoa
        private static void BatazbesteData()
        {
            Console.Clear(); // Pantaila garbitu
            string KonexiyueStr = "Server=localhost;Database=supermarket;Uid=root;Pwd=12345678;"; // Databaserako konexio-lerroa
            using (MySqlConnection konek = new MySqlConnection(KonexiyueStr)) // Konexioa sortu
            {
                try
                {
                    konek.Open(); // Konektatu
                    Console.WriteLine("\r\nConnected to the database\r\n");
                    string peti = "SELECT date, AVG(totala) FROM tickets GROUP BY date;"; // SQL kontsulta
                    using (MySqlCommand komandue = new MySqlCommand(peti, konek)) // Komandoa sortu
                    {
                        using (MySqlDataReader irakurri = komandue.ExecuteReader()) // Datuak irakurri
                        {
                            while (irakurri.Read()) // Datuak irakurtzen
                            {
                                Console.WriteLine("Data: " + irakurri["date"]); // Data
                                Console.WriteLine("Batazbestekoa: " + irakurri["AVG(totala)"]); // Batazbestekoa
                                Console.WriteLine("");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Errorea:" + ex.Message); // Errorea kudeatu
                }
            }
            Console.WriteLine("Sakatu intro aurrera jarraitzeko."); // Mezua
            Console.ReadLine();
        }
    }
}