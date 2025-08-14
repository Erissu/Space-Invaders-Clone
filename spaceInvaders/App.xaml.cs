using spaceInvaders.Views;

namespace spaceInvaders;

public partial class App : Application
{
    protected Window? MainWindow { get; private set; }
    protected IHost? Host { get; private set; }

    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        var builder = this.CreateBuilder(args)
            .Configure(host => host
#if DEBUG
                .UseEnvironment(Environments.Development)
#endif
                .UseLogging(configure: (context, logBuilder) =>
                {
                    logBuilder.SetMinimumLevel(LogLevel.Information);
                })
            );

        MainWindow = builder.Window;
        Host = builder.Build();

        if (MainWindow.Content is not Frame frame)
        {
            frame = new Frame();
            MainWindow.Content = frame;
        }
        
        frame.Navigate(typeof(HomePage), args.Arguments);

        MainWindow.Activate();
    }
}
