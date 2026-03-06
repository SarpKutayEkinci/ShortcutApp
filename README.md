ShortcutApp 🚀
ShortcutApp is a fast, extensible, and keyboard-centric productivity tool built with C# WPF. It aims to minimize mouse dependency by providing instant access to applications, web queries, and system commands through a dedicated global window.

🎯 Purpose
The main goal of this project is to streamline your workflow. Instead of searching through menus or clicking icons, you can trigger a minimal window from anywhere in Windows, type a short keyword, and immediately reach your destination.

✨ Key Features
Global Accessibility: Bind a dedicated key combination (Alt + Space or Alt + G) to summon the app instantly, even if it's running in the background.

Mouse-Less Workflow: Designed for power users who prefer keyboard shortcuts for opening apps like VS Code, Steam, or Notepad.

Dynamic Command Mapping: All shortcuts are stored in an external shortcuts.json file. You can add, remove, or update your shortcuts without ever touching the source code.

Smart Web Queries: Built-in support for Google searches directly from the launcher.

🛠️ How It Works
The Engine (Win32 API)
The application uses low-level Windows integration to "hook" into the system's message loop:

RegisterHotKey: Tells Windows to reserve your specific key binding just for this app.

HwndSource: Acts as a listener that catches the WM_HOTKEY signal sent by Windows whenever you press your keys.

The UI (WPF)
Focused Interaction: When the window appears, it uses the Dispatcher to force focus onto the search bar, ensuring you can start typing immediately without a single click.

Minimalist Design: A clean, distraction-free interface that stays out of your way until needed.
