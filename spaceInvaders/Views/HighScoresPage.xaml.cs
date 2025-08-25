using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace spaceInvaders.Views;

public sealed partial class HighScoresPage : Page
{
    public HighScoresPage()
    {
        this.InitializeComponent();
    }

    private void BackButton_Click(object sender, RoutedEventArgs e)
    {
        if (this.Frame.CanGoBack)
        {
            this.Frame.GoBack();
        }
    }
}
