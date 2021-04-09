using UnityEngine;

namespace sc.terrain.proceduralpainter
{
    [ExecuteInEditMode]
    public class TerrainChangeListener : MonoBehaviour
    {
        public static TerrainPainter Current;
        
        [HideInInspector]
        public Terrain terrain;

        void OnTerrainChanged(TerrainChangedFlags flags)
        {
            if (!terrain || !Current) return;

            if ((flags & TerrainChangedFlags.Heightmap) != 0)
            {
                if(Current.autoRepaint) Current.RepaintTerrain(terrain);
            }
        }
    }
}