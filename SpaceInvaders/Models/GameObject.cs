using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace SpaceInvaders.Models
{
    public abstract class GameObject
    {
        public FrameworkElement Visual { get; protected set; }

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

        public double Width => Visual.Width;
        public double Height => Visual.Height;

        // A propriedade agora tem um "private set" para proteger a lógica.
        public bool IsAlive { get; private set; } = true;

        protected GameObject(FrameworkElement visual)
        {
            Visual = visual;
        }

        public virtual void Update() { }

        // Método público para que outras classes possam "matar" este objeto.
        public void Kill()
        {
            IsAlive = false;
        }
    }
}
