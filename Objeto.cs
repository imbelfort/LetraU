using OpenTK;
using System;
using OpenTK.Graphics;
using System.Collections.Generic;

namespace LetraU
{
    public class Objeto
    {
        public Dictionary<String, Partes> listaDePartes;
        public Color4 color;
        public Punto centro;

        // Propiedades de transformación
        public Vector3 Posicion { get; private set; }
        public Vector3 Rotacion { get; private set; }
        public Vector3 Escala { get; private set; }

        public Objeto()
        {
            this.listaDePartes = new Dictionary<String, Partes>();
            this.color = new Color4(0, 0, 0, 0);
            this.centro = new Punto(0, 0, 0);

            this.Posicion = new Vector3(0, 0, 0);
            this.Rotacion = new Vector3(0, 0, 0);
            this.Escala = new Vector3(1, 1, 1);
        }

        public void addParte(String nombre, Partes nuevaParte)
        {
            listaDePartes.Add(nombre, nuevaParte);
        }

        public void deleteParte(String nombre)
        {
            this.listaDePartes.Remove(nombre);
        }

        public Partes getParte(String nombre)
        {
            return this.listaDePartes[nombre];
        }

        /* public void dibujarParte(Vector3 centro)
         {
             foreach (Partes partes in listaDePartes.Values)
             {
                 partes.dibujarPoligono(centro);
             }
         }*/

        public void dibujarParte(Vector3 centro)
        {
            foreach (Partes partes in listaDePartes.Values)
            {
                // Pasamos las transformaciones del objeto a cada parte
                partes.dibujarPoligono(centro, this.Posicion, this.Rotacion, this.Escala);
            }
        }



        public void setColor(String parte, String poligono, Color4 color)
        {
            this.color = color;
            listaDePartes[parte].setColor(poligono, this.color);
        }

        public void setCentro(Punto centro)
        {
            this.centro = centro;
            foreach (Partes parteActual in listaDePartes.Values)
            {
                parteActual.setCentro(centro);
            }
        }

        // Métodos de transformación
        public void Trasladar(float x, float y, float z)
        {
            this.Posicion = new Vector3(x, y, z);
        }

        public void Trasladar(Vector3 posicion)
        {
            this.Posicion = posicion;
        }

        public void Rotar(float x, float y, float z)
        {
            this.Rotacion = new Vector3(x, y, z);
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

        public Objeto clonar()
        {
            Objeto clon = new Objeto();
            clon.color = this.color;

            // Clonar cada parte
            foreach (var kvp in this.listaDePartes)
            {
                Partes parteClon = new Partes();
                parteClon.color = kvp.Value.color;

                // Clonar cada polígono en la parte
                foreach (var kvpPoligono in kvp.Value.listaDePoligonos)
                {
                    Poligono poligonoClon = new Poligono(kvpPoligono.Value.color);

                    // Clonar los vértices
                    foreach (var vertice in kvpPoligono.Value.Vertices)
                    {
                        poligonoClon.addVertice(vertice.x, vertice.y, vertice.z);
                    }

                    parteClon.add(kvpPoligono.Key, poligonoClon);
                }

                clon.addParte(kvp.Key, parteClon);
            }

            return clon;
        }
    }
}
