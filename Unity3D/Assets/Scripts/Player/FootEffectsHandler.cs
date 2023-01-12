using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LayerManager;

public abstract class FootEffectsHandler : MonoBehaviour, IPoolUser
{
    public ObjectPooler ObjectPooler { get; set; }
    public string Tag { get; set; } = "Tracks";
    [SerializeField] protected Transform bottom;
    [SerializeField] protected Terrain terrain;
    [SerializeField] protected AudioManager audioManager;
    
    [Space]
    [Header("Sound Volume Level")]
    [SerializeField][Range(0, 1)] protected float low = 0;
    [SerializeField][Range(0, 1)] protected float middle = .5f;
    [SerializeField][Range(0, 1)] protected float high = 1;

    [SerializeField] string[] TextureIdxToSoundName;

    protected abstract void OnDisable();
    protected abstract void Step(bool isL = true);
    protected void StepR() => Step();
    protected void StepL() => Step(false);
    protected void Start()
    {
        ObjectPooler = ObjectPooler.Instance;
    }
    protected int GetGroundIdx()
    {
        int groundIdx = 0;
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 10, GetMask(Layers.Ground)))
        {
            Vector2 pos = GetPositionOnTerrain();
            float[] texturesAtPoint = GetTerrainTextures(pos);
            for (int i = 0; i < texturesAtPoint.Length; i++)
            {
                float texAmount = texturesAtPoint[i];
                if (texAmount > 0)
                {
                    groundIdx = i;
                    break;
                }
            }
        }
        return groundIdx;
    }
    protected string FindGroundType()
    {
        int groundIdx = GetGroundIdx();

        if (PlayerManager.Instance.statManager.currentZone != null)
            groundIdx = 1; // TODO handle grass

        return TextureIdxToSoundName[groundIdx];
    }
    protected Vector2 GetPositionOnTerrain()
    {
        Vector3 terrainPos = PlayerManager.Instance.transform.position - terrain.transform.position;
        Vector3 mapPosition = new Vector3(
            terrainPos.x/terrain.terrainData.size.x,
            0,
            terrainPos.z / terrain.terrainData.size.z
        );

        return new Vector2(
            (int)(mapPosition.x * terrain.terrainData.alphamapWidth),
            (int)(mapPosition.y * terrain.terrainData.alphamapHeight)
        );
    }
    protected float[] GetTerrainTextures(Vector2 loc)
    {
        float[,,] aMap = terrain.terrainData.GetAlphamaps((int)loc.x, (int)loc.y, 1, 1);
        return new float[]
        {
            aMap[0,0,0],
            aMap[0,0,1],
        };

    }
}
