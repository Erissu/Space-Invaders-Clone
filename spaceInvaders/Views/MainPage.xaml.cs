using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using spaceInvaders.GameLogic;
using Windows.System;

namespace spaceInvaders.Views;
public sealed partial class MainPage : Page, IGameView
{
    private readonly GameManager _gameManager;

    public MainPage()
    {
        this.InitializeComponent();
        
        _gameManager = new GameManager(this);

        this.Loaded += OnPageLoaded;
    }

    private void OnPageLoaded(object sender, RoutedEventArgs e)
    {
        this.Focus(FocusState.Programmatic);
        _gameManager.StartGame();
    }
    
    public void AddGameObject(UIElement sprite)
    {
        GameCanvas.Children.Add(sprite);
    }

    public void RemoveGameObject(UIElement sprite)
    {
        GameCanvas.Children.Remove(sprite);
    }

    public void UpdateScore(int score)
    {
        ScoreText.Text = $"SCORE: {score}";
    }

    public void UpdateLives(int lives)
    {
        LivesText.Text = $"LIVES: {lives}";
    }

    public void ShowGameOver(bool isVisible)
    {
        GameOverText.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
    }

    public void ShowMenuButton(bool isVisible)
    {
        MenuButton.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
    }
    
    //  Métodos de Evento
    private void Page_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        _gameManager.HandleKeyDown(e.Key);
    }

    private void Page_KeyUp(object sender, KeyRoutedEventArgs e)
    {
        _gameManager.HandleKeyUp(e.Key);
    }
    
    private void MenuButton_Click(object sender, RoutedEventArgs e)
    {
        if (this.Frame.CanGoBack)
        {
            this.Frame.GoBack();
        }
    }
}
