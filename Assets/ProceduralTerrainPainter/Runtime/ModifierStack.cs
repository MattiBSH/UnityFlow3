// Procedural Terrain Painter by Staggart Creations http://staggart.xyz
// Copyright protected under Unity Asset Store EULA

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.TerrainAPI;

namespace sc.terrain.proceduralpainter
{
    public class ModifierStack
    {
        private static int m_resolution;
        
        private static float heightScale;
        public static Material filterMat;

        private static RenderTexture alphaMap;

        private static int HeightmapID = Shader.PropertyToID("_Heightmap");
        private static int HeightmapScaleID = Shader.PropertyToID("_HeightmapScale");
        private static int NormalMapID = Shader.PropertyToID("_NormalMap");
        private static int TerrainPosScaleID = Shader.PropertyToID("_TerrainPosScale");
        private static int TerrainBoundsID = Shader.PropertyToID("_TerrainBounds");

        /// <summary>
        /// Call this once, per terrain. Sets up the Blit material and generates a normal map
        /// </summary>
        /// <param name="terrain"></param>
        /// <param name="bounds"></param>
        /// <param name="rtDsc"></param>
        public static void Configure(Terrain terrain, Bounds bounds, int resolution)
        {
            if (m_resolution != resolution || alphaMap == null)
            {
                alphaMap = new RenderTexture(resolution, resolution, 0, RenderTextureFormat.R8);
            }
            m_resolution = resolution;
            
            if (!filterMat) filterMat = new Material(Shader.Find("Hidden/TerrainPainter/Modifier"));
            
            //Note: heightmap borders aren't blended with neighboring texels, causes visible seams at low resolutions
            filterMat.SetTexture(HeightmapID, terrain.terrainData.heightmapTexture);
            filterMat.SetTexture(NormalMapID, terrain.normalmapTexture);

            heightScale = bounds.max.y - bounds.min.y;
            filterMat.SetFloat(HeightmapScaleID, heightScale);

            //Used to reconstruct the global-bounds aligned UV
            float invWidth = 1.0f / bounds.size.x;
            float invHeight = 1.0f / bounds.size.z;
            
            var terrainPosScale = new Vector4(
                (terrain.GetPosition().x * invWidth) - (bounds.min.x * invWidth),
                (terrain.GetPosition().z * invHeight) - (bounds.min.z * invHeight),
                terrain.terrainData.size.x / bounds.size.x,
                terrain.terrainData.size.z / bounds.size.z
            );
            
            filterMat.SetVector(TerrainPosScaleID, terrainPosScale);
            filterMat.SetVector(TerrainBoundsID, new Vector4(bounds.min.x, bounds.max.z, bounds.size.x, bounds.size.z));
        }

        
        /// <summary>
        /// Executes the modifiers in order and generates the necessary splatmaps
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="modifiers"></param>
        /// <param name="output"></param>
        public static void Execute(Terrain terrain, TerrainLayer layer, List<Modifier> modifiers)
        {
            Graphics.ClearRandomWriteTargets();
            Graphics.SetRenderTarget(alphaMap);
            
            //Start with a full white base
            Graphics.Blit(Texture2D.whiteTexture, alphaMap);

            //Reverse order
            for (int i = modifiers.Count-1; i >= 0; i--)
            {
                modifiers[i].Configure(filterMat);
                modifiers[i].Execute(alphaMap);
            }

            //Scale splatmap to fit in terrain
            Vector2 scaledSplatmapSize = new Vector2((terrain.terrainData.size.x / m_resolution) * m_resolution, (terrain.terrainData.size.z / m_resolution) * m_resolution);

            //PaintContext handles creation of splatmaps. Subtracting the weights of a splatmap, from the ones before it.
            //A single pixels of all combined alpha maps must not exceed a value of one. The 2nd pass of Hidden/TerrainEngine/TerrainLayerUtils is used internally to do this
            //Note: Paintcontext must use a serialized terrain reference, otherwise breaks when executing "ApplyDelayedActions" when the project is saved!
            PaintContext c = TerrainPaintUtility.BeginPaintTexture(terrain, new Rect(0, 0, scaledSplatmapSize.x, scaledSplatmapSize.y), layer);
                    
            Graphics.Blit(alphaMap, c.destinationRenderTexture);
                    
            TerrainPaintUtility.EndPaintTexture(c, "Painted terrain");
        }
    }
}