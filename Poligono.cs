using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace LetraU
{
    public class Poligono
    {
        public List<Punto> Vertices;
        public Color4 color;
        public Punto centro;

        public Poligono(Color4 color)
        {
            this.Vertices = new List<Punto>();
            this.color = color;
            this.centro = new Punto(0, 0, 0);
        }

        public void addVertice(float x, float y, float z)
        {
            Vertices.Add(new Punto(x, y, z));
        }

        public void setColor(Color4 color)
        {
            this.color = color;
        }

        public void setCentro(Punto centro)
        {
            this.centro = centro;
        }

        public void RotarAlrededorCentroMasa(float angle)
        {
            // Calcular el centro de masa
            Punto centroMasa = CalcularCentroMasa();

            // Rotar cada vértice alrededor del centro de masa
            for (int i = 0; i < Vertices.Count; i++)
            {
                Vertices[i] = RotateVertex(Vertices[i], angle, centroMasa);
            }
        }

        private Punto RotateVertex(Punto vertex, float angle, Punto center)
        {
            float cosA = (float)Math.Cos(angle);
            float sinA = (float)Math.Sin(angle);
            float x = cosA * (vertex.x - center.x) + sinA * (vertex.y - center.y) + center.x;
            float y = sinA * -(vertex.x - center.x) + cosA * (vertex.y - center.y) + center.y;
            float z = vertex.z; // La rotación en el plano XY no afecta al eje Z

            return new Punto(x, y, z);
        }

        private Punto CalcularCentroMasa()
        {
            float sumaX = 0, sumaY = 0, sumaZ = 0;

            foreach (var vertice in Vertices)
            {
                sumaX += vertice.x;
                sumaY += vertice.y;
                sumaZ += vertice.z;
            }

            float promedioX = sumaX / Vertices.Count;
            float promedioY = sumaY / Vertices.Count;
            float promedioZ = sumaZ / Vertices.Count;

            return new Punto(promedioX, promedioY, promedioZ);
        }

        // Método de dibujo
        public void Dibujar(Vector3 centro)
        {
            GL.PushMatrix();
            GL.Translate(new Vector3(centro.X, centro.Y, centro.Z));

            GL.Begin(PrimitiveType.Polygon);
            GL.Color4(color);

            foreach (var vertice in Vertices)
            {
                GL.Vertex3(new Vector3(vertice.x, vertice.y, vertice.z));
            }

            GL.End();
            GL.PopMatrix();
        }

        // Nuevo método de dibujo con transformaciones
        public void Dibujar(Vector3 centro, Vector3 posicion, Vector3 rotacion, Vector3 escala)
        {
            GL.PushMatrix();

            // Primero aplicamos la traslación al centro de la escena
            GL.Translate(new Vector3(centro.X, centro.Y, centro.Z));

            // Luego aplicamos la traslación del objeto
            GL.Translate(posicion);

            // Aplicamos las rotaciones
            GL.Rotate(rotacion.X, 1.0f, 0.0f, 0.0f); // Rotación en X
            GL.Rotate(rotacion.Y, 0.0f, 1.0f, 0.0f); // Rotación en Y
            GL.Rotate(rotacion.Z, 0.0f, 0.0f, 1.0f); // Rotación en Z

            // Aplicamos la escala
            GL.Scale(escala);

            // Dibujamos el polígono
            GL.Begin(PrimitiveType.Polygon);
            GL.Color4(color);

            foreach (var vertice in Vertices)
            {
                GL.Vertex3(new Vector3(vertice.x, vertice.y, vertice.z));
            }

            GL.End();

            GL.PopMatrix();
        }

        // Método auxiliar para rotación en el eje Y
        public void Rotar(float angle, String eje)
        {
            if (eje == "y" || eje == "Y")
            {
                for (int i = 0; i < Vertices.Count; i++)
                {
                    Vertices[i] = RotateVertexY(Vertices[i], angle, new Vector3(centro.x, centro.y, centro.z));
                }
            }
        }

        private Punto RotateVertexY(Punto vertex, float angle, Vector3 centro)
        {
            float cosA = (float)Math.Cos(angle);
            float sinA = (float)Math.Sin(angle);
            float x = (vertex.x - centro.X) * cosA - (vertex.z - centro.Z) * sinA + centro.X;
            float z = (vertex.x - centro.X) * sinA + (vertex.z - centro.Z) * cosA + centro.Z;
            return new Punto(x, vertex.y, z);
        }

    }
}