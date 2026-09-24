# Architecture

## Host boundary

Streamer.bot owns the live `CPH` object. The `streamerbot/OpenCommandManager.cs` Action resolves Streamer.bot's installation directory, loads `dll/CreatorForge.CommandManager.dll`, and calls:

```csharp
CreatorForge.CommandManager.Core.CommandManagerApp.Show(CPH)
```

The DLL starts a dedicated STA thread and owns one WinForms message loop. Repeated launch requests activate the existing window instead of creating duplicates.

## Milestone boundary

Milestone 3N deliberately contains no command-management reflection. It proves only:

- assembly discovery and loading;
- entry-point invocation;
- CPH object handoff;
- single-window lifecycle;
- safe UI threading;
- close and reopen behavior.

Milestone 4N adds an isolated `ICommandService` adapter. The UI will never perform reflection directly.

## Deployment constraint

The DLL is loaded into Streamer.bot's main application domain. Close Streamer.bot before replacing an installed DLL; an individual loaded assembly cannot be unloaded safely.

