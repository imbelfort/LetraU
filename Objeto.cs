using OpenTK;
using System;
using OpenTK.Graphics;
using System.Collections.Generic;

namespace LetraU
{
    public class Objeto : Transformaciones
    {
        public Dictionary<String, Partes> listaDePartes;
        public Color4 color;
        public Punto centro;

        public Objeto()
        {
            this.listaDePartes = new Dictionary<String, Partes>();
            this.color = new Color4(0, 0, 0, 0);
            this.centro = new Punto(0, 0, 0);

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
        public void dibujarParte(Vector3 centro, Vector3 escenarioPosicion, Vector3 escenarioRotacion, Vector3 escenarioEscala)
        {
            foreach (Partes partes in listaDePartes.Values)
            {
                // Combinamos las transformaciones del escenario con las del objeto
                Vector3 posicionFinal = escenarioPosicion + this.Posicion;
                Vector3 rotacionFinal = escenarioRotacion + this.Rotacion;
                Vector3 escalaFinal = new Vector3(
                    escenarioEscala.X * this.Escala.X,
                    escenarioEscala.Y * this.Escala.Y,
                    escenarioEscala.Z * this.Escala.Z
                );

                partes.dibujarPoligono(centro, posicionFinal, rotacionFinal, escalaFinal);
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

        public void setColor(Color4 Color)
        {
            foreach (var parte in listaDePartes)
            {
                foreach (var poligono in parte.Value.listaDePoligonos)
                {
                    setColor(parte.Key, poligono.Key, Color);
                }
            }
        }
    }
}
