using Microsoft.UI.Xaml;
using spaceInvaders.Models;

namespace spaceInvaders.Views;
public interface IGameView
{
    void AddGameObject(UIElement sprite);
    void RemoveGameObject(UIElement sprite);
    void UpdateScore(int score);
    void UpdateLives(int lives);
    void ShowGameOver(bool isVisible);
    void ShowMenuButton(bool isVisible);
}
