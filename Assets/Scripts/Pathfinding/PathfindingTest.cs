using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Pooling;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class PathfindingTest : MonoBehaviour
{
    public WorldMapController WorldMapController;
    public Transform Player;
    public Light Pointer;

    private Camera _cam;
    private List<Light> _highlightList = new();
    private List<Hex3> _path;
    private Hex3 _currentTargetTile;
    private bool _isMoving;

    private void Awake()
    {
        _cam = Camera.main;
    }

    private void Update()
    {
        WorldMapController.UnloadChunksInRange(transform);
        WorldMapController.LoadChunksInRange(transform);
        
        // var targetTile = HighlightTile();
        // if (targetTile != _currentTargetTile)
        // {
        //     if (TryGetPath(targetTile, out var path))
        //     {
        //         _path = path.ToList();
        //         HighlightPath(_path);
        //     }
        //     
        //     path.Dispose();
        // }
        //
        // if (Input.GetMouseButtonDown(0) && !_isMoving)
        // {
        //     StartCoroutine(MoveAlongPath(_path));
        // }
        //
        // _currentTargetTile = targetTile;
    }

    public Hex3 HighlightTile()
    {
        var worldPos = GetPointerWorldPosition();
        var mapScale = WorldMapController.MapScale;
        var hexPos = Hex3.XZToHex3(worldPos, mapScale);
        Pointer.transform.position = hexPos.ToVector3XZ(mapScale) + Vector3.up * 8f;
        Pointer.color = Color.red;

        return hexPos;
    }
    
    public void HighlightPath(List<Hex3> path)
    {
        if (_isMoving) return;
        
        for (int i = 0; i < _highlightList.Count; i++)
        {
            var tileSpotLight = _highlightList[i];
            tileSpotLight.Pool("TileSpotLight");
            tileSpotLight.gameObject.SetActive(false);
        }
        
        _highlightList.Clear();
        
        for (int i = 0; i < path.Count; i++)
        {
            var pathNode = path[i];
            var pathNodeWorldPos = pathNode.ToVector3XZ(WorldMapController.MapScale);
            var tileSpotLight = Pointer.Retrieve("TileSpotLight");
            tileSpotLight.transform.position = pathNodeWorldPos + Vector3.up * 8f;
            tileSpotLight.color = Color.white;
            _highlightList.Add(tileSpotLight);
        }
    }

    public bool TryGetPath(Hex3 targetHexPos, out NativeList<Hex3> path)
    {
        var biome = WorldMapController.BiomeGen.GetBiome(targetHexPos);
        var traversable = WorldMapController.BiomeGen.GetTileType(targetHexPos) == TileType.Island && biome != Biome.River && biome != Biome.FrozenRiver && biome != Biome.Obstacle;

        if (!traversable)
        {
            path = default;
            return false;
        }
        
        var mapScale = WorldMapController.MapScale;
        var playerPos = Player.transform.position;
            
        var startHexPos = Hex3.XZToHex3(playerPos, mapScale);
        path = new NativeList<Hex3>(Allocator.TempJob);

        var pathJob = new PathfindingJob
        {
            BiomeGen = WorldMapController.BiomeGen,
            Start = startHexPos,
            End = targetHexPos,
            Path = path
        };

        var pathJobHandle = pathJob.Schedule();
        pathJobHandle.Complete();

        return path.Length > 0;
    }

    public IEnumerator MoveAlongPath(List<Hex3> path)
    {
        var pathIndex = 0;

        while (pathIndex < path.Count)
        {
            _isMoving = true;
            var playerPos = Player.transform.position;
            var targetPos = path[pathIndex].ToVector3XZ(WorldMapController.MapScale) + Vector3.up * 2f;

            Player.DOMove(targetPos, 0.5f);
            if (Vector3.Distance(playerPos, targetPos) <= 0.5f)
            {
                var tileSpotLight = _highlightList[pathIndex];
                tileSpotLight.Pool("TileSpotLight");
                tileSpotLight.gameObject.SetActive(false);
                DOTween.Kill(Player);
                pathIndex++;
            }
            
            yield return null;
        }
        
        _highlightList.Clear();
        _isMoving = false;
    }

    public Vector3 GetPointerWorldPosition()
    {
        var mousePosition = Input.mousePosition;
        mousePosition.z = _cam.nearClipPlane;
         
        var mouseWorldPosition = _cam.ScreenToWorldPoint(mousePosition);
        var origin = _cam.transform.position;
        var direction = (mouseWorldPosition - origin).normalized;

        var planeNormal = new Vector3(0, 1, 0);
        var planeD = 0;

        var t = - (Vector3.Dot(planeNormal, origin) + planeD) / Vector3.Dot(planeNormal, direction);
        var intersectionPoint = origin + t * direction;

        return intersectionPoint;
    }
}
