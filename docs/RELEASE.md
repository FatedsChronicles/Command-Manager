# Release procedure

This procedure creates the allowlisted release archive described in the implementation plan. Do not package a launcher Action until its normal live-host workflow has passed and all temporary test triggers and diagnostic sub-actions have been removed.

## 1. Prepare the Streamer.bot Action export

In the designated Streamer.bot test copy:

1. confirm `CF Command Manager - Open` contains the single normal launcher Execute C# Code sub-action;
2. confirm the Action has no temporary Core -> Test trigger;
3. open Streamer.bot's Export dialog;
4. add only the `CF Command Manager - Open` Action to the export;
5. verify that no unrelated Actions, commands, triggers, queues, or user data are selected;
6. export the result to `release\Command_Manager.cf` in this repository;
7. close Streamer.bot before continuing.

Treat the export string as opaque application data. Do not edit or attempt to generate it manually.

## 2. Create the package

From the repository root, run:

```powershell
.\package.ps1
```

The packaging script performs the Release build unless `-SkipBuild` is explicitly supplied. It reads the DLL version, copies only the approved release files, generates `SHA256SUMS.txt`, creates the ZIP, checks its exact entry list, rejects build caches and local-path documentation, and writes a checksum beside the archive.

Outputs are written under `artifacts`:

```text
CreatorForge-CommandManager-x.y.z.zip
CreatorForge-CommandManager-x.y.z.zip.sha256
```

## 3. Inspect and validate

Before distribution:

1. inspect the ZIP and confirm it contains exactly the six documented files;
2. extract it into a new temporary folder;
3. verify every line in `SHA256SUMS.txt` against the extracted file;
4. perform the clean-install gate from `docs\TESTING.md` on a separate Streamer.bot configuration;
5. perform the upgrade and rollback gates on the designated test copy;
6. do not distribute the archive until the exact packaged DLL passes those gates.
