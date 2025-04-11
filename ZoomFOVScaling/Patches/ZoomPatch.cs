using HarmonyLib;

namespace ZoomFOVScaling.Patches
{
    [HarmonyPatch(typeof(ItemEquippable))]
    internal static class ZoomPatch
    {
        [HarmonyPatch(nameof(ItemEquippable.GetWorldCameraZoomFov))]
        [HarmonyPostfix]
        private static void Post_GetZoomFOV(ref float __result)
        {
            __result = __result / Configuration.BaseFOV * CellSettingsManager.SettingsData.Video.Fov.Value;
        }

        [HarmonyPatch(nameof(ItemEquippable.GetItemFovZoom))]
        [HarmonyPostfix]
        private static void Post_GetItemZoomFOV(ItemEquippable __instance, ref float __result)
        {
            if (__instance.TryCast<Gear.BulletWeapon>() == null) return;

            if (Configuration.FOVAffectWeaponZoom)
                __result = __result / Configuration.BaseFOV * CellSettingsManager.SettingsData.Video.Fov.Value;
        }

        [HarmonyPatch(nameof(ItemEquippable.GetItemFovNormal))]
        [HarmonyPostfix]
        private static void Post_GetItemFOV(ItemEquippable __instance, ref float __result)
        {
            if (__instance.TryCast<Gear.BulletWeapon>() == null) return;

            if (Configuration.FOVAffectWeaponHip)
                __result = __result / Configuration.BaseFOV * CellSettingsManager.SettingsData.Video.Fov.Value;
        }
    }
}
