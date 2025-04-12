namespace LetraT
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var game = new Game(800, 640)) // Crea una instancia de la clase Game
            {
                game.Run(60.0); // Ejecuta la aplicación de juego
            }
        }
    }
}
