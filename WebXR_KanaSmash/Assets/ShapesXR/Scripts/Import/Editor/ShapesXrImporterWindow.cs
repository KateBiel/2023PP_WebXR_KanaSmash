using ShapesXr.Import.Core;
using UnityEditor;
using UnityEngine;

namespace ShapesXr
{
    public class ShapesXrImporterWindow : EditorWindow
    {
        private const int CodeLength = 6;
        
        private static string _spaceCode = "";
        private static Texture2D _logo;

        private static bool _showSettings;

        private static SerializedObject _importSettingsObject;
        private static SerializedProperty _materialMapperProperty;
        private static SerializedProperty _importedDataDirectoryProperty;
        private static SerializedProperty _materialModeProperty;
        
        [MenuItem("ShapesXR/Importer")]
        public static void ShowWindow()
        {
            GetWindow<ShapesXrImporterWindow>(false, "ShapesXR Importer", true);
        }

        private void OnEnable()
        {
            _importSettingsObject = new SerializedObject(ImportSettings.Instance);
            
            _importedDataDirectoryProperty = _importSettingsObject.FindProperty("_importedDataDirectory");
            _materialMapperProperty = _importSettingsObject.FindProperty("_materialMapper");
            _materialModeProperty = _importSettingsObject.FindProperty("_materialMode");

            ImportSettingsProvider.ImportSettings = ImportSettings.Instance;
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();

            DrawLogo();

            EditorGUILayout.BeginHorizontal();

            _spaceCode = GUILayout.TextField(_spaceCode);

            if (GUILayout.Button("Import Space"))
            {
                var trimmedCode = _spaceCode.Trim().Replace(" ", "");

                if (string.IsNullOrEmpty(trimmedCode) || trimmedCode.Length != CodeLength)
                {
                    Debug.LogError("Cannot import space: Incorrect code entered");
                }
                else
                {
                    SpaceImporter.ImportSpace(_spaceCode.Replace(" ", "")); 
                }
            }

            EditorGUILayout.EndHorizontal();

            _showSettings = EditorGUILayout.Foldout(_showSettings, "Settings", EditorStyles.foldoutHeader);

            if (!_showSettings)
            {
                EditorGUILayout.EndVertical();
                return;
            }
            
            _importSettingsObject.Update();
            
            EditorGUILayout.Space();
            
            EditorGUILayout.LabelField("General", EditorStyles.boldLabel);
            
            EditorGUILayout.PropertyField(_importedDataDirectoryProperty);
            EditorGUILayout.Space();
            
            EditorGUILayout.LabelField("Material", EditorStyles.boldLabel);
            
            EditorGUILayout.PropertyField(_materialModeProperty,new GUIContent("Import Mode"));
            EditorGUILayout.PropertyField(_materialMapperProperty,new GUIContent("Mapper"));

            var rawString = _importedDataDirectoryProperty.stringValue;
            _importedDataDirectoryProperty.stringValue = rawString.Trim(' ', '\\', '/');
            
            _importSettingsObject.ApplyModifiedProperties();

            EditorGUILayout.EndVertical();
        }

        private void DrawLogo()
        {
            EditorGUILayout.BeginHorizontal();
            
            GUILayout.FlexibleSpace();
            GUILayout.Label(ImportResources.ShapesXrLogo);
            GUILayout.FlexibleSpace();
            
            EditorGUILayout.EndHorizontal();
        }
    }
}