using Microsoft.UI.Xaml.Shapes;
using spaceInvaders.Models;
using System;
using System.Collections.Generic;
using Windows.UI;

namespace spaceInvaders.GameLogic;

// A única responsabilidade desta classe é criar os objetos do jogo.
internal class GameObjectFactory
{
    public GameObjectFactory() { }
    public List<Alien> CreateAliens()
    {
        var aliens = new List<Alien>();
        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 10; col++)
            {
                Color alienColor; int scoreValue;
                if (row == 0) { alienColor = Color.FromArgb(255, 255, 100, 100); scoreValue = 30; }
                else if (row < 3) { alienColor = Color.FromArgb(255, 255, 180, 0); scoreValue = 20; }
                else { alienColor = Color.FromArgb(255, 255, 0, 255); scoreValue = 10; }
                
                var alien = new Alien(scoreValue, alienColor)
                {
                    X = 60 + col * 55,
                    Y = 50 + row * 40
                };
                aliens.Add(alien);
            }
        }
        return aliens;
    }

    public List<Rectangle> CreateBarriers()
    {
        var barriers = new List<Rectangle>();
        int barrierCount = 4;
        double barrierBlockWidth = 10, barrierBlockHeight = 10;
        double barrierStartX = 100, barrierSpacing = 160;

        for (int i = 0; i < barrierCount; i++)
        {
            double currentBarrierX = barrierStartX + (i * barrierSpacing);
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 6; col++)
                {
                    var block = new Rectangle
                    {
                        Width = barrierBlockWidth, Height = barrierBlockHeight,
                        Fill = new Microsoft.UI.Xaml.Media.SolidColorBrush(Color.FromArgb(255, 0, 200, 0))
                    };
                    Canvas.SetLeft(block, currentBarrierX + col * barrierBlockWidth);
                    Canvas.SetTop(block, 440 + row * barrierBlockHeight);
                    barriers.Add(block);
                }
            }
        }
        return barriers;
    }

    public Bullet CreatePlayerBullet(Player player)
    {
        return new Bullet(Bullet.BulletType.Player)
        {
            X = player.X + player.Width / 2 - 2.5, // 2.5 é a metade da largura da bala
            Y = player.Y - 15 // um pouco acima do jogador
        };
    }

    public Bullet CreateEnemyBullet(Alien shootingAlien)
    {
        return new Bullet(Bullet.BulletType.Enemy)
        {
            X = shootingAlien.X + shootingAlien.Width / 2 - 2.5,
            Y = shootingAlien.Y + shootingAlien.Height
        };
    }
}
