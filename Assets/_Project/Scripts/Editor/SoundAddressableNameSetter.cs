using System;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEngine;

namespace MergeBoard.Editor
{
    public class SoundAddressableNameSetter : AssetPostprocessor
    {
        private const string SoundPath = "Assets/_Project/Res_Addr/Sound/";
        
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            if (!settings)
            {
                Debug.LogWarning("AddressableAssetSettings not found.");
                return;
            }
            var soundGroup = settings.FindGroup("Sound");
            
            foreach (var assetPath in importedAssets)
            {
                if (!assetPath.StartsWith(SoundPath))
                    continue;
                
                var guid = AssetDatabase.AssetPathToGUID(assetPath);
                var entry = settings.FindAssetEntry(guid) ?? settings.CreateOrMoveEntry(guid, soundGroup);
                
                var type = assetPath.Substring(SoundPath.Length, 3);
                var fileName = System.IO.Path.GetFileNameWithoutExtension(assetPath);
                var addrName = $"Addr/Sound/{type}/{fileName}";

                if (entry.address == addrName)
                    continue;
                
                entry.address = addrName;

                Debug.Log($"Set addressable name for {assetPath} to {addrName}");
            }
            
            AssetDatabase.SaveAssets();
        }
    }
}