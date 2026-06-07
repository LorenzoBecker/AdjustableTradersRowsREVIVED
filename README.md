# Adjustable Traders Rows REVIVED

Compatibility update of Adjustable Trader Rows for SPT 4.x.

Original mod by JustNU.

## What it does

This is a small client-side BepInEx plugin for SPT.

It changes the trader selection screen layout so the number of trader cards per row can be configured through the BepInEx config file.

It does not modify trader data, trader assortments, quests, prices, profiles, or server-side logic.

## Installation

Place the compiled DLL in:

BepInEx/plugins/

## Configuration

After launching the game once, a BepInEx config file is generated.

The main setting is:

Trader Columns = 8

Change this value to control how many trader cards are displayed per row.

## Build requirements

This project requires local references to the current SPT/EFT/BepInEx assemblies.

Required DLLs must be placed locally in a `lib/` folder before building.

These DLLs are not included in this repository because they are not owned by this project.

## Credits

Original mod by JustNU.

SPT 4.x compatibility update by LorenzoBecker.
