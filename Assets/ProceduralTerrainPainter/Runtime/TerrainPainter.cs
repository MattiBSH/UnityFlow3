// Procedural Terrain Painter by Staggart Creations http://staggart.xyz
// Copyright protected under Unity Asset Store EULA

using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if __MICROSPLAT__
using JBooth.MicroSplat;
#endif

namespace sc.terrain.proceduralpainter
{
    [ExecuteInEditMode]
    public class TerrainPainter : MonoBehaviour
    {
        public const string Version = "1.0.1";

        public Terrain[] terrains;
        [Attributes.ResolutionDropdown(64, 1024)] 
        public int splatmapResolution = 256;
        [Attributes.ResolutionDropdown(16, 2048)]
        [Tooltip("The color/base map is a pre-rendered texture for the terrain color. This is shown on the terrain in the distance. High resolutions usually have little benefit")]
        public int colorMapResolution = 256;

        public List<LayerSettings> layerSettings = new List<LayerSettings>();

        /// <summary>
        /// Repaints a terrain if its heightmap is modified. The AddOrRemoveTerrainListeners must be used to set up for the terrains. In the editor, the inspector automatically handles this
        /// </summary>
        [Tooltip("Automatically repaint the terrains if their heightmap is modified. Repaints when the left-mouse button is released")]
        public bool autoRepaint;
        public List<TerrainChangeListener> terrainListeners = new List<TerrainChangeListener>();
        public Bounds bounds;
        
#if VEGETATION_STUDIO_PRO
        [Tooltip("Refreshes the vegetation systems after painting. If vegetation items use terrain layers masks, this is useful")]
        public bool refreshVegetationOnPaint;
#endif
        #if __MICROSPLAT__
        [Tooltip("Assign the TextureArrayConfig asset here. Adding, removing or re-ordering layers will be also be applied to the texture array")]
        public TextureArrayConfig msTexArray;
        #endif
        
        [SerializeField]
        //Reference it once here, so it gets included in a build
        private Shader filterShader;
        
        private void OnEnable()
        {
            if (!filterShader) filterShader = Shader.Find("Hidden/TerrainPainter/Modifier");

            TerrainChangeListener.Current = this;
        }

        /// <summary>
        /// Applies the splatmapResolution value to all terrains. This must be called when changing the resolution before repainting a single terrain. Automatically done in the <see cref="RepaintAll"/> function
        /// </summary>
        public void ResizeSplatmaps()
        {
            //Needs to happen before repainting, all terrains must have the same splatmap resolution. PaintContext throws warnings otherwise
            
            foreach (Terrain terrain in terrains)
            {
                if(terrain) terrain.terrainData.alphamapResolution = splatmapResolution;
            }
        }
        
        public void RecalculateBounds()
        {
            bounds = Utilities.RecalculateBounds(terrains);
        }

        [ContextMenu("Assign active terrains")]
        public void AssignActiveTerrains()
        {
            SetTargetTerrains(Terrain.activeTerrains);
            RepaintAll();
        }

        public void SetTargetTerrains(Terrain[] terrains)
        {
            this.terrains = terrains;
            RecalculateBounds();
        }

        /// <summary>
        /// Repaints all the assigned terrains using the current configuration
        /// </summary>
        public void RepaintAll()
        {
            if (layerSettings.Count == 0) return;

            ResizeSplatmaps();

            foreach (Terrain terrain in terrains)
            {
                if (!terrain)
                {
                    Debug.LogError("Missing terrain assigned to TerrainPainter", this);
                    continue;
                }
                
                RepaintTerrain(terrain);
            }
        }

        /// <summary>
        /// Repaints an individual terrain
        /// </summary>
        /// <param name="terrain"></param>
        public void RepaintTerrain(Terrain terrain)
        {
            if (layerSettings.Count == 0 || terrain == null) return;
            
            ModifierStack.Configure(terrain, bounds, splatmapResolution);
            
            for (int i = layerSettings.Count-1; i >= 0; i--)
            {
                //Disabled or no settings, paint fill with black
                if (!layerSettings[i].enabled && i < layerSettings.Count-1)
                {
                    continue;
                }

                ModifierStack.Execute(terrain, layerSettings[i].layer, layerSettings[i].modifierStack);
            }
                
            //Regenerate basemap
            terrain.terrainData.baseMapResolution = colorMapResolution;
            terrain.terrainData.SetBaseMapDirty();

            RefreshVegetation(terrain);
            
            terrain.Flush();

#if UNITY_EDITOR
            EditorUtility.SetDirty(terrain.terrainData);
#endif
        }

        private void RefreshVegetation(Terrain terrain)
        {
#if VEGETATION_STUDIO_PRO
            AwesomeTechnologies.VegetationStudio.VegetationStudioManager manager = AwesomeTechnologies.VegetationStudio.VegetationStudioManager.Instance;

            if (refreshVegetationOnPaint && manager)
            {
#if UNITY_2019_1_OR_NEWER
                //VS requires the CPU data
                terrain.terrainData.SyncTexture(TerrainData.AlphamapTextureName);
#endif
                foreach (AwesomeTechnologies.VegetationSystem.VegetationSystemPro sys in manager.VegetationSystemList)
                {
                    if (sys == null) continue;
                    
                    sys.ClearCache();
                    sys.RefreshTerrainArea(terrain.terrainData.bounds);
                }
#if UNITY_EDITOR
                SceneView.RepaintAll();
#endif
            }
#endif
        }

        public void CreateSettingsForLayer(TerrainLayer layer)
        {
            if (layer == null) return;
            
            LayerSettings s = new LayerSettings();
            s.layer = layer;
            s.modifierStack = new List<Modifier>();
            
            layerSettings.Insert(0, s);

            SetTerrainLayers();
        }

        /// <summary>
        /// Adds or removes the TerrainChangeListener component from all assigned terrains. If <see cref="autoRepaint"/> is enabled, terrains will be repainted when height is modified
        /// </summary>
        /// <param name="value"></param>
        public void AddOrRemoveTerrainListeners(bool value)
        {
            if (value)
            {
                RemoveTerrainListeners();

                foreach (Terrain terrain in terrains)
                {
                    TerrainChangeListener listener = terrain.GetComponent<TerrainChangeListener>();
                    if (!listener) listener = terrain.gameObject.AddComponent<TerrainChangeListener>();

                    listener.terrain = terrain;
                    this.terrainListeners.Add(listener);
                }
            }
            else
            {
                RemoveTerrainListeners();
            }
        }

        private void RemoveTerrainListeners()
        {
            for (int i = 0; i < terrainListeners.Count; i++)
            {
                DestroyImmediate(terrainListeners[i]);
            }
            
            terrainListeners.Clear();
        }
        
        /// <summary>
        /// Ensures that all configured layers are in fact assigned to the terrains. Also removed if they were.
        /// </summary>
        [ContextMenu("Set terrain layers")]
        public void SetTerrainLayers()
        {
            TerrainLayer[] layers = Utilities.SettingsToLayers(layerSettings);
            
            foreach (Terrain terrain in terrains)
            {
                terrain.terrainData.terrainLayers = layers;
                terrain.terrainData.SetBaseMapDirty();
                
#if UNITY_EDITOR
                EditorUtility.SetDirty(terrain.terrainData);
#endif
            }
        }
    }
}