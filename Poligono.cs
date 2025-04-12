using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace LetraT
{
    public class Poligono
    {
        float rotarEjeY;

        public List<Punto> Vertices;
        public Color4 color;
        public Punto centro;

        public Poligono(Color4 color)
        {
            this.Vertices = new List<Punto>();
            this.color = color;
            this.centro = new Punto(0, 0, 0);
            this.rotarEjeY = 0.02f;
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

        public void Dibujar(Vector3 centro)
        {
            GL.PushMatrix();
            GL.Translate(new Vector3(centro.X, centro.Y, centro.Z));

            GL.Begin(PrimitiveType.Polygon);
            GL.Color4(color);

            Rotar(rotarEjeY, "y");

            foreach (var vertice in Vertices)
            {
                GL.Vertex3(new Vector3(vertice.x, vertice.y, vertice.z));
            }

            GL.End();
            GL.PopMatrix();
        }


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