using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using SpaceInvaders.Services;
using Windows.System;

namespace SpaceInvaders
{
    public sealed partial class MainPage : Page
    {
        private GameManager? _gameManager;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            // Esconde o menu e mostra a tela do jogo
            StartScreen.Visibility = Visibility.Collapsed;
            InGameUI.Visibility = Visibility.Visible;
            
            // Se o jogo nunca foi criado, cria uma nova instância
            if (_gameManager == null)
            {
                _gameManager = new GameManager(GameCanvas, ScoreText, Life1, Life2, Life3, Life4, Life5, Life6, PlayerImage);
                
                // Anexa os eventos de teclado APENAS UMA VEZ
                InGameUI.KeyDown += GameUI_KeyDown;
                InGameUI.KeyUp += GameUI_KeyUp;
            }
            
            // Inicia o jogo (ou reinicia, se já existia)
            _gameManager.StartGame();
            
            // Garante que a tela do jogo possa receber os comandos do teclado
            InGameUI.Focus(FocusState.Programmatic);
        }

        // Envia o comando de tecla pressionada para o GameManager
        private void GameUI_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            _gameManager?.OnKeyDown(e.Key);
        }

        // Envia o comando de tecla solta para o GameManager
        private void GameUI_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            _gameManager?.OnKeyUp(e.Key);
        }
    }
}
