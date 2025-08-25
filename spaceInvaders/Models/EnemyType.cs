namespace SpaceInvaders.Models
{
    /// <summary>
    /// Classe bem simples, só para guardar os dados de um tipo de inimigo.
    /// Funciona como uma "ficha" com a imagem e os pontos de cada alien.
    /// </summary>
    public class EnemyType
    {
        // O caminho para o arquivo de imagem do inimigo.
        public  required string ImageSource { get; set; }
        
        // Quantos pontos o jogador ganha ao destruir este tipo de inimigo.
        public int Points { get; set; }
    }
}
