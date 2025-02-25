﻿using ABI_RC.Core;
using ABI_RC.Core.EventSystem;
using ABI_RC.Core.InteractionSystem;
using ABI_RC.Core.Player;
using ABI_RC.Core.Savior;
using ABI_RC.Core.UI;
using ABI_RC.Systems.Camera;
using UnityEngine;

namespace NAK.DesktopVRSwitch;

internal class TryCatchHell
{
    internal static void TryCatchWrapper(Action action, string errorMsg, params object[] msgArgs)
    {
        try
        {
            action();
        }
        catch (Exception ex)
        {
            DesktopVRSwitch.Logger.Error(string.Format(errorMsg, msgArgs));
            DesktopVRSwitch.Logger.Msg(ex.Message);
        }
    }

    internal static void CloseCohtmlMenus()
    {
        TryCatchWrapper(() =>
        {
            DesktopVRSwitch.Logger.Msg("Closing ViewManager & CVR_MenuManager menus.");
            ViewManager.Instance.UiStateToggle(false);
            CVR_MenuManager.Instance.ToggleQuickMenu(false);
        },
        "Setting CheckVR hasVrDeviceLoaded failed.");
    }

    internal static void SetCheckVR(bool enableVR)
    {
        TryCatchWrapper(() =>
        {
            DesktopVRSwitch.Logger.Msg($"Setting CheckVR hasVrDeviceLoaded to {enableVR}.");
            CheckVR.Instance.hasVrDeviceLoaded = enableVR;
        },
        "Setting CheckVR hasVrDeviceLoaded failed.");
    }

    internal static void SetMetaPort(bool enableVR)
    {
        TryCatchWrapper(() =>
        {
            DesktopVRSwitch.Logger.Msg($"Setting MetaPort isUsingVr to {enableVR}.");
            MetaPort.Instance.isUsingVr = enableVR;
        },
        "Setting MetaPort isUsingVr failed.");
    }

    internal static void RepositionCohtmlHud(bool enableVR)
    {
        TryCatchWrapper(() =>
        {
            DesktopVRSwitch.Logger.Msg("Configuring new hud affinity for CohtmlHud.");
            CohtmlHud.Instance.gameObject.transform.parent = enableVR ? PlayerSetup.Instance.vrCamera.transform : PlayerSetup.Instance.desktopCamera.transform;
            CVRTools.ConfigureHudAffinity();
            CohtmlHud.Instance.gameObject.transform.localScale = new Vector3(1.2f, 1f, 1.2f);
        },
        "Error parenting CohtmlHud to active camera.");
    }

    internal static void UpdateHudOperations(bool enableVR)
    {
        TryCatchWrapper(() =>
        {
            DesktopVRSwitch.Logger.Msg("Switching HudOperations worldLoadingItem & worldLoadStatus.");
            HudOperations.Instance.worldLoadingItem = enableVR ? HudOperations.Instance.worldLoadingItemVr : HudOperations.Instance.worldLoadingItemDesktop;
            HudOperations.Instance.worldLoadStatus = enableVR ? HudOperations.Instance.worldLoadStatusVr : HudOperations.Instance.worldLoadStatusDesktop;
        },
        "Failed switching HudOperations objects.");
    }

    internal static void DisableMirrorCanvas()
    {
        TryCatchWrapper(() =>
        {
            DesktopVRSwitch.Logger.Msg("Forcing PortableCamera canvas mirroring off.");
            //tell the game we are in mirror mode so itll disable it (if enabled)
            PortableCamera.Instance.mode = MirroringMode.Mirror;
            PortableCamera.Instance.ChangeMirroring();
        },
        "Failed to disable PortableCamera canvas mirroring.");
    }

    internal static void SwitchActiveCameraRigs(bool enableVR)
    {
        TryCatchWrapper(() =>
        {
            DesktopVRSwitch.Logger.Msg("Switching active PlayerSetup camera rigs. Updating Desktop camera FOV.");
            PlayerSetup.Instance.desktopCameraRig.SetActive(!enableVR);
            PlayerSetup.Instance.vrCameraRig.SetActive(enableVR);
            CVR_DesktopCameraController.UpdateFov();
            //uicamera has script that copies fov from desktop cam
            //toggling the cameras on/off resets aspect ratio
            //so when rigs switch, that is already handled
        },
        "Failed to switch active camera rigs or update Desktop camera FOV.");
    }

    internal static void PauseInputInteractions(bool toggle)
    {
        TryCatchWrapper(() =>
        {
            DesktopVRSwitch.Logger.Msg($"Setting CVRInputManager inputEnabled & CVR_InteractableManager enableInteractions to {!toggle}");
            CVRInputManager.Instance.inputEnabled = !toggle;
            CVR_InteractableManager.enableInteractions = !toggle;
        },
        "Failed to toggle CVRInputManager inputEnabled & CVR_InteractableManager enableInteractions.");
    }

    internal static void ResetCVRInputManager()
    {
        TryCatchWrapper(() =>
        {
            DesktopVRSwitch.Logger.Msg("Resetting CVRInputManager inputs.");
            //just in case
            CVRInputManager.Instance.blockedByUi = false;
            //sometimes head can get stuck, so just in case
            CVRInputManager.Instance.independentHeadToggle = false;
            //just nice to load into desktop with idle gesture
            CVRInputManager.Instance.gestureLeft = 0f;
            CVRInputManager.Instance.gestureLeftRaw = 0f;
            CVRInputManager.Instance.gestureRight = 0f;
            CVRInputManager.Instance.gestureRightRaw = 0f;
            //turn off finger tracking input
            CVRInputManager.Instance.individualFingerTracking = false;
        },
        "Failed to reset CVRInputManager inputs.");
    }

    internal static void ReloadLocalAvatar()
    {
        TryCatchWrapper(() =>
        {
            DesktopVRSwitch.Logger.Msg("Attempting to reload current local avatar from GUID.");
            AssetManagement.Instance.LoadLocalAvatar(MetaPort.Instance.currentAvatarGuid);
        },
        "Failed to reload local avatar.");
    }

    internal static void UpdateRichPresence()
    {
        TryCatchWrapper(() =>
        {
            if (MetaPort.Instance.settings.GetSettingsBool("ImplementationRichPresenceDiscordEnabled", true))
            {
                DesktopVRSwitch.Logger.Msg("Forcing Discord Rich Presence update.");
                MetaPort.Instance.settings.SetSettingsBool("ImplementationRichPresenceDiscordEnabled", false);
                MetaPort.Instance.settings.SetSettingsBool("ImplementationRichPresenceDiscordEnabled", true);
            }
            if (MetaPort.Instance.settings.GetSettingsBool("ImplementationRichPresenceSteamEnabled", true))
            {
                DesktopVRSwitch.Logger.Msg("Forcing Steam Rich Presence update.");
                MetaPort.Instance.settings.SetSettingsBool("ImplementationRichPresenceSteamEnabled", false);
                MetaPort.Instance.settings.SetSettingsBool("ImplementationRichPresenceSteamEnabled", true);
            }
        },
        "Failed to update Discord & Steam Rich Presence.");
    }

    internal static void UpdateGestureReconizerCam()
    {
        TryCatchWrapper(() =>
        {
            DesktopVRSwitch.Logger.Msg("Updating CVRGestureRecognizer _camera to active camera.");
            CVRGestureRecognizer.Instance._camera = PlayerSetup.Instance.GetActiveCamera().GetComponent<Camera>();
        },
        "Failed to update CVRGestureRecognizer camera.");
    }

    internal static void UpdateMenuCoreData(bool enableVR)
    {
        TryCatchWrapper(() =>
        {
            DesktopVRSwitch.Logger.Msg("Updating CVR_Menu_Data core data.");
            CVR_MenuManager.Instance.coreData.core.inVr = enableVR;
        },
        "Failed to update CVR_Menu_Data core data.");
    }
}
