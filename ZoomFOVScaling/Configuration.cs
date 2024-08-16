using BepInEx.Configuration;
using BepInEx;
using System.IO;
using GTFO.API.Utilities;
using System;

namespace ZoomFOVScaling
{
    internal static class Configuration
    {
        private readonly static ConfigEntry<float> _baseFOV;
        public static float BaseFOV => _baseFOV.Value;
        private readonly static ConfigEntry<bool> _fovAffectWeaponZoom;
        public static bool FOVAffectWeaponZoom => _fovAffectWeaponZoom.Value;
        private readonly static ConfigEntry<bool> _fovAffectWeaponHip;
        public static bool FOVAffectWeaponHip => _fovAffectWeaponHip.Value;

        private readonly static ConfigEntry<bool> _useSensitivityScaling;
        public static bool UseSensitivityScaling => _useSensitivityScaling.Value;
        private readonly static ConfigEntry<bool> _useCustomSensitivityCurve;
        public static bool UseCustomSensitivityCurve => _useCustomSensitivityCurve.Value;
        private readonly static ConfigEntry<float> _sensitivityCurveFOVMin;
        public static float SensitivityCurveFOVMin => _sensitivityCurveFOVMin.Value;
        private readonly static ConfigEntry<float> _sensitivityCurveFOVMax;
        public static float SensitivityCurveFOVMax => _sensitivityCurveFOVMax.Value;
        private readonly static ConfigEntry<float> _sensitivityCurveSensMin;
        public static float SensitivityCurveSensMin => _sensitivityCurveSensMin.Value;
        private readonly static ConfigEntry<float> _sensitivityCurveSensMax;
        public static float SensitivityCurveSensMax => _sensitivityCurveSensMax.Value;

        private readonly static ConfigFile configFile;

        static Configuration()
        {
            configFile = new ConfigFile(Path.Combine(Paths.ConfigPath, EntryPoint.MODNAME + ".cfg"), saveOnInit: true);
            string section = "Field of View Settings";
            _baseFOV = configFile.Bind(section, "Base Field of View", 70f, "The field of view considered as 1x zoom.\nFor vanilla behavior, set this to your field of view.");
            _fovAffectWeaponZoom = configFile.Bind(section, "Zoom FOV Affects Weapons", false, "Field of view modifies weapon model field of view while aiming.");
            _fovAffectWeaponHip = configFile.Bind(section, "Hip FOV Affects Weapons", false, "Field of view modifies weapon model field of view while not aiming.");

            section = "Sensitivity Settings";
            _useSensitivityScaling = configFile.Bind(section, "Use Sensitivity Scaling", false, "Overrides the base aim sensitivity curve and scales sensitivity by field of view instead.\nIf enabled, recommended to raise Mouse Aim Mode Sensitivity Scaling in-game.");
            _useCustomSensitivityCurve = configFile.Bind(section, "Use Custom Sensitivity Curve", false, "Changes the base aim sensitivity curve to use the values below.\nThe curve scales linearly between minimum and maximum values.\nSensitivity Scaling overrides this.");
            _sensitivityCurveFOVMin = configFile.Bind(section, "Sensitivity Curve FOV Min", 10f, "The field of view at or below which minimum sensitivity applies.");
            _sensitivityCurveFOVMax = configFile.Bind(section, "Sensitivity Curve FOV Max", 40f, "The field of view at or above which maximum sensitivity applies.");
            _sensitivityCurveSensMin = configFile.Bind(section, "Sensitivity Curve Scalar Min", 0.2f, "The scalar applied to sensitivity when field of view is at the minimum value.");
            _sensitivityCurveSensMax = configFile.Bind(section, "Sensitivity Curve Scalar Max", 1f, "The scalar applied to sensitivity when field of view is at the maximum value.");

            BoundValues();
        }

        internal static void Init()
        {
            LiveEditListener listener = LiveEdit.CreateListener(Paths.ConfigPath, EntryPoint.MODNAME + ".cfg", false);
            listener.FileChanged += OnFileChanged;
        }

        private static void OnFileChanged(LiveEditEventArgs _)
        {
            configFile.Reload();
            BoundValues();
        }

        private static void BoundValues()
        {
            _baseFOV.Value = Math.Max(_baseFOV.Value, 1f);
            if (_sensitivityCurveFOVMax.Value < _sensitivityCurveFOVMin.Value)
                (_sensitivityCurveFOVMax.Value, _sensitivityCurveFOVMin.Value) = (_sensitivityCurveFOVMin.Value, _sensitivityCurveFOVMax.Value);
        }
    }
}
