using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace spaceInvaders.Views;

public sealed partial class HomePage : Page
{
    public HomePage()
    {
        this.InitializeComponent();
    }

    private void NewGameButton_Click(object sender, RoutedEventArgs e)
    {
        this.Frame.Navigate(typeof(MainPage));
    }

    private void HighScoresButton_Click(object sender, RoutedEventArgs e)
    {
        this.Frame.Navigate(typeof(HighScoresPage));
    }

    private void ControlsButton_Click(object sender, RoutedEventArgs e)
    {
        this.Frame.Navigate(typeof(ControlsPage));
    }
}
