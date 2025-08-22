
# Space Invaders

Este projeto do clássico jogo de arcade Space Invaders, desenvolvido em C\# utilizando a plataforma Uno Platform com Skia para renderização em desktop. O objetivo é recriar a jogabilidade nostálgica do original, adicionando funcionalidades modernas e uma estrutura de código organizada.

## 🚀 Funcionalidades Implementadas

O jogo atualmente conta com as seguintes funcionalidades:

* **Tela Inicial:** Um menu de início com o título do jogo, uma tabela de pontuação dos inimigos e instruções de controle.
* **Controles do Jogador:**
    * Movimentação horizontal usando as **setas direcionais (← →)** ou as teclas **A** e **D**.
    * Disparo de projéteis com a tecla **Espaço**.
* **Sistema de Vidas:** O jogador começa com um número definido de vidas, que são exibidas na tela. Perde-se uma vida ao ser atingido por um projétil inimigo.
* **Sistema de Pontuação (Score):**
    * A pontuação é exibida e atualizada em tempo real.
    * Ganham-se pontos ao destruir os aliens, com valores diferentes para cada tipo de inimigo.
* **Condição de Vitória:** O jogo é vencido ao atingir **500 pontos**.
* **Enxame de Inimigos:**
    * Os aliens se movem em um enxame coeso, marchando de um lado para o outro.
    * Quando o enxame atinge a borda da tela, ele desce e inverte a direção do movimento.
    * A velocidade do enxame aumenta conforme o número de aliens diminui.
* **Barreiras Destrutíveis:** Quatro barreiras estão posicionadas acima do jogador, oferecendo proteção. Elas podem ser destruídas tanto pelos tiros do jogador quanto pelos dos inimigos.
* **Efeitos Sonoros:** Implementação de sons para ações importantes como tiro, destruição de inimigo e colisão com barreiras (em fase final de depuração).

## 🛠️ Tecnologias Utilizadas

* **Linguagem:** C\#
* **Framework:** .NET
* **Plataforma de UI:** Uno Platform
* **Renderização:** Skia (para Desktop - Windows)
* **IDE:** JetBrains Rider / Visual Studio

## 📂 Estrutura do Projeto

O código-fonte está organizado nas seguintes pastas para manter uma estrutura limpa e de fácil manutenção:

* **`Assets/`**: Contém todos os recursos visuais e sonoros do jogo.
    * `Images/`: Sprites do jogador, inimigos, etc.
    * `Sounds/`: Efeitos sonoros em formato `.mp3`.
* **`Models/`**: Define as classes que representam os objetos do jogo (entidades de dados).
    * `GameObject.cs`: Classe base para todos os objetos visíveis.
    * `Player.cs`: Representa a nave do jogador.
    * `Enemy.cs`: Representa um único alien.
    * `EnemyType.cs`: Define as propriedades de cada tipo de alien (imagem, pontos).
* **`Services/`**: Contém as classes que gerenciam a lógica e as regras do jogo.
    * `GameManager.cs`: O "cérebro" do jogo. Orquestra o loop principal, as colisões, o estado do jogo e a interação entre os outros serviços.
    * `EnemyManager.cs`: Gerencia a criação, movimentação e comportamento do enxame de inimigos.
    * `SoundManager.cs`: Responsável por carregar e tocar todos os efeitos sonoros.

## ⚙️ Como Executar o Projeto

1.  **Clone o repositório:**
    ```bash
    git clone https://gitlab.com/David_Alexss/spaceinvaders
    ```
2.  **Abra o projeto:**
    * Abra o arquivo da solução (`.sln`) com o JetBrains Rider ou Visual Studio.
3.  **Restaure as dependências:**
    * A IDE deve restaurar os pacotes NuGet automaticamente. Se não, faça-o manualmente.
4.  **Compile e Execute:**
    * Certifique-se de que o projeto `SpaceInvaders.Skia.WinUI` esteja definido como projeto de inicialização.
    * Execute o projeto em modo de **Debug** (F5) ou **Release** (Ctrl+F5).
