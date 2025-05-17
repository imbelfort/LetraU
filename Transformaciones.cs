using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace LetraU
{
    public class Transformaciones
    {
        public Vector3 Posicion { get; set; } // Cambiado a set público para permitir acceso desde AccionTraslacion
        public Vector3 Rotacion { get; protected set; }
        public Vector3 Escala { get; protected set; }

        public Transformaciones()
        {
            this.Posicion = new Vector3(0, 0, 0);
            this.Rotacion = new Vector3(0, 0, 0);
            this.Escala = new Vector3(1, 1, 1);
        }

        public void Trasladar(float x, float y, float z)
        {
            this.Posicion = new Vector3(x, y, z);
        }

        public void Trasladar(Vector3 posicion)
        {
            this.Posicion = posicion;
        }

        public void TrasladarIncremental(float x, float y, float z)
        {
            this.Posicion += new Vector3(x, y, z);
        }

        public void TrasladarIncremental(Vector3 incremento)
        {
            this.Posicion += incremento;
        }

        public void Rotar(float x, float y, float z)
        {
            this.Rotacion = new Vector3(x, y, z);
        }

        public void RotarIncremental(float x, float y, float z)
        {
            this.Rotacion += new Vector3(x, y, z);
        }

        public void Rotar(Vector3 rotacion)
        {
            this.Rotacion = rotacion;
        }

        public void Escalar(float x, float y, float z)
        {
            this.Escala = new Vector3(x, y, z);
        }

        public void Escalar(float escalaUniforme)
        {
            this.Escala = new Vector3(escalaUniforme, escalaUniforme, escalaUniforme);
        }

        public void Escalar(Vector3 escala)
        {
            this.Escala = escala;
        }

        public void ReiniciarTransformaciones()
        {
            this.Posicion = new Vector3(0, 0, 0);
            this.Rotacion = new Vector3(0, 0, 0);
            this.Escala = new Vector3(1, 1, 1);
        }
    }
}