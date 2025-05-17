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

            // Posicionar el auto sobre la carretera
            if (escenario != null && escenario.listaDeObjetos.ContainsKey("auto"))
            {
                Objeto auto = escenario.listaDeObjetos["auto"];
                //animacionAuto.AgregarAccion(new AccionTraslacion(auto, new Vector3(6f, 4f, 0f), 1.5f));
                // Posicionar el auto sobre la carretera
                auto.Escalar(0.4f);
                //auto.Trasladar(new OpenTK.Vector3(-3.0f, 0.3f, 0.0f)); // Y positivo para que el auto esté sobre la carretera
                //auto.Trasladar(new OpenTK.Vector3(0.0f, 0.5f, 0.0f));
               // auto.Trasladar(new OpenTK.Vector3(3.0f, 1.0f, -1.2f));
                //auto.Rotar(new Vector3(0.0f, 45.0f, 0.0f)); // Girar 90 grados en el eje Y
                //Console.WriteLine("Auto posicionado sobre la carretera.");

                // Traslado del inicio a la curva
                Vector3 inicio = new Vector3(-5.0f, 0.6f, 0.0f);
                Vector3 curva = new Vector3(0.0f, 0.8f, 0.0f);
                Vector3 movimiento1 = curva - inicio;
                animacionAuto.AgregarAccion(new AccionTraslacion(auto, movimiento1, 3.0f)); // 2 segundos

                // Rotación en Y 45 grados
                animacionAuto.AgregarAccion(new AccionRotacionObjeto(auto, "y", 45f, 1.0f)); // 1 segundo

                // Traslado desde la curva hasta el final
                Vector3 final = new Vector3(3.0f, 2.0f, -0.2f);
                Vector3 movimiento2 = final - curva;
                animacionAuto.AgregarAccion(new AccionTraslacion(auto, movimiento2, 3.0f)); // 2 segundos

                // Configurar animación de rotación para las ruedas (si existen)
                if (auto.listaDePartes.ContainsKey("rueda1"))
                {
                    try
                    {
                        Partes rueda1 = auto.getParte("rueda1");

                        // Verificar qué polígonos existen en la rueda
                        Console.WriteLine("Polígonos en rueda1:");
                        foreach (var poligonoKey in rueda1.listaDePoligonos.Keys)
                        {
                            Console.WriteLine($"- {poligonoKey}");
                        }

                        // Usar el primer polígono disponible para la animación
                        if (rueda1.listaDePoligonos.Count > 0)
                        {
                            string primerPoligono = new List<string>(rueda1.listaDePoligonos.Keys)[0];
                            AccionRotacion rotacionRueda1 = new AccionRotacion(
                                rueda1.getPoligono(primerPoligono), "x", 2.0f);
                            animacionAuto.AgregarAccion(rotacionRueda1);
                            Console.WriteLine($"Animación de rotación configurada para rueda1 con polígono '{primerPoligono}'.");
                        }
                        else
                        {
                            Console.WriteLine("La rueda1 no tiene polígonos para animar.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al configurar la animación de rueda1: {ex.Message}");
                    }
                }

                if (auto.listaDePartes.ContainsKey("rueda2"))
                {
                    try
                    {
                        Partes rueda2 = auto.getParte("rueda2");

                        // Verificar qué polígonos existen en la rueda
                        Console.WriteLine("Polígonos en rueda2:");
                        foreach (var poligonoKey in rueda2.listaDePoligonos.Keys)
                        {
                            Console.WriteLine($"- {poligonoKey}");
                        }

                        // Usar el primer polígono disponible para la animación
                        if (rueda2.listaDePoligonos.Count > 0)
                        {
                            string primerPoligono = new List<string>(rueda2.listaDePoligonos.Keys)[0];
                            AccionRotacion rotacionRueda2 = new AccionRotacion(
                                rueda2.getPoligono(primerPoligono), "x", 2.0f);
                            animacionAuto.AgregarAccion(rotacionRueda2);
                            Console.WriteLine($"Animación de rotación configurada para rueda2 con polígono '{primerPoligono}'.");
                        }
                        else
                        {
                            Console.WriteLine("La rueda2 no tiene polígonos para animar.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al configurar la animación de rueda2: {ex.Message}");
                    }
                }
            }
            else
            {
                Console.WriteLine("No se encontró el objeto 'auto' para posicionar.");
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
                if (animacionAuto.EstaEjecutando())
                {
                    animacionAuto.Actualizar(deltaTime);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en la animación: {ex.Message}");
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