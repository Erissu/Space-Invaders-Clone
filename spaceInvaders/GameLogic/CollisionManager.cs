using spaceInvaders.Models;
using spaceInvaders.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.UI.Xaml.Controls;

namespace spaceInvaders.GameLogic;

internal class CollisionManager
{
    private readonly IGameView _view;               // Usa a interface
    private readonly Action<int> _onAlienKilled;
    private readonly Action _onPlayerHit;
    private readonly Action _onWaveCleared;

    public CollisionManager(IGameView view, Action<int> onAlienKilled, Action onPlayerHit, Action onWaveCleared)
    {
        _view = view;
        _onAlienKilled = onAlienKilled;
        _onPlayerHit = onPlayerHit;
        _onWaveCleared = onWaveCleared;
    }

    public void CheckCollisions(Player player, List<Alien> aliens, List<Bullet> bullets, List<Rectangle> barrierBlocks)
    {
        var playerRect = new Rect(player.X, player.Y, player.Width, player.Height);
        foreach (var bullet in bullets.ToList())
        {
            var bulletRect = new Rect(bullet.X, bullet.Y, bullet.Width, bullet.Height);
            bool bulletDestroyed = false;
            foreach (var block in barrierBlocks.ToList())
            {
                var blockRect = new Rect(Canvas.GetLeft(block), Canvas.GetTop(block), block.Width, block.Height);
                var intersection = bulletRect;
                intersection.Intersect(blockRect);
                if (!intersection.IsEmpty)
                {
                    _view.RemoveGameObject(bullet.Sprite); bullets.Remove(bullet);
                    _view.RemoveGameObject(block); barrierBlocks.Remove(block);
                    bulletDestroyed = true;
                    break;
                }
            }
            if (bulletDestroyed) continue;
            if (bullet.Type == Bullet.BulletType.Player)
            {
                foreach (var alien in aliens.ToList())
                {
                    var alienRect = new Rect(alien.X, alien.Y, alien.Width, alien.Height);
                    var intersectionRect = bulletRect;
                    intersectionRect.Intersect(alienRect);
                    if (!intersectionRect.IsEmpty)
                    {
                        _view.RemoveGameObject(bullet.Sprite); bullets.Remove(bullet);
                        _view.RemoveGameObject(alien.Sprite); aliens.Remove(alien);
                        _onAlienKilled(alien.ScoreValue);
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
                    _view.RemoveGameObject(bullet.Sprite); bullets.Remove(bullet);
                    _onPlayerHit();
                    bulletDestroyed = true;
                }
            }
        }
        if (!aliens.Any()) { _onWaveCleared(); }
    }
}
