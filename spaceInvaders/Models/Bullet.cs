// spaceInvaders/Models/Bullet.cs
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Windows.UI;

namespace spaceInvaders.Models;

public class Bullet : GameObject
{
    public enum BulletType { Player, Enemy }
    public BulletType Type { get; }

    public Bullet(BulletType type) : base(5, 15) // Define tamanho padrão das balas
    {
        Type = type;
        var color = (type == BulletType.Player) 
            ? Color.FromArgb(255, 255, 255, 0)  // Amarelo para jogador
            : Color.FromArgb(255, 255, 255, 255); // Branco para inimigo

        var rect = new Rectangle
        {
            Width = this.Width,
            Height = this.Height,
            Fill = new SolidColorBrush(color)
        };
        Sprite = rect;
    }
}
