using System;
using OpenTK;
using OpenTK.Graphics;
using System.Collections.Generic;

namespace LetraU
{
    public class AutoCreator
    {

        public static Escenario CrearAutoEscenarioConCarretera()
        {
            // Crear el escenario base
            Escenario escenario = CrearAutoEscenario();

            // Crear y agregar la carretera al escenario
            Objeto carretera = Carretera.CrearCarretera();
            escenario.addObjeto("carretera", carretera);

            return escenario;
        }
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

            // Techo (ahora en la parte superior de la cabina)
            Poligono techo = new Poligono(colorChasis);
            techo.addVertice(-0.6f, 0.5f, 0.3f);   // Frente izquierdo
            techo.addVertice(0.6f, 0.5f, 0.3f);    // Frente derecho
            techo.addVertice(0.6f, 0.5f, -0.3f);   // Atrás derecho
            techo.addVertice(-0.6f, 0.5f, -0.3f);  // Atrás izquierdo
            chasis.add("techo", techo);

            // Parte superior del auto (cabina)
            Poligono cabina = new Poligono(colorChasis);
            cabina.addVertice(-0.6f, 0.1f, 0.3f);   // Abajo izquierda
            cabina.addVertice(0.6f, 0.1f, 0.3f);    // Abajo derecha
            cabina.addVertice(0.6f, 0.5f, 0.3f);    // Arriba derecha
            cabina.addVertice(-0.6f, 0.5f, 0.3f);   // Arriba izquierda
            chasis.add("cabina_frente", cabina);

            Poligono cabinaAtras = new Poligono(colorChasis);
            cabinaAtras.addVertice(-0.6f, 0.1f, -0.3f);
            cabinaAtras.addVertice(0.6f, 0.1f, -0.3f);
            cabinaAtras.addVertice(0.6f, 0.5f, -0.3f);
            cabinaAtras.addVertice(-0.6f, 0.5f, -0.3f);
            chasis.add("cabina_atras", cabinaAtras);

            // Lado izquierdo cabina (vertical)
            Poligono ladoIzquierdoCabina = new Poligono(colorChasis);
            ladoIzquierdoCabina.addVertice(-0.6f, 0.1f, 0.3f);   // Abajo frente
            ladoIzquierdoCabina.addVertice(-0.6f, 0.1f, -0.3f);  // Abajo atrás
            ladoIzquierdoCabina.addVertice(-0.6f, 0.5f, -0.3f);  // Arriba atrás
            ladoIzquierdoCabina.addVertice(-0.6f, 0.5f, 0.3f);   // Arriba frente
            chasis.add("ladoIzquierdoCabina", ladoIzquierdoCabina);

            // Lado derecho cabina (vertical)
            Poligono ladoDerechoCabina = new Poligono(colorChasis);
            ladoDerechoCabina.addVertice(0.6f, 0.1f, 0.3f);   // Abajo frente
            ladoDerechoCabina.addVertice(0.6f, 0.1f, -0.3f);  // Abajo atrás
            ladoDerechoCabina.addVertice(0.6f, 0.5f, -0.3f);  // Arriba atrás
            ladoDerechoCabina.addVertice(0.6f, 0.5f, 0.3f);   // Arriba frente
            chasis.add("ladoDerechoCabina", ladoDerechoCabina);

            // Lado izquierdo chasis (cerrar volumen entre base y techo)
            Poligono ladoIzquierdoChasisVolumen = new Poligono(colorChasis);
            ladoIzquierdoChasisVolumen.addVertice(-1.0f, -0.3f, 0.5f);   // Base frente abajo
            ladoIzquierdoChasisVolumen.addVertice(-1.0f, -0.3f, -0.5f);  // Base atrás abajo
            ladoIzquierdoChasisVolumen.addVertice(-0.6f, 0.5f, -0.3f);   // Techo atrás arriba
            ladoIzquierdoChasisVolumen.addVertice(-0.6f, 0.5f, 0.3f);    // Techo frente arriba
            chasis.add("ladoIzquierdoChasisVolumen", ladoIzquierdoChasisVolumen);

            // Lado derecho chasis (cerrar volumen)
            Poligono ladoDerechoChasisVolumen = new Poligono(colorChasis);
            ladoDerechoChasisVolumen.addVertice(1.0f, -0.3f, 0.5f);
            ladoDerechoChasisVolumen.addVertice(1.0f, -0.3f, -0.5f);
            ladoDerechoChasisVolumen.addVertice(0.6f, 0.5f, -0.3f);
            ladoDerechoChasisVolumen.addVertice(0.6f, 0.5f, 0.3f);
            chasis.add("ladoDerechoChasisVolumen", ladoDerechoChasisVolumen);

            // Frente del chasis (cerrar volumen vertical)
            Poligono frenteVolumen = new Poligono(colorChasis);
            frenteVolumen.addVertice(-1.0f, -0.3f, 0.5f);
            frenteVolumen.addVertice(1.0f, -0.3f, 0.5f);
            frenteVolumen.addVertice(0.6f, 0.5f, 0.3f);
            frenteVolumen.addVertice(-0.6f, 0.5f, 0.3f);
            chasis.add("frenteVolumen", frenteVolumen);

            // Atrás del chasis (cerrar volumen vertical)
            Poligono atrasVolumen = new Poligono(colorChasis);
            atrasVolumen.addVertice(-1.0f, -0.3f, -0.5f);
            atrasVolumen.addVertice(1.0f, -0.3f, -0.5f);
            atrasVolumen.addVertice(0.6f, 0.5f, -0.3f);
            atrasVolumen.addVertice(-0.6f, 0.5f, -0.3f);
            chasis.add("atrasVolumen", atrasVolumen);

            // Ventanas
            Poligono ventanaFrente = new Poligono(colorVentanas);
            ventanaFrente.addVertice(-0.55f, 0.12f, 0.31f);   // Abajo izquierdo
            ventanaFrente.addVertice(0.55f, 0.12f, 0.31f);    // Abajo derecho
            ventanaFrente.addVertice(0.55f, 0.48f, 0.29f);    // Arriba derecho (un poco atrás en Z para simular inclinación)
            ventanaFrente.addVertice(-0.55f, 0.48f, 0.29f);   // Arriba izquierdo

            chasis.add("ventana_frente", ventanaFrente);

            Poligono ventanaAtras = new Poligono(colorVentanas);
            ventanaAtras.addVertice(-0.55f, 0.12f, -0.29f);
            ventanaAtras.addVertice(0.55f, 0.12f, -0.29f);
            ventanaAtras.addVertice(0.55f, 0.48f, -0.31f);
            ventanaAtras.addVertice(-0.55f, 0.48f, -0.31f);
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