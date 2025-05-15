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
        private Animacion animacion;
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
            animacion = new Animacion();

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


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GL.ClearColor(0.0f, 0.5f, 0.0f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

        AccionTraslacion accionTraslacionDerecha = new AccionTraslacion(
        escenario1.getObjeto("auto"), new Vector3(0.01f, 0, 0), duracion: 5f);

        // Agregar la acción a la animación
        animacion.AgregarAccion(accionTraslacionDerecha);

        AccionRotacion accionRotacionRueda1 = new AccionRotacion(
            escenario1.getObjeto("auto").getParte("rueda1").getPoligono("rueda1"), "z", 5);
        animacion.AgregarAccion(accionRotacionRueda1);
        AccionRotacion accionRotacionRueda2 = new AccionRotacion(
            escenario1.getObjeto("auto").getParte("rueda2").getPoligono("rueda2"), "z", 5);
        animacion.AgregarAccion(accionRotacionRueda2);
        AccionRotacion accionRotacionRueda3 = new AccionRotacion(
            escenario1.getObjeto("auto").getParte("rueda3").getPoligono("rueda3"), "z", 5);
        animacion.AgregarAccion(accionRotacionRueda3);
        AccionRotacion accionRotacionRueda4 = new AccionRotacion(
            escenario1.getObjeto("auto").getParte("rueda4").getPoligono("rueda4"), "z", 5);
        animacion.AgregarAccion(accionRotacionRueda4);

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
        // Actualizar la animación
        animacion.Actualizar((float)e.Time);

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

        if (!animacion.EstaEjecutando())
        {
            AccionTraslacionCaida girar = new AccionTraslacionCaida(
                escenario1.getObjeto("auto"), new Vector3(0,-0.02f,0), 7);
            animacion.AgregarAccion(girar);
        }

            SwapBuffers();
        }
    }
}