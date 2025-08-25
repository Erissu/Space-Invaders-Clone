using Microsoft.UI;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Windows.UI;

namespace SpaceInvaders.Models
{
    /// <summary>
    /// Representa um único bloco (pixel) de uma barreira protetora.
    /// </summary>
    public class BarrierBlock : GameObject
    {
        public int Health { get; private set; }

        // Define as cores para os diferentes estados de dano
        private static readonly SolidColorBrush FullHealthBrush = new(Color.FromArgb(255, 124, 252, 0));  // #7CFC00
        private static readonly SolidColorBrush DamagedBrush = new(Color.FromArgb(255, 96, 192, 0));     // Verde mais escuro
        private static readonly SolidColorBrush CriticalBrush = new(Color.FromArgb(255, 64, 128, 0));    // Verde muito escuro

        public BarrierBlock(double x, double y, int size) 
            : base(new Rectangle { Width = size, Height = size }, size, size)
        {
            X = x;
            Y = y;
            // CORRIGIDO: Vida de volta para 3
            Health = 3; 
            UpdateColor();
        }

        /// <summary>
        /// Reduz a vida do bloco e atualiza a sua cor.
        /// </summary>
        /// <returns>Verdadeiro se o bloco foi destruído (vida chegou a 0).</returns>
        public bool TakeHit()
        {
            Health--;
            UpdateColor();
            return Health <= 0;
        }

        /// <summary>
        /// Altera a cor do bloco com base nos seus pontos de vida.
        /// </summary>
        private void UpdateColor()
        {
            if (Visual is Rectangle rect)
            {
                // EFEITO RESTAURADO: A cor muda com o dano
                rect.Fill = Health switch
                {
                    3 => FullHealthBrush,
                    2 => DamagedBrush,
                    1 => CriticalBrush,
                    _ => new SolidColorBrush(Colors.Transparent)
                };
            }
        }
    }
}
