// spaceInvaders/Models/Alien.cs
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Windows.UI;

namespace spaceInvaders.Models;

public class Alien : GameObject
{
    public int ScoreValue { get; private set; }

    public Alien(int scoreValue, Color color) : base(40, 25) // Define tamanho padrão dos aliens
    {
        ScoreValue = scoreValue;
        
        var rect = new Rectangle
        {
            Width = this.Width,
            Height = this.Height,
            Fill = new SolidColorBrush(color)
        };
        Sprite = rect;
    }
}
