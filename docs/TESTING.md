# Testing

## Release build

Run from the repository root:

```powershell
.\build.ps1
```

The build treats compiler warnings as errors and must produce:

```text
src\CreatorForge.CommandManager\bin\Release\CreatorForge.CommandManager.dll
```

## Deterministic harness

After the Release build, run:

```powershell
.\qa\CommandManager.Harness\bin\Release\CreatorForge.CommandManager.Harness.exe --verify
```

The harness verifies:

- six fake commands summarize as four enabled and two disabled;
- name, alias, group, ID, state, No Group, combined, and long-text filters compose correctly;
- Name, Aliases, Group, ID, State, and Action sort in the requested direction;
- fake commands pass enable, disable, no-op, missing-command, rejected-mutation, and disappearance paths;
- a null `GetCommands()` response becomes a logged error result;
- a malformed command object becomes a logged error result identifying the missing property;
- the reflection adapter passes enable, disable, no-op, and failed-verification paths.
- bulk operations mutate only previewed IDs, report serialized progress, perform a final refresh, and continue after a partial failure.

Run the harness without `--verify` to inspect the native UI with fake commands:

```powershell
.\qa\CommandManager.Harness\bin\Release\CreatorForge.CommandManager.Harness.exe
```

## Milestone 5N live gate

Close Streamer.bot before replacing a previously loaded DLL. Copy the successful Release DLL into the test installation's `dll` directory, start that test installation, and run the Command Manager launcher Action.

Confirm that:

1. the total, enabled, and disabled counts match Streamer.bot;
2. displayed names, groups, aliases, IDs, and states match the live command records;
3. Refresh reloads the current records without opening another window;
4. each command exposes one action matching its authoritative state;
5. a safe temporary command passes disabled to enabled to disabled;
6. Streamer.bot and Command Manager agree after each operation;
7. the control shows a busy state and repeated clicks cannot overlap;
8. a no-op and a missing-command result are handled without an optimistic state change.

Do not use a production Streamer.bot installation for this gate.

## Milestone 6 live gate

Use the designated Streamer.bot test copy and confirm that:

1. the unfiltered view reports 67 visible commands from the 67-command baseline;
2. search matches a name, alias, group, and ID fragment without changing authoritative totals;
3. All, Enabled, and Disabled state filters report 67, 61, and 6 visible commands;
4. the group filter includes No Group and composes with search and state;
5. a no-results search keeps the layout usable and reports 0 of 67;
6. Refresh preserves the active search, state filter, group filter, selected command, and scroll context;
7. clearing the filters returns to the 67 / 61 / 6 baseline.

## Milestone 7 live gate

Use only the designated Streamer.bot test copy. Choose a small, reversible scope and record its command IDs and starting states before confirming any bulk change.

Confirm that:

1. Enable visible and Disable visible preview only commands in the current filtered result that need a state change;
2. Enable group and Disable group remain unavailable until a specific group or No Group is selected;
3. a group preview includes the whole selected group even when search or state filters hide some of its commands;
4. the confirmation dialog lists the exact affected command names and IDs;
5. Cancel closes the preview and performs no mutations;
6. confirmation runs one command at a time, reports progress, and disables overlapping controls;
7. the result summary reports succeeded, changed, and failed counts accurately;
8. Streamer.bot and Command Manager agree after the final authoritative refresh;
9. restoring the starting states succeeds through a second confirmed bulk operation.

## Milestone 8 manual gate

Do not use a production Streamer.bot installation. Build first, install the Release DLL into the designated test copy while Streamer.bot is closed, and launch Command Manager through a temporary Core → Test trigger.

Manually confirm that:

1. version 0.6.0 opens with Live sync enabled and the 10-second interval selected;
2. 5, 10, 30, and 60-second intervals can be selected and the status reflects the chosen interval;
3. turning Live sync off prevents automatic updates and shows Live sync paused;
4. turning Live sync on resumes updates without reopening the manager;
5. direct Streamer.bot changes to command state, name, group, aliases, additions, and removals appear after the next poll;
6. active search, state filter, group filter, selected row, and scroll position remain stable when a poll updates data;
7. polling does not overlap manual refresh, an individual mutation, or a confirmed bulk mutation;
8. one or two temporary polling failures retain the displayed command snapshot and show retry status;
9. three consecutive failures show Live data may be stale without clearing the grid;
10. restoring Streamer.bot availability clears the stale indicator and refreshes authoritative state automatically;
11. closing Command Manager stops polling cleanly, and reopening starts with a fresh initial read.

## Milestone 8.1 manual gate

Build and install version 0.6.1 using the same designated Streamer.bot test copy used for Milestone 8. Do not use a production installation.

Manually confirm that:

1. clicking Name sorts commands ascending and shows an upward sort marker;
2. clicking Name again sorts descending and reverses the marker;
3. Aliases, Group, ID, State, and Action each sort ascending on their first click and descending on their second click;
4. State ascending groups Disabled before Enabled, while Action ascending groups Disable before Enable;
5. the chosen column and direction remain active after changing Search, State, and Group filters;
6. the chosen column and direction remain active after Refresh and after at least one live synchronization poll;
7. a safe individual state mutation updates the row and places it correctly when sorting by State or Action;
8. the selected command and scroll context remain stable when filtered, refreshed, synchronized, or mutated;
9. summary totals and the visible-result count do not change merely because the order changes;
10. enable/disable buttons still operate on the command shown in their row after every sort direction.
11. after an individual enable or disable completes, the mouse wheel immediately scrolls the command grid without first clicking the grid again.

### Manual failure and recovery diagnostic

The live Streamer.bot API cannot be made to fail safely while its in-process Command Manager window remains open. After building, launch the manual diagnostic yourself with:

```powershell
.\qa\CommandManager.Harness\bin\Release\CreatorForge.CommandManager.Harness.exe --manual-sync
```

This opens Command Manager with fake commands and a separate diagnostic control window. It does not connect to or change Streamer.bot. Set the interval to 5 seconds, select Force query failures, and wait for three failed polls. Confirm that the six-command grid remains visible and Live data may be stale appears. Select Restore successful queries and confirm that the next poll clears the warning and restores the normal synchronized state. Close Command Manager to end the diagnostic.

## Milestone 9 manual gate

Do not use a production Streamer.bot installation. Validate the exact version 0.7.0 DLL that will be packaged.

### Accessibility and diagnostics

1. at the minimum main-window size, confirm State and every other filter, button, summary, grid, status, and live-sync control remain fully visible;
2. at the minimum Diagnostics size, confirm its privacy notice, report area, vertical scrollbar, Copy diagnostics button, and Close button remain fully visible;
3. confirm both windows remain usable at 100%, 125%, and 150% Windows display scaling with no clipped primary controls;
4. confirm Tab follows Search, State, Group, Diagnostics, Refresh, command grid, bulk controls, Live sync, and interval selection; select a specific group so all four bulk buttons are enabled;
5. confirm Tab enters and leaves the command grid as one control while arrow keys navigate rows and columns;
6. with Live sync enabled at 5 seconds, continue tabbing across several poll boundaries and confirm focus never jumps to another control;
7. confirm Ctrl+F focuses and selects Search, Ctrl+D opens Diagnostics, F5 refreshes, and Escape closes Diagnostics;
8. confirm visible focus indicators and readable text remain present in the default dark theme;
9. confirm Diagnostics reports version, runtime, operating system, counts, visible rows, filter/sort metadata, and synchronization health;
10. confirm Diagnostics contains no command names, aliases, groups, IDs, or search text;
11. confirm Copy diagnostics places the exact displayed report on the clipboard;
12. confirm the Diagnostics header and title bar display the Creator Forge logo without clipping.

### Upgrade and regression

1. upgrade the designated Streamer.bot test copy from the backed-up 0.6.1 DLL to 0.7.0 while Streamer.bot is closed;
2. confirm the expected 67 / 61 / 6 baseline and no launcher errors;
3. repeat discovery, individual mutation and restoration, one small confirmed bulk mutation and restoration, filtering, sorting, Refresh, one live poll, duplicate-open focus, close/reopen, and stale-data recovery;
4. roll back to the backed-up 0.6.1 DLL while Streamer.bot is closed and confirm it opens with the baseline;
5. reinstall the exact 0.7.0 DLL intended for packaging and confirm its hash.

### Clean-host installation

1. use a separate clean or disposable Streamer.bot configuration, not the 67-command test copy;
2. install the DLL and import only the packaged launcher Action;
3. confirm the manager opens, reports that clean configuration's authoritative totals, and remains responsive;
4. create one temporary command manually in Streamer.bot and confirm it appears after live synchronization;
5. pass enable, disable, filtering, sorting, Refresh, close/reopen, and command-removal synchronization against that temporary command;
6. remove the temporary command and temporary Core -> Test trigger before closing the clean configuration.

### Release package

1. export only the clean launcher Action as described in `docs\RELEASE.md`;
2. run `package.ps1` manually and retain its complete output;
3. confirm the ZIP contains exactly the DLL, Streamer.bot import, README, installation guide, changelog, and checksum file;
4. confirm no build caches, source files, local paths, logs, or user command data are present;
5. extract the ZIP and verify every checksum;
6. install from the extracted package for the final clean-host check.
