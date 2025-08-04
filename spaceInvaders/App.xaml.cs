// App.xaml.cs

namespace spaceInvaders;

public partial class App : Application
{
    protected Window? MainWindow { get; private set; }
    protected IHost? Host { get; private set; }

    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        var builder = this.CreateBuilder(args)
            // Adiciona serviços de logging e configuração (padrão)
            .Configure(host => host
#if DEBUG
                // Habilita o hot reload para XAML em modo Debug.
                .UseEnvironment(Environments.Development)
#endif
                .UseLogging(configure: (context, logBuilder) =>
                {
                    // Configura o logging
                    logBuilder.SetMinimumLevel(LogLevel.Information);
                })
            );

        MainWindow = builder.Window;

        Host = builder.Build();

        // Limpamos toda a configuração de serviços e navegação desnecessária.
        // O código abaixo simplesmente cria e exibe nossa MainPage.
        // Para o nosso jogo simples, não precisamos de injeção de dependência ou navegação complexa.
        if (MainWindow.Content is not Frame frame)
        {
            frame = new Frame();
            MainWindow.Content = frame;
        }

        // Navega diretamente para a página do jogo.
        frame.Navigate(typeof(HomePage), args.Arguments);

        MainWindow.Activate();
    }
}
