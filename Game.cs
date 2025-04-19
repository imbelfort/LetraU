using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;

namespace LetraU
{
    public class Game : GameWindow
    {
        private Escenario escenario;
        private float anguloRotacion = 0.0f;


        public Game(int width, int height) : base(width, height, GraphicsMode.Default, "Diseño Letra U - 3D")
        {
            // Inicializar directamente la letra U en lugar de cargar desde archivo
            //this.escenario = InicializarLetraU();

            //Opcional: Guardar el escenario para futuras ejecuciones
            //Serializador.SerializarObjeto<Escenario>(escenario, "escenarioU.json");

            // Cargar desde archivo después de serializar una vez:
            escenario = Serializador.DeserializarObjeto<Escenario>("escenarioU.json");

            // Inicializar Objetos
            Objeto letraU = escenario.getObjeto("letraU");
            Objeto letraU2 = letraU.clonar();
            Objeto letraU3 = letraU.clonar();

            letraU2.centro = new Punto(8.0f, 0.0f, 0.0f);
            escenario.addObjeto("letraU2", letraU2);

            letraU3.centro = new Punto(8.0f, 0.0f, 0.0f);
            escenario.addObjeto("letraU3", letraU3);

            // Aplicar transformaciones a la letra U original
            letraU.Trasladar(new Vector3(-1.5f, 0.0f, 0.0f));
            letraU.Escalar(0.7f);
            letraU.Rotar(70, 45, 10);

            letraU2.Trasladar(new Vector3(1.5f, 0.0f, 0.0f));
            letraU2.Escalar(0.8f);

            letraU3.Trasladar(new Vector3(0.5f, 0.4f, 0.0f));
            letraU3.Escalar(0.5f);


        }

        //Metodo que contiene todos los vertices de la Letra U
        /*public static Escenario InicializarLetraU()
        {
            // Crear un nuevo escenario centrado en el origen
            Escenario escenario = new Escenario(new Vector3(0, 0, 0));

            // Crear objeto principal para la letra U
            Objeto letraU = new Objeto();
            letraU.centro = new Punto(0, 0, 0);

            // Barra vertical izquierda de la U
            Partes barraIzquierda = new Partes();

            // Cara frontal de la barra izquierda
            Poligono izquierdaFrontal = new Poligono(new Color4(1.0f, 0.0f, 0.0f, 1.0f));
            izquierdaFrontal.addVertice(-0.8f, 0.8f, 0.1f);  // Superior derecha
            izquierdaFrontal.addVertice(-0.8f, -0.8f, 0.1f); // Inferior derecha
            izquierdaFrontal.addVertice(-0.5f, -0.8f, 0.1f); // Inferior izquierda
            izquierdaFrontal.addVertice(-0.5f, 0.8f, 0.1f);  // Superior izquierda

            // Cara posterior de la barra izquierda
            Poligono izquierdaPosterior = new Poligono(new Color4(0.8f, 0.0f, 0.0f, 1.0f));
            izquierdaPosterior.addVertice(-0.8f, 0.8f, -0.1f);
            izquierdaPosterior.addVertice(-0.5f, 0.8f, -0.1f);
            izquierdaPosterior.addVertice(-0.5f, -0.8f, -0.1f);
            izquierdaPosterior.addVertice(-0.8f, -0.8f, -0.1f);

            // Cara superior de la barra izquierda
            Poligono izquierdaSuperior = new Poligono(new Color4(0.9f, 0.0f, 0.0f, 1.0f));
            izquierdaSuperior.addVertice(-0.8f, 0.8f, 0.1f);
            izquierdaSuperior.addVertice(-0.5f, 0.8f, 0.1f);
            izquierdaSuperior.addVertice(-0.5f, 0.8f, -0.1f);
            izquierdaSuperior.addVertice(-0.8f, 0.8f, -0.1f);

            // Cara inferior de la barra izquierda
            Poligono izquierdaInferior = new Poligono(new Color4(0.9f, 0.0f, 0.0f, 1.0f));
            izquierdaInferior.addVertice(-0.8f, -0.8f, 0.1f);
            izquierdaInferior.addVertice(-0.8f, -0.8f, -0.1f);
            izquierdaInferior.addVertice(-0.5f, -0.8f, -0.1f);
            izquierdaInferior.addVertice(-0.5f, -0.8f, 0.1f);

            // Cara lateral izquierda de la barra izquierda
            Poligono izquierdaLateral = new Poligono(new Color4(0.7f, 0.0f, 0.0f, 1.0f));
            izquierdaLateral.addVertice(-0.8f, 0.8f, 0.1f);
            izquierdaLateral.addVertice(-0.8f, 0.8f, -0.1f);
            izquierdaLateral.addVertice(-0.8f, -0.8f, -0.1f);
            izquierdaLateral.addVertice(-0.8f, -0.8f, 0.1f);

            // Cara lateral derecha de la barra izquierda
            Poligono izquierdaLateralDerecha = new Poligono(new Color4(0.7f, 0.0f, 0.0f, 1.0f));
            izquierdaLateralDerecha.addVertice(-0.5f, 0.8f, 0.1f);
            izquierdaLateralDerecha.addVertice(-0.5f, -0.8f, 0.1f);
            izquierdaLateralDerecha.addVertice(-0.5f, -0.8f, -0.1f);
            izquierdaLateralDerecha.addVertice(-0.5f, 0.8f, -0.1f);

            // Añadir polígonos a la barra izquierda
            barraIzquierda.add("frontal", izquierdaFrontal);
            barraIzquierda.add("posterior", izquierdaPosterior);
            barraIzquierda.add("superior", izquierdaSuperior);
            barraIzquierda.add("inferior", izquierdaInferior);
            barraIzquierda.add("lateralIzquierda", izquierdaLateral);
            barraIzquierda.add("lateralDerecha", izquierdaLateralDerecha);

            // Barra vertical derecha de la U
            Partes barraDerecha = new Partes();

            // Cara frontal de la barra derecha
            Poligono derechaFrontal = new Poligono(new Color4(1.0f, 0.0f, 0.0f, 1.0f));
            derechaFrontal.addVertice(0.5f, 0.8f, 0.1f);
            derechaFrontal.addVertice(0.5f, -0.8f, 0.1f);
            derechaFrontal.addVertice(0.8f, -0.8f, 0.1f);
            derechaFrontal.addVertice(0.8f, 0.8f, 0.1f);

            // Cara posterior de la barra derecha
            Poligono derechaPosterior = new Poligono(new Color4(0.8f, 0.0f, 0.0f, 1.0f));
            derechaPosterior.addVertice(0.5f, 0.8f, -0.1f);
            derechaPosterior.addVertice(0.8f, 0.8f, -0.1f);
            derechaPosterior.addVertice(0.8f, -0.8f, -0.1f);
            derechaPosterior.addVertice(0.5f, -0.8f, -0.1f);

            // Cara superior de la barra derecha
            Poligono derechaSuperior = new Poligono(new Color4(0.9f, 0.0f, 0.0f, 1.0f));
            derechaSuperior.addVertice(0.5f, 0.8f, 0.1f);
            derechaSuperior.addVertice(0.8f, 0.8f, 0.1f);
            derechaSuperior.addVertice(0.8f, 0.8f, -0.1f);
            derechaSuperior.addVertice(0.5f, 0.8f, -0.1f);

            // Cara inferior de la barra derecha
            Poligono derechaInferior = new Poligono(new Color4(0.9f, 0.0f, 0.0f, 1.0f));
            derechaInferior.addVertice(0.5f, -0.8f, 0.1f);
            derechaInferior.addVertice(0.8f, -0.8f, 0.1f);
            derechaInferior.addVertice(0.8f, -0.8f, -0.1f);
            derechaInferior.addVertice(0.5f, -0.8f, -0.1f);

            // Cara lateral izquierda de la barra derecha
            Poligono derechaLateralIzquierda = new Poligono(new Color4(0.7f, 0.0f, 0.0f, 1.0f));
            derechaLateralIzquierda.addVertice(0.5f, 0.8f, 0.1f);
            derechaLateralIzquierda.addVertice(0.5f, 0.8f, -0.1f);
            derechaLateralIzquierda.addVertice(0.5f, -0.8f, -0.1f);
            derechaLateralIzquierda.addVertice(0.5f, -0.8f, 0.1f);

            // Cara lateral derecha de la barra derecha
            Poligono derechaLateralDerecha = new Poligono(new Color4(0.7f, 0.0f, 0.0f, 1.0f));
            derechaLateralDerecha.addVertice(0.8f, 0.8f, 0.1f);
            derechaLateralDerecha.addVertice(0.8f, -0.8f, 0.1f);
            derechaLateralDerecha.addVertice(0.8f, -0.8f, -0.1f);
            derechaLateralDerecha.addVertice(0.8f, 0.8f, -0.1f);

            // Añadir polígonos a la barra derecha
            barraDerecha.add("frontal", derechaFrontal);
            barraDerecha.add("posterior", derechaPosterior);
            barraDerecha.add("superior", derechaSuperior);
            barraDerecha.add("inferior", derechaInferior);
            barraDerecha.add("lateralIzquierda", derechaLateralIzquierda);
            barraDerecha.add("lateralDerecha", derechaLateralDerecha);

            // Parte inferior de la U
            Partes parteInferior = new Partes();

            // Cara frontal de la parte inferior
            Poligono inferiorFrontal = new Poligono(new Color4(1.0f, 0.0f, 0.0f, 1.0f));
            inferiorFrontal.addVertice(-0.5f, -0.8f, 0.1f);
            inferiorFrontal.addVertice(-0.5f, -0.5f, 0.1f);
            inferiorFrontal.addVertice(0.5f, -0.5f, 0.1f);
            inferiorFrontal.addVertice(0.5f, -0.8f, 0.1f);

            // Cara posterior de la parte inferior
            Poligono inferiorPosterior = new Poligono(new Color4(0.8f, 0.0f, 0.0f, 1.0f));
            inferiorPosterior.addVertice(-0.5f, -0.8f, -0.1f);
            inferiorPosterior.addVertice(0.5f, -0.8f, -0.1f);
            inferiorPosterior.addVertice(0.5f, -0.5f, -0.1f);
            inferiorPosterior.addVertice(-0.5f, -0.5f, -0.1f);

            // Cara inferior de la parte inferior
            Poligono caraInferior = new Poligono(new Color4(0.9f, 0.0f, 0.0f, 1.0f));
            caraInferior.addVertice(-0.5f, -0.8f, 0.1f);
            caraInferior.addVertice(0.5f, -0.8f, 0.1f);
            caraInferior.addVertice(0.5f, -0.8f, -0.1f);
            caraInferior.addVertice(-0.5f, -0.8f, -0.1f);

            // Cara superior de la parte inferior
            Poligono caraSuperiorInferior = new Poligono(new Color4(0.9f, 0.0f, 0.0f, 1.0f));
            caraSuperiorInferior.addVertice(-0.5f, -0.5f, 0.1f);
            caraSuperiorInferior.addVertice(-0.5f, -0.5f, -0.1f);
            caraSuperiorInferior.addVertice(0.5f, -0.5f, -0.1f);
            caraSuperiorInferior.addVertice(0.5f, -0.5f, 0.1f);

            // Añadir polígonos a la parte inferior
            parteInferior.add("frontal", inferiorFrontal);
            parteInferior.add("posterior", inferiorPosterior);
            parteInferior.add("inferior", caraInferior);
            parteInferior.add("superior", caraSuperiorInferior);

            // Añadir partes al objeto letra U
            letraU.addParte("barraIzquierda", barraIzquierda);
            letraU.addParte("barraDerecha", barraDerecha);
            letraU.addParte("parteInferior", parteInferior);

            // Añadir el objeto letra U al escenario
            escenario.addObjeto("letraU", letraU);

            return escenario;
        } */

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GL.ClearColor(0.0f, 0.5f, 0.0f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Width, Height);
            float aspectRatio = (float)Width / Height;
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(45.0f), aspectRatio, 0.1f, 100.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            // Actualizar ángulo de rotación para animación
            anguloRotacion += 0.5f * (float)e.Time;

            // Ejemplo: animar uno de los objetos
            Objeto letraU2 = escenario.getObjeto("letraU2");
            if (letraU2 != null)
            {
                letraU2.Rotar(0, anguloRotacion, 0);
            }

            // Manejar entrada del teclado para controlar transformaciones
            var keyboard = Keyboard.GetState();

            // Ejemplo: Mover la primera letra con teclas de flecha
            Objeto letraU = escenario.getObjeto("letraU");
            if (letraU != null)
            {
                Vector3 posicionActual = letraU.Posicion;

                if (keyboard[Key.Up])
                    letraU.Trasladar(posicionActual.X, posicionActual.Y + 0.01f, posicionActual.Z);
                if (keyboard[Key.Down])
                    letraU.Trasladar(posicionActual.X, posicionActual.Y - 0.01f, posicionActual.Z);
                if (keyboard[Key.Left])
                    letraU.Trasladar(posicionActual.X - 0.01f, posicionActual.Y, posicionActual.Z);
                if (keyboard[Key.Right])
                    letraU.Trasladar(posicionActual.X + 0.01f, posicionActual.Y, posicionActual.Z);

                // Escalar con teclas más/menos
                if (keyboard[Key.Plus] || keyboard[Key.KeypadPlus])
                {
                    Vector3 escalaActual = letraU.Escala;
                    letraU.Escalar(escalaActual.X + 0.01f, escalaActual.Y + 0.01f, escalaActual.Z + 0.01f);
                }
                if (keyboard[Key.Minus] || keyboard[Key.KeypadMinus])
                {
                    Vector3 escalaActual = letraU.Escala;
                    letraU.Escalar(escalaActual.X - 0.01f, escalaActual.Y - 0.01f, escalaActual.Z - 0.01f);
                }
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Configura la cámara
            Matrix4 modelview = Matrix4.LookAt(
                new Vector3(0.0f, 0.0f, 4.5f), // Posición de la cámara
                new Vector3(0.0f, 0.1f, 0.0f), // Punto de mira
                Vector3.UnitY); // Vector arriba
            GL.LoadMatrix(ref modelview);

            escenario.dibujar(new Vector3(0, 0, 0));

            SwapBuffers();
        }


    }
}