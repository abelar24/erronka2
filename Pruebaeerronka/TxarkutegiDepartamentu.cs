using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

namespace TiketBAI
{
    public class TxarkutegiDepartamentu
    {
        // Tiketak irakurtzen dituen metodo estatikoa eta Tiketa objektuen zerrenda bat itzultzen duena
        public static List<Tiketa> Tiketairakurri()
        {
            // Tiketen fitxategien edukia gordetzeko zerrenda
            List<string> ticketahut = new List<string>();
            // Tiketa objektuak gordetzeko zerrenda
            List<Tiketa> tiketakt = new List<Tiketa>();
            // Tiketen fitxategiak dauden direktorioa
            string direktoiu = @"Data/txarkutei/tickets";

            // Direktorioa existitzen den egiaztatzen du
            if (Directory.Exists(direktoiu))
            {
                // Direktorioko fitxategi guztiak lortzen ditu
                string[] artxiboak = Directory.GetFiles(direktoiu);

                // Direktorioko fitxategirik ez badago, mezu bat erakusten du
                if (artxiboak.Length == 0)
                {
                    Console.WriteLine($"Ez da fitxategirik aurkitu: {direktoiu}");
                }
                else
                {
                    // Direktorioko fitxategi bakoitza iteratzen du
                    foreach (string artxibo in artxiboak)
                    {
                        // Fitxategiaren edukia irakurtzen du eta mozten du
                        string infor = File.ReadAllText(artxibo).Trim();
                        // Edukia hutsik badago, mezu bat erakusten du
                        if (string.IsNullOrEmpty(infor))
                        {
                            Console.WriteLine($"Hutsa: {artxibo}");
                        }
                        else
                        {
                            // Fitxategia irakurtzen ari dela adierazten duen mezu bat erakusten du
                            Console.WriteLine($"Fitxategia irakurtzen: {artxibo}");
                            // Fitxategiaren edukia zerrendara gehitzen du
                            ticketahut.Add(infor);
                        }
                    }

                    int kontadorie = 0;
                    // Tiketen edukien zerrenda iteratzen du
                    foreach (string ticket in ticketahut)
                    {
                        // Tiketaren edukia zatitzen du '$' karakterea erabiliz
                        string[] parts = ticket.Split('$');
                        // Tiketaren formatua ez bada zuzena, mezu bat erakusten du eta hurrengo tiketarekin jarraitzen du
                        if (parts.Length < 5)
                        {
                            Console.WriteLine($"Tiket formatua ez da onartu: {artxiboak[kontadorie]}");
                            continue;
                        }

                        try
                        {
                            // Autosalmenta kasua kudeatzen du
                            int saltzailleId;
                            string saltzaileCode = parts[1].Trim();
                            string saltzaileIzena = parts[1].Trim();
                            if (saltzaileCode.ToLower() == "autosalmenta")
                            {
                                saltzailleId = 0;
                                saltzaileCode = "000";
                                saltzaileIzena = "Autosalmenta";
                            }
                            else
                            {
                                // Saltzailearen kodea zenbaki bihurtzen du
                                saltzailleId = int.Parse(saltzaileCode);
                            }

                            // Tiketaren zatiak datu mota egokietara bihurtzen ditu
                            double preziokiloko = double.Parse(parts[2].Trim().Split('\n')[0].Replace(',', '.'), CultureInfo.InvariantCulture);
                            double pisua = double.Parse(parts[3].Trim().Replace(',', '.'), CultureInfo.InvariantCulture);
                            double totala = double.Parse(parts[4].Trim().Replace(',', '.'), CultureInfo.InvariantCulture);

                            // Produktu objektu bat sortzen du tiketako datuekin
                            Produktu produktu = new Produktu(parts[0].Trim(), new Saltzaile(saltzailleId, saltzaileCode, saltzaileIzena), preziokiloko, pisua, totala, "txarkutei");
                            // Tiketaren IDa lortzen du fitxategiaren izenetik
                            string id = artxiboak[kontadorie].Substring(direktoiu.Length + 1, 14);
                            // IDa zatitzen du data eta ordu zatietan
                            string datapartea = id.Substring(0, 8);
                            string denborapartea = id.Substring(8);
                            // Data eta ordu zatiak datu mota egokietara bihurtzen ditu
                            DateOnly data = DateOnly.ParseExact(datapartea, "yyyyMMdd");
                            TimeOnly time = TimeOnly.ParseExact(denborapartea, "HHmmss");
                            // Tiketa objektu bat sortzen du tiketako datuekin
                            Tiketa tiketa = new Tiketa(id, data, time, produktu);
                            // Tiketa objektua zerrendara gehitzen du
                            tiketakt.Add(tiketa);

                            // Tiketen fitxategien babeskopia egiteko direktorioa
                            string backupnun = @"Backup/txarkutei/tickets";
                            // Babeskopia direktorioa existitzen den egiaztatzen du, ez badago, sortzen du
                            if (!Directory.Exists(backupnun))
                            {
                                Directory.CreateDirectory(backupnun);
                            }
                            // Tiketen fitxategia babeskopia direktorioan mugitzen du
                            string noa = Path.Combine(backupnun, Path.GetFileName(artxiboak[kontadorie]));
                            File.Move(artxiboak[kontadorie], noa);
                        }
                        catch (FormatException ex)
                        {
                            // Formatu salbuespena gertatzen bada, errore mezu bat erakusten du
                            Console.WriteLine($"Errorea: {artxiboak[kontadorie]}, Error: {ex.Message}");
                        }

                        kontadorie++;
                    }
                }
            }
            else
            {
                // Direktorioa existitzen ez bada, mezu bat erakusten du
                Console.WriteLine($"Ez da bilatu: {direktoiu}");
            }

            // Tiketa objektuen zerrenda itzultzen du
            return tiketakt;
        }
    }
}
