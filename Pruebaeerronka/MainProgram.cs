using System;
using System.Collections.Generic;
using System.IO;
using TiketBAI;

namespace SupermarketApp
{
    public class Program
    {
        public static void Main()
        {
            // Tiketa objektuen zerrendak sortzen dira departamentu bakoitzerako
            List<Tiketa> tiketakt = new List<Tiketa>();
            List<Tiketa> haragiDepartamentu = new List<Tiketa>();
            List<Tiketa> txarkutegiDepartamentu = new List<Tiketa>();
            List<Tiketa> frutaDepartamentu = new List<Tiketa>();
            List<Tiketa> okindegiDepartamentu = new List<Tiketa>();
            int kontadorie = 0;

            // Bucle infinitua erabiltzailearen aukerak kudeatzeko
            while (true)
            {
                Console.Clear();
                Console.WriteLine("");
                Console.WriteLine("\r\n /$$$$$$$$ /$$$$$$ /$$   /$$ /$$$$$$$$ /$$$$$$$$       /$$$$$$$   /$$$$$$  /$$$$$$\r\n|__  $$__/|_  $$_/| $$  /$$/| $$_____/|__  $$__/      | $$__  $$ /$$__  $$|_  $$_/\r\n   | $$     | $$  | $$ /$$/ | $$         | $$         | $$  \\ $$| $$  \\ $$  | $$  \r\n   | $$     | $$  | $$$$$/  | $$$$$      | $$         | $$$$$$$ | $$$$$$$$  | $$  \r\n   | $$     | $$  | $$  $$  | $$__/      | $$         | $$__  $$| $$__  $$  | $$  \r\n   | $$     | $$  | $$\\  $$ | $$         | $$         | $$  \\ $$| $$  | $$  | $$  \r\n   | $$    /$$$$$$| $$ \\  $$| $$$$$$$$   | $$         | $$$$$$$/| $$  | $$ /$$$$$$\r\n   |__/   |______/|__/  \\__/|________/   |__/         |_______/ |__/  |__/|______/\r\n                                                                                  \r\n                                                                                  \r\n                                                                                  \r\n");
                Console.WriteLine("1.- Tiketen prozesua");
                Console.WriteLine("2.- Estatistikak");
                Console.WriteLine("3.- Irten");
                Console.Write("Zein eragiketa egin nahi duzu?: ");
                string? sartutakue = Console.ReadLine();
                try
                {
                    // Erabiltzaileak sartutako balioa zenbaki bihurtzen saiatzen da
                    if (int.TryParse(sartutakue, out kontadorie))
                    {
                        // Aukera baliozkoa den egiaztatzen du
                        if (kontadorie >= 1 && kontadorie <= 3)
                        {
                            switch (kontadorie)
                            {
                                case 1:
                                    try
                                    {
                                        // Harategiko tiketak irakurtzen
                                        Console.WriteLine("Harategiko tiketak irakurtzen...");
                                        haragiDepartamentu = HaragiDepartamentu.Tiketairakurri();
                                        Console.WriteLine($"{haragiDepartamentu.Count} tiket gorde dira harategiko departamendutik.");

                                        // Txarkutegiko tiketak irakurtzen
                                        Console.WriteLine("Txarkutegiko tiketak irakurtzen...");
                                        txarkutegiDepartamentu = TxarkutegiDepartamentu.Tiketairakurri();
                                        Console.WriteLine($"{txarkutegiDepartamentu.Count} tiket gorde dira txarkutegiko departamendutik.");

                                        // Frutategiko tiketak irakurtzen
                                        Console.WriteLine("Frutategitik tiketak irakurtzen...");
                                        frutaDepartamentu = FrutaDepartamentu.Tiketairakurri();
                                        Console.WriteLine($"{frutaDepartamentu.Count} tiket gorde dira frutategiko departamendutik.");

                                        // Okindegiko tiketak irakurtzen
                                        Console.WriteLine("Okindegitik tiketak irakurtzen...");
                                        okindegiDepartamentu = OkindegiDepartamentu.Tiketairakurri();
                                        Console.WriteLine($"{okindegiDepartamentu.Count} tiket gorde dira okindegiko departamendutik.");

                                        // Tiketa guztiak zerrenda nagusian gehitzen dira
                                        tiketakt.AddRange(haragiDepartamentu);
                                        tiketakt.AddRange(txarkutegiDepartamentu);
                                        tiketakt.AddRange(frutaDepartamentu);
                                        tiketakt.AddRange(okindegiDepartamentu);
                                        Console.WriteLine($"{tiketakt.Count} tiket gorde dira guztira");

                                        // XML fitxategiak sortzen
                                        Console.WriteLine("XML fitxategiak sortzen...");
                                        XMLitekue.XMLsortu(tiketakt);
                                        Console.WriteLine("XML fitxategiak sortu dira.");

                                        // Datu basearekin konexioa eta tiketak sartzen
                                        string KonexiyueStr = "Server=localhost;Database=supermarket;Uid=root;Pwd=12345678;";
                                        Datubasie datubasie = new Datubasie(KonexiyueStr);

                                        string opa = datubasie.TiketakSartu(tiketakt);
                                        Console.WriteLine(opa);
                                    }
                                    catch (Exception ex)
                                    {
                                        // Errorea gertatzen bada, mezu bat erakusten du
                                        Console.WriteLine("Errorea: " + ex.Message);
                                    }
                                    Console.WriteLine("Sakatu intro aurrera jarraitzeko.");
                                    Console.ReadLine();
                                    break;
                                case 2:
                                    try
                                    {
                                        // Estatistikak ikusten
                                        Estatistikak.View();
                                    }
                                    catch (Exception ex)
                                    {
                                        // Errorea gertatzen bada, mezu bat erakusten du
                                        Console.WriteLine("Errorea: " + ex.Message);
                                    }
                                    Console.WriteLine("Sakatu intro aurrera jarraitzeko.");
                                    Console.ReadLine();
                                    break;
                                case 3:
                                    try
                                    {
                                        // Babeskopia sortzen
                                        Backupe.CreateBackup();
                                        Console.WriteLine("\r\nBackupa eginda.");
                                    }
                                    catch (Exception ex)
                                    {
                                        // Errorea gertatzen bada, mezu bat erakusten du
                                        Console.WriteLine("Errorea: " + ex.Message);
                                    }
                                    Console.WriteLine("Sakatu intro irteteko");
                                    Console.ReadLine();
                                    Environment.Exit(0);
                                    break;
                                default:
                                    // Aukera ez baliozkoa bada, mezu bat erakusten du
                                    Console.WriteLine("Ez da onartu operazioa");
                                    Console.WriteLine("Sakatu intro aurrera jarraitzeko.");
                                    Console.ReadLine();
                                    break;
                            }
                        }
                        else
                        {
                            // Aukera ez baliozkoa bada, mezu bat erakusten du
                            Console.WriteLine("\r\n Sartu 1etik 3rako zenbaki bat");
                            Console.WriteLine("Sakatu intro aurrera jarraitzeko.");
                            Console.ReadLine();
                        }
                    }
                    else
                    {
                        // Sartutako balioa zenbakia ez bada, mezu bat erakusten du
                        Console.WriteLine("\r\n Sartu zenbaki bat");
                        Console.WriteLine("Sakatu intro aurrera jarraitzeko.");
                        Console.ReadLine();
                    }
                }
                catch
                {
                    // Salbuespena gertatzen bada, mezu bat erakusten du
                    Console.WriteLine("Sartu zenbaki bat");
                }
            }
        }
    }
}
