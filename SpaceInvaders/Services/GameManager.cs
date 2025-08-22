using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using SpaceInvaders.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.System;
using Windows.UI;

namespace SpaceInvaders.Services
{
    public class GameManager
    {
        private readonly Canvas _gameCanvas;
        private readonly Image _playerImage;
        public Action<int>? OnScoreUpdated;
        public Action<int>? OnLivesUpdated;
        public Action<bool, int>? OnGameOver;
        private readonly SoundManager _soundManager;
        private readonly EnemyManager _enemyManager;
        private Player? _player;
        private readonly List<Rectangle> _playerBullets = new();
        private readonly List<Rectangle> _enemyBullets = new();
        private readonly List<BarrierBlock> _barrierBlocks = new();
        private readonly DispatcherTimer _gameTimer = new();
        private readonly Random _random = new();
        private int _score;
        private int _nextExtraLifeScore;
        private const int MaxPlayerLives = 6;
        private const double PlayerSpeed = 8;
        private const double PlayerBulletSpeed = -15;
        private const double EnemyBulletSpeed = 5;
        private bool _isLeftPressed;
        private bool _isRightPressed;
        private readonly SolidColorBrush _playerBulletBrush = new(Color.FromArgb(255, 124, 252, 0));
        private readonly SolidColorBrush _enemyBulletBrush = new(Color.FromArgb(255, 255, 255, 255));
        private Ufo? _ufo;
        private int _ufoSpawnTimer;
        
        public bool IsPaused => !_gameTimer.IsEnabled;

        public GameManager(Canvas gameCanvas, Image playerImage)
        {
            _gameCanvas = gameCanvas;
            _playerImage = playerImage;
            _soundManager = new SoundManager();
            _gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            _gameTimer.Tick += GameLoop;
            var enemyTypes = new Dictionary<string, EnemyType> {
                {"alien1", new EnemyType { ImageSource = "ms-appx:///Assets/Images/alien1.png", Points = 10 }},
                {"alien2", new EnemyType { ImageSource = "ms-appx:///Assets/Images/alien2.png", Points = 20 }},
                {"alien3", new EnemyType { ImageSource = "ms-appx:///Assets/Images/alien3.png", Points = 40 }},
                {"alien4", new EnemyType { ImageSource = "ms-appx:///Assets/Images/alien4.png", Points = 200 }}
            };
            _enemyManager = new EnemyManager(_gameCanvas, enemyTypes);
        }

        public void StartGame() { SetupNewGame(); _gameTimer.Start(); }
        public void StopGame() { _gameTimer.Stop(); _soundManager.StopMarch(); }
        public void PauseGame() { _gameTimer.Stop(); _soundManager.StopMarch(); }
        public void ResumeGame() { _gameTimer.Start(); _soundManager.StartMarch(); }
        
        private void SetupNewGame() { _gameCanvas.Children.Clear(); _gameCanvas.Children.Add(_playerImage); _playerBullets.Clear(); _enemyBullets.Clear(); _barrierBlocks.Clear(); _player = new Player(_playerImage, 3); _player.X = (_gameCanvas.Width / 2) - (_player.Width / 2); _player.Y = 550; _score = 0; OnScoreUpdated?.Invoke(_score); OnLivesUpdated?.Invoke(_player.Lives); _nextExtraLifeScore = 1000; CreatePixelatedBarrier(100, 450); CreatePixelatedBarrier(355, 450); CreatePixelatedBarrier(610, 450); _enemyManager.SpawnWave(); ResetUfoTimer(); _soundManager.StartMarch(); }
        private void GameLoop(object? sender, object e) { if (_player is null) return; UpdatePlayerPosition(); _player.Update(); _enemyManager.Update(_gameCanvas.Width, _barrierBlocks.Select(b => (Rectangle)b.Visual).ToList()); UpdateUfo(); MoveBullets(); HandleEnemyShooting(); CheckCollisions(); if (_enemyManager.IsSwarmDestroyed) _enemyManager.SpawnWave(); if (_enemyManager.AliensHaveLanded) EndGame(false); }
        private void CheckCollisions() { if (_player is null) return; foreach (var bullet in _playerBullets.ToList()) { foreach (var enemy in _enemyManager.Enemies.ToList()) { if (CheckCollision(bullet, enemy.Visual)) { _score += enemy.Points; OnScoreUpdated?.Invoke(_score); _soundManager.Play("invaderkilled"); CheckForExtraLife(); _enemyManager.RemoveEnemy(enemy); _gameCanvas.Children.Remove(bullet); _playerBullets.Remove(bullet); goto nextPlayerBullet; } } if (_ufo != null) { if (CheckCollision(bullet, _ufo.Visual)) { _score += _ufo.Points; OnScoreUpdated?.Invoke(_score); _soundManager.Play("invaderkilled"); RemoveUfo(); _gameCanvas.Children.Remove(bullet); _playerBullets.Remove(bullet); goto nextPlayerBullet; } } nextPlayerBullet:; } foreach (var bullet in _enemyBullets.ToList()) { if (CheckCollision(bullet, _player.Visual)) { _player.TakeHit(); OnLivesUpdated?.Invoke(_player.Lives); _soundManager.Play("explosion"); _gameCanvas.Children.Remove(bullet); _enemyBullets.Remove(bullet); if (!_player.IsAlive) { EndGame(false); return; } } } var allBullets = _playerBullets.Concat(_enemyBullets).ToList(); foreach (var bullet in allBullets) { foreach (var block in _barrierBlocks.ToList()) { if (CheckCollision(bullet, block.Visual)) { bool isDestroyed = block.TakeHit(); if (isDestroyed) { _gameCanvas.Children.Remove(block.Visual); _barrierBlocks.Remove(block); } _gameCanvas.Children.Remove(bullet); if (_playerBullets.Contains(bullet)) _playerBullets.Remove(bullet); if (_enemyBullets.Contains(bullet)) _enemyBullets.Remove(bullet); _soundManager.Play("barrier"); goto nextCombinedBullet; } } nextCombinedBullet:; } }
        private void CreatePixelatedBarrier(double x, double y) { int blockSize = 15; int barrierWidthInBlocks = 6; int barrierHeightInBlocks = 4; for (int row = 0; row < barrierHeightInBlocks; row++) { for (int col = 0; col < barrierWidthInBlocks; col++) { if (row > 2 && col > 0 && col < 5) { if (row > 3 || (col > 1 && col < 4)) continue; } var block = new BarrierBlock(x + col * blockSize, y + row * blockSize, blockSize); _gameCanvas.Children.Add(block.Visual); _barrierBlocks.Add(block); } } }
        private void FireEnemyBullet(Enemy enemy) { var bullet = new Rectangle { Width = 5, Height = 15, Fill = _enemyBulletBrush }; Canvas.SetLeft(bullet, enemy.X + enemy.Width / 2 - 2.5); Canvas.SetTop(bullet, enemy.Y + enemy.Height); _enemyBullets.Add(bullet); _gameCanvas.Children.Add(bullet); }
        private void EndGame(bool playerWon) { _gameTimer.Stop(); _soundManager.StopMarch(); OnGameOver?.Invoke(playerWon, _score); }
        private void FirePlayerBullet() { if (_player is null || _player.IsStunned || _playerBullets.Count >= 1) return; var bullet = new Rectangle { Width = 5, Height = 15, Fill = _playerBulletBrush }; Canvas.SetLeft(bullet, _player.X + _player.Width / 2 - 2.5); Canvas.SetTop(bullet, _player.Y - 15); _playerBullets.Add(bullet); _gameCanvas.Children.Add(bullet); _soundManager.Play("shoot"); }
        private void UpdateUfo() { if (_ufo != null) { _ufo.Update(); if (_ufo.X > _gameCanvas.Width) RemoveUfo(); } else { _ufoSpawnTimer--; if (_ufoSpawnTimer <= 0) SpawnUfo(); } }
        private void SpawnUfo() { _soundManager.Play("shoot"); double startX = -45; double speed = 3; _ufo = new Ufo(startX, 30, speed); _gameCanvas.Children.Add(_ufo.Visual); }
        private void RemoveUfo() { if (_ufo != null) { _gameCanvas.Children.Remove(_ufo.Visual); _ufo = null; ResetUfoTimer(); } }
        private void ResetUfoTimer() => _ufoSpawnTimer = _random.Next(500, 1000);
        private void UpdatePlayerPosition() { if (_player is null) return; if (_isLeftPressed) _player.Move(-PlayerSpeed, _gameCanvas.Width); if (_isRightPressed) _player.Move(PlayerSpeed, _gameCanvas.Width); }
        public void HandleKeyDown(VirtualKey key) { if (!_gameTimer.IsEnabled) return; switch (key) { case VirtualKey.Left or VirtualKey.A: _isLeftPressed = true; break; case VirtualKey.Right or VirtualKey.D: _isRightPressed = true; break; case VirtualKey.Space: FirePlayerBullet(); break; } }
        public void HandleKeyUp(VirtualKey key) { if (!_gameTimer.IsEnabled) return; switch (key) { case VirtualKey.Left or VirtualKey.A: _isLeftPressed = false; break; case VirtualKey.Right or VirtualKey.D: _isRightPressed = false; break; } }
        private void HandleEnemyShooting() { if (_random.Next(100) < 2) { var shooter = _enemyManager.GetRandomShooter(); if (shooter != null) FireEnemyBullet(shooter); } }
        private void MoveBullets() { foreach (var bullet in _playerBullets.ToList()) { Canvas.SetTop(bullet, Canvas.GetTop(bullet) + PlayerBulletSpeed); if (Canvas.GetTop(bullet) < 0) { _gameCanvas.Children.Remove(bullet); _playerBullets.Remove(bullet); } } foreach (var bullet in _enemyBullets.ToList()) { Canvas.SetTop(bullet, Canvas.GetTop(bullet) + EnemyBulletSpeed); if (Canvas.GetTop(bullet) > _gameCanvas.Height) { _gameCanvas.Children.Remove(bullet); _enemyBullets.Remove(bullet); } } }
        private void CheckForExtraLife() { if (_player != null && _score >= _nextExtraLifeScore) { _player.AddLife(MaxPlayerLives); OnLivesUpdated?.Invoke(_player.Lives); _nextExtraLifeScore += 1000; } }
        private bool CheckCollision(FrameworkElement a, FrameworkElement b) { if (a is null || b is null) return false; double ax = Canvas.GetLeft(a); double ay = Canvas.GetTop(a); double bx = Canvas.GetLeft(b); double by = Canvas.GetTop(b); return ax < bx + b.Width && ax + a.Width > bx && ay < by + b.Height && ay + a.Height > by; }
    }
}
