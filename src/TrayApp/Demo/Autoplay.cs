using System.Windows.Threading;
using TrayApp.Features;
using TrayApp.Shell;

namespace TrayApp.Demo;

/// <summary>--autoplay: a 6–8 s scenario for gif.ps1 (DispatcherTimer + list of steps). The app spec defines its own steps.</summary>
public static class Autoplay
{
    public static void Start(MainWindow w)
    {
        // Seconds after the window is on screen; gif.ps1 starts recording when --rect-file appears, ffmpeg needs ~1 s
        (double At, Action Do)[] steps =
        [
            (1.5, () => MainPage.Items.Add(new(Loc.T("Demo.Backup"), @"D:\Backup\demo", false))),
            (2.5, () => MainPage.Items[^1].On = true),
            (3.5, () => MainPage.Items[2].On = true),
            (5.0, () => w.Navigate("settings")),
            (6.5, () => w.Navigate("main")),
        ];
        var started = DateTime.UtcNow;
        var next = 0;
        var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(50) };
        timer.Tick += (_, _) =>
        {
            while (next < steps.Length && (DateTime.UtcNow - started).TotalSeconds >= steps[next].At) steps[next++].Do();
            if (next == steps.Length) timer.Stop();
        };
        timer.Start();
    }
}
