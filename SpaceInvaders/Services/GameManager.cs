using Microsoft.UI;
using Microsoft.UI.Xaml;
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
        private readonly TextBlock _scoreText;
        private readonly Image[] _lifeImages;
        private readonly Image _playerImage;
        private readonly EnemyManager _enemyManager;
        // O '?' aqui resolve um aviso de compilação (CS8625)
        private Player? _player;
        private readonly List<Rectangle> _playerBullets = new();
        private readonly List<Rectangle> _enemyBullets = new();
        private readonly List<Rectangle> _barrierBlocks = new();
        private readonly DispatcherTimer _gameTimer = new();
        private readonly Random _random = new();

        private int _score;
        private int _nextExtraLifeScore;

        private const int WinScore = 500;
        private const int InitialPlayerLives = 3;
        private const int MaxPlayerLives = 6;
        private const double PlayerSpeed = 8;
        private const double PlayerBulletSpeed = -15;
        private const double EnemyBulletSpeed = 5;

        private bool _isLeftPressed;
        private bool _isRightPressed;

        public GameManager(Canvas gameCanvas, TextBlock scoreText, Image life1, Image life2, Image life3, Image life4, Image life5, Image life6, Image playerImage)
        {
            _gameCanvas = gameCanvas;
            _scoreText = scoreText;
            _playerImage = playerImage;
            _lifeImages = new[] { life1, life2, life3, life4, life5, life6 };

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

        public void StartGame()
        {
            SetupNewGame();
            _gameTimer.Start();
        }

        private void SetupNewGame()
        {
            _gameCanvas.Children.Clear();
            _playerBullets.Clear();
            _enemyBullets.Clear();
            _barrierBlocks.Clear();

            _player = new Player(_playerImage, InitialPlayerLives);
            _gameCanvas.Children.Add(_player.Visual);
            _player.X = (_gameCanvas.Width / 2) - (_player.Width / 2);
            _player.Y = 550;
    
            _score = 0;
            _scoreText.Text = "0";
            _nextExtraLifeScore = 1000;
            UpdateLivesDisplay();

            CreatePixelatedBarrier(80, 450);
            CreatePixelatedBarrier(260, 450);
            CreatePixelatedBarrier(440, 450);
            CreatePixelatedBarrier(620, 450);

            _enemyManager.SpawnWave();
        }
        
        // O '?' nos parâmetros resolve um aviso de compilação (CS8622)
        private void GameLoop(object? sender, object e)
        {
            // Se o jogador for nulo por algum motivo, não faz nada.
            if (_player is null) return;
            
            UpdatePlayerPosition();
            _player.Update();
            _enemyManager.Update(_gameCanvas.Width, _barrierBlocks);
            
            MoveBullets();
            HandleEnemyShooting();
            CheckCollisions();

            if (_enemyManager.IsSwarmDestroyed)
            {
                _enemyManager.SpawnWave();
            }
        }
        
        private void UpdatePlayerPosition()
        {
            // Verifica se _player não é nulo antes de usar
            if (_player is null) return;

            if (_isLeftPressed)
            {
                _player.Move(-PlayerSpeed, _gameCanvas.Width);
            }
            if (_isRightPressed)
            {
                _player.Move(PlayerSpeed, _gameCanvas.Width);
            }
        }

        public void OnKeyDown(VirtualKey key)
        {
            if (!_gameTimer.IsEnabled) return;
            switch (key)
            {
                case VirtualKey.Left or VirtualKey.A: _isLeftPressed = true; break;
                case VirtualKey.Right or VirtualKey.D: _isRightPressed = true; break;
                case VirtualKey.Space: FirePlayerBullet(); break;
            }
        }

        public void OnKeyUp(VirtualKey key)
        {
            if (!_gameTimer.IsEnabled) return;
            switch (key)
            {
                case VirtualKey.Left or VirtualKey.A: _isLeftPressed = false; break;
                case VirtualKey.Right or VirtualKey.D: _isRightPressed = false; break;
            }
        }
        
        private void HandleEnemyShooting()
        {
            if (_random.Next(100) < 2)
            {
                var shooter = _enemyManager.GetRandomShooter();
                if (shooter != null)
                {
                    FireEnemyBullet(shooter);
                }
            }
        }

        private void CheckCollisions()
        {
            if (_player is null) return;

            foreach (var bullet in _playerBullets.ToList())
            {
                foreach (var enemy in _enemyManager.Enemies.ToList())
                {
                    if (CheckCollision(bullet, enemy.Visual))
                    {
                        _score += enemy.Points;
                        _scoreText.Text = _score.ToString();
                        
                        if (_score >= WinScore)
                        {
                            EndGame(true);
                            return;
                        }

                        CheckForExtraLife();
                        
                        _enemyManager.RemoveEnemy(enemy);
                        
                        _gameCanvas.Children.Remove(bullet);
                        _playerBullets.Remove(bullet);
                        goto nextPlayerBullet;
                    }
                }
                nextPlayerBullet:;
            }

            foreach (var bullet in _enemyBullets.ToList())
            {
                if (CheckCollision(bullet, _player.Visual))
                {
                    _player.TakeHit();
                    UpdateLivesDisplay();

                    _gameCanvas.Children.Remove(bullet);
                    _enemyBullets.Remove(bullet);

                    if (!_player.IsAlive)
                    {
                        EndGame(false);
                        return;
                    }
                }
            }

            var allBullets = _playerBullets.Concat(_enemyBullets).ToList();
            foreach (var bullet in allBullets)
            {
                foreach (var block in _barrierBlocks.ToList())
                {
                    if (CheckCollision(bullet, block))
                    {
                        _gameCanvas.Children.Remove(block);
                        _barrierBlocks.Remove(block);
                        _gameCanvas.Children.Remove(bullet);
                        if (_playerBullets.Contains(bullet)) _playerBullets.Remove(bullet);
                        if (_enemyBullets.Contains(bullet)) _enemyBullets.Remove(bullet);
                        goto nextCombinedBullet;
                    }
                }
                nextCombinedBullet:;
            }
        }
        
        private void MoveBullets()
        {
            foreach (var bullet in _playerBullets.ToList())
            {
                Canvas.SetTop(bullet, Canvas.GetTop(bullet) + PlayerBulletSpeed);
                if (Canvas.GetTop(bullet) < 0)
                {
                    _gameCanvas.Children.Remove(bullet);
                    _playerBullets.Remove(bullet);
                }
            }
            foreach (var bullet in _enemyBullets.ToList())
            {
                Canvas.SetTop(bullet, Canvas.GetTop(bullet) + EnemyBulletSpeed);
                if (Canvas.GetTop(bullet) > _gameCanvas.Height)
                {
                    _gameCanvas.Children.Remove(bullet);
                    _enemyBullets.Remove(bullet);
                }
            }
        }
        
        private void FirePlayerBullet()
        {
            if (_player is null || _player.IsStunned || _playerBullets.Count >= 1) return;
            
            var bullet = new Rectangle { Width = 5, Height = 15, Fill = new SolidColorBrush(Colors.LawnGreen) };
            Canvas.SetLeft(bullet, _player.X + _player.Width / 2 - 2.5);
            Canvas.SetTop(bullet, _player.Y - 15);
            _playerBullets.Add(bullet);
            _gameCanvas.Children.Add(bullet);
        }
        
        private void FireEnemyBullet(Enemy enemy)
        {
            var bullet = new Rectangle { Width = 5, Height = 15, Fill = new SolidColorBrush(Colors.White) };
            Canvas.SetLeft(bullet, enemy.X + enemy.Width / 2 - 2.5);
            Canvas.SetTop(bullet, enemy.Y + enemy.Height);
            _enemyBullets.Add(bullet);
            _gameCanvas.Children.Add(bullet);
        }
        
        private void CheckForExtraLife()
        {
            if (_player != null && _score >= _nextExtraLifeScore)
            {
                _player.AddLife(MaxPlayerLives);
                UpdateLivesDisplay();
                _nextExtraLifeScore += 1000;
            }
        }

        private void UpdateLivesDisplay()
        {
            if (_player is null) return;
            
            for (int i = 0; i < _lifeImages.Length; i++)
            {
                _lifeImages[i].Visibility = _player.Lives > i ? Visibility.Visible : Visibility.Collapsed;
            }
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
        
        private void CreatePixelatedBarrier(double startX, double startY)
        {
            int blockSize = 5;
            int barrierWidthInBlocks = 16;
            int barrierHeightInBlocks = 12;
            for (int row = 0; row < barrierHeightInBlocks; row++)
            {
                for (int col = 0; col < barrierWidthInBlocks; col++)
                {
                    if (row > 6 && col > 3 && col < 12) { if (row > 8 || (col > 5 && col < 10)) continue; }
                    Rectangle block = new Rectangle { Width = blockSize, Height = blockSize, Fill = new SolidColorBrush(Colors.LawnGreen) };
                    Canvas.SetLeft(block, startX + col * blockSize);
                    Canvas.SetTop(block, startY + row * blockSize);
                    _gameCanvas.Children.Add(block);
                    _barrierBlocks.Add(block);
                }
            }
        }
        
        private async void EndGame(bool playerWon)
        {
            _gameTimer.Stop();
            var dialog = new ContentDialog {
                Title = playerWon ? "Você venceu!" : "Fim de Jogo",
                Content = $"Sua pontuação final foi: {_score}",
                CloseButtonText = "Jogar Novamente",
                XamlRoot = _gameCanvas.XamlRoot
            };
            await dialog.ShowAsync();
            
            SetupNewGame();
            _gameTimer.Start();
        }
    }
}
