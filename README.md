<<<<<<< HEAD

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
=======
# Space Invaders Hollow Edition



## Getting started

To make it easy for you to get started with GitLab, here's a list of recommended next steps.

Already a pro? Just edit this README.md and make it your own. Want to make it easy? [Use the template at the bottom](#editing-this-readme)!

## Add your files

- [ ] [Create](https://docs.gitlab.com/ee/user/project/repository/web_editor.html#create-a-file) or [upload](https://docs.gitlab.com/ee/user/project/repository/web_editor.html#upload-a-file) files
- [ ] [Add files using the command line](https://docs.gitlab.com/topics/git/add_files/#add-files-to-a-git-repository) or push an existing Git repository with the following command:

```
cd existing_repo
git remote add origin https://gitlab.com/Erisson.Oliveira/space-invaders-hollow-edition.git
git branch -M main
git push -uf origin main
```

## Integrate with your tools

- [ ] [Set up project integrations](https://gitlab.com/Erisson.Oliveira/space-invaders-hollow-edition/-/settings/integrations)

## Collaborate with your team

- [ ] [Invite team members and collaborators](https://docs.gitlab.com/ee/user/project/members/)
- [ ] [Create a new merge request](https://docs.gitlab.com/ee/user/project/merge_requests/creating_merge_requests.html)
- [ ] [Automatically close issues from merge requests](https://docs.gitlab.com/ee/user/project/issues/managing_issues.html#closing-issues-automatically)
- [ ] [Enable merge request approvals](https://docs.gitlab.com/ee/user/project/merge_requests/approvals/)
- [ ] [Set auto-merge](https://docs.gitlab.com/user/project/merge_requests/auto_merge/)

## Test and Deploy

Use the built-in continuous integration in GitLab.

- [ ] [Get started with GitLab CI/CD](https://docs.gitlab.com/ee/ci/quick_start/)
- [ ] [Analyze your code for known vulnerabilities with Static Application Security Testing (SAST)](https://docs.gitlab.com/ee/user/application_security/sast/)
- [ ] [Deploy to Kubernetes, Amazon EC2, or Amazon ECS using Auto Deploy](https://docs.gitlab.com/ee/topics/autodevops/requirements.html)
- [ ] [Use pull-based deployments for improved Kubernetes management](https://docs.gitlab.com/ee/user/clusters/agent/)
- [ ] [Set up protected environments](https://docs.gitlab.com/ee/ci/environments/protected_environments.html)

***

# Editing this README

When you're ready to make this README your own, just edit this file and use the handy template below (or feel free to structure it however you want - this is just a starting point!). Thanks to [makeareadme.com](https://www.makeareadme.com/) for this template.

## Suggestions for a good README

Every project is different, so consider which of these sections apply to yours. The sections used in the template are suggestions for most open source projects. Also keep in mind that while a README can be too long and detailed, too long is better than too short. If you think your README is too long, consider utilizing another form of documentation rather than cutting out information.

## Name
Choose a self-explaining name for your project.

## Description
Let people know what your project can do specifically. Provide context and add a link to any reference visitors might be unfamiliar with. A list of Features or a Background subsection can also be added here. If there are alternatives to your project, this is a good place to list differentiating factors.

## Badges
On some READMEs, you may see small images that convey metadata, such as whether or not all the tests are passing for the project. You can use Shields to add some to your README. Many services also have instructions for adding a badge.

## Visuals
Depending on what you are making, it can be a good idea to include screenshots or even a video (you'll frequently see GIFs rather than actual videos). Tools like ttygif can help, but check out Asciinema for a more sophisticated method.

## Installation
Within a particular ecosystem, there may be a common way of installing things, such as using Yarn, NuGet, or Homebrew. However, consider the possibility that whoever is reading your README is a novice and would like more guidance. Listing specific steps helps remove ambiguity and gets people to using your project as quickly as possible. If it only runs in a specific context like a particular programming language version or operating system or has dependencies that have to be installed manually, also add a Requirements subsection.

## Usage
Use examples liberally, and show the expected output if you can. It's helpful to have inline the smallest example of usage that you can demonstrate, while providing links to more sophisticated examples if they are too long to reasonably include in the README.

## Support
Tell people where they can go to for help. It can be any combination of an issue tracker, a chat room, an email address, etc.

## Roadmap
If you have ideas for releases in the future, it is a good idea to list them in the README.

## Contributing
State if you are open to contributions and what your requirements are for accepting them.

For people who want to make changes to your project, it's helpful to have some documentation on how to get started. Perhaps there is a script that they should run or some environment variables that they need to set. Make these steps explicit. These instructions could also be useful to your future self.

You can also document commands to lint the code or run tests. These steps help to ensure high code quality and reduce the likelihood that the changes inadvertently break something. Having instructions for running tests is especially helpful if it requires external setup, such as starting a Selenium server for testing in a browser.

## Authors and acknowledgment
Show your appreciation to those who have contributed to the project.

## License
For open source projects, say how it is licensed.

## Project status
If you have run out of energy or time for your project, put a note at the top of the README saying that development has slowed down or stopped completely. Someone may choose to fork your project or volunteer to step in as a maintainer or owner, allowing your project to keep going. You can also make an explicit request for maintainers.
>>>>>>> b27a6b1009bfefc1880b79e5d0db58b6bfdaaba0
