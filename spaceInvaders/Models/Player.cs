// spaceInvaders/Models/Player.cs
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;

namespace spaceInvaders.Models;

public class Player : GameObject
{
    public Player() : base(50, 25) // Define a largura e altura do jogador
    {
        // Cria o visual do jogador
        var rect = new Rectangle
        {
            Width = this.Width,
            Height = this.Height,
            Fill = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 255, 0)) // Verde
        };
        Sprite = rect; // Atribui o retângulo criado à propriedade Sprite
    }
}
