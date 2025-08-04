using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace SpaceInvaders.Models
{
    /// <summary>
    /// A classe base para todos os objetos visuais do jogo (Jogador, Inimigos).
    /// </summary>
    public abstract class GameObject
    {
        public FrameworkElement Visual { get; }
        public double Width { get; }
        public double Height { get; }

        public double X
        {
            get => Canvas.GetLeft(Visual);
            set => Canvas.SetLeft(Visual, value);
        }

        public double Y
        {
            get => Canvas.GetTop(Visual);
            set => Canvas.SetTop(Visual, value);
        }

        // CORRIGIDO: O construtor agora aceita o elemento visual e suas dimensões.
        protected GameObject(FrameworkElement visual, double width, double height)
        {
            Visual = visual;
            Width = width;
            Height = height;
            Visual.Width = width;
            Visual.Height = height;
        }
    }
}
