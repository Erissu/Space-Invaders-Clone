using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using SpaceInvaders.Models;
using SpaceInvaders.Services;
using System.Collections.ObjectModel;
using Windows.System;

namespace SpaceInvaders.ViewModels
{
    public partial class GameViewModel : ObservableObject
    {
        private GameManager? _gameManager;
        private readonly ScoreService _scoreService;

        [ObservableProperty]
        private bool _isStartScreenVisible = true;
        [ObservableProperty]
        private bool _isGameScreenVisible = false;
        [ObservableProperty]
        private bool _isGameOverScreenVisible = false;
        [ObservableProperty]
        private bool _isHighScoreScreenVisible = false;
        [ObservableProperty]
        private bool _isPauseScreenVisible = false;

        [ObservableProperty]
        private int _score;
        [ObservableProperty]
        private bool _isLife1Visible, _isLife2Visible, _isLife3Visible;
        
        [ObservableProperty]
        private string _gameOverTitle = "Fim de Jogo";
        [ObservableProperty]
        private int _finalScore;
        [ObservableProperty]
        private string _playerName = "JOGADOR";
        
        [ObservableProperty]
        private bool _isSavingScoreVisible = true;
        [ObservableProperty]
        private bool _isPostSaveOptionsVisible = false;
        
        public ObservableCollection<HighScoreEntry> HighScores { get; } = new();

        public GameViewModel()
        {
            _scoreService = new ScoreService();
        }
        
        [RelayCommand]
        private void StartGame(object? gameElements)
        {
            IsStartScreenVisible = false;
            IsGameOverScreenVisible = false;
            IsHighScoreScreenVisible = false;
            IsPauseScreenVisible = false;
            IsGameScreenVisible = true;
            if (gameElements is object[] elements && elements.Length == 2) {
                var canvas = (Canvas)elements[0];
                var playerImage = (Image)elements[1];
                _gameManager = new GameManager(canvas, playerImage);
                _gameManager.OnScoreUpdated = (newScore) => Score = newScore;
                _gameManager.OnLivesUpdated = UpdateLivesDisplay;
                _gameManager.OnGameOver = HandleGameOver;
                _gameManager.StartGame();
            }
        }
        
        private void HandleGameOver(bool playerWon, int finalScore) { IsGameScreenVisible = false; GameOverTitle = playerWon ? "Você Venceu!" : "Fim de Jogo"; FinalScore = finalScore; PlayerName = "JOGADOR"; IsSavingScoreVisible = true; IsPostSaveOptionsVisible = false; IsGameOverScreenVisible = true; }
        
        [RelayCommand]
        private void SaveScore() { if (string.IsNullOrWhiteSpace(PlayerName)) return; var newEntry = new HighScoreEntry(PlayerName.ToUpper(), FinalScore); _scoreService.AddAndSaveScore(newEntry); IsSavingScoreVisible = false; IsPostSaveOptionsVisible = true; }
        
        [RelayCommand]
        private void ReturnToMenu() { IsGameOverScreenVisible = false; IsHighScoreScreenVisible = false; IsPauseScreenVisible = false; IsGameScreenVisible = false; IsStartScreenVisible = true; _gameManager?.StopGame(); _gameManager = null; }
        
        [RelayCommand]
        private void ShowHighScores() { HighScores.Clear(); var scores = _scoreService.LoadHighScores(); foreach (var score in scores) { HighScores.Add(score); } IsStartScreenVisible = false; IsHighScoreScreenVisible = true; }

        [RelayCommand]
        private void PauseGame() { if (_gameManager is { IsPaused: false }) { _gameManager.PauseGame(); IsPauseScreenVisible = true; } }
        
        [RelayCommand]
        private void ResumeGame() { if (_gameManager is { IsPaused: true }) { _gameManager.ResumeGame(); IsPauseScreenVisible = false; } }

        private void UpdateLivesDisplay(int lives) { IsLife1Visible = lives >= 1; IsLife2Visible = lives >= 2; IsLife3Visible = lives >= 3; }
        
        [RelayCommand]
        private void KeyDown(VirtualKey key) => _gameManager?.HandleKeyDown(key);
        [RelayCommand]
        private void KeyUp(VirtualKey key) => _gameManager?.HandleKeyUp(key);
    }
}
