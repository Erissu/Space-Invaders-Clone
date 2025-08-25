using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using SpaceInvaders.ViewModels;
using Windows.System;

namespace SpaceInvaders
{
    public sealed partial class MainPage : Page
    {
        public GameViewModel ViewModel { get; }

        public MainPage()
        {
            this.InitializeComponent();
            ViewModel = new GameViewModel();
            this.DataContext = ViewModel;
        }

        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            var gameElements = new object[] { GameCanvas, PlayerImage };
            if (ViewModel.StartGameCommand.CanExecute(gameElements))
            {
                ViewModel.StartGameCommand.Execute(gameElements);
            }
            InGameUI.KeyDown -= GameUI_KeyDown; 
            InGameUI.KeyUp -= GameUI_KeyUp;
            InGameUI.KeyDown += GameUI_KeyDown;
            InGameUI.KeyUp += GameUI_KeyUp;
            InGameUI.Focus(FocusState.Programmatic);
        }
        
        private void GameUI_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            // CORRIGIDO: Agora verifica a tecla Escape (ESC) em vez de 'P'.
            if (e.Key == VirtualKey.Escape)
            {
                if (ViewModel.IsPauseScreenVisible)
                {
                    ViewModel.ResumeGameCommand.Execute(null);
                }
                else
                {
                    ViewModel.PauseGameCommand.Execute(null);
                }
            }
            else
            {
                ViewModel.KeyDownCommand.Execute(e.Key);
            }
        }

        private void GameUI_KeyUp(object sender, KeyRoutedEventArgs e) => ViewModel.KeyUpCommand.Execute(e.Key);
    }
}
