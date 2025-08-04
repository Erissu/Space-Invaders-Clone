using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Windows.UI;

namespace spaceInvaders.Models;

public class Bullet : GameObject
{
    public enum BulletType { Player, Enemy }
    public BulletType Type { get; }

    public Bullet(BulletType type) : base(5, 15) 
    {
        Type = type;
        var color = (type == BulletType.Player) 
            ? Color.FromArgb(255, 255, 255, 0)  
            : Color.FromArgb(255, 255, 255, 255); 

        var rect = new Rectangle
        {
            Width = this.Width,
            Height = this.Height,
            Fill = new SolidColorBrush(color)
        };
        Sprite = rect;
    }
}
