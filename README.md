# Creator Forge Command Manager

Creator Forge Command Manager is a native Windows control panel for managing commands in a running Streamer.bot installation.

The production UI is a .NET Framework 4.8 WinForms DLL loaded by a small Streamer.bot C# Action. It does not require a browser, local web server, HTTP server, or WebSocket connection.

## Current status

Milestone 3N provides the native loading foundation:

- `CreatorForge.CommandManager.dll`
- a single-window WinForms entry point running on its own STA thread
- a Streamer.bot launcher Action source file
- clear missing-DLL and launch-failure logging
- a deterministic Release build script

Command discovery and state controls begin in Milestones 4N and 5N after the DLL loading boundary passes live-host testing.

## Build

From PowerShell:

```powershell
.\build.ps1
```

The Release DLL is written to:

```text
src\CreatorForge.CommandManager\bin\Release\CreatorForge.CommandManager.dll
```

## Live-host test

Do not install the DLL until the build has passed. Then follow [docs/INSTALLATION.md](docs/INSTALLATION.md).

