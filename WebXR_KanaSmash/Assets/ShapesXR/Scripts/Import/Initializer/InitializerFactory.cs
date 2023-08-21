using System.Collections.Generic;
using System.Linq;
using ShapesXr.Import.Core;
using UnityEngine;

namespace ShapesXr
{
    public static class InitializerFactory
    {
        private static readonly SizeInitializer SizeInitializer = new SizeInitializer();
        private static readonly TextInitializer TextInitializer = new TextInitializer();
        private static readonly ImageInitializer ImageInitializer = new ImageInitializer();
        private static readonly ModelInitializer ModelInitializer = new ModelInitializer();
        
        public static IInitializer GetInitializer(PropertyReactorComponent reactor, BasePreset preset)
        {
            return reactor switch
            {
                CharacterReactorComponent r => GetCharacterInitializer(preset),
                SizePropertyReactor r => SizeInitializer,
                TextReactorComponent r => TextInitializer,
                StrokePropertyReactor r => GetStrokeInitializer(preset),
                ImageReactor r => ImageInitializer,
                ModelReactor r => ModelInitializer,

                BaseMaterialReactor r => GetMaterialInitializer(r),
                
                _ => new NullInitializer(reactor.ToString())
            };
        }

        private static IInitializer GetCharacterInitializer(BasePreset preset)
        {
            if (preset is CharacterPreset characterPreset)
            {
                return new CharacterInitializer(characterPreset.Pose);
            }

            return new NullInitializer(preset.name);
        }

        private static IInitializer GetStrokeInitializer(BasePreset preset)
        {
            return preset switch
            {
                BaseBrushPreset p => new StrokeInitializer(p.GetParameters()),
                _ => new NullInitializer(preset.ToString())
            };
        }

        private static IInitializer GetMaterialInitializer(BaseMaterialReactor reactor)
        {
            List<MeshRenderer> renderers;
            
            var materialAssigner = reactor.GetComponent<MaterialAssigner>();

            if (materialAssigner)
            {
                renderers = new List<MeshRenderer>();

                foreach (var group in materialAssigner.Groups)
                {
                    foreach (var renderer in group.Renderers)
                    {
                        if(renderer is MeshRenderer meshRenderer)
                        {
                            renderers.Add(meshRenderer);
                        }
                    }
                }
            }
            else
            {
                renderers = reactor.GetComponentsInChildren<MeshRenderer>().ToList();
            }

            return reactor switch
            {
                MaterialReactor r => new MaterialInitializer(renderers),
                ImageMaterialReactor r => new ImageMaterialInitializer(renderers),
                ModelMaterialReactor r => new ModelMaterialInitializer(renderers),

                _ => new NullInitializer(reactor.ToString())
            };
        }
    }
}