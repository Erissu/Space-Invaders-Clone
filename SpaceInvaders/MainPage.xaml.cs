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
            this.Loaded += OnPageLoaded;
        }

        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            _gameManager = new GameManager(GameCanvas, ScoreText, Life1, Life2, Life3, Life4, Life5, Life6, PlayerImage);
        }
        
        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            StartScreen.Visibility = Visibility.Collapsed;
            InGameUI.Visibility = Visibility.Visible;
            
            _gameManager?.StartGame();
            
            // --- CORREÇÃO FINAL E MAIS IMPORTANTE ---
            // 1. Damos o foco diretamente para a área do jogo (o Grid InGameUI).
            InGameUI.Focus(FocusState.Programmatic);

            // 2. Anexamos os eventos de teclado diretamente a essa área do jogo.
            // Agora, quando o InGameUI estiver focado, ele VAI ouvir as teclas.
            InGameUI.KeyDown += (s, args) => _gameManager?.OnKeyDown(args.Key);
            InGameUI.KeyUp += (s, args) => _gameManager?.OnKeyUp(args.Key);
        }
    }
}
