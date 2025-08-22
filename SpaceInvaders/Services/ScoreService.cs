using SpaceInvaders.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SpaceInvaders.Services
{
    /// <summary>
    /// Gere a persistência (leitura e escrita) da tabela de placares num ficheiro de texto.
    /// </summary>
    public class ScoreService
    {
        private readonly string _filePath;

        public ScoreService()
        {
            // Define o caminho completo para o nosso ficheiro de placares na mesma pasta do executável.
            _filePath = Path.Combine(AppContext.BaseDirectory, "highscores.txt");
        }

        /// <summary>
        /// Carrega os placares do ficheiro highscores.txt.
        /// </summary>
        /// <returns>Uma lista de entradas de placar, ordenada da maior para a menor pontuação.</returns>
        public List<HighScoreEntry> LoadHighScores()
        {
            // Se o ficheiro não existir, cria um com alguns dados de exemplo.
            if (!File.Exists(_filePath))
            {
                CreateDummyFile();
            }

            var scores = new List<HighScoreEntry>();
            var lines = File.ReadAllLines(_filePath);

            foreach (var line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length == 2 && int.TryParse(parts[1], out int score))
                {
                    scores.Add(new HighScoreEntry(parts[0], score));
                }
            }

            // Ordena as pontuações e retorna a lista.
            return scores.OrderByDescending(s => s.Score).ToList();
        }

        /// <summary>
        /// Adiciona uma nova pontuação e guarda a lista atualizada no ficheiro.
        /// </summary>
        public void AddAndSaveScore(HighScoreEntry newEntry)
        {
            var scores = LoadHighScores();
            scores.Add(newEntry);
            
            // Ordena novamente e mantém apenas os 10 melhores resultados.
            var topScores = scores.OrderByDescending(s => s.Score).Take(10).ToList();

            var lines = topScores.Select(s => $"{s.PlayerName},{s.Score}").ToList();
            File.WriteAllLines(_filePath, lines);
        }

        /// <summary>
        /// Cria um ficheiro de placares de exemplo se nenhum existir.
        /// </summary>
        private void CreateDummyFile()
        {
            var dummyScores = new List<string>
            {
                "CPU,350",
                "ACE,200",
                "BOT,50"
            };
            File.WriteAllLines(_filePath, dummyScores);
        }
    }
}
