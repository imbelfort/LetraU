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
                // Solo aplicamos transformaciones a las acciones que aún no han terminado
                if (tiempoTranscurrido <= accion.Duracion)
                {
                    accion.AplicarTransformacion(tiempoTranscurrido);
                }
                else
                {
                    // Si la acción ha terminado, la eliminamos
                    acciones.Remove(accion);
                }
            }

            // Si no quedan acciones, reiniciamos o hacemos algo más
            if (acciones.Count == 0)
            {
                // Aquí podríamos reiniciar o iniciar una nueva secuencia de animación
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

        public AccionTransformacion(float duracion)
        {
            Duracion = duracion;
        }

        public abstract void AplicarTransformacion(float tiempoRelativo);
    }

    public class AccionTraslacion : AccionTransformacion
    {
        private Vector3 vectorTraslacion;
        private Objeto objeto;

        public AccionTraslacion(Objeto objeto, Vector3 vectorTraslacion, float duracion)
            : base(duracion)
        {
            this.objeto = objeto;
            this.vectorTraslacion = vectorTraslacion;
        }

        public override void AplicarTransformacion(float tiempoRelativo)
        {
            float factor = tiempoRelativo / Duracion;
            Vector3 traslacion = vectorTraslacion * factor;
            objeto.Trasladar(traslacion);
        }
    }

    public class AccionTraslacionCaida : AccionTransformacion
    {
        private Vector3 vectorTraslacion;
        private Objeto objeto;

        public AccionTraslacionCaida(Objeto objeto, Vector3 vectorTraslacion, float duracion)
            : base(duracion)
        {
            this.objeto = objeto;
            this.vectorTraslacion = vectorTraslacion;
        }

        public override void AplicarTransformacion(float tiempoRelativo)
        {
            float factor = tiempoRelativo / Duracion;
            Vector3 traslacion = vectorTraslacion * factor;
            objeto.Trasladar(traslacion);
        }
    }

    public class AccionRotacion : AccionTransformacion
    {
        private string eje;
        private Poligono parte;

        public AccionRotacion(Poligono objeto, string eje, float duracion)
            : base(duracion)
        {
            this.parte = objeto;
            this.eje = eje;
        }

        public override void AplicarTransformacion(float tiempoRelativo)
        {
            float angulo = 360f * tiempoRelativo / Duracion;
            parte.RotarAlrededorCentroMasa(angulo);
        }
    }


    public class AccionRotacionObjeto : AccionTransformacion
    {
        private Objeto objeto;
        private string eje;
        private float anguloTotal;
        private float anguloAplicado = 0;
        private float tiempoAcumulado = 0f;

        public AccionRotacionObjeto(Objeto objeto, string eje, float anguloTotal, float duracion)
            : base(duracion)
        {
            this.objeto = objeto;
            this.eje = eje.ToLower();
            this.anguloTotal = anguloTotal;
        }

        public override void AplicarTransformacion(float tiempoRelativo)
        {
            if (tiempoRelativo <= 0)
                return;

            // La diferencia de tiempo desde la última actualización
            float deltaFactor = (tiempoRelativo - tiempoAcumulado) / Duracion;

            // Calculamos el incremento de ángulo proporcional al tiempo transcurrido
            float deltaAngulo = anguloTotal * deltaFactor;

            // Aplicamos el incremento de rotación según el eje
            if (eje == "x")
                objeto.RotarIncremental(deltaAngulo, 0f, 0f);
            else if (eje == "y")
                objeto.RotarIncremental(0f, deltaAngulo, 0f);
            else if (eje == "z")
                objeto.RotarIncremental(0f, 0f, deltaAngulo);

            // Actualizamos el tiempo acumulado y ángulo aplicado
            tiempoAcumulado = tiempoRelativo;
            anguloAplicado += deltaAngulo;
        }
    }

}
