using System;
using System.Collections.Generic;
using OpenTK;

namespace LetraU
{
    /// <summary>
    /// Clase que representa una animación individual con un valor inicial, final, duración y tipo de interpolación
    /// </summary>
    public class Animacion
    {
        // Tipo de animación
        public enum TipoAnimacion
        {
            Traslacion,
            Rotacion,
            Escala
        }

        // Tipo de interpolación
        public enum TipoInterpolacion
        {
            Lineal,
            SuavizadoInicial,  // Ease-in
            SuavizadoFinal,    // Ease-out
            SuavizadoCompleto  // Ease-in-out
        }

        // Propiedades de la animación
        public TipoAnimacion Tipo { get; private set; }
        public Vector3 ValorInicial { get; private set; }
        public Vector3 ValorFinal { get; private set; }
        public float TiempoInicio { get; private set; }
        public float Duracion { get; private set; }
        public TipoInterpolacion Interpolacion { get; private set; }
        public string NombreObjeto { get; private set; }
        public string NombreParte { get; private set; } // Puede ser null si la animación es para un objeto completo

        // Constructor para animaciones de objetos
        public Animacion(string nombreObjeto, TipoAnimacion tipo, Vector3 valorInicial, Vector3 valorFinal, 
                        float tiempoInicio, float duracion, TipoInterpolacion interpolacion = TipoInterpolacion.Lineal)
        {
            NombreObjeto = nombreObjeto;
            NombreParte = null;
            Tipo = tipo;
            ValorInicial = valorInicial;
            ValorFinal = valorFinal;
            TiempoInicio = tiempoInicio;
            Duracion = duracion;
            Interpolacion = interpolacion;
        }

        // Constructor para animaciones de partes específicas
        public Animacion(string nombreObjeto, string nombreParte, TipoAnimacion tipo, Vector3 valorInicial, Vector3 valorFinal, 
                        float tiempoInicio, float duracion, TipoInterpolacion interpolacion = TipoInterpolacion.Lineal)
            : this(nombreObjeto, tipo, valorInicial, valorFinal, tiempoInicio, duracion, interpolacion)
        {
            NombreParte = nombreParte;
        }

        // Método para calcular el valor actual de la animación en un tiempo específico
        public Vector3 CalcularValorActual(float tiempoActual)
        {
            // Si no ha comenzado la animación
            if (tiempoActual < TiempoInicio)
                return ValorInicial;

            // Si ha terminado la animación
            if (tiempoActual > TiempoInicio + Duracion)
                return ValorFinal;

            // Calcular el progreso de la animación (de 0 a 1)
            float progresoLineal = (tiempoActual - TiempoInicio) / Duracion;
            float progresoInterpolado;

            // Aplicar la interpolación correspondiente
            switch (Interpolacion)
            {
                case TipoInterpolacion.SuavizadoInicial:
                    progresoInterpolado = progresoLineal * progresoLineal;
                    break;
                case TipoInterpolacion.SuavizadoFinal:
                    progresoInterpolado = 1 - (1 - progresoLineal) * (1 - progresoLineal);
                    break;
                case TipoInterpolacion.SuavizadoCompleto:
                    if (progresoLineal < 0.5f)
                        progresoInterpolado = 2 * progresoLineal * progresoLineal;
                    else
                        progresoInterpolado = 1 - Math.Pow(-2 * progresoLineal + 2, 2) / 2;
                    break;
                default: // Lineal
                    progresoInterpolado = progresoLineal;
                    break;
            }

            // Calcular la interpolación entre el valor inicial y final
            return new Vector3(
                ValorInicial.X + (ValorFinal.X - ValorInicial.X) * progresoInterpolado,
                ValorInicial.Y + (ValorFinal.Y - ValorInicial.Y) * progresoInterpolado,
                ValorInicial.Z + (ValorFinal.Z - ValorInicial.Z) * progresoInterpolado
            );
        }

        // Método para verificar si la animación está activa en un tiempo específico
        public bool EstaActiva(float tiempoActual)
        {
            return tiempoActual >= TiempoInicio && tiempoActual <= TiempoInicio + Duracion;
        }

        // Método para verificar si la animación ha terminado
        public bool HaTerminado(float tiempoActual)
        {
            return tiempoActual > TiempoInicio + Duracion;
        }

        // Método para obtener el tiempo de finalización de la animación
        public float ObtenerTiempoFin()
        {
            return TiempoInicio + Duracion;
        }
    }
}