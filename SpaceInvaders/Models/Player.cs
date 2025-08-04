using Microsoft.UI.Xaml.Controls;

namespace SpaceInvaders.Models
{
    /// <summary>
    /// Representa o jogador (a nave).
    /// </summary>
    public class Player : GameObject
    {
        public int Lives { get; private set; }
        public bool IsAlive => Lives > 0;
        public bool IsStunned { get; private set; }
        
        private int _stunTimer;
        private const int StunDuration = 50; // Duração do atordoamento (em "ticks" do jogo)

        /// <summary>
        /// Construtor do jogador.
        /// </summary>
        public Player(Image playerImage, int initialLives) 
            : base(playerImage, 50, 40) // Passa a imagem e o tamanho para a classe base
        {
            Lives = initialLives;
        }

        /// <summary>
        /// Move o jogador horizontalmente, com limites na tela.
        /// </summary>
        public void Move(double speed, double canvasWidth)
        {
            if (IsStunned) return;

            var nextX = X + speed;
            if (nextX >= 0 && nextX <= canvasWidth - Width)
            {
                X = nextX;
            }
        }

        /// <summary>
        /// Chamado quando o jogador é atingido.
        /// </summary>
        public void TakeHit()
        {
            if (IsStunned) return;

            Lives--;
            if (IsAlive)
            {
                IsStunned = true;
                _stunTimer = StunDuration;
                Visual.Opacity = 0.5; // Efeito visual de "piscar"
            }
        }

        /// <summary>
        /// Adiciona uma vida ao jogador, até o máximo permitido.
        /// </summary>
        public void AddLife(int maxLives)
        {
            if (Lives < maxLives)
            {
                Lives++;
            }
        }
        
        /// <summary>
        /// Atualiza o estado do jogador a cada frame (ex: temporizador de atordoamento).
        /// </summary>
        public void Update()
        {
            if (IsStunned)
            {
                _stunTimer--;
                if (_stunTimer <= 0)
                {
                    IsStunned = false;
                    Visual.Opacity = 1.0; // Restaura a opacidade normal
                }
            }
        }
    }
}
