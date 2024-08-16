using FirstPersonItem;
using HarmonyLib;
using ZoomFOVScaling.Utils;

namespace ZoomFOVScaling.Patches
{
    [HarmonyPatch]
    internal static class SensitivityPatch
    {
        [HarmonyPatch(typeof(FPIS_Aim), nameof(FPIS_Aim.Enter))]
        [HarmonyWrapSafe]
        [HarmonyPostfix]
        private static void Post_AimEnter(FPIS_Aim __instance)
        {
            if (Configuration.UseSensitivityScaling)
            {
                __instance.Holder.MouseLookSpeed.Target = __instance.Holder.LookCamFov.Target 
                    / CellSettingsManager.SettingsData.Video.Fov.Value
                    * CellSettingsManager.SettingsData.Gameplay.LookSpeedAiming.Value;
            }
            else if (Configuration.UseCustomSensitivityCurve)
            {
                __instance.Holder.MouseLookSpeed.Target = __instance.Holder.LookCamFov.Target.Map(
                        Configuration.SensitivityCurveFOVMin,
                        Configuration.SensitivityCurveFOVMax,
                        Configuration.SensitivityCurveSensMin,
                        Configuration.SensitivityCurveSensMax
                    ) * CellSettingsManager.SettingsData.Gameplay.LookSpeedAiming.Value;
            }
        }
    }
}
