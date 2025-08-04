using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;

namespace spaceInvaders.Models;

public abstract class GameObject
{
    public double X { get; set; }
    public double Y { get; set; }
    
    public double Width { get; protected set; }
    public double Height { get; protected set; }
    public UIElement Sprite { get; protected set; }
    
    protected GameObject(double width, double height)
    {
        Width = width;
        Height = height;
        Sprite = new Rectangle(); 
    }
    
    public void UpdatePositionOnCanvas()
    {
        Canvas.SetLeft(Sprite, X);
        Canvas.SetTop(Sprite, Y);
    }
}
