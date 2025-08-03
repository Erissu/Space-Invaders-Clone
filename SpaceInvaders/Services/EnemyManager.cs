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
        // ... (o resto do código aqui não muda) ...
        private readonly Canvas _gameCanvas;
        private readonly Dictionary<string, EnemyType> _enemyTypes;
        
        public List<Enemy> Enemies { get; private set; } = new();

        private double _enemyDirection = 1;
        private double _enemySpeed = 1;
        private int _ticksSinceLastMove = 0;
        private int _moveThreshold = 30;

        public bool IsSwarmDestroyed => !Enemies.Any();

        public EnemyManager(Canvas gameCanvas, Dictionary<string, EnemyType> enemyTypes)
        {
            _gameCanvas = gameCanvas;
            _enemyTypes = enemyTypes;
        }

        public void SpawnWave()
        {
            Enemies.Clear();

            double startX = 50;
            double startY = 50;
            int enemySpacing = 50; 

            string[] rowTypes = { "alien3", "alien3", "alien2", "alien2", "alien1", "alien1" };

            for (int row = 0; row < rowTypes.Length; row++)
            {
                for (int col = 0; col < 11; col++)
                {
                    var enemyType = _enemyTypes[rowTypes[row]];
                    var enemy = new Enemy(enemyType, startX + col * enemySpacing, startY + row * enemySpacing);
                    Enemies.Add(enemy);
                    _gameCanvas.Children.Add(enemy.Visual);
                }
            }
        }

        /// <summary>
        /// Remove um inimigo do jogo (quando ele é atingido).
        /// </summary>
        public void RemoveEnemy(Enemy enemy)
        {
            // CORREÇÃO: Usando o método Kill() para ser consistente.
            enemy.Kill(); 
            _gameCanvas.Children.Remove(enemy.Visual);
            Enemies.Remove(enemy);
    
            if (_moveThreshold > 5)
            {
                _moveThreshold -= 1;
            }
        }
        
        // ... (o resto do arquivo EnemyManager.cs continua igual) ...
        public void Update(double gameWidth, List<Rectangle> barriers)
        {
            _ticksSinceLastMove++;
            if (_ticksSinceLastMove < _moveThreshold)
            {
                return;
            }
            _ticksSinceLastMove = 0;

            bool changeDirection = false;
            foreach (var enemy in Enemies)
            {
                if ((_enemyDirection > 0 && enemy.X + enemy.Width > gameWidth) || (_enemyDirection < 0 && enemy.X < 0))
                {
                    changeDirection = true;
                    break;
                }
            }

            if (changeDirection)
            {
                _enemyDirection *= -1;
                foreach (var enemy in Enemies)
                {
                    enemy.Y += 20;
                }
            }
            else
            {
                foreach (var enemy in Enemies)
                {
                    enemy.X += _enemySpeed * _enemyDirection;
                }
            }
            
            foreach (var enemy in Enemies)
            {
                enemy.Update();

                foreach (var block in barriers.ToList())
                {
                    double ax = Canvas.GetLeft(enemy.Visual);
                    double ay = Canvas.GetTop(enemy.Visual);
                    double bx = Canvas.GetLeft(block);
                    double by = Canvas.GetTop(block);

                    if (ax < bx + block.Width && ax + enemy.Width > bx && ay < by + block.Height && ay + enemy.Height > by)
                    {
                        _gameCanvas.Children.Remove(block);
                        barriers.Remove(block);
                    }
                }
            }
        }
        
        public Enemy? GetRandomShooter()
        {
            if (!Enemies.Any()) return null;

            var random = new Random();
            
            var columns = Enemies.GroupBy(e => e.X);
            
            var nonEmptyColumns = columns.Where(c => c.Any()).ToList();
            if (!nonEmptyColumns.Any()) return null;

            var randomColumn = nonEmptyColumns[random.Next(nonEmptyColumns.Count)];
            
            return randomColumn.OrderByDescending(e => e.Y).FirstOrDefault();
        }
    }
}
