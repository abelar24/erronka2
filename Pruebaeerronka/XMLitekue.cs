using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace TiketBAI
{
    public class XMLitekue
    {
        // Tiketa objektuen zerrenda bat hartzen duen metodo estatikoa eta XML fitxategiak sortzen dituena
        public static void XMLsortu(List<Tiketa> tiketakt)
        {
            // XML fitxategiak gordetzeko direktorioa
            string direktoiu = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TicketBAI_resources", "xmls");
            // Direktorioa existitzen ez bada, sortzen du
            if (!Directory.Exists(direktoiu))
            {
                Directory.CreateDirectory(direktoiu);
            }

            // Fakturen XML fitxategiaren bidea
            string facturasNun = Path.Combine(direktoiu, "facturas.xml");

            XDocument facturasDok;
            // Fakturen XML fitxategia existitzen bada, kargatzen du; bestela, berria sortzen du
            if (File.Exists(facturasNun))
            {
                facturasDok = XDocument.Load(facturasNun);
            }
            else
            {
                facturasDok = new XDocument(new XElement("Facturas"));
            }

            // Tiketa bakoitza iteratzen du
            foreach (Tiketa tiketa in tiketakt)
            {
                try
                {
                    // Tiketaren IDa erabiliz XML fitxategiaren izena sortzen du
                    string izenaa = $"{tiketa.Id}.xml";
                    string nunn = Path.Combine(direktoiu, izenaa);
                    // XML fitxategia idazten du
                    using (StreamWriter writer = new StreamWriter(nunn))
                    {
                        writer.WriteLine("<Faktura>");
                        writer.WriteLine("    <Ticketa>");
                        writer.WriteLine("        <Baskula>" + tiketa.Produktu.Department + "</Baskula>");
                        writer.WriteLine("        <Produktua>" + tiketa.Produktu.Name + "</Produktua>");
                        writer.WriteLine("        <Saltzailea>" + tiketa.Produktu.Saltzaile.Code + "</Saltzailea>");
                        writer.WriteLine("        <PrezioKiloko>" + tiketa.Produktu.Preziokiloko + "</PrezioKiloko>");
                        writer.WriteLine("        <Pisua>" + tiketa.Produktu.Pisua + "</Pisua>");
                        writer.WriteLine("        <Totala>" + tiketa.Produktu.Totala + "</Totala>");
                        writer.WriteLine("        <TicketZenbakia>" + tiketa.Id + "</TicketZenbakia>");
                        writer.WriteLine("        <Eguna>" + tiketa.Data.ToString("dd/MM/yyyy") + "</Eguna>");
                        writer.WriteLine("        <Ordua>" + tiketa.Time.ToString("HH:mm") + "</Ordua>");
                        writer.WriteLine("    </Ticketa>");
                        writer.WriteLine("</Faktura>");
                    }
                    // XML fitxategia sortu dela adierazten duen mezu bat erakusten du
                    Console.WriteLine($"XML artxiboa sortuta, ticket ID: {tiketa.Id}");

                    // Fakturen XML dokumentuan fitxategi berriaren erreferentzia gehitzen du
                    facturasDok.Root.Add(new XElement("InvoiceFile", izenaa));
                }
                catch (Exception ex)
                {
                    // Errorea gertatzen bada, mezu bat erakusten du
                    Console.WriteLine($"Ezin izan da XML artxiboa sortu, tiket ID: {tiketa.Id}, Errorea: {ex.Message}");
                }
            }

            // Fakturen XML dokumentua gordetzen du
            facturasDok.Save(facturasNun);
        }
    }
}
