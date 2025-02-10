//garepen frogaa

using System;
using System.IO;
using TiketBAI;

namespace TiketBAI
{
    public class Backupe
    {
        public static void CreateBackup()
        {
            // Babeskopia egiteko beharrezko direktorioen izenak definitu
            string direktoio = @"XMLFiles\"; // Jatorrizko fitxategiak gordetzen diren karpeta
            string backupnun = @"Backup\"; // Babeskopia gordeko den karpeta

            // Backup direktorioa existitzen ez bada, sortu
            if (!Directory.Exists(backupnun))
            {
                Directory.CreateDirectory(backupnun);
            }

            // Direktorioan dauden fitxategi guztiak kopiatu backup karpetara
            foreach (var artxi in Directory.GetFiles(direktoio))
            {
                try
                {
                    // Fitxategiaren izena lortu
                    string izen = Path.GetFileName(artxi);
                    // Fitxategiaren helbide berria sortu backup karpetan
                    string noa = Path.Combine(backupnun, izen);
                    // Fitxategia kopiatu backup karpetara (baldin badago, ordezkatu)
                    File.Copy(artxi, noa, true);
                }
                catch (Exception ex)
                {
                    // Errore bat gertatuz gero, errore mezua erakutsi
                    Console.WriteLine($"Ezin izan da {artxi} artxiboa backupean gorde, errorea: {ex.Message}");
                }
            }

            // Amaieran, erabiltzaileari jakinarazi backup-a sortu dela
            Console.WriteLine("Backupa sortuta!");
        }
    }
}
