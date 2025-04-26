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
        //private float anguloRotacion = 0.0f;

        // Enumeraciones para los modos de edición
        enum ModoEdicion
        {
            Escenario,
            Objeto,
            Parte
        }
        enum TipoTransformacion
        {
            Traslacion,
            Rotacion,
            Escala
        }
        // Variables de control
        private ModoEdicion modoActual = ModoEdicion.Objeto;
        private TipoTransformacion transformacionActual = TipoTransformacion.Traslacion;
        private string objetoSeleccionado = "letraU";
        private string parteSeleccionada = "barraIzquierda";
        private float velocidadTransformacion = 0.01f;
        private float velocidadRotacion = 1.0f;
        private float velocidadEscala = 0.01f;
        public Game(int width, int height) : base(width, height, GraphicsMode.Default, "Diseño Letra U - 3D")
        {
            // Inicializar directamente la letra U en lugar de cargar desde archivo
            //this.escenario = InicializarLetraU();

            //Guardar el escenario para futuras ejecuciones
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

            //letraU2.setColor(new Color4(0.0f, 0.0f, 1.0f, 1.0f)); // Azul
            //letraU3.setColor(new Color4(0.5f, 0.0f, 1.0f, 1.0f)); //Violeta

            // transformaciones
            letraU.Trasladar(new Vector3(-1.5f, 0.0f, 0.0f));
            letraU.Escalar(0.5f);
            letraU.Rotar(0, 0, 30);

            letraU2.Trasladar(new Vector3(0f, 0.0f, 0.0f));
            letraU2.Escalar(0.8f);
            

            letraU3.Trasladar(new Vector3(1.7f, 0.0f, 0.0f));
            letraU3.Escalar(0.9f);
            letraU3.Rotar(50, 0, 0);

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

            // Manejar entrada del teclado
            var keyboard = Keyboard.GetState();

            // Cambiar el modo de edición
            if (keyboard[Key.Number1])
                modoActual = ModoEdicion.Escenario;
            else if (keyboard[Key.Number2])
                modoActual = ModoEdicion.Objeto;
            else if (keyboard[Key.Number3])
                modoActual = ModoEdicion.Parte;

            // Cambiar tipo de transformación
            if (keyboard[Key.T])
                transformacionActual = TipoTransformacion.Traslacion;
            else if (keyboard[Key.R])
                transformacionActual = TipoTransformacion.Rotacion;
            else if (keyboard[Key.E])
                transformacionActual = TipoTransformacion.Escala;

            // Seleccionar objeto
            if (keyboard[Key.F1])
                objetoSeleccionado = "letraU";
            else if (keyboard[Key.F2])
                objetoSeleccionado = "letraU2";
            else if (keyboard[Key.F3])
                objetoSeleccionado = "letraU3";

            // Seleccionar parte (si el modo es Parte)
            if (keyboard[Key.F4])
                parteSeleccionada = "barraIzquierda";
            else if (keyboard[Key.F5])
                parteSeleccionada = "barraDerecha";
            else if (keyboard[Key.F6])
                parteSeleccionada = "parteInferior";

            // Aplicar transformaciones según el modo y tipo seleccionados
            switch (modoActual)
            {
                case ModoEdicion.Escenario:
                    AplicarTransformacionEscenario(keyboard);
                    break;
                case ModoEdicion.Objeto:
                    AplicarTransformacionObjeto(keyboard);
                    break;
                case ModoEdicion.Parte:
                    AplicarTransformacionParte(keyboard);
                    break;
            }
        }

        private void AplicarTransformacionEscenario(KeyboardState keyboard)
        {
            switch (transformacionActual)
            {
                case TipoTransformacion.Traslacion:
                    Vector3 posicionActual = escenario.Posicion;
                    if (keyboard[Key.Up])
                        escenario.Trasladar(posicionActual.X, posicionActual.Y + velocidadTransformacion, posicionActual.Z);
                    if (keyboard[Key.Down])
                        escenario.Trasladar(posicionActual.X, posicionActual.Y - velocidadTransformacion, posicionActual.Z);
                    if (keyboard[Key.Left])
                        escenario.Trasladar(posicionActual.X - velocidadTransformacion, posicionActual.Y, posicionActual.Z);
                    if (keyboard[Key.Right])
                        escenario.Trasladar(posicionActual.X + velocidadTransformacion, posicionActual.Y, posicionActual.Z);
                    if (keyboard[Key.PageUp])
                        escenario.Trasladar(posicionActual.X, posicionActual.Y, posicionActual.Z + velocidadTransformacion);
                    if (keyboard[Key.PageDown])
                        escenario.Trasladar(posicionActual.X, posicionActual.Y, posicionActual.Z - velocidadTransformacion);
                    break;
                case TipoTransformacion.Rotacion:
                    Vector3 rotacionActual = escenario.Rotacion;
                    if (keyboard[Key.Up])
                        escenario.Rotar(rotacionActual.X + velocidadRotacion, rotacionActual.Y, rotacionActual.Z);
                    if (keyboard[Key.Down])
                        escenario.Rotar(rotacionActual.X - velocidadRotacion, rotacionActual.Y, rotacionActual.Z);
                    if (keyboard[Key.Left])
                        escenario.Rotar(rotacionActual.X, rotacionActual.Y + velocidadRotacion, rotacionActual.Z);
                    if (keyboard[Key.Right])
                        escenario.Rotar(rotacionActual.X, rotacionActual.Y - velocidadRotacion, rotacionActual.Z);
                    if (keyboard[Key.PageUp])
                        escenario.Rotar(rotacionActual.X, rotacionActual.Y, rotacionActual.Z + velocidadRotacion);
                    if (keyboard[Key.PageDown])
                        escenario.Rotar(rotacionActual.X, rotacionActual.Y, rotacionActual.Z - velocidadRotacion);
                    break;
                case TipoTransformacion.Escala:
                    Vector3 escalaActual = escenario.Escala;
                    if (keyboard[Key.Plus] || keyboard[Key.KeypadPlus])
                        escenario.Escalar(escalaActual.X + velocidadEscala, escalaActual.Y + velocidadEscala, escalaActual.Z + velocidadEscala);
                    if (keyboard[Key.Minus] || keyboard[Key.KeypadMinus])
                        escenario.Escalar(escalaActual.X - velocidadEscala, escalaActual.Y - velocidadEscala, escalaActual.Z - velocidadEscala);
                    if (keyboard[Key.X])
                        escenario.Escalar(escalaActual.X + velocidadEscala, escalaActual.Y, escalaActual.Z);
                    if (keyboard[Key.Y])
                        escenario.Escalar(escalaActual.X, escalaActual.Y + velocidadEscala, escalaActual.Z);
                    if (keyboard[Key.Z])
                        escenario.Escalar(escalaActual.X, escalaActual.Y, escalaActual.Z + velocidadEscala);
                    break;
            }
        }

        private void AplicarTransformacionObjeto(KeyboardState keyboard)
        {
            Objeto objetoActual = escenario.getObjeto(objetoSeleccionado);
            if (objetoActual == null) return;

            switch (transformacionActual)
            {
                case TipoTransformacion.Traslacion:
                    Vector3 posicionActual = objetoActual.Posicion;
                    if (keyboard[Key.Up])
                        objetoActual.Trasladar(posicionActual.X, posicionActual.Y + velocidadTransformacion, posicionActual.Z);
                    if (keyboard[Key.Down])
                        objetoActual.Trasladar(posicionActual.X, posicionActual.Y - velocidadTransformacion, posicionActual.Z);
                    if (keyboard[Key.Left])
                        objetoActual.Trasladar(posicionActual.X - velocidadTransformacion, posicionActual.Y, posicionActual.Z);
                    if (keyboard[Key.Right])
                        objetoActual.Trasladar(posicionActual.X + velocidadTransformacion, posicionActual.Y, posicionActual.Z);
                    if (keyboard[Key.PageUp])
                        objetoActual.Trasladar(posicionActual.X, posicionActual.Y, posicionActual.Z + velocidadTransformacion);
                    if (keyboard[Key.PageDown])
                        objetoActual.Trasladar(posicionActual.X, posicionActual.Y, posicionActual.Z - velocidadTransformacion);
                    break;
                case TipoTransformacion.Rotacion:
                    Vector3 rotacionActual = objetoActual.Rotacion;
                    if (keyboard[Key.Up])
                        objetoActual.Rotar(rotacionActual.X + velocidadRotacion, rotacionActual.Y, rotacionActual.Z);
                    if (keyboard[Key.Down])
                        objetoActual.Rotar(rotacionActual.X - velocidadRotacion, rotacionActual.Y, rotacionActual.Z);
                    if (keyboard[Key.Left])
                        objetoActual.Rotar(rotacionActual.X, rotacionActual.Y + velocidadRotacion, rotacionActual.Z);
                    if (keyboard[Key.Right])
                        objetoActual.Rotar(rotacionActual.X, rotacionActual.Y - velocidadRotacion, rotacionActual.Z);
                    if (keyboard[Key.PageUp])
                        objetoActual.Rotar(rotacionActual.X, rotacionActual.Y, rotacionActual.Z + velocidadRotacion);
                    if (keyboard[Key.PageDown])
                        objetoActual.Rotar(rotacionActual.X, rotacionActual.Y, rotacionActual.Z - velocidadRotacion);
                    break;
                case TipoTransformacion.Escala:
                    Vector3 escalaActual = objetoActual.Escala;
                    if (keyboard[Key.Plus] || keyboard[Key.KeypadPlus])
                        objetoActual.Escalar(escalaActual.X + velocidadEscala, escalaActual.Y + velocidadEscala, escalaActual.Z + velocidadEscala);
                    if (keyboard[Key.Minus] || keyboard[Key.KeypadMinus])
                        objetoActual.Escalar(escalaActual.X - velocidadEscala, escalaActual.Y - velocidadEscala, escalaActual.Z - velocidadEscala);
                    if (keyboard[Key.X])
                        objetoActual.Escalar(escalaActual.X + velocidadEscala, escalaActual.Y, escalaActual.Z);
                    if (keyboard[Key.Y])
                        objetoActual.Escalar(escalaActual.X, escalaActual.Y + velocidadEscala, escalaActual.Z);
                    if (keyboard[Key.Z])
                        objetoActual.Escalar(escalaActual.X, escalaActual.Y, escalaActual.Z + velocidadEscala);
                    break;
            }
        }

        private void AplicarTransformacionParte(KeyboardState keyboard)
        {
            Objeto objetoActual = escenario.getObjeto(objetoSeleccionado);
            if (objetoActual == null) return;

            Partes parteActual = objetoActual.getParte(parteSeleccionada);
            if (parteActual == null) return;

            switch (transformacionActual)
            {
                case TipoTransformacion.Traslacion:
                    Vector3 posicionActual = parteActual.Posicion;
                    if (keyboard[Key.Up])
                        parteActual.Trasladar(posicionActual.X, posicionActual.Y + velocidadTransformacion, posicionActual.Z);
                    if (keyboard[Key.Down])
                        parteActual.Trasladar(posicionActual.X, posicionActual.Y - velocidadTransformacion, posicionActual.Z);
                    if (keyboard[Key.Left])
                        parteActual.Trasladar(posicionActual.X - velocidadTransformacion, posicionActual.Y, posicionActual.Z);
                    if (keyboard[Key.Right])
                        parteActual.Trasladar(posicionActual.X + velocidadTransformacion, posicionActual.Y, posicionActual.Z);
                    if (keyboard[Key.PageUp])
                        parteActual.Trasladar(posicionActual.X, posicionActual.Y, posicionActual.Z + velocidadTransformacion);
                    if (keyboard[Key.PageDown])
                        parteActual.Trasladar(posicionActual.X, posicionActual.Y, posicionActual.Z - velocidadTransformacion);
                    break;
                case TipoTransformacion.Rotacion:
                    Vector3 rotacionActual = parteActual.Rotacion;
                    if (keyboard[Key.Up])
                        parteActual.Rotar(rotacionActual.X + velocidadRotacion, rotacionActual.Y, rotacionActual.Z);
                    if (keyboard[Key.Down])
                        parteActual.Rotar(rotacionActual.X - velocidadRotacion, rotacionActual.Y, rotacionActual.Z);
                    if (keyboard[Key.Left])
                        parteActual.Rotar(rotacionActual.X, rotacionActual.Y + velocidadRotacion, rotacionActual.Z);
                    if (keyboard[Key.Right])
                        parteActual.Rotar(rotacionActual.X, rotacionActual.Y - velocidadRotacion, rotacionActual.Z);
                    if (keyboard[Key.PageUp])
                        parteActual.Rotar(rotacionActual.X, rotacionActual.Y, rotacionActual.Z + velocidadRotacion);
                    if (keyboard[Key.PageDown])
                        parteActual.Rotar(rotacionActual.X, rotacionActual.Y, rotacionActual.Z - velocidadRotacion);
                    break;
                case TipoTransformacion.Escala:
                    Vector3 escalaActual = parteActual.Escala;
                    if (keyboard[Key.Plus] || keyboard[Key.KeypadPlus])
                        parteActual.Escalar(escalaActual.X + velocidadEscala, escalaActual.Y + velocidadEscala, escalaActual.Z + velocidadEscala);
                    if (keyboard[Key.Minus] || keyboard[Key.KeypadMinus])
                        parteActual.Escalar(escalaActual.X - velocidadEscala, escalaActual.Y - velocidadEscala, escalaActual.Z - velocidadEscala);
                    if (keyboard[Key.X])
                        parteActual.Escalar(escalaActual.X + velocidadEscala, escalaActual.Y, escalaActual.Z);
                    if (keyboard[Key.Y])
                        parteActual.Escalar(escalaActual.X, escalaActual.Y + velocidadEscala, escalaActual.Z);
                    if (keyboard[Key.Z])
                        parteActual.Escalar(escalaActual.X, escalaActual.Y, escalaActual.Z + velocidadEscala);
                    break;
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

            // Renderizar estado actual
            string modoText = $"Modo: {modoActual}, Transformación: {transformacionActual}, Objeto: {objetoSeleccionado}";
            if (modoActual == ModoEdicion.Parte)
                modoText += $", Parte: {parteSeleccionada}";

            // Sería ideal mostrar este texto en pantalla, pero eso requeriría una biblioteca de texto
            Console.WriteLine(modoText);

            // Dibujar escenario con las transformaciones aplicadas
            escenario.dibujar(new Vector3(0, 0, 0));

            SwapBuffers();
        }
    }
}