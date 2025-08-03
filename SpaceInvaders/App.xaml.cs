using Microsoft.UI.Xaml;

namespace SpaceInvaders
{
    public sealed partial class App : Application
    {
        /// <summary>
        /// Expõe a janela principal da aplicação para que outras partes do código possam acessá-la.
        /// </summary>
        public static Window? MainWindow { get; private set; }

        public App()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Chamado quando a aplicação é iniciada.
        /// </summary>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // System.Diagnostics.Debugger.Break();
            }
#endif
            // Cria a janela principal, que servirá de contêiner para o jogo.
            MainWindow = new Window();

            // Cria o "quadro" inicial onde a página do jogo será carregada.
            var rootFrame = new Frame();
            rootFrame.Navigate(typeof(MainPage), args.Arguments);

            // Define o quadro como o conteúdo da janela.
            MainWindow.Content = rootFrame;
            
            // Mostra a janela na tela.
            MainWindow.Activate();
        }
    }
}
