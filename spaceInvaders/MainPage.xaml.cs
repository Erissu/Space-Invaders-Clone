using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Shapes;
using spaceInvaders.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.System;
using Windows.UI;

namespace spaceInvaders;

public sealed partial class MainPage : Page
{
    // --- Configurações ---
    private const double PlayerSpeed = 8.0;
    private const double BulletSpeed = 15.0;
    private const double AlienSpeed = 2.0;
    private const double EnemyBulletSpeed = 7.0;
    private const int EnemyShootChance = 5;

    // --- Objetos do Jogo (Estilo POO) ---
    private readonly Player _player = new(); // Objeto jogador criado aqui
    private readonly List<Alien> _aliens = new();
    private readonly List<Bullet> _bullets = new();
    private readonly List<Rectangle> _barrierBlocks = new();

    // --- Controle do Jogo ---
    private readonly DispatcherTimer _gameTimer = new();
    private int _score;
    private int _playerLives;
    private bool _isGameOver;
    private int _alienDirection = 1;
    private readonly TimeSpan _playerShootCooldown = TimeSpan.FromMilliseconds(400);
    private DateTime _lastPlayerShotTime;
    private readonly Random _random = new();
    
    private bool _isMovingLeft;
    private bool _isMovingRight;

    public MainPage()
    {
        this.InitializeComponent();
        this.Loaded += OnPageLoaded;
    }

    private void OnPageLoaded(object sender, RoutedEventArgs e)
    {
        GameCanvas.Focus(FocusState.Programmatic);
        _gameTimer.Interval = TimeSpan.FromMilliseconds(16);
        _gameTimer.Tick += GameLoop;
        StartGame();
    }

    private void StartGame()
    {
        // Limpeza
        GameCanvas.Children.Clear(); // Limpa tudo do canvas de uma vez
        _bullets.Clear();
        _aliens.Clear();
        _barrierBlocks.Clear();

        // Reinicialização
        _score = 0; ScoreText.Text = "SCORE: 0";
        _playerLives = 3; LivesText.Text = $"LIVES: {_playerLives}";
        _isGameOver = false; GameOverText.Visibility = Visibility.Collapsed;
        _alienDirection = 1;

        // ▼▼▼ MUDANÇA: Lógica de criação do jogador agora está aqui ▼▼▼
        _player.X = (800 / 2) - (_player.Width / 2); // Centraliza
        _player.Y = 520;
        _player.UpdatePositionOnCanvas();
        GameCanvas.Children.Add(_player.Sprite);

        // Criação dos Aliens
        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 10; col++)
            {
                Color alienColor;
                int scoreValue;
                if (row == 0) { alienColor = Color.FromArgb(255, 255, 100, 100); scoreValue = 30; }
                else if (row < 3) { alienColor = Color.FromArgb(255, 255, 180, 0); scoreValue = 20; }
                else { alienColor = Color.FromArgb(255, 255, 0, 255); scoreValue = 10; }

                var alien = new Alien(scoreValue, alienColor) { X = 60 + col * 55, Y = 50 + row * 40 };
                alien.UpdatePositionOnCanvas();
                _aliens.Add(alien);
                GameCanvas.Children.Add(alien.Sprite);
            }
        }
        
        CreateBarriers();
        // Re-adiciona os textos de UI ao canvas (já que limpamos tudo)
        GameCanvas.Children.Add(GameOverText);

        _gameTimer.Start();
    }
    
    private void GameLoop(object? sender, object e)
    {
        if (_isGameOver) return;
        MovePlayer();
        MoveBullets();
        MoveAliens();
        EnemyShoot();
        CheckCollisions();
    }

    private void MovePlayer()
    {
        if (_isMovingLeft && _player.X > 0)
        {
            _player.X -= PlayerSpeed;
        }
        // MUDANÇA: Usa GameCanvas.ActualWidth para o limite
        if (_isMovingRight && _player.X + _player.Width < GameCanvas.ActualWidth)
        {
            _player.X += PlayerSpeed;
        }
        _player.UpdatePositionOnCanvas(); // Atualiza o visual do jogador
    }
    
    // ... O resto do código permanece o mesmo ...
    private void MoveBullets()
    {
        foreach (var bullet in _bullets.ToList())
        {
            if (bullet.Type == Bullet.BulletType.Player)
            {
                bullet.Y -= BulletSpeed;
                if (bullet.Y < 0) { _bullets.Remove(bullet); GameCanvas.Children.Remove(bullet.Sprite); }
            }
            else
            {
                bullet.Y += EnemyBulletSpeed;
                if (bullet.Y > GameCanvas.ActualHeight) { _bullets.Remove(bullet); GameCanvas.Children.Remove(bullet.Sprite); }
            }
            bullet.UpdatePositionOnCanvas();
        }
    }

    private void MoveAliens()
    {
        bool edgeReached = false;
        foreach (var alien in _aliens)
        {
            alien.X += (AlienSpeed * _alienDirection);
            if ((_alienDirection > 0 && alien.X + alien.Width > GameCanvas.ActualWidth) || (_alienDirection < 0 && alien.X < 0))
                edgeReached = true;
            if (alien.Y + alien.Height > _player.Y)
                EndGame();
        }
        if (edgeReached)
        {
            _alienDirection *= -1;
            foreach (var alien in _aliens)
                alien.Y += 20;
        }
        foreach (var alien in _aliens) alien.UpdatePositionOnCanvas();
    }

    private void PlayerShoot()
    {
        if (DateTime.Now - _lastPlayerShotTime < _playerShootCooldown) return;
        _lastPlayerShotTime = DateTime.Now;

        var bullet = new Bullet(Bullet.BulletType.Player)
        {
            X = _player.X + _player.Width / 2 - 2.5,
            Y = _player.Y - 15
        };
        bullet.UpdatePositionOnCanvas();
        _bullets.Add(bullet);
        GameCanvas.Children.Add(bullet.Sprite);
    }

    private void EnemyShoot()
    {
        if (_random.Next(0, 100) < EnemyShootChance && _aliens.Any())
        {
            var bottomAliens = _aliens.GroupBy(a => a.X).Select(g => g.OrderByDescending(a => a.Y).First());
            var shootingAlien = bottomAliens.ElementAt(_random.Next(bottomAliens.Count()));
            var bullet = new Bullet(Bullet.BulletType.Enemy)
            {
                X = shootingAlien.X + shootingAlien.Width / 2 - 2.5,
                Y = shootingAlien.Y + shootingAlien.Height
            };
            bullet.UpdatePositionOnCanvas();
            _bullets.Add(bullet);
            GameCanvas.Children.Add(bullet.Sprite);
        }
    }

    private void CheckCollisions()
    {
        var playerRect = new Rect(_player.X, _player.Y, _player.Width, _player.Height);
        
        foreach (var bullet in _bullets.ToList())
        {
            var bulletRect = new Rect(bullet.X, bullet.Y, bullet.Width, bullet.Height);
            bool bulletDestroyed = false;

            foreach (var block in _barrierBlocks.ToList())
            {
                var blockRect = new Rect(Canvas.GetLeft(block), Canvas.GetTop(block), block.Width, block.Height);
                var intersection = bulletRect;
                intersection.Intersect(blockRect);
                if (!intersection.IsEmpty)
                {
                    GameCanvas.Children.Remove(bullet.Sprite); _bullets.Remove(bullet);
                    GameCanvas.Children.Remove(block); _barrierBlocks.Remove(block);
                    bulletDestroyed = true;
                    break;
                }
            }
            if (bulletDestroyed) continue;

            if (bullet.Type == Bullet.BulletType.Player)
            {
                foreach (var alien in _aliens.ToList())
                {
                    var alienRect = new Rect(alien.X, alien.Y, alien.Width, alien.Height);
                    var intersectionRect = bulletRect;
                    intersectionRect.Intersect(alienRect);
                    if (!intersectionRect.IsEmpty)
                    {
                        GameCanvas.Children.Remove(bullet.Sprite); _bullets.Remove(bullet);
                        GameCanvas.Children.Remove(alien.Sprite); _aliens.Remove(alien);
                        _score += alien.ScoreValue;
                        ScoreText.Text = $"SCORE: {_score}";
                        bulletDestroyed = true;
                        break;
                    }
                }
            }
            else if (bullet.Type == Bullet.BulletType.Enemy)
            {
                var intersectionRectPlayer = playerRect;
                intersectionRectPlayer.Intersect(bulletRect);
                if (!intersectionRectPlayer.IsEmpty)
                {
                    GameCanvas.Children.Remove(bullet.Sprite); _bullets.Remove(bullet);
                    _playerLives--;
                    LivesText.Text = $"LIVES: {_playerLives}";
                    if (_playerLives <= 0) EndGame();
                    bulletDestroyed = true;
                }
            }
        }
        if (!_aliens.Any()) StartGame();
    }
    
    private void CreateBarriers()
    {
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
                    _barrierBlocks.Add(block);
                    GameCanvas.Children.Add(block);
                }
            }
        }
    }

    private void EndGame()
    {
        _isGameOver = true;
        _gameTimer.Stop();
        GameOverText.Visibility = Visibility.Visible;
    }

    private void GameCanvas_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (_isGameOver)
        {
            if (e.Key == VirtualKey.Enter) StartGame();
            return;
        }
        switch (e.Key)
        {
            case VirtualKey.Left: case VirtualKey.A: _isMovingLeft = true; break;
            case VirtualKey.Right: case VirtualKey.D: _isMovingRight = true; break;
            case VirtualKey.Space: PlayerShoot(); break;
        }
    }

    private void GameCanvas_KeyUp(object sender, KeyRoutedEventArgs e)
    {
        switch (e.Key)
        {
            case VirtualKey.Left: case VirtualKey.A: _isMovingLeft = false; break;
            case VirtualKey.Right: case VirtualKey.D: _isMovingRight = false; break;
        }
    }
}
