// spaceInvaders/Models/GameObject.cs
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;

namespace spaceInvaders.Models;

public abstract class GameObject
{
    // Posição do objeto no jogo
    public double X { get; set; }
    public double Y { get; set; }

    // Dimensões do objeto
    public double Width { get; protected set; }
    public double Height { get; protected set; }

    // O elemento visual (Rectangle) que representa este objeto na tela
    public UIElement Sprite { get; protected set; }

    // Construtor que força as classes filhas a definir um tamanho
    protected GameObject(double width, double height)
    {
        Width = width;
        Height = height;
        // O Sprite será criado nas classes filhas
        Sprite = new Rectangle(); 
    }

    // Método para atualizar a posição do Sprite no Canvas
    public void UpdatePositionOnCanvas()
    {
        Canvas.SetLeft(Sprite, X);
        Canvas.SetTop(Sprite, Y);
    }
}
