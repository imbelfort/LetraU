using System;
using OpenTK;
using System.Collections.Generic;

namespace LetraT
{
    public class Escenario
    {
        public Dictionary<String, Objeto> listaDeObjetos;
        protected Vector3 centro;

        public Escenario(Vector3 centro)
        {
            this.listaDeObjetos = new Dictionary<String, Objeto>();
            this.centro = centro;
        }

        public void addObjeto(String nombre, Objeto nuevoObjeto)
        {
            this.listaDeObjetos.Add(nombre, nuevoObjeto);
        }

        public void deleteObjeto(String objetoAEliminar)
        {
            this.listaDeObjetos.Remove(objetoAEliminar);
        }

        public Objeto getObjeto(String nombre)
        {
            return this.listaDeObjetos[nombre];
        }

        public void setObjeto(String nombre, Objeto objeto)
        {
            this.listaDeObjetos[nombre] = objeto;
        }

        public void dibujar(Vector3 centro)
        {
            foreach (Objeto objetoActual in this.listaDeObjetos.Values)
            {
                objetoActual.dibujarParte(centro);
            }
        }
    }
}
