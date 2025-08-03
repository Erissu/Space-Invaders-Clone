using Microsoft.UI.Xaml.Controls;

namespace SpaceInvaders.Models
{
    public class Player : GameObject
    {
        public int Lives { get; private set; }
        public bool IsStunned => _stunCounter > 0;
        
        private int _stunCounter;
        private const int StunDurationInTicks = 75;

        public Player(Image visual, int initialLives) : base(visual)
        {
            Lives = initialLives;
        }

        public void Move(double dx, double gameWidth)
        {
            if (IsStunned) return;

            double newX = X + dx;
            if (newX >= 0 && newX <= gameWidth - this.Width)
            {
                X = newX;
            }
        }

        public void TakeHit()
        {
            if (IsStunned) return;
            
            Lives--;
            if (Lives > 0)
            {
                _stunCounter = StunDurationInTicks;
            }
            else
            {
                // CORREÇÃO: Agora ele chama o método Kill() da classe base.
                Kill();
            }
        }

        public void AddLife(int maxLives)
        {
            if (Lives < maxLives)
            {
                Lives++;
            }
        }

        public override void Update()
        {
            if (IsStunned)
            {
                _stunCounter--;
                Visual.Opacity = (_stunCounter % 10 < 5) ? 1.0 : 0.2;
            }
            else
            {
                Visual.Opacity = 1.0;
            }
            base.Update();
        }
    }
}
