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
            escenario = Serializador.DeserializarObjeto<Escenario>("auto.json");

            // Inicializar Objetos
            Objeto letraU = escenario.getObjeto("letraU");


            // transformaciones
            letraU.Trasladar(new Vector3(-1.5f, 0.0f, 0.0f));
            letraU.Escalar(0.5f);
            letraU.Rotar(0, 0, 30);

        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GL.ClearColor(0.0f, 0.5f, 0.0f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

        AccionTraslacion accionTraslacionDerecha = new AccionTraslacion(
        escenario.getObjeto("auto"), new Vector3(0.01f, 0, 0), duracion: 5f);

        // Agregar la acción a la animación
        animacion.AgregarAccion(accionTraslacionDerecha);

        AccionRotacion accionRotacionRueda1 = new AccionRotacion(
            escenario.getObjeto("auto").getParte("rueda1").getPoligono("rueda1"), "z", 5);
        animacion.AgregarAccion(accionRotacionRueda1);
        AccionRotacion accionRotacionRueda2 = new AccionRotacion(
            escenario.getObjeto("auto").getParte("rueda2").getPoligono("rueda2"), "z", 5);
        animacion.AgregarAccion(accionRotacionRueda2);
        AccionRotacion accionRotacionRueda3 = new AccionRotacion(
            escenario.getObjeto("auto").getParte("rueda3").getPoligono("rueda3"), "z", 5);
        animacion.AgregarAccion(accionRotacionRueda3);
        AccionRotacion accionRotacionRueda4 = new AccionRotacion(
            escenario.getObjeto("auto").getParte("rueda4").getPoligono("rueda4"), "z", 5);
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