# Pharaoh: A New Era - Personal Mods

This repository contains my personal mods for Pharaoh: A New Era, built using the modding framework in the main repository.

## Structure
 
Each subdirectory is an individual mod project with its own .csproj file. These mods are automatically included in the solution when the main repository is loaded.

## Creating a New Mod

1. Create a new folder in this repository with your mod name
2. Add a minimal .csproj file (see template below)
3. Create a `Plugin.cs` file with your mod code

### Example .csproj Template:
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <AssemblyName>YourModName</AssemblyName>
    <BepInExPluginName>Your Mod Display Name With Spaces</BepInExPluginName>
    <Description>Description of your mod</Description>
    <Version>1.0.0</Version>
    <RootNamespace>YourModName</RootNamespace>
  </PropertyGroup>
</Project>
```

## Building

All mods are automatically included in the build process of the main repository. No additional steps are needed to build them.
