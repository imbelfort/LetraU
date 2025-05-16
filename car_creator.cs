using System;
using OpenTK;
using OpenTK.Graphics;
using System.Collections.Generic;

namespace LetraU
{
    public class AutoCreator
    {
        public static Escenario CrearAutoEscenario()
        {
            // Crear un nuevo escenario
            Escenario escenario = new Escenario(new Vector3(0, 0, 0));

            // Crear el objeto auto
            Objeto auto = new Objeto();

            // Colores
            Color4 colorChasis = new Color4(0.2f, 0.2f, 0.8f, 1.0f);    // Azul
            Color4 colorRuedas = new Color4(0.1f, 0.1f, 0.1f, 1.0f);    // Negro
            Color4 colorVentanas = new Color4(0.7f, 0.7f, 0.9f, 0.8f);  // Celeste transparente

            // Crear las partes del auto

            // 1. Chasis principal
            Partes chasis = new Partes();
            Poligono baseChasis = new Poligono(colorChasis);

            // Base inferior del chasis (parte de abajo)
            baseChasis.addVertice(-1.0f, -0.3f, 0.5f);  // Frente izquierdo
            baseChasis.addVertice(1.0f, -0.3f, 0.5f);   // Frente derecho
            baseChasis.addVertice(1.0f, -0.3f, -0.5f);  // Atrás derecho
            baseChasis.addVertice(-1.0f, -0.3f, -0.5f); // Atrás izquierdo
            chasis.add("base", baseChasis);

            // Frente del coche
            Poligono frenteChasis = new Poligono(colorChasis);
            frenteChasis.addVertice(-1.0f, -0.3f, 0.5f);  // Abajo izquierdo
            frenteChasis.addVertice(1.0f, -0.3f, 0.5f);   // Abajo derecho
            frenteChasis.addVertice(1.0f, 0.1f, 0.5f);    // Arriba derecho
            frenteChasis.addVertice(-1.0f, 0.1f, 0.5f);   // Arriba izquierdo
            chasis.add("frente", frenteChasis);

            // Atrás del coche
            Poligono atrasChasis = new Poligono(colorChasis);
            atrasChasis.addVertice(-1.0f, -0.3f, -0.5f);  // Abajo izquierdo
            atrasChasis.addVertice(1.0f, -0.3f, -0.5f);   // Abajo derecho
            atrasChasis.addVertice(1.0f, 0.1f, -0.5f);    // Arriba derecho
            atrasChasis.addVertice(-1.0f, 0.1f, -0.5f);   // Arriba izquierdo
            chasis.add("atras", atrasChasis);

            // Lado izquierdo
            Poligono ladoIzquierdo = new Poligono(colorChasis);
            ladoIzquierdo.addVertice(-1.0f, -0.3f, 0.5f);  // Frente abajo
            ladoIzquierdo.addVertice(-1.0f, -0.3f, -0.5f); // Atrás abajo
            ladoIzquierdo.addVertice(-1.0f, 0.1f, -0.5f);  // Atrás arriba
            ladoIzquierdo.addVertice(-1.0f, 0.1f, 0.5f);   // Frente arriba
            chasis.add("ladoIzquierdo", ladoIzquierdo);

            // Lado derecho
            Poligono ladoDerecho = new Poligono(colorChasis);
            ladoDerecho.addVertice(1.0f, -0.3f, 0.5f);  // Frente abajo
            ladoDerecho.addVertice(1.0f, -0.3f, -0.5f); // Atrás abajo
            ladoDerecho.addVertice(1.0f, 0.1f, -0.5f);  // Atrás arriba
            ladoDerecho.addVertice(1.0f, 0.1f, 0.5f);   // Frente arriba
            chasis.add("ladoDerecho", ladoDerecho);

            // Techo
            Poligono techo = new Poligono(colorChasis);
            techo.addVertice(-0.8f, 0.1f, 0.3f);  // Frente izquierdo
            techo.addVertice(0.8f, 0.1f, 0.3f);   // Frente derecho
            techo.addVertice(0.8f, 0.1f, -0.3f);  // Atrás derecho
            techo.addVertice(-0.8f, 0.1f, -0.3f); // Atrás izquierdo
            chasis.add("techo", techo);

            // Parte superior del auto (cabina)
            Poligono cabina = new Poligono(colorChasis);
            cabina.addVertice(-0.8f, 0.1f, 0.3f);   // Frente izquierdo
            cabina.addVertice(0.8f, 0.1f, 0.3f);    // Frente derecho
            cabina.addVertice(0.8f, 0.5f, 0.0f);    // Centro derecho
            cabina.addVertice(-0.8f, 0.5f, 0.0f);   // Centro izquierdo
            chasis.add("cabina_frente", cabina);

            Poligono cabinaAtras = new Poligono(colorChasis);
            cabinaAtras.addVertice(-0.8f, 0.1f, -0.3f);  // Atrás izquierdo
            cabinaAtras.addVertice(0.8f, 0.1f, -0.3f);   // Atrás derecho
            cabinaAtras.addVertice(0.8f, 0.5f, 0.0f);    // Centro derecho
            cabinaAtras.addVertice(-0.8f, 0.5f, 0.0f);   // Centro izquierdo
            chasis.add("cabina_atras", cabinaAtras);

            // Ventanas
            Poligono ventanaFrente = new Poligono(colorVentanas);
            ventanaFrente.addVertice(-0.7f, 0.15f, 0.25f);    // Abajo izquierdo
            ventanaFrente.addVertice(0.7f, 0.15f, 0.25f);     // Abajo derecho
            ventanaFrente.addVertice(0.7f, 0.45f, 0.05f);    // Arriba derecho
            ventanaFrente.addVertice(-0.7f, 0.45f, 0.05f);   // Arriba izquierdo
            chasis.add("ventana_frente", ventanaFrente);

            Poligono ventanaAtras = new Poligono(colorVentanas);
            ventanaAtras.addVertice(-0.7f, 0.15f, -0.25f);   // Abajo izquierdo
            ventanaAtras.addVertice(0.7f, 0.15f, -0.25f);    // Abajo derecho
            ventanaAtras.addVertice(0.7f, 0.45f, -0.05f);    // Arriba derecho
            ventanaAtras.addVertice(-0.7f, 0.45f, -0.05f);   // Arriba izquierdo
            chasis.add("ventana_atras", ventanaAtras);

            // Añadir el chasis al auto
            auto.addParte("chasis", chasis);

            // 2. Ruedas (simplificadas como polígonos hexagonales para simular círculos)
            // Rueda delantera izquierda
            CrearRueda(auto, "rueda1", -0.7f, -0.4f, 0.5f, colorRuedas);

            // Rueda delantera derecha
            CrearRueda(auto, "rueda2", 0.7f, -0.4f, 0.5f, colorRuedas);

            // Rueda trasera izquierda
            CrearRueda(auto, "rueda3", -0.7f, -0.4f, -0.5f, colorRuedas);

            // Rueda trasera derecha
            CrearRueda(auto, "rueda4", 0.7f, -0.4f, -0.5f, colorRuedas);

            // Añadir el auto al escenario
            escenario.addObjeto("auto", auto);

            return escenario;
        }

        private static void CrearRueda(Objeto auto, string nombre, float x, float y, float z, Color4 colorRueda)
        {
            Partes rueda = new Partes();
            Poligono poligonoRueda = new Poligono(colorRueda);

            // Crear un hexágono simple para representar una rueda
            float radio = 0.2f;
            int numSegmentos = 6;

            for (int i = 0; i < numSegmentos; i++)
            {
                float angulo = 2 * (float)Math.PI * i / numSegmentos;
                float xPos = x + radio * (float)Math.Cos(angulo);
                float yPos = y + radio * (float)Math.Sin(angulo);
                poligonoRueda.addVertice(xPos, yPos, z);
            }

            rueda.add(nombre, poligonoRueda);
            auto.addParte(nombre, rueda);

            // Añadir el centro de la rueda para mejor rotación visual
            Poligono centroRueda = new Poligono(new Color4(0.3f, 0.3f, 0.3f, 1.0f));
            float radioCentro = 0.05f;
            for (int i = 0; i < numSegmentos; i++)
            {
                float angulo = 2 * (float)Math.PI * i / numSegmentos;
                float xPos = x + radioCentro * (float)Math.Cos(angulo);
                float yPos = y + radioCentro * (float)Math.Sin(angulo);
                centroRueda.addVertice(xPos, yPos, z + 0.001f); // Ligeramente adelante para ser visible
            }
            rueda.add(nombre + "_centro", centroRueda);
        }

        public static void GuardarEscenario(Escenario escenario, string nombreArchivo)
        {
            Serializador.SerializarObjeto<Escenario>(escenario, nombreArchivo);
        }
    }
}