using spaceInvaders.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace spaceInvaders.GameLogic;

// A única responsabilidade desta classe é gerenciar a criação de novos objetos durante o jogo.
internal class SpawnManager
{
    private readonly GameObjectFactory _factory;

    // As variáveis de controle de tiro agora vivem aqui.
    private readonly TimeSpan _playerShootCooldown = TimeSpan.FromMilliseconds(400);
    private DateTime _lastPlayerShotTime;
    private readonly Random _random = new();
    private const int EnemyShootChance = 5;
    
    // Flag para registrar o pedido de tiro do jogador
    private bool _playerShootRequested = false;

    public SpawnManager(GameObjectFactory factory)
    {
        _factory = factory;
    }

    // Método chamado pelo GameManager para registrar que o jogador quer atirar.
    public void RequestPlayerShoot()
    {
        _playerShootRequested = true;
    }

    // O método principal, chamado a cada ciclo do Game Loop.
    // Ele retorna uma lista de todas as novas balas que foram criadas neste ciclo.
    public List<Bullet> Update(Player player, List<Alien> aliens)
    {
        var newBullets = new List<Bullet>();

        // Processa o tiro do jogador
        if (_playerShootRequested)
        {
            if (DateTime.Now - _lastPlayerShotTime > _playerShootCooldown)
            {
                _lastPlayerShotTime = DateTime.Now;
                var playerBullet = _factory.CreatePlayerBullet(player);
                newBullets.Add(playerBullet);
                
                // Toca o som aqui ou retorna um evento de som
            }
            _playerShootRequested = false; // Reseta o pedido
        }

        // Processa o tiro dos inimigos
        if (_random.Next(0, 100) < EnemyShootChance && aliens.Any())
        {
            var bottomAliens = aliens.GroupBy(a => a.X).Select(g => g.OrderByDescending(a => a.Y).First());
            var shootingAlien = bottomAliens.ElementAt(_random.Next(bottomAliens.Count()));
            var enemyBullet = _factory.CreateEnemyBullet(shootingAlien);
            newBullets.Add(enemyBullet);
        }

        return newBullets;
    }
}
