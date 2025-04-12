using System;
using System.IO;
using Newtonsoft.Json;

namespace LetraT
{
    public class Serializador
    {
        public static void SerializarObjeto<T>(T objeto, string rutaArchivo)
        {
            try
            {
                string json = JsonConvert.SerializeObject(objeto, Formatting.Indented);
                File.WriteAllText(rutaArchivo, json);
                Console.WriteLine($"Objeto serializado y guardado en el archivo: {rutaArchivo}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al serializar el objeto: {ex.Message}");
            }
        }

        public static T DeserializarObjeto<T>(string rutaArchivo)
        {
            try
            {
                string json = File.ReadAllText(rutaArchivo);
                T objeto = JsonConvert.DeserializeObject<T>(json);
                Console.WriteLine($"Objeto deserializado desde el archivo: {rutaArchivo}");
                return objeto;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al deserializar el objeto: {ex.Message}");
                return default(T);
            }
        }
    }
}
