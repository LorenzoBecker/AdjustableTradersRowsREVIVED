<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net472</TargetFramework>
    <AssemblyName>com.JustNU.AdjustableTraderRows</AssemblyName>
    <RootNamespace>AdjustableTraderRows</RootNamespace>
    <LangVersion>latest</LangVersion>
    <Nullable>disable</Nullable>
    <Optimize>true</Optimize>
  </PropertyGroup>

  <ItemGroup>
    <!--
      These DLLs must come from the user's local SPT/EFT/BepInEx installation.
      They are intentionally not included in the public repository.
    -->
    <Reference Include="BepInEx">
      <HintPath>..\lib\BepInEx.dll</HintPath>
      <Private>false</Private>
    </Reference>
    <Reference Include="UnityEngine">
      <HintPath>..\lib\UnityEngine.dll</HintPath>
      <Private>false</Private>
    </Reference>
    <Reference Include="UnityEngine.CoreModule">
      <HintPath>..\lib\UnityEngine.CoreModule.dll</HintPath>
      <Private>false</Private>
    </Reference>
    <Reference Include="UnityEngine.UI">
      <HintPath>..\lib\UnityEngine.UI.dll</HintPath>
      <Private>false</Private>
    </Reference>
  </ItemGroup>
</Project>
