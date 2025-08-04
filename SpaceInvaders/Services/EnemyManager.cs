using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Shapes;
using SpaceInvaders.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceInvaders.Services
{
    public class EnemyManager
    {
        private readonly Canvas _gameCanvas;
        private readonly Dictionary<string, EnemyType> _enemyTypes;
        private readonly List<Enemy> _enemies = new();
        private readonly Random _random = new();

        private double _swarmX;
        private double _swarmY;
        private double _swarmSpeed = 5;
        private bool _movingRight = true;
        
        private const double SwarmMoveDownStep = 20;
        private double _moveTimer;
        private double _moveInterval = 1.0;

        public List<Enemy> Enemies => _enemies;
        public bool IsSwarmDestroyed => !_enemies.Any();
        
        public EnemyManager(Canvas gameCanvas, Dictionary<string, EnemyType> enemyTypes)
        {
            _gameCanvas = gameCanvas;
            _enemyTypes = enemyTypes;
        }

        public void SpawnWave()
        {
            Enemies.Clear();
            _moveInterval = 1.0; 
            _moveTimer = _moveInterval;
            
            string[] alienRows = { "alien3", "alien2", "alien2", "alien1", "alien1" };
            for (int row = 0; row < alienRows.Length; row++)
            {
                for (int col = 0; col < 11; col++)
                {
                    var enemyTypeKey = alienRows[row];
                    double initialX = col * 55 + 30;
                    double initialY = row * 45 + 50;
                    var enemy = new Enemy(_enemyTypes[enemyTypeKey], initialX, initialY);
                    
                    Enemies.Add(enemy);
                    _gameCanvas.Children.Add(enemy.Visual);
                }
            }
            _swarmX = 0;
            _swarmY = 0;
        }

        // A assinatura do método está correta e espera as barreiras.
        public void Update(double canvasWidth, List<Rectangle> barriers)
        {
            _moveTimer -= 0.02; 
            if (_moveTimer <= 0)
            {
                bool wallHit = false;
                double nextSwarmX = _swarmX + (_movingRight ? _swarmSpeed : -_swarmSpeed);

                foreach (var enemy in Enemies)
                {
                    double futureX = enemy.X - _swarmX + nextSwarmX;
                    if (futureX < 0 || futureX + enemy.Width > canvasWidth)
                    {
                        wallHit = true;
                        break;
                    }
                }

                if (wallHit)
                {
                    _movingRight = !_movingRight;
                    _swarmY += SwarmMoveDownStep;
                }
                else
                {
                    _swarmX = nextSwarmX;
                }
                
                foreach (var enemy in Enemies)
                {
                    enemy.UpdatePosition(_swarmX, _swarmY);
                }
                
                double remainingRatio = (double)Enemies.Count / 55.0;
                _moveInterval = remainingRatio;
                if (_moveInterval < 0.1) _moveInterval = 0.1;
                _moveTimer = _moveInterval;
            }
            
            // Lógica de colisão dos inimigos com as barreiras
            foreach (var enemy in Enemies.ToList())
            {
                foreach (var block in barriers.ToList())
                {
                    if (CheckCollision(enemy.Visual, block))
                    {
                        _gameCanvas.Children.Remove(block);
                        barriers.Remove(block);
                    }
                }
            }
        }

        public void RemoveEnemy(Enemy enemy)
        {
            _gameCanvas.Children.Remove(enemy.Visual);
            Enemies.Remove(enemy);
        }

        public Enemy? GetRandomShooter()
        {
            if (!Enemies.Any()) return null;
            return Enemies[_random.Next(Enemies.Count)];
        }
        
        private bool CheckCollision(FrameworkElement elementA, FrameworkElement elementB)
        {
            if (elementA is null || elementB is null) return false;
            double ax = Canvas.GetLeft(elementA);
            double ay = Canvas.GetTop(elementA);
            double bx = Canvas.GetLeft(elementB);
            double by = Canvas.GetTop(elementB);
            return ax < bx + elementB.Width && ax + elementA.Width > bx && ay < by + elementB.Height && ay + elementA.Height > by;
        }
    }
}
