using System;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using CreatorForge.CommandManager.UI;

namespace CreatorForge.CommandManager.Core
{
    public static class CommandManagerApp
    {
        private static readonly object SyncRoot = new object();
        private static MainWindow _window;
        private static bool _starting;

        public static void Show()
        {
            Show(null);
        }

        public static void Show(object streamerBotApi)
        {
            MainWindow existingWindow;
            bool startNewWindow = false;

            lock (SyncRoot)
            {
                existingWindow = _window;
                if (existingWindow == null || existingWindow.IsDisposed)
                {
                    if (_starting)
                        return;

                    _starting = true;
                    startNewWindow = true;
                }
            }

            if (startNewWindow)
            {
                try
                {
                    StartUiThread(streamerBotApi);
                }
                catch
                {
                    lock (SyncRoot)
                    {
                        _starting = false;
                    }

                    throw;
                }

                return;
            }

            ActivateWindow(existingWindow);
        }

        private static void StartUiThread(object streamerBotApi)
        {
            var uiThread = new Thread(() => RunUi(streamerBotApi));
            uiThread.Name = "Creator Forge Command Manager UI";
            uiThread.IsBackground = true;
            uiThread.SetApartmentState(ApartmentState.STA);
            uiThread.Start();
        }

        private static void RunUi(object streamerBotApi)
        {
            MainWindow createdWindow = null;

            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                createdWindow = new MainWindow(streamerBotApi);
                lock (SyncRoot)
                {
                    _window = createdWindow;
                    _starting = false;
                }

                Application.Run(createdWindow);
            }
            catch (Exception ex)
            {
                TryLogError(streamerBotApi, "[CF Command Manager] Native UI thread failed: " + ex);
            }
            finally
            {
                lock (SyncRoot)
                {
                    if (ReferenceEquals(_window, createdWindow))
                        _window = null;

                    _starting = false;
                }

                if (createdWindow != null)
                    createdWindow.Dispose();
            }
        }

        private static void ActivateWindow(MainWindow window)
        {
            if (window == null || window.IsDisposed)
                return;

            try
            {
                window.BeginInvoke(new Action(() =>
                {
                    if (window.IsDisposed)
                        return;

                    if (window.WindowState == FormWindowState.Minimized)
                        window.WindowState = FormWindowState.Normal;

                    window.Show();
                    window.BringToFront();
                    window.Activate();
                }));
            }
            catch (InvalidOperationException)
            {
                // The existing window is closing. A later request can open a new one.
            }
        }

        private static void TryLogError(object streamerBotApi, string message)
        {
            if (streamerBotApi == null)
                return;

            try
            {
                MethodInfo method = streamerBotApi.GetType().GetMethod(
                    "LogError",
                    BindingFlags.Public | BindingFlags.Instance,
                    null,
                    new[] { typeof(string) },
                    null);

                if (method != null)
                    method.Invoke(streamerBotApi, new object[] { message });
            }
            catch
            {
                // Logging must never cause a second UI-thread failure.
            }
        }
    }
}
