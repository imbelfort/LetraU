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
        private bool autoCreado = false;

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
        private string objetoSeleccionado = "auto";
        private string parteSeleccionada = "rueda1";
        private float velocidadTransformacion = 0.01f;
        private float velocidadRotacion = 1.0f;
        private float velocidadEscala = 0.01f;
        private bool animacionActiva = true;
        private bool autoEnMovimiento = true;
        private float distanciaRecorrida = 0f;
        private const float DISTANCIA_MAXIMA = 3.0f;

        public Game(int width, int height) : base(width, height, GraphicsMode.Default, "Auto Animado - 3D")
        {
            // Crear un nuevo auto primero
            Console.WriteLine("Creando un nuevo auto...");
            escenario = AutoCreator.CrearAutoEscenario();

            // Guardar el escenario para futuras ejecuciones
            try
            {
                AutoCreator.GuardarEscenario(escenario, "auto.json");
                Console.WriteLine("Auto creado y guardado en auto.json");
                autoCreado = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar auto.json: {ex.Message}");
            }

            try
            {
                // Si quieres cargar desde el archivo recién creado (opcional)
                // escenario = Serializador.DeserializarObjeto<Escenario>("auto.json");
                // if (escenario != null)
                // {
                //    Console.WriteLine("Auto cargado correctamente desde auto.json");
                //    autoCreado = true;
                // }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar auto.json: {ex.Message}");
            }

            // Inicializar la animación
            animacion = new Animacion();

            // Aplicar transformaciones iniciales al auto
            if (autoCreado && escenario.listaDeObjetos.ContainsKey("auto"))
            {
                Objeto auto = escenario.getObjeto("auto");
                auto.Trasladar(new Vector3(-1.5f, 0.0f, 0.0f));
                auto.Escalar(0.8f);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GL.ClearColor(0.0f, 0.5f, 0.0f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            // Habilitar transparencia para las ventanas del auto
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            // Solo iniciar la animación si el escenario y el auto existen
            if (escenario != null && autoCreado)
            {
                IniciarAnimacionRuedas();
            }
            else
            {
                Console.WriteLine("Advertencia: No se puede iniciar la animación. El escenario o el auto no están disponibles.");
            }
        }

        private void IniciarAnimacionRuedas()
        {
            if (!autoCreado || escenario == null) return;

            try
            {
                Objeto auto = escenario.getObjeto("auto");

                // Agregar animación de traslación para el auto completo
                AccionTraslacion accionTraslacionDerecha = new AccionTraslacion(
                    auto, new Vector3(0.01f, 0, 0), duracion: 5f);
                animacion.AgregarAccion(accionTraslacionDerecha);

                // Verificar que las ruedas existen antes de animar
                string[] ruedas = { "rueda1", "rueda2", "rueda3", "rueda4" };
                foreach (string rueda in ruedas)
                {
                    if (auto.listaDePartes.ContainsKey(rueda) &&
                        auto.getParte(rueda).listaDePoligonos.ContainsKey(rueda))
                    {
                        AccionRotacion accionRotacionRueda = new AccionRotacion(
                            auto.getParte(rueda).getPoligono(rueda), "z", 5);
                        animacion.AgregarAccion(accionRotacionRueda);
                    }
                    else
                    {
                        Console.WriteLine($"Advertencia: La rueda {rueda} no existe en el auto");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al iniciar animación de ruedas: {ex.Message}");
            }
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

            if (animacionActiva && escenario != null)
            {
                // Actualizar la animación
                animacion.Actualizar((float)e.Time);
            }

            // Manejar entrada del teclado
            var keyboard = Keyboard.GetState();

            // Toggle animación con tecla A
            if (keyboard.IsKeyDown(Key.A) && !prevKeyboard.IsKeyDown(Key.A))
            {
                animacionActiva = !animacionActiva;
                Console.WriteLine($"Animación: {(animacionActiva ? "Activada" : "Desactivada")}");
            }

            // Reset posición con tecla R
            if (keyboard.IsKeyDown(Key.R) && !prevKeyboard.IsKeyDown(Key.R))
            {
                ReiniciarPosicionAuto();
            }

            // Control manual del auto con flechas
            if (!animacionActiva && autoCreado && escenario != null)
            {
                try
                {
                    Objeto auto = escenario.getObjeto("auto");
                    Vector3 nuevaPosicion = auto.Posicion;

                    if (keyboard.IsKeyDown(Key.Right))
                    {
                        nuevaPosicion.X += velocidadTransformacion * 3;
                        RotarRuedas(5);
                    }
                    if (keyboard.IsKeyDown(Key.Left))
                    {
                        nuevaPosicion.X -= velocidadTransformacion * 3;
                        RotarRuedas(-5);
                    }
                    if (keyboard.IsKeyDown(Key.Up))
                    {
                        nuevaPosicion.Z += velocidadTransformacion * 3;
                        RotarRuedas(5);
                    }
                    if (keyboard.IsKeyDown(Key.Down))
                    {
                        nuevaPosicion.Z -= velocidadTransformacion * 3;
                        RotarRuedas(5);
                    }

                    auto.Trasladar(nuevaPosicion);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error en control manual: {ex.Message}");
                }
            }

            prevKeyboard = keyboard;
        }

        private KeyboardState prevKeyboard;

        private void RotarRuedas(float angulo)
        {
            if (!autoCreado || escenario == null) return;

            try
            {
                Objeto auto = escenario.getObjeto("auto");
                string[] ruedas = { "rueda1", "rueda2", "rueda3", "rueda4" };

                foreach (string rueda in ruedas)
                {
                    if (auto.listaDePartes.ContainsKey(rueda) &&
                        auto.getParte(rueda).listaDePoligonos.ContainsKey(rueda))
                    {
                        auto.getParte(rueda).getPoligono(rueda).RotarAlrededorCentroMasa(angulo);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al rotar ruedas: {ex.Message}");
            }
        }

        private void ReiniciarPosicionAuto()
        {
            if (!autoCreado || escenario == null) return;

            try
            {
                Objeto auto = escenario.getObjeto("auto");
                auto.Trasladar(new Vector3(-1.5f, 0.0f, 0.0f));
                auto.Escalar(0.8f);
                auto.Rotar(new Vector3(0, 0, 0));

                distanciaRecorrida = 0f;
                autoEnMovimiento = true;

                // Reiniciar animación
                animacion.Reiniciar();
                IniciarAnimacionRuedas();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al reiniciar posición: {ex.Message}");
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Configura la cámara
            Matrix4 modelview = Matrix4.LookAt(
                new Vector3(0.0f, 1.0f, 4.5f), // Posición de la cámara
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
            if (escenario != null)
            {
                escenario.dibujar(new Vector3(0, 0, 0));
            }
            else
            {
                Console.WriteLine("Error: El escenario es nulo. No se puede dibujar.");
            }

            // Si no hay animación activa o se terminó, iniciar una nueva
            if (autoCreado && escenario != null && !animacion.EstaEjecutando() && animacionActiva)
            {
                if (autoEnMovimiento)
                {
                    try
                    {
                        Objeto auto = escenario.getObjeto("auto");
                        distanciaRecorrida += 0.01f;

                        // Si llega al límite derecho, iniciar animación de caída
                        if (distanciaRecorrida >= DISTANCIA_MAXIMA)
                        {
                            AccionTraslacionCaida caida = new AccionTraslacionCaida(
                                auto, new Vector3(0, -0.02f, 0), 7);
                            animacion.AgregarAccion(caida);
                            autoEnMovimiento = false;
                        }
                        else
                        {
                            // Continuar movimiento normal
                            IniciarAnimacionRuedas();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error en animación: {ex.Message}");
                    }
                }
                else
                {
                    // Después de la caída, reiniciar la posición
                    ReiniciarPosicionAuto();
                }
            }

            SwapBuffers();
        }
    }
}