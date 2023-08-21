using System.IO;
using System.Net;
using MessagePack;
using ShapesXr.Import.Core;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ShapesXr
{
    public static class SpaceImporter
    {
        private static bool _messagePackInitialized;

        public static void ImportSpace(string accessCode)
        {
            if (!CheckSettingsValidness())
            {
                return;
            }
            
            string spaceUrl = GetSpaceUrl(accessCode);

            if (string.IsNullOrEmpty(spaceUrl))
            {
                Debug.LogError("Error importing space");
                return;
            }
            
            var spaceInfoData = GetSpaceInfoDataStream(spaceUrl);

            if (spaceInfoData == null)
            {
                Debug.LogError($"Error downloading space data");
            }

            if (!_messagePackInitialized)
            {
                Utils.InitializeMessagePack();
                _messagePackInitialized = true;
            }

            var spaceInfo = MessagePackSerializer.Deserialize<SpaceInfo>(spaceInfoData);
            
            var spaceDescriptor = CreateSpaceDescriptorObject(accessCode, out var descriptorObject);

            EditsExecutor.ExecuteAll(spaceDescriptor, spaceInfo.Edits);

            spaceDescriptor.ReadObjectPresets();

            string spaceDataPath = PathUtils.GetSpaceDataPath(accessCode);
            
            
            if (Directory.Exists(spaceDataPath))
            {
                Directory.Delete(spaceDataPath, true);
            }
            
            string metaPath = spaceDataPath + ".meta";

            if (File.Exists(metaPath))
            {
                File.Delete(metaPath);
            }

            AssetDatabase.Refresh();

            MaterialCollection.Refresh(ImportSettings.Instance);
            
            ResourceDownloader.DownloadAllResources(spaceDescriptor);

            var objectSpawner = new ObjectSpawner(spaceDescriptor);
            objectSpawner.SpawnAllObjects();

            Selection.activeObject = descriptorObject;
        }

        private static bool CheckSettingsValidness()
        {
            if (PathUtils.PathContainsInvalidCharacters(ImportSettingsProvider.ImportSettings.ImportedDataDirectory))
            {
                Debug.LogError($"Incorrect Imported Data Directory path: {ImportSettingsProvider.ImportSettings.ImportedDataDirectory}");
                return false;
            }

            if (ImportSettingsProvider.ImportSettings.MaterialMapper == null)
            {
                Debug.LogError("Material Mapper not found. Please specify one in ShapesXR Importer settings");
                return false;
            }
            
            return true;
        }
        
        private static string GetSpaceUrl(string spaceCode)
        {
#if SHAPES_XR_DEV
            string requestUrl = $"https://api-dev.shapes.app/accesscode/space-url/{spaceCode}";
#else
            string requestUrl = $"https://api.shapes.app/accesscode/space-url/{spaceCode}";
#endif

            var webRequest = (HttpWebRequest)WebRequest.Create(requestUrl);
            webRequest.Method = "GET";
            webRequest.Timeout = NetUtils.MinRequestTimeout;
            
            try
            {
                var response = (HttpWebResponse)webRequest.GetResponse();
                var reader = new StreamReader(response.GetResponseStream());

                return reader.ReadToEnd().Trim('"');
            }
            catch (WebException e)
            {
                Debug.LogError($"Error getting space Url: {e.Message}");
                return null;
            }
        }

        private static Stream GetSpaceInfoDataStream(string spaceUrl)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(spaceUrl);
            webRequest.Method = "GET";
            webRequest.Timeout = NetUtils.MinRequestTimeout;
            
            try
            {
                var response = (HttpWebResponse)webRequest.GetResponse();

                if (response.ContentLength == 0)
                {
                    return null;
                }
                
                return response.GetResponseStream();

            }
            catch (WebException e)
            {
                Debug.LogError($"Error getting space info: {e.Message}");
                return null;
            }
        }
        
        private static ISpaceDescriptor CreateSpaceDescriptorObject(string accessCode, out GameObject descriptorObject)
        {
            descriptorObject = Object.Instantiate(ImportResources.SpaceDescriptorPrefab);
            descriptorObject.name = $"Space {accessCode.Insert(3, " ")}";

            var spaceDescriptor = descriptorObject.GetComponent<ISpaceDescriptor>();
            spaceDescriptor.AccessCode = accessCode;
            
            Properties.InitProperties(spaceDescriptor.PropertyHub);

            return spaceDescriptor;
        }
    }
}