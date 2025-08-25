using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Shapes;
using spaceInvaders.Models;
using spaceInvaders.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.System;
using NAudio.Wave;
using Windows.ApplicationModel;

namespace spaceInvaders.GameLogic;

internal class GameManager
{
    private readonly IGameView _view;
    private readonly GameObjectFactory _gameObjectFactory;
    private readonly CollisionManager _collisionManager;
    private readonly MovementManager _movementManager;
    private readonly Player _player = new();
    private readonly List<Alien> _aliens = new();
    private readonly List<Bullet> _bullets = new();
    private readonly List<Rectangle> _barrierBlocks = new();
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

    public GameManager(IGameView view)
    {
        _view = view;
        _gameTimer.Interval = TimeSpan.FromMilliseconds(16);
        _gameTimer.Tick += GameLoop;
        _gameObjectFactory = new GameObjectFactory();
        _movementManager = new MovementManager();
        _collisionManager = new CollisionManager(
            _view,
            onAlienKilled: (scoreValue) => { _score += scoreValue; _view.UpdateScore(_score); PlaySound("invaderkilled.wav"); },
            onPlayerHit: () => { _playerLives--; _view.UpdateLives(_playerLives); if (_playerLives <= 0) EndGame(); },
            onWaveCleared: () => { StartGame(); }
        );
    }
    
    public void StartGame()
    {
        // Limpeza dos objetos visuais
        foreach(var b in _bullets) _view.RemoveGameObject(b.Sprite);
        foreach(var a in _aliens) _view.RemoveGameObject(a.Sprite);
        foreach(var b in _barrierBlocks) _view.RemoveGameObject(b);
        
        _view.RemoveGameObject(_player.Sprite);

        // Limpa as listas de controle
        _bullets.Clear();
        _aliens.Clear();
        _barrierBlocks.Clear();
        
        // Reinicialização do Estado
        _score = 0; _view.UpdateScore(0);
        _playerLives = 3; _view.UpdateLives(3);
        _isGameOver = false; _view.ShowGameOver(false); _view.ShowMenuButton(false);
        _alienDirection = 1;
        
        // Criação dos Objetos
        _player.X = (800 / 2) - (_player.Width / 2); _player.Y = 520;
        _player.UpdatePositionOnCanvas();
        _view.AddGameObject(_player.Sprite);
        
        var newAliens = _gameObjectFactory.CreateAliens();
        _aliens.AddRange(newAliens);
        foreach(var alien in newAliens) { alien.UpdatePositionOnCanvas(); _view.AddGameObject(alien.Sprite); }

        var newBarriers = _gameObjectFactory.CreateBarriers();
        _barrierBlocks.AddRange(newBarriers);
        foreach (var block in newBarriers) { _view.AddGameObject(block); }
        
        _gameTimer.Start();
    }
    
    private void GameLoop(object? sender, object e)
    {
        if (_isGameOver) return;
        
        bool shouldEndGame = _movementManager.UpdateAllPositions(_player, _aliens, _bullets, _isMovingLeft, _isMovingRight, ref _alienDirection, 800, 600);
        if (shouldEndGame)
        {
            EndGame();
            return;
        }

        CleanupOffscreenBullets();
        EnemyShoot();
        _collisionManager.CheckCollisions(_player, _aliens, _bullets, _barrierBlocks);
    }
    
    private void CleanupOffscreenBullets()
    {
        foreach (var bullet in _bullets.ToList())
        {
            if (bullet.Y < 0 || bullet.Y > 600)
            {
                _bullets.Remove(bullet);
                _view.RemoveGameObject(bullet.Sprite);
            }
        }
    }
    
    private void PlayerShoot()
    {
        if (DateTime.Now - _lastPlayerShotTime < _playerShootCooldown) return;
        _lastPlayerShotTime = DateTime.Now;
        PlaySound("shoot.wav");
        var bullet = _gameObjectFactory.CreatePlayerBullet(_player);
        bullet.UpdatePositionOnCanvas();
        _bullets.Add(bullet);
        _view.AddGameObject(bullet.Sprite);
    }
    
    private void EnemyShoot()
    {
        if (_random.Next(0, 100) < 5 && _aliens.Any())
        {
            var bottomAliens = _aliens.GroupBy(a => a.X).Select(g => g.OrderByDescending(a => a.Y).First());
            var shootingAlien = bottomAliens.ElementAt(_random.Next(bottomAliens.Count()));
            var bullet = _gameObjectFactory.CreateEnemyBullet(shootingAlien);
            bullet.UpdatePositionOnCanvas();
            _bullets.Add(bullet);
            _view.AddGameObject(bullet.Sprite);
        }
    }
    
    private void EndGame()
    {
        _isGameOver = true;
        _gameTimer.Stop();
        _view.ShowGameOver(true);
        _view.ShowMenuButton(true);
    }
    
    private async void PlaySound(string soundFileName)
    {
        try
        {
            var installFolder = Package.Current.InstalledLocation;
            var soundsFolder = await installFolder.GetFolderAsync(@"Assets\Sounds");
            var soundFile = await soundsFolder.GetFileAsync(soundFileName);
            var soundFilePath = soundFile.Path;
            var audioReader = new AudioFileReader(soundFilePath);
            var waveOut = new WaveOutEvent();
            waveOut.PlaybackStopped += (s, a) => { waveOut.Dispose(); audioReader.Dispose(); };
            waveOut.Init(audioReader);
            waveOut.Play();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Erro ao tocar o som {soundFileName}: {ex.Message}");
        }
    }
    
    public void HandleKeyDown(VirtualKey key)
    {
        if (_isGameOver)
        {
            if (key == VirtualKey.Enter) StartGame();
            return;
        }
        switch (key)
        {
            case VirtualKey.Left: case VirtualKey.A: _isMovingLeft = true; break;
            case VirtualKey.Right: case VirtualKey.D: _isMovingRight = true; break;
            case VirtualKey.Space: PlayerShoot(); break;
        }
    }

    public void HandleKeyUp(VirtualKey key)
    {
        switch (key)
        {
            case VirtualKey.Left: case VirtualKey.A: _isMovingLeft = false; break;
            case VirtualKey.Right: case VirtualKey.D: _isMovingRight = false; break;
        }
    }
}
