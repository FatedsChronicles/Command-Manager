# Creator Forge Command Manager

Creator Forge Command Manager is a native Windows interface for inspecting and safely enabling or disabling commands in a running Streamer.bot installation.

This release contains:

- `CreatorForge.CommandManager.dll` - the native Command Manager interface;
- `Command_Manager.cf` - the Streamer.bot launcher Action export;
- `INSTALLATION.md` - installation, upgrade, rollback, and troubleshooting instructions;
- `CHANGELOG.md` - release history;
- `SHA256SUMS.txt` - SHA-256 checksums for every packaged file.

The application uses Streamer.bot's supported command discovery and enabled-state APIs. Command creation and deletion are intentionally not included because Streamer.bot 1.0.7 does not expose a supported mutation API for those operations.

Read `INSTALLATION.md` before replacing an existing DLL. Streamer.bot must be closed while its loaded Command Manager DLL is installed, upgraded, rolled back, or removed.

_____



# Installation, upgrade, and rollback

Creator Forge Command Manager is a native Windows DLL loaded inside Streamer.bot. It has been validated against Streamer.bot 1.0.7 and requires .NET Framework 4.8.

Test a new release in a copied or separate Streamer.bot configuration before using it with your normal streaming setup.

## New installation

1. Close Streamer.bot completely.
2. Extract the release archive into a temporary folder.
3. Verify the included files against `SHA256SUMS.txt`.
4. Copy `CreatorForge.CommandManager.dll` into Streamer.bot's `dll` folder. Create that folder if it does not already exist.
5. Start Streamer.bot.
6. Open Streamer.bot's Import dialog and paste or load the complete contents of `Command_Manager.cf`.
7. Confirm that the import contains only the `CF Command Manager - Open` Action and its launcher code.
8. Complete the import.
9. Add the launcher Action to whichever normal trigger or Stream Deck workflow you want to use.

For an initial validation, create a temporary Core -> Test trigger, run that trigger, and remove it again after Command Manager opens correctly. Do not use an Action context-menu test for the acceptance gate.

## Upgrade

An individual assembly loaded into Streamer.bot cannot be unloaded safely. Never overwrite the Command Manager DLL while Streamer.bot is running.

1. Note the version currently shown in the Command Manager header and close Command Manager.
2. Close Streamer.bot completely.
3. Copy the existing `CreatorForge.CommandManager.dll` from Streamer.bot's `dll` folder into a safe backup folder.
4. Copy the new DLL into the `dll` folder, replacing the existing file.
5. Compare the installed DLL's SHA-256 hash with the release checksum.
6. Start Streamer.bot and launch Command Manager normally.
7. Confirm the new version, command totals, filters, sorting, mutations, and live synchronization before deleting the backup.

The launcher Action does not need to be re-imported unless the release notes explicitly say that its source changed.

## Rollback

1. Close Command Manager and Streamer.bot completely.
2. Replace the current DLL in Streamer.bot's `dll` folder with the backed-up DLL.
3. Start Streamer.bot and launch Command Manager.
4. Confirm that the previous version appears in the header and can read the expected command totals.

If the launcher Action was also upgraded, restore its previous exported Action or paste the previous launcher source before testing the rollback.

## Removal

1. Close Streamer.bot completely.
2. Remove `CreatorForge.CommandManager.dll` from Streamer.bot's `dll` folder.
3. Start Streamer.bot and delete the `CF Command Manager - Open` Action or disconnect its normal trigger.

Removing Command Manager does not delete or edit any Streamer.bot commands.

## Diagnostics

Open **Diagnostics** in Command Manager, or press **Ctrl+D**, to view a metadata-only report. The report includes application/runtime information, counts, filter and sort state, and synchronization health. It intentionally excludes command names, aliases, groups, IDs, and search text.

Use **Copy diagnostics** when you need to provide the report with an error description. Review copied information before sharing it.

## Troubleshooting

### The launcher reports that the DLL is missing

Confirm that the file is named exactly `CreatorForge.CommandManager.dll` and is inside the active Streamer.bot installation's `dll` folder. Multiple Streamer.bot copies can easily lead to updating the wrong folder.

### The old version still appears

Close Streamer.bot completely and check Task Manager for a remaining Streamer.bot process. Confirm the source and installed DLL hashes match before restarting.

### Windows blocks the downloaded DLL

Close Streamer.bot, open the DLL's Properties window, and use **Unblock** if Windows shows that option. Only unblock a file after its SHA-256 checksum matches the trusted release checksum.

### Command discovery fails

Open Diagnostics and record the application version, command-service state, and synchronization information. Review Streamer.bot's log for entries beginning with `[CF Command Manager]`.

### Live data may be stale

The last authoritative snapshot remains visible after repeated query failures. Confirm Streamer.bot is responsive, then use Refresh or wait for the next live-sync interval. A later successful read clears the stale warning automatically.

### Mouse-wheel scrolling stops after a state change

Version 0.7.0 restores focus to the command row after an individual mutation. Confirm the installed DLL version and hash if scrolling still requires clicking the grid first.
#

