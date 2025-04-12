using System;
using OpenTK;
using OpenTK.Graphics;
using System.Collections.Generic;
//using System.Runtime.Serialization;

namespace LetraT
{
    public class Partes
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
    }
}
