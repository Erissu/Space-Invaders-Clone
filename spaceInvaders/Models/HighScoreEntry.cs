namespace SpaceInvaders.Models
{
    /// <summary>
    /// Representa uma única entrada na tabela de placares, com o nome do jogador e a sua pontuação.
    /// </summary>
    public class HighScoreEntry
    {
        public string PlayerName { get; set; }
        public int Score { get; set; }

        public HighScoreEntry(string playerName, int score)
        {
            PlayerName = playerName;
            Score = score;
        }
    }
}
