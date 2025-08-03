using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using System;

namespace SpaceInvaders.Models
{
    /// <summary>
    /// Representa um único inimigo (alien) na tela.
    /// Ele "herda" da classe GameObject, então já tem posição (X, Y), largura, altura, etc.
    /// </summary>
    public class Enemy : GameObject
    {
        // Guarda os pontos que este inimigo específico vale.
        public int Points { get; }

        /// <summary>
        /// Construtor do Inimigo.
        /// </summary>
        /// <param name="type">O tipo de inimigo (que contém a imagem e os pontos).</param>
        /// <param name="initialX">Posição X inicial.</param>
        /// <param name="initialY">Posição Y inicial.</param>
        public Enemy(EnemyType type, double initialX, double initialY) 
            // Aqui chamamos o construtor da classe "pai" (GameObject),
            // criando uma nova imagem para este inimigo.
            : base(new Image
            {
                Source = new BitmapImage(new Uri(type.ImageSource)),
                Width = 40,
                Height = 40
            })
        {
            // Definimos a posição inicial e os pontos.
            X = initialX;
            Y = initialY;
            Points = type.Points;
        }
    }
}
