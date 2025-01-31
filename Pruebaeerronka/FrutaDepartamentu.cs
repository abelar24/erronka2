using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

namespace TiketBAI
{
    public class FrutaDepartamentu
    {
        // Método estático que lee los tickets y devuelve una lista de objetos Tiketa
        public static List<Tiketa> Tiketairakurri()
        {
            // Lista para almacenar el contenido de los archivos de tickets
            List<string> ticketahut = new List<string>();
            // Lista para almacenar los objetos Tiketa creados
            List<Tiketa> tiketakt = new List<Tiketa>();
            // Directorio donde se encuentran los archivos de tickets
            string direktoiu = @"Data/frutak/tickets";

            // Verifica si el directorio existe
            if (Directory.Exists(direktoiu))
            {
                // Obtiene todos los archivos en el directorio
                string[] artxiboak = Directory.GetFiles(direktoiu);

                // Si no hay archivos en el directorio, muestra un mensaje
                if (artxiboak.Length == 0)
                {
                    Console.WriteLine($"Ez da fitxategirik aurkitu: {direktoiu}");
                }
                else
                {
                    // Itera sobre cada archivo en el directorio
                    foreach (string artxibo in artxiboak)
                    {
                        // Lee el contenido del archivo y lo recorta
                        string infor = File.ReadAllText(artxibo).Trim();
                        // Si el contenido está vacío, muestra un mensaje
                        if (string.IsNullOrEmpty(infor))
                        {
                            Console.WriteLine($"Hutsa: {artxibo}");
                        }
                        else
                        {
                            // Muestra un mensaje indicando que se está leyendo el archivo
                            Console.WriteLine($"Fitxategia irakurtzen: {artxibo}");
                            // Añade el contenido del archivo a la lista
                            ticketahut.Add(infor);
                        }
                    }

                    int kontadorie = 0;
                    // Itera sobre cada ticket en la lista de contenidos de archivos
                    foreach (string ticket in ticketahut)
                    {
                        // Divide el contenido del ticket en partes usando el carácter '$'
                        string[] parts = ticket.Split('$');
                        // Si el formato del ticket no es válido, muestra un mensaje y continúa con el siguiente ticket
                        if (parts.Length < 5)
                        {
                            Console.WriteLine($"Tiket formatua ez da onartu: {artxiboak[kontadorie]}");
                            continue;
                        }

                        try
                        {
                            int saltzailleId;
                            string saltzaileCode = parts[1].Trim();
                            string saltzaileIzena = parts[1].Trim();
                            // Verifica si el código del vendedor es "autosalmenta"
                            if (saltzaileCode.ToLower() == "autosalmenta")
                            {
                                saltzailleId = 0;
                                saltzaileCode = "000";
                                saltzaileIzena = "Autosalmenta";
                            }
                            else
                            {
                                // Convierte el código del vendedor a un entero
                                saltzailleId = int.Parse(saltzaileCode);
                            }

                            // Convierte las partes del ticket a los tipos de datos correspondientes
                            double preziokiloko = double.Parse(parts[2].Trim().Split('\n')[0].Replace(',', '.'), CultureInfo.InvariantCulture);
                            double pisua = double.Parse(parts[3].Trim().Replace(',', '.'), CultureInfo.InvariantCulture);
                            double totala = double.Parse(parts[4].Trim().Replace(',', '.'), CultureInfo.InvariantCulture);

                            // Crea un objeto Producto con los datos del ticket
                            Produktu produktu = new Produktu(parts[0].Trim(), new Saltzaile(saltzailleId, saltzaileCode, saltzaileIzena), preziokiloko, pisua, totala, "frutak");
                            // Obtiene el ID del ticket a partir del nombre del archivo
                            string id = artxiboak[kontadorie].Substring(direktoiu.Length + 1, 14);
                            // Divide el ID en la parte de la fecha y la parte de la hora
                            string datapartea = id.Substring(0, 8);
                            string denborapartea = id.Substring(8);
                            // Convierte las partes de la fecha y la hora a los tipos de datos correspondientes
                            DateOnly data = DateOnly.ParseExact(datapartea, "yyyyMMdd");
                            TimeOnly time = TimeOnly.ParseExact(denborapartea, "HHmmss");
                            // Crea un objeto Tiketa con los datos del ticket
                            Tiketa tiketa = new Tiketa(id, data, time, produktu);
                            // Añade el objeto Tiketa a la lista
                            tiketakt.Add(tiketa);

                            // Directorio de respaldo para los archivos de tickets
                            string backupnun = @"Backup/frutak/tickets";
                            // Verifica si el directorio de respaldo existe, si no, lo crea
                            if (!Directory.Exists(backupnun))
                            {
                                Directory.CreateDirectory(backupnun);
                            }
                            // Mueve el archivo de ticket al directorio de respaldo
                            string noa = Path.Combine(backupnun, Path.GetFileName(artxiboak[kontadorie]));
                            File.Move(artxiboak[kontadorie], noa);
                        }
                        catch (FormatException ex)
                        {
                            // Muestra un mensaje de error si ocurre una excepción de formato
                            Console.WriteLine($"Errorea: {artxiboak[kontadorie]}, Error: {ex.Message}");
                        }

                        kontadorie++;
                    }
                }
            }
            else
            {
                // Muestra un mensaje si el directorio no existe
                Console.WriteLine($"Ez da bilatu: {direktoiu}");
            }

            // Devuelve la lista de objetos Tiketa
            return tiketakt;
        }
    }
}
