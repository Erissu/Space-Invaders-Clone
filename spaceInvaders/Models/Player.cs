using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;

namespace spaceInvaders.Models;

public class Player : GameObject
{
    public Player() : base(50, 25) 
    {
        var rect = new Rectangle
        {
            Width = this.Width,
            Height = this.Height,
            Fill = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 255, 0)) // Verde
        };
        Sprite = rect; 
    }
}
