# Milestone 3N live-host installation

This installation is only for the native DLL loading test.

1. Build the Release configuration with `build.ps1`.
2. Close Streamer.bot before replacing an existing Command Manager DLL.
3. Copy `CreatorForge.CommandManager.dll` into Streamer.bot's `dll` folder.
4. Start Streamer.bot.
5. Create an Action named `CF Command Manager - Open` in the `Creator Forge - Command Manager` group.
6. Add one **Core -> C# -> Execute C# Code** sub-action.
7. Paste the complete contents of `streamerbot/OpenCommandManager.cs` into the editor.
8. Compile, then Save and Compile.
9. Add a temporary **Core -> Test** trigger for the live-host test.
10. Run the Test trigger.

Expected result: one native Command Manager window opens and Streamer.bot remains responsive.

Test the following sequence before moving to Milestone 4N:

- Open the manager.
- Trigger the Action again; the existing window should be activated without opening a duplicate.
- Close the manager.
- Trigger the Action again; a fresh window should open.
- Review the Streamer.bot log for launch errors.

Remove the temporary Test trigger after validation. Keep the `CF Command Manager - Open` Action.

