using ShapesXr.Import.Core;
using UnityEngine;

namespace ShapesXr
{
    [CreateAssetMenu(fileName = "ImportSettings", menuName = "ShapesXR/Import Settings")]
    public class ImportSettings : ScriptableObject, IImportSettings
    {
        [SerializeField] private string _importedDataDirectory = "ImportedSpaces";
        [SerializeField] private MaterialImportMode _materialMode = MaterialImportMode.CombineSimilar;
        [SerializeField] private MaterialMapper _materialMapper;
        
        private static ImportSettings _instance;
        public static ImportSettings Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = Resources.Load<ImportSettings>("ImportSettings");
                }

                return _instance;
            }
        }
        
        public string ImportedDataDirectory => Instance._importedDataDirectory;
        public MaterialImportMode MaterialMode => Instance._materialMode;
        public IMaterialMapper MaterialMapper => Instance._materialMapper;
    }
}