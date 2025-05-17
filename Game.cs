using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LetraU
{
    public class Game : GameWindow
    {
        private Escenario escenario;
        private bool autoCreado = false;
        private Animacion animacionAuto;
        private bool animacionIniciada = false;

        // Enumeraciones para los modos de edición
        enum ModoEdicion { Escenario, Objeto, Parte }
        enum TipoTransformacion { Traslacion, Rotacion, Escala }

        private ModoEdicion modoActual = ModoEdicion.Objeto;
        private TipoTransformacion transformacionActual = TipoTransformacion.Traslacion;
        private string objetoSeleccionado = "auto";
        private string parteSeleccionada = "rueda1";

        public Game(int width, int height) : base(width, height, GraphicsMode.Default, "Auto sobre Carretera - 3D")
        {
            Console.WriteLine("Creando un nuevo escenario con auto y carretera...");
            try
            {
                escenario = AutoCreator.CrearAutoEscenarioConCarretera();
                autoCreado = true;
                Console.WriteLine("Escenario con auto y carretera creado exitosamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear escenario: {ex.Message}");
                escenario = AutoCreator.CrearAutoEscenario();
                autoCreado = true;
            }

            try
            {
                AutoCreator.GuardarEscenario(escenario, "auto_con_carretera.json");
                Console.WriteLine("Escenario guardado en auto_con_carretera.json");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar escenario: {ex.Message}");
            }

            // Inicializar la animación
            animacionAuto = new Animacion();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GL.ClearColor(0.5f, 0.7f, 1.0f, 1.0f); // Cielo
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            ConfigurarAnimacionAuto();
        }

        private void ConfigurarAnimacionAuto()
        {
            // Posicionar el auto sobre la carretera
            if (escenario != null && escenario.listaDeObjetos.ContainsKey("auto"))
            {
                Objeto auto = escenario.listaDeObjetos["auto"];
                auto.Escalar(0.4f);
                auto.Trasladar(new Vector3(0, 0, 0));
                animacionAuto.Reiniciar();

                Vector3 posicionInicial = new Vector3(-4.0f, 0.2f, 0.0f);

                animacionAuto.AgregarAccion(new AccionTraslacion(auto, posicionInicial, 0.01f));

                Vector3 posicionEsquina = new Vector3(1.5f, 0.3f, 0.0f);  
                Vector3 movimiento1 = posicionEsquina - posicionInicial;
                animacionAuto.AgregarAccion(new AccionTraslacion(auto, movimiento1, 5.0f));

                animacionAuto.AgregarAccion(new AccionRotacionObjeto(auto, "y", 45f, 2.5f));

                Vector3 posicionFinal = new Vector3(4.0f, 0.5f, -4.0f);
                Vector3 movimiento2 = posicionFinal - posicionEsquina;

                animacionAuto.AgregarAccion(new AccionTraslacion(auto, movimiento2, 6.0f));

                ConfigurarAnimacionRuedas(auto);

                animacionIniciada = true;
                Console.WriteLine("Animación del auto configurada correctamente para carretera.");
            }
            else
            {
                Console.WriteLine("No se encontró el objeto 'auto' para posicionar.");
            }
        }

        // Método auxiliar para configurar la animación de las ruedas
        private void ConfigurarAnimacionRuedas(Objeto auto)
        {
            string[] ruedas = { "rueda1", "rueda2", "rueda3", "rueda4" };

            foreach (string ruedaNombre in ruedas)
            {
                if (auto.listaDePartes.ContainsKey(ruedaNombre))
                {
                    try
                    {
                        Partes rueda = auto.getParte(ruedaNombre);

                        // Verificar qué polígonos existen en la rueda
                        Console.WriteLine($"Polígonos en {ruedaNombre}:");
                        foreach (var poligonoKey in rueda.listaDePoligonos.Keys)
                        {
                            Console.WriteLine($"- {poligonoKey}");
                        }

                        // Usar el primer polígono disponible para la animación
                        if (rueda.listaDePoligonos.Count > 0)
                        {
                            string primerPoligono = new List<string>(rueda.listaDePoligonos.Keys)[0];
                            AccionRotacion rotacionRueda = new AccionRotacion(
                                rueda.getPoligono(primerPoligono), "x", 5.0f); // Duración total de la animación

                            animacionAuto.AgregarAccion(rotacionRueda);
                            Console.WriteLine($"Animación de rotación configurada para {ruedaNombre} con polígono '{primerPoligono}'.");
                        }
                        else
                        {
                            Console.WriteLine($"La {ruedaNombre} no tiene polígonos para animar.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al configurar la animación de {ruedaNombre}: {ex.Message}");
                    }
                }
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Width, Height);
            float aspectRatio = (float)Width / Height;
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), aspectRatio, 0.1f, 100.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            // Actualizar la animación
            try
            {
                float deltaTime = (float)e.Time;
                if (animacionIniciada && animacionAuto.EstaEjecutando())
                {
                    animacionAuto.Actualizar(deltaTime);
                }
                else if (animacionIniciada && !animacionAuto.EstaEjecutando())
                {
                    // Si la animación terminó, podemos reiniciarla o hacer otra cosa
                    // Por ejemplo, reiniciar la animación después de un tiempo
                    animacionIniciada = false;
                    Console.WriteLine("Animación completada.");

                    // Si quieres que la animación se reinicie automáticamente, descomenta estas líneas:
                    // System.Threading.Thread.Sleep(2000); // Esperar 2 segundos
                    // ConfigurarAnimacionAuto();            // Reiniciar animación
                    // animacionIniciada = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en la animación: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Cámara - ajustada para ver mejor el auto sobre la carretera
            Matrix4 modelview = Matrix4.LookAt(
                new Vector3(2.0f, 2.0f, 5.0f),  // Posición de la cámara
                new Vector3(0.0f, 0.5f, 0.0f),  // Punto al que mira
                Vector3.UnitY);                 // Vector "arriba"
            GL.LoadMatrix(ref modelview);

            string modoText = $"Modo: {modoActual}, Transformación: {transformacionActual}, Objeto: {objetoSeleccionado}";
            if (modoActual == ModoEdicion.Parte)
                modoText += $", Parte: {parteSeleccionada}";

            Console.WriteLine(modoText);

            if (escenario != null)
            {
                escenario.dibujar(new Vector3(0, 0, 0));
            }
            else
            {
                Console.WriteLine("Error: El escenario es nulo. No se puede dibujar.");
            }

            SwapBuffers();
        }
    }
}