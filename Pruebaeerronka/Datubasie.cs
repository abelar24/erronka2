using System;
using System.Collections.Generic;
using MySqlConnector;

namespace TiketBAI
{
    public class Datubasie
    {
        // Datu-basearekin konexiorako katearen aldagaia
        private string KonexiyueStr;

        // Eraikitzailea: konexio katearekin objektua sortu
        public Datubasie(string KonexiyueStr)
        {
            this.KonexiyueStr = KonexiyueStr;
        }

        // Tiketak datu-basean sartzeko metodoa
        public string TiketakSartu(List<Tiketa> tiketakt)
        {
            // MySQL konexioa sortu konexio katearekin
            using (MySqlConnection konexioa = new MySqlConnection(KonexiyueStr))
            {
                try
                {
                    // Konexioa ireki
                    konexioa.Open();

                    // SQL kontsulta prestatu, tiketaren datuak sartzeko
                    string petiziyue = "INSERT INTO tickets (id, date, time, departamendua, produktu_izena, saltzaile_id, saltzaile_izena, preziokiloko, pisua, totala) VALUES (@id, @date, @time, @departamendua, @produktu_izena, @saltzaile_id, @saltzaile_izena, @preziokiloko, @pisua, @totala)";

                    // Tiketen zerrenda iteratu eta bakoitza datu-basean sartu
                    foreach (Tiketa tiketa in tiketakt)
                    {
                        using (MySqlCommand komanduin = new MySqlCommand(petiziyue, konexioa))
                        {
                            // Parametroak bete
                            komanduin.Parameters.AddWithValue("@id", tiketa.Id);
                            komanduin.Parameters.AddWithValue("@date", tiketa.Data);
                            komanduin.Parameters.AddWithValue("@time", tiketa.Time);
                            komanduin.Parameters.AddWithValue("@departamendua", tiketa.Produktu.Department);
                            komanduin.Parameters.AddWithValue("@produktu_izena", tiketa.Produktu.Name);
                            komanduin.Parameters.AddWithValue("@saltzaile_id", tiketa.Produktu.Saltzaile.Id);
                            komanduin.Parameters.AddWithValue("@saltzaile_izena", tiketa.Produktu.Saltzaile.Name);
                            komanduin.Parameters.AddWithValue("@preziokiloko", tiketa.Produktu.Preziokiloko);
                            komanduin.Parameters.AddWithValue("@pisua", tiketa.Produktu.Pisua);
                            komanduin.Parameters.AddWithValue("@totala", tiketa.Produktu.Totala);

                            // Kontsulta exekutatu datuak sartzeko
                            komanduin.ExecuteNonQuery();
                        }
                    }

                    // Mezua itzuli datuak ondo gorde direla adierazteko
                    return "\r\nDatuak ondo gorde dira!";
                }
                catch (Exception ex)
                {
                    // Errore bat gertatzen bada, errore mezua erakutsi
                    Console.WriteLine("Errorea:" + ex.Message);
                    return "Errorea";
                }
            }
        }
    }
}
