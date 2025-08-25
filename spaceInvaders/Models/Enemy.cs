﻿using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using System;

namespace SpaceInvaders.Models
{
    public class Enemy : GameObject
    {
        public int Points { get; }
        private readonly double _initialX;
        private readonly double _initialY;

        public Enemy(EnemyType type, double initialX, double initialY) 
            // CORRIGIDO: Aumentei o tamanho de 40x30 para 50x38
            : base(new Image { Source = new BitmapImage(new Uri(type.ImageSource)) }, 50, 38)
        {
            Points = type.Points;
            _initialX = initialX;
            _initialY = initialY;
            X = _initialX;
            Y = _initialY;
        }

        public void UpdatePosition(double swarmOffsetX, double swarmOffsetY)
        {
            X = _initialX + swarmOffsetX;
            Y = _initialY + swarmOffsetY;
        }
    }
}
