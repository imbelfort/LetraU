using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;

namespace LetraU
{
    public class Carretera
    {
        public static Objeto CrearCarretera()
        {
            Objeto carretera = new Objeto();

            Partes parteCarretera = new Partes();

            Color4 colorAsfalto = new Color4(0.2f, 0.2f, 0.2f, 1.0f);      // parte superior (asfalto)
            Color4 colorLaterales = new Color4(0.1f, 0.1f, 0.1f, 1.0f);    // lados y fondo (laterales)

            Vector3 a1Anterior = Vector3.Zero, a2Anterior = Vector3.Zero;
            Vector3 a3Anterior = Vector3.Zero, a4Anterior = Vector3.Zero;

            List<Vector3> trayectoria = new List<Vector3>
            {
                new Vector3(-4.0f, 0.0f, 0.0f),
                new Vector3(0.0f, 0.2f, 0.0f),
                new Vector3(3.0f, 0.4f, -3.0f),
                new Vector3(5.0f, 0.5f, -5.0f)
            };

            float ancho = 1.2f;
            float altura = 0.5f;

            for (int i = 0; i < trayectoria.Count - 1; i++)
            {
                Vector3 ini = trayectoria[i];
                Vector3 fin = trayectoria[i + 1];
                Vector3 dir = fin - ini;

                if (dir.LengthSquared > 0)
                    dir.Normalize();
                else
                    dir = Vector3.UnitZ; // fallback en caso de vector cero

                Vector3 perp = Vector3.Cross(dir, Vector3.UnitY) * (ancho / 2f);

                // Vértices superiores (arriba)
                Vector3 a1 = ini + perp;  // derecha inicio arriba
                Vector3 a2 = ini - perp;  // izquierda inicio arriba
                Vector3 a3 = fin - perp;  // izquierda fin arriba
                Vector3 a4 = fin + perp;  // derecha fin arriba

                // Vértices inferiores (abajo)
                Vector3 b1 = new Vector3(a1.X, a1.Y - altura, a1.Z);
                Vector3 b2 = new Vector3(a2.X, a2.Y - altura, a2.Z);
                Vector3 b3 = new Vector3(a3.X, a3.Y - altura, a3.Z);
                Vector3 b4 = new Vector3(a4.X, a4.Y - altura, a4.Z);

                if (i > 0)
                {
                    // Unir asfalto exterior entre segmentos para continuidad
                    Poligono unionAsfalto = new Poligono(colorAsfalto);
                    unionAsfalto.addVertice(a4Anterior.X, a4Anterior.Y, a4Anterior.Z);
                    unionAsfalto.addVertice(a3Anterior.X, a3Anterior.Y, a3Anterior.Z);
                    unionAsfalto.addVertice(a2.X, a2.Y, a2.Z);
                    unionAsfalto.addVertice(a1.X, a1.Y, a1.Z);
                    parteCarretera.add("unionAsfalto" + i, unionAsfalto);
                }

                // Actualizar vértices anteriores
                a1Anterior = a1;
                a2Anterior = a2;
                a3Anterior = a3;
                a4Anterior = a4;

                // Polígonos principales

                // Asfalto (superior)
                Poligono asfalto = new Poligono(colorAsfalto);
                asfalto.addVertice(a1.X, a1.Y, a1.Z);
                asfalto.addVertice(a2.X, a2.Y, a2.Z);
                asfalto.addVertice(a3.X, a3.Y, a3.Z);
                asfalto.addVertice(a4.X, a4.Y, a4.Z);
                parteCarretera.add("asfalto" + i, asfalto);

                // Lateral izquierdo
                Poligono lateralIzq = new Poligono(colorLaterales);
                lateralIzq.addVertice(a2.X, a2.Y, a2.Z);
                lateralIzq.addVertice(b2.X, b2.Y, b2.Z);
                lateralIzq.addVertice(b3.X, b3.Y, b3.Z);
                lateralIzq.addVertice(a3.X, a3.Y, a3.Z);
                parteCarretera.add("lateralIzq" + i, lateralIzq);

                // Lateral derecho
                Poligono lateralDer = new Poligono(colorLaterales);
                lateralDer.addVertice(a4.X, a4.Y, a4.Z);
                lateralDer.addVertice(b4.X, b4.Y, b4.Z);
                lateralDer.addVertice(b1.X, b1.Y, b1.Z);
                lateralDer.addVertice(a1.X, a1.Y, a1.Z);
                parteCarretera.add("lateralDer" + i, lateralDer);

                // Fondo inferior
                Poligono fondo = new Poligono(colorLaterales);
                fondo.addVertice(b1.X, b1.Y, b1.Z);
                fondo.addVertice(b2.X, b2.Y, b2.Z);
                fondo.addVertice(b3.X, b3.Y, b3.Z);
                fondo.addVertice(b4.X, b4.Y, b4.Z);
                parteCarretera.add("fondo" + i, fondo);
            }

            carretera.addParte("baseCarretera", parteCarretera);
            return carretera;
        }
    }
}

