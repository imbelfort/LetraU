using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;

namespace LetraU
{
    /// <summary>
    /// Clase que gestiona todas las animaciones como un guion o libreto completo
    /// </summary>
    public class Libreto
    {
        private List<Animacion> animaciones;
        private float tiempoComienzo;
        private float tiempoActual;
        private float tiempoTotal;
        private bool reproduciendo;
        private bool enBucle;

        /// <summary>
        /// Constructor del libreto de animaciones
        /// </summary>
        public Libreto()
        {
            animaciones = new List<Animacion>();
            ResetearTiempo();
            reproduciendo = false;
            enBucle = false;
        }

        /// <summary>
        /// Resetea los tiempos de reproducción
        /// </summary>
        public void ResetearTiempo()
        {
            tiempoComienzo = 0;
            tiempoActual = 0;
        }

        /// <summary>
        /// Agrega una nueva animación al libreto
        /// </summary>
        public void AgregarAnimacion(Animacion animacion)
        {
            animaciones.Add(animacion);
            
            // Actualizar el tiempo total del libreto si es necesario
            float tiempoFinAnimacion = animacion.ObtenerTiempoFin();
            if (tiempoFinAnimacion > tiempoTotal)
            {
                tiempoTotal = tiempoFinAnimacion;
            }
        }

        /// <summary>
        /// Agrega una secuencia de animaciones que deben ejecutarse en orden
        /// </summary>
        public void AgregarSecuencia(List<Animacion> secuenciaAnimaciones)
        {
            foreach (var animacion in secuenciaAnimaciones)
            {
                AgregarAnimacion(animacion);
            }
        }

        /// <summary>
        /// Crea una animación de traslación y la añade al libreto
        /// </summary>
        public void AgregarAnimacionTraslacion(string nombreObjeto, Vector3 desde, Vector3 hasta, 
                                              float tiempoInicio, float duracion, 
                                              Animacion.TipoInterpolacion interpolacion = Animacion.TipoInterpolacion.Lineal)
        {
            AgregarAnimacion(new Animacion(
                nombreObjeto, 
                Animacion.TipoAnimacion.Traslacion, 
                desde, 
                hasta, 
                tiempoInicio, 
                duracion, 
                interpolacion));
        }

        /// <summary>
        /// Crea una animación de rotación y la añade al libreto
        /// </summary>
        public void AgregarAnimacionRotacion(string nombreObjeto, Vector3 desde, Vector3 hasta, 
                                            float tiempoInicio, float duracion, 
                                            Animacion.TipoInterpolacion interpolacion = Animacion.TipoInterpolacion.Lineal)
        {
            AgregarAnimacion(new Animacion(
                nombreObjeto, 
                Animacion.TipoAnimacion.Rotacion, 
                desde, 
                hasta, 
                tiempoInicio, 
                duracion, 
                interpolacion));
        }

        /// <summary>
        /// Crea una animación de escala y la añade al libreto
        /// </summary>
        public void AgregarAnimacionEscala(string nombreObjeto, Vector3 desde, Vector3 hasta, 
                                          float tiempoInicio, float duracion, 
                                          Animacion.TipoInterpolacion interpolacion = Animacion.TipoInterpolacion.Lineal)
        {
            AgregarAnimacion(new Animacion(
                nombreObjeto, 
                Animacion.TipoAnimacion.Escala, 
                desde, 
                hasta, 
                tiempoInicio, 
                duracion, 
                interpolacion));
        }

        /// <summary>
        /// Crea una animación para una parte específica de un objeto y la añade al libreto
        /// </summary>
        public void AgregarAnimacionParte(string nombreObjeto, string nombreParte, 
                                         Animacion.TipoAnimacion tipo, Vector3 desde, Vector3 hasta, 
                                         float tiempoInicio, float duracion, 
                                         Animacion.TipoInterpolacion interpolacion = Animacion.TipoInterpolacion.Lineal)
        {
            AgregarAnimacion(new Animacion(
                nombreObjeto, 
                nombreParte, 
                tipo, 
                desde, 
                hasta, 
                tiempoInicio, 
                duracion, 
                interpolacion));
        }

        /// <summary>
        /// Inicia la reproducción del libreto de animaciones
        /// </summary>
        public void Iniciar(bool enBucle = false)
        {
            reproduciendo = true;
            this.enBucle = enBucle;
            tiempoComienzo = (float)DateTime.Now.TimeOfDay.TotalSeconds;
            tiempoActual = 0;
        }

        /// <summary>
        /// Pausa la reproducción del libreto
        /// </summary>
        public void Pausar()
        {
            reproduciendo = false;
        }

        /// <summary>
        /// Reanuda la reproducción del libreto desde donde se pausó
        /// </summary>
        public void Reanudar()
        {
            if (!reproduciendo)
            {
                tiempoComienzo = (float)DateTime.Now.TimeOfDay.TotalSeconds - tiempoActual;
                reproduciendo = true;
            }
        }

        /// <summary>
        /// Detiene la reproducción del libreto y reinicia al principio
        /// </summary>
        public void Detener()
        {
            reproduciendo = false;
            ResetearTiempo();
        }

        /// <summary>
        /// Actualiza el estado de todas las animaciones basado en el tiempo actual
        /// </summary>
        public void Actualizar(Escenario escenario)
        {
            if (!reproduciendo)
                return;

            // Actualizar el tiempo actual
            float tiempoReal = (float)DateTime.Now.TimeOfDay.TotalSeconds - tiempoComienzo;
            
            // Comprobar si debemos reiniciar en caso de bucle
            if (tiempoReal > tiempoTotal && enBucle)
            {
                tiempoComienzo = (float)DateTime.Now.TimeOfDay.TotalSeconds;
                tiempoReal = 0;
            }
            else if (tiempoReal > tiempoTotal && !enBucle)
            {
                // Si terminamos y no estamos en bucle, detener la reproducción
                Detener();
                return;
            }

            tiempoActual = tiempoReal;

            // Colecciones para acumular transformaciones por objeto/parte
            Dictionary<string, Dictionary<Animacion.TipoAnimacion, Vector3>> transformacionesObjetos = new Dictionary<string, Dictionary<Animacion.TipoAnimacion, Vector3>>();
            Dictionary<string, Dictionary<string, Dictionary<Animacion.TipoAnimacion, Vector3>>> transformacionesPartes = new Dictionary<string, Dictionary<string, Dictionary<Animacion.TipoAnimacion, Vector3>>>();

            // Procesar cada animación
            foreach (var animacion in animaciones)
            {
                // Si la animación está activa en este momento
                if (animacion.EstaActiva(tiempoActual))
                {
                    Vector3 valorActual = animacion.CalcularValorActual(tiempoActual);

                    // Si es una animación de parte específica
                    if (animacion.NombreParte != null)
                    {
                        // Inicializar diccionario si no existe
                        if (!transformacionesPartes.ContainsKey(animacion.NombreObjeto))
                        {
                            transformacionesPartes[animacion.NombreObjeto] = new Dictionary<string, Dictionary<Animacion.TipoAnimacion, Vector3>>();
                        }
                        
                        if (!transformacionesPartes[animacion.NombreObjeto].ContainsKey(animacion.NombreParte))
                        {
                            transformacionesPartes[animacion.NombreObjeto][animacion.NombreParte] = new Dictionary<Animacion.TipoAnimacion, Vector3>();
                        }

                        transformacionesPartes[animacion.NombreObjeto][animacion.NombreParte][animacion.Tipo] = valorActual;
                    }
                    // Si es una animación de objeto completo
                    else
                    {
                        // Inicializar diccionario si no existe
                        if (!transformacionesObjetos.ContainsKey(animacion.NombreObjeto))
                        {
                            transformacionesObjetos[animacion.NombreObjeto] = new Dictionary<Animacion.TipoAnimacion, Vector3>();
                        }

                        transformacionesObjetos[animacion.NombreObjeto][animacion.Tipo] = valorActual;
                    }
                }
            }

            // Aplicar todas las transformaciones acumuladas a los objetos
            foreach (var entry in transformacionesObjetos)
            {
                string nombreObjeto = entry.Key;
                var transformaciones = entry.Value;

                if (escenario.listaDeObjetos.ContainsKey(nombreObjeto))
                {
                    Objeto objeto = escenario.getObjeto(nombreObjeto);

                    // Aplicar cada tipo de transformación
                    if (transformaciones.ContainsKey(Animacion.TipoAnimacion.Traslacion))
                        objeto.Trasladar(transformaciones[Animacion.TipoAnimacion.Traslacion]);
                    
                    if (transformaciones.ContainsKey(Animacion.TipoAnimacion.Rotacion))
                        objeto.Rotar(transformaciones[Animacion.TipoAnimacion.Rotacion]);
                    
                    if (transformaciones.ContainsKey(Animacion.TipoAnimacion.Escala))
                        objeto.Escalar(transformaciones[Animacion.TipoAnimacion.Escala]);
                }
            }

            // Aplicar todas las transformaciones acumuladas a las partes de objetos
            foreach (var objetoEntry in transformacionesPartes)
            {
                string nombreObjeto = objetoEntry.Key;
                
                if (escenario.listaDeObjetos.ContainsKey(nombreObjeto))
                {
                    Objeto objeto = escenario.getObjeto(nombreObjeto);
                    
                    foreach (var parteEntry in objetoEntry.Value)
                    {
                        string nombreParte = parteEntry.Key;
                        var transformaciones = parteEntry.Value;
                        
                        if (objeto.listaDePartes.ContainsKey(nombreParte))
                        {
                            Partes parte = objeto.getParte(nombreParte);
                            
                            // Aplicar cada tipo de transformación
                            if (transformaciones.ContainsKey(Animacion.TipoAnimacion.Traslacion))
                                parte.Trasladar(transformaciones[Animacion.TipoAnimacion.Traslacion]);
                            
                            if (transformaciones.ContainsKey(Animacion.TipoAnimacion.Rotacion))
                                parte.Rotar(transformaciones[Animacion.TipoAnimacion.Rotacion]);
                            
                            if (transformaciones.ContainsKey(Animacion.TipoAnimacion.Escala))
                                parte.Escalar(transformaciones[Animacion.TipoAnimacion.Escala]);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Indica si el libreto está reproduciendo actualmente
        /// </summary>
        public bool EstaReproduciendo()
        {
            return reproduciendo;
        }

        /// <summary>
        /// Obtiene el tiempo total del libreto de animaciones
        /// </summary>
        public float ObtenerTiempoTotal()
        {
            return tiempoTotal;
        }

        /// <summary>
        /// Obtiene el tiempo actual de reproducción
        /// </summary>
        public float ObtenerTiempoActual()
        {
            return tiempoActual;
        }

        /// <summary>
        /// Obtiene el porcentaje de progreso de la animación (0-100)
        /// </summary>
        public float ObtenerPorcentajeProgreso()
        {
            if (tiempoTotal <= 0)
                return 0;
            
            return (tiempoActual / tiempoTotal) * 100;
        }

        /// <summary>
        /// Crea un libreto predefinido con animaciones para un auto
        /// </summary>
        public static Libreto CrearLibretoAuto(string nombreAuto)
        {
            Libreto libreto = new Libreto();
            
            // Ejemplo: Auto avanzando en línea recta
            libreto.AgregarAnimacionTraslacion(
                nombreAuto,
                new Vector3(-3, 0, 0),      // Posición inicial
                new Vector3(3, 0, 0),       // Posición final
                0,                          // Tiempo inicio
                5,                          // Duración
                Animacion.TipoInterpolacion.SuavizadoInicial);  // Comienza lento y acelera
            
            // Ejemplo: Auto girando sus ruedas
            if (nombreAuto != null)
            {
                // Suponiendo que el auto tiene partes llamadas "ruedaDelanteraIzquierda", etc.
                string[] ruedas = { "ruedaDelanteraIzquierda", "ruedaDelanteraDerecha", "ruedaTraseraIzquierda", "ruedaTrasera" };
                
                foreach (string rueda in ruedas)
                {
                    libreto.AgregarAnimacionParte(
                        nombreAuto,
                        rueda,
                        Animacion.TipoAnimacion.Rotacion,
                        new Vector3(0, 0, 0),        // Rotación inicial
                        new Vector3(0, 0, 1080),     // Rotación final (3 vueltas completas)
                        0,                           // Tiempo inicio
                        5,                           // Duración
                        Animacion.TipoInterpolacion.Lineal);
                }
            }
            
            // Ejemplo: Auto rebotando ligeramente
            libreto.AgregarAnimacionTraslacion(
                nombreAuto,
                new Vector3(0, 0, 0),          // Sin desplazamiento vertical al inicio
                new Vector3(0, 0.2f, 0),       // Ligero rebote hacia arriba
                2,                            // Tiempo inicio
                0.5f,                         // Duración
                Animacion.TipoInterpolacion.SuavizadoCompleto);
            
            libreto.AgregarAnimacionTraslacion(
                nombreAuto,
                new Vector3(0, 0.2f, 0),      // Desde punto más alto
                new Vector3(0, 0, 0),         // De vuelta a posición original
                2.5f,                         // Tiempo inicio
                0.5f,                         // Duración
                Animacion.TipoInterpolacion.SuavizadoCompleto);
            
            return libreto;
        }
    }
}