using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using System;

namespace SpaceInvaders.Models
{
    /// <summary>
    /// Representa a nave vermelha (UFO) que atravessa o topo do ecrã.
    /// </summary>
    public class Ufo : GameObject
    {
        public int Points { get; }
        public double Speed { get; }

        public Ufo(double x, double y, double speed) 
            : base(new Image { Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/alien4.png")) }, 45, 25)
        {
            X = x;
            Y = y;
            Speed = speed;

            // A pontuação da nave vermelha é aleatória
            int[] possibleScores = { 50, 100, 150, 200, 300 };
            Points = possibleScores[new Random().Next(possibleScores.Length)];
        }

        /// <summary>
        /// Move o UFO horizontalmente.
        /// </summary>
        public void Update()
        {
            X += Speed;
        }
    }
}
