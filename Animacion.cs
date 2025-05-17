using System;
using OpenTK;
using OpenTK.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace LetraU
{
    public class Animacion
    {
        private List<AccionTransformacion> acciones;
        private float tiempoTotal;
        private float tiempoTranscurrido;

        public Animacion()
        {
            acciones = new List<AccionTransformacion>();
            tiempoTotal = 0;
            tiempoTranscurrido = 0;
        }

        public void AgregarAccion(AccionTransformacion accion)
        {
            acciones.Add(accion);
            tiempoTotal += accion.Duracion;
        }

        public void Actualizar(float deltaTime)
        {
            tiempoTranscurrido += deltaTime;

            // Crear una copia temporal para evitar problemas al remover elementos durante la iteración
            List<AccionTransformacion> accionesTemp = new List<AccionTransformacion>(acciones);

            foreach (var accion in accionesTemp)
            {
                // Si la acción aún no ha terminado su tiempo de ejecución, la aplicamos
                if (accion.TiempoEjecutado < accion.Duracion)
                {
                    // Calculamos solo el tiempo delta para esta actualización
                    float deltaEfectivo = Math.Min(deltaTime, accion.Duracion - accion.TiempoEjecutado);
                    accion.AplicarTransformacion(deltaEfectivo);
                    accion.TiempoEjecutado += deltaEfectivo;

                    // Si después de actualizar, la acción ha completado su tiempo, la eliminamos
                    if (accion.TiempoEjecutado >= accion.Duracion)
                    {
                        acciones.Remove(accion);
                    }
                }
            }

            // Si no quedan acciones, reiniciamos o hacemos algo más
            if (acciones.Count == 0)
            {
                Console.WriteLine("Todas las acciones han terminado. Animación completa.");
            }
        }

        public void Reiniciar()
        {
            tiempoTranscurrido = 0;
            acciones.Clear();
            tiempoTotal = 0;
        }

        public bool EstaEjecutando()
        {
            return acciones.Count > 0;
        }
    }


    public abstract class AccionTransformacion
    {
        public float Duracion { get; protected set; }
        public float TiempoEjecutado { get; set; }

        public AccionTransformacion(float duracion)
        {
            Duracion = duracion;
            TiempoEjecutado = 0;
        }

        public abstract void AplicarTransformacion(float deltaTiempo);
    }

    public class AccionTraslacion : AccionTransformacion
    {
        private Vector3 vectorTraslacion;
        private Objeto objeto;
        private Vector3 traslacionAcumulada;

        public AccionTraslacion(Objeto objeto, Vector3 vectorTraslacion, float duracion)
            : base(duracion)
        {
            this.objeto = objeto;
            this.vectorTraslacion = vectorTraslacion;
            this.traslacionAcumulada = Vector3.Zero;
        }

        public override void AplicarTransformacion(float deltaTiempo)
        {
            // Calculamos qué proporción del vector total debe aplicarse en este paso
            float proporcion = deltaTiempo / Duracion;

            // Calculamos el vector de traslación incremental para este paso
            Vector3 incremento = vectorTraslacion * proporcion;

            // Acumulamos la traslación aplicada para seguimiento
            traslacionAcumulada += incremento;

            // Aplicamos el incremento a la posición actual del objeto
            objeto.Trasladar(objeto.Posicion + incremento);

            // Console.WriteLine($"Traslación aplicada: {incremento}, Total: {traslacionAcumulada}, Proporción: {TiempoEjecutado / Duracion * 100}%");
        }
    }

    public class AccionTraslacionCaida : AccionTransformacion
    {
        private Vector3 vectorTraslacion;
        private Objeto objeto;
        private Vector3 traslacionAcumulada;

        public AccionTraslacionCaida(Objeto objeto, Vector3 vectorTraslacion, float duracion)
            : base(duracion)
        {
            this.objeto = objeto;
            this.vectorTraslacion = vectorTraslacion;
            this.traslacionAcumulada = Vector3.Zero;
        }

        public override void AplicarTransformacion(float deltaTiempo)
        {
            // Calculamos qué proporción del vector total debe aplicarse en este paso
            float proporcion = deltaTiempo / Duracion;

            // Aplicamos una función cuadrática para simular aceleración de caída
            float factorCaida = (TiempoEjecutado / Duracion) * (TiempoEjecutado / Duracion);

            // Calculamos el vector de traslación incremental para este paso
            Vector3 incremento = vectorTraslacion * proporcion * (1 + factorCaida);

            // Acumulamos la traslación aplicada para seguimiento
            traslacionAcumulada += incremento;

            // Aplicamos el incremento a la posición actual del objeto
            objeto.Trasladar(objeto.Posicion + incremento);
        }
    }

    public class AccionRotacion : AccionTransformacion
    {
        private string eje;
        private Poligono parte;
        private float anguloTotal;
        private float anguloAplicado;

        public AccionRotacion(Poligono objeto, string eje, float duracion)
            : base(duracion)
        {
            this.parte = objeto;
            this.eje = eje;
            this.anguloTotal = 360f; // Rotación completa por defecto
            this.anguloAplicado = 0f;
        }

        public override void AplicarTransformacion(float deltaTiempo)
        {
            // Calculamos el incremento de ángulo proporcional al tiempo delta
            float deltaAngulo = anguloTotal * (deltaTiempo / Duracion);

            // Aplicamos la rotación
            parte.RotarAlrededorCentroMasa(deltaAngulo);

            // Acumulamos el ángulo aplicado
            anguloAplicado += deltaAngulo;
        }
    }


    public class AccionRotacionObjeto : AccionTransformacion
    {
        private Objeto objeto;
        private string eje;
        private float anguloTotal;
        private float anguloAplicado = 0;

        public AccionRotacionObjeto(Objeto objeto, string eje, float anguloTotal, float duracion)
            : base(duracion)
        {
            this.objeto = objeto;
            this.eje = eje.ToLower();
            this.anguloTotal = anguloTotal;
        }

        public override void AplicarTransformacion(float deltaTiempo)
        {
            // Calculamos el incremento de ángulo proporcional al tiempo delta
            float deltaAngulo = anguloTotal * (deltaTiempo / Duracion);

            // Aplicamos el incremento de rotación según el eje
            if (eje == "x")
                objeto.RotarIncremental(deltaAngulo, 0f, 0f);
            else if (eje == "y")
                objeto.RotarIncremental(0f, deltaAngulo, 0f);
            else if (eje == "z")
                objeto.RotarIncremental(0f, 0f, deltaAngulo);

            // Acumulamos el ángulo aplicado
            anguloAplicado += deltaAngulo;

            // Console.WriteLine($"Rotación aplicada: {deltaAngulo}° en eje {eje}, Total: {anguloAplicado}°, Proporción: {TiempoEjecutado / Duracion * 100}%");
        }
    }
}