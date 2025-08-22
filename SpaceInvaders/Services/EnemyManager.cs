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

        private double _swarmX, _swarmY;
        
        private double _swarmSpeed = 12;
        
        private bool _movingRight = true;
        private const double SwarmMoveDownStep = 20;
        private double _moveTimer;
        private double _moveInterval;
        
        // úmero total de inimigos agora é 50 (10 colunas * 5 linhas)
        private const double TotalEnemies = 50.0;

        public List<Enemy> Enemies => _enemies;
        public bool IsSwarmDestroyed => !_enemies.Any();
        public bool AliensHaveLanded { get; private set; }

        public EnemyManager(Canvas gameCanvas, Dictionary<string, EnemyType> enemyTypes)
        {
            _gameCanvas = gameCanvas;
            _enemyTypes = enemyTypes;
        }

        public void SpawnWave()
        {
            AliensHaveLanded = false;
            _enemies.Clear();
            
            _moveInterval = 0.7;
            _moveTimer = _moveInterval;

            string[] alienRows = { "alien3", "alien2", "alien2", "alien1", "alien1" };
            // CORRIGIDO: O loop agora cria 10 colunas em vez de 11.
            for (int row = 0; row < alienRows.Length; row++) {
                for (int col = 0; col < 10; col++) {
                    var enemyTypeKey = alienRows[row];
                    double initialX = col * 65 + 30;
                    double initialY = row * 50 + 50;
                    var enemy = new Enemy(_enemyTypes[enemyTypeKey], initialX, initialY);
                    _enemies.Add(enemy);
                    _gameCanvas.Children.Add(enemy.Visual);
                }
            }
            _swarmX = 0;
            _swarmY = 0;
        }

        public void Update(double canvasWidth, List<Rectangle> barriers)
        {
            _moveTimer -= 0.02; 
            if (_moveTimer <= 0) {
                bool wallHit = false;
                double nextSwarmX = _swarmX + (_movingRight ? _swarmSpeed : -_swarmSpeed);
                foreach (var enemy in _enemies) {
                    double futureX = enemy.X - _swarmX + nextSwarmX;
                    if (futureX < 0 || futureX + enemy.Width > canvasWidth) {
                        wallHit = true;
                        break;
                    }
                }
                if (wallHit) {
                    _movingRight = !_movingRight;
                    _swarmY += SwarmMoveDownStep;
                } else {
                    _swarmX = nextSwarmX;
                }
                foreach (var enemy in _enemies) {
                    enemy.UpdatePosition(_swarmX, _swarmY);
                    if (enemy.Y + enemy.Height >= 550) AliensHaveLanded = true;
                }
                
                // CORRIGIDO: A fórmula de velocidade agora usa o novo total de inimigos.
                double remainingRatio = (double)_enemies.Count / TotalEnemies;
                _moveInterval = (remainingRatio * 0.6) + 0.04;
                _moveTimer = _moveInterval;
            }
        }

        public void RemoveEnemy(Enemy enemy) { _gameCanvas.Children.Remove(enemy.Visual); _enemies.Remove(enemy); }
        public Enemy? GetRandomShooter() => !_enemies.Any() ? null : _enemies[_random.Next(_enemies.Count)];
    }
}
