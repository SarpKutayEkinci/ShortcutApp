using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Input;
using System.Diagnostics;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        // --- Win32 API Tanımları ---
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private const int WM_HOTKEY = 0x0312;
        private const int HOTKEY_ID = 9001;

        // Modifiers
        private const uint MOD_ALT = 0x0001;

        // Key Codes
        private const uint VK_SPACE = 0x20;
        private const uint VK_G = 0x47;

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            IntPtr handle = new WindowInteropHelper(this).Handle;
            HwndSource source = HwndSource.FromHwnd(handle);
            source.AddHook(HwndHook);

            
            bool isRegistered = RegisterHotKey(handle, HOTKEY_ID, MOD_ALT, VK_SPACE);

            if (!isRegistered)
            {
                
                isRegistered = RegisterHotKey(handle, HOTKEY_ID, MOD_ALT, VK_G);
                if (!isRegistered)
                {
                    MessageBox.Show("Kısayol kaydedilemedi!");
                }
            }

            
            this.Hide();
        }

        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_HOTKEY && wParam.ToInt32() == HOTKEY_ID)
            {
                ShowLauncher();
                handled = true;
            }
            return IntPtr.Zero;
        }

        private void ShowLauncher()
        {
            this.Show();
            this.WindowState = WindowState.Normal;
            this.Visibility = Visibility.Visible;
            this.Topmost = true;
            this.Activate();

            
            Dispatcher.BeginInvoke(new Action(() =>
            {
                MySearchTextBox.Focus();
                Keyboard.Focus(MySearchTextBox);
            }), System.Windows.Threading.DispatcherPriority.Render);
        }

        private void MySearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string query = MySearchTextBox.Text.Trim();
                if (string.IsNullOrEmpty(query)) return;

                ExecuteCommand(query.ToLower());

                this.Hide();
                MySearchTextBox.Clear();
            }
            else if (e.Key == Key.Escape)
            {
                this.Hide();
            }
        }

        private void ExecuteCommand(string query)
        {
            try
            {
                if (query == "vsc")
                {
                    Process.Start(new ProcessStartInfo("code") { UseShellExecute = true });
                }
                else if (query.StartsWith("g "))
                {
                    string search = query.Substring(2);
                    Process.Start(new ProcessStartInfo($"https://google.com/search?q={search}") { UseShellExecute = true });
                }
                else if (query == "not")
                {
                    Process.Start(new ProcessStartInfo("notepad.exe") { UseShellExecute = true });
                }
                else if (query == "steam")
                {
                    Process.Start(new ProcessStartInfo("steam://open/main") { UseShellExecute = true });
                }
                else if (query == "wsl")
                {
                   
                    string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                    var startInfo = new ProcessStartInfo
                    {
                        FileName = "wsl.exe",
                        
                        WorkingDirectory = desktopPath,
                        UseShellExecute = false
                    };

                    
                    Process.Start(startInfo)?.WaitForExit();
                }


                else if (query == "kapat" || query == "exit")
                {

                    Application.Current.Shutdown();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            
            IntPtr handle = new WindowInteropHelper(this).Handle;
            UnregisterHotKey(handle, HOTKEY_ID);
            base.OnClosed(e);
        }
    }
}