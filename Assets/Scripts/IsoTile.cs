using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using NaughtyAttributes;

[CreateAssetMenu]
public class IsoTile : TileBase
{
  [ShowAssetPreview]
  public Sprite sprite;

  public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
  {
    tileData.sprite = sprite;
    tileData.flags = TileFlags.LockAll;
  }
}
