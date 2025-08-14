using spaceInvaders.Models;
using System.Collections.Generic;
using System.Linq;

namespace spaceInvaders.GameLogic;

internal class MovementManager
{
    private const double PlayerSpeed = 8.0;
    private const double BulletSpeed = 15.0;
    private const double AlienSpeed = 2.0;
    private const double EnemyBulletSpeed = 7.0;
    
    public bool UpdateAllPositions(
        Player player, 
        List<Alien> aliens, 
        List<Bullet> bullets, 
        bool isMovingLeft, 
        bool isMovingRight, 
        ref int alienDirection,
        double canvasWidth, 
        double canvasHeight)
    {
        MovePlayer(player, isMovingLeft, isMovingRight, canvasWidth);
        MoveBullets(bullets);
        
        bool aliensReachedBottom = MoveAliens(aliens, ref alienDirection, canvasWidth, player.Y);

        // Atualiza o visual de todos na tela.
        player.UpdatePositionOnCanvas();
        foreach (var alien in aliens) alien.UpdatePositionOnCanvas();
        foreach (var bullet in bullets) bullet.UpdatePositionOnCanvas();

        return aliensReachedBottom;
    }

    private void MovePlayer(Player player, bool isMovingLeft, bool isMovingRight, double canvasWidth)
    {
        if (isMovingLeft && player.X > 0) player.X -= PlayerSpeed;
        if (isMovingRight && player.X + player.Width < canvasWidth) player.X += PlayerSpeed;
    }
    private bool MoveAliens(List<Alien> aliens, ref int alienDirection, double canvasWidth, double playerY)
    {
        bool edgeReached = false;
        foreach (var alien in aliens)
        {
            alien.X += (AlienSpeed * alienDirection);
            if ((alienDirection > 0 && alien.X + alien.Width > canvasWidth) || (alienDirection < 0 && alien.X < 0))
                edgeReached = true;
            
            if (alien.Y + alien.Height > playerY)
            {
                return true; // Informa que o jogo deve acabar
            }
        }

        if (edgeReached)
        {
            alienDirection *= -1;
            foreach (var alien in aliens)
                alien.Y += 20;
        }

        return false; // Se chegou até aqui, o jogo continua.
    }

    private void MoveBullets(List<Bullet> bullets)
    {
        foreach (var bullet in bullets.ToList())
        {
            if (bullet.Type == Bullet.BulletType.Player) bullet.Y -= BulletSpeed;
            else bullet.Y += EnemyBulletSpeed;
        }
    }
}
