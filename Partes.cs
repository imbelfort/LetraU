using System;
using OpenTK;
using OpenTK.Graphics;
using System.Collections.Generic;
//using System.Runtime.Serialization;

namespace LetraU
{
    public class Partes : Transformaciones
    {
        public Dictionary<String, Poligono> listaDePoligonos;
        public Color4 color;
        public Punto centro;
        public Partes()
        {
            listaDePoligonos = new Dictionary<string, Poligono>();
            this.color = new Color4(0, 0, 0, 0);

        }

        public void add(String nombre, Poligono poligono)
        {
            this.listaDePoligonos.Add(nombre, poligono);
        }

        public void delete(String nombre)
        {
            this.listaDePoligonos.Remove(nombre);
        }

        public Poligono getPoligono(String nombre)
        {
            return this.listaDePoligonos[nombre];
        }

        public void setColor(String nombre, Color4 color)
        {
            this.color = color;
            listaDePoligonos[nombre].setColor(this.color);
        }

        public void setCentro(Punto centro)
        {
            this.centro = centro;
            foreach (Poligono poligono in listaDePoligonos.Values)
            {
                poligono.setCentro(centro);
            }
        }

        public void dibujarPoligono(Vector3 centro)
        {
            foreach (Poligono poligono in listaDePoligonos.Values)
            {
                poligono.Dibujar(centro);
            }
        }
        public void dibujarPoligono(Vector3 centro, Vector3 objetoPosicion, Vector3 objetoRotacion, Vector3 objetoEscala)
        {
            foreach (Poligono poligono in listaDePoligonos.Values)
            {
                // Combinamos las transformaciones del objeto con las de la parte
                Vector3 posicionFinal = objetoPosicion + this.Posicion;
                Vector3 rotacionFinal = objetoRotacion + this.Rotacion;
                Vector3 escalaFinal = new Vector3(
                    objetoEscala.X * this.Escala.X,
                    objetoEscala.Y * this.Escala.Y,
                    objetoEscala.Z * this.Escala.Z);

                poligono.Dibujar(centro, posicionFinal, rotacionFinal, escalaFinal);
            }
        }

        public void desplazar(Vector3 vector)
        {
            foreach (Poligono poligono in listaDePoligonos.Values)
            {
              //  poligono.desplazamiento(vector);
            }
        }
    }
}
