﻿using MelonLoader;
using NAK.DesktopVRSwitch.Properties;
using System.Reflection;

[assembly: AssemblyVersion(AssemblyInfoParams.Version)]
[assembly: AssemblyFileVersion(AssemblyInfoParams.Version)]
[assembly: AssemblyInformationalVersion(AssemblyInfoParams.Version)]
[assembly: AssemblyTitle(nameof(NAK.DesktopVRSwitch))]
[assembly: AssemblyCompany(AssemblyInfoParams.Author)]
[assembly: AssemblyProduct(nameof(NAK.DesktopVRSwitch))]

[assembly: MelonInfo(
    typeof(NAK.DesktopVRSwitch.DesktopVRSwitch),
    nameof(NAK.DesktopVRSwitch),
    AssemblyInfoParams.Version,
    AssemblyInfoParams.Author,
    downloadLink: "https://github.com/NotAKidOnSteam/NAK_CVR_Mods/tree/main/DesktopVRSwitch"
)]

[assembly: MelonGame("Alpha Blend Interactive", "ChilloutVR")]
[assembly: MelonPlatform(MelonPlatformAttribute.CompatiblePlatforms.WINDOWS_X64)]
[assembly: MelonPlatformDomain(MelonPlatformDomainAttribute.CompatibleDomains.MONO)]
[assembly: MelonColor(ConsoleColor.DarkCyan)]
[assembly: HarmonyDontPatchAll]

namespace NAK.DesktopVRSwitch.Properties;
internal static class AssemblyInfoParams
{
    public const string Version = "4.3.6";
    public const string Author = "NotAKidoS";
}