using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

// https://mazebert.com/forum/news/isometric-depth-sorting--id775/
// https://shaunlebron.github.io/IsometricBlocks/
// https://en.wikipedia.org/wiki/Topological_sorting
// http://bannalia.blogspot.com/2008/02/filmation-math.html
// https://github.com/as3isolib/as3isolib.v1
// http://rotates.org/phaser/iso/
// https://forum.unity.com/threads/isometric-tilemap-sorting-issues.554914/
// http://andrewrussell.net/2016/06/how-2-5d-sorting-works-in-river-city-ransom-underground/
// https://en.wikipedia.org/wiki/Tarjan%27s_strongly_connected_components_algorithm

[DisallowMultipleComponent]
[RequireComponent(typeof(Grid))]
public class IsoWorld : MonoBehaviour
{
  public enum RatioType
  {
    Isometric, // 1 : sqrt(3)
    Classic, // 1 : 2
    Military, // 1 : 1
    Custom,
  }

  public enum ZType
  {
    Natural, // All line in cube is same
    Mechanical, // Height line is the same as projected tile height
  }

  public float Ratio
  {
    get
    {
      switch (ratioType)
      {
        case RatioType.Isometric: return 1 / Mathf.Sqrt(3);
        case RatioType.Classic: return 0.5f;
        case RatioType.Military: return 1;
        case RatioType.Custom:
        default: return customRatio;
      }
    }
  }

  public Vector2 xAxis => new Vector2(baseSize / 2, Ratio * baseSize / 2);
  public Vector2 yAxis => new Vector2(-baseSize / 2, Ratio * baseSize / 2);
  public Vector2 zAxis => new Vector2(0, Ratio * baseSize * getZScale());

  public Matrix4x4 matrix => new Matrix4x4(xAxis, yAxis, zAxis, Vector4.zero);

  [SerializeField]
  [OnValueChanged("UpdateRatio")]
  private float baseSize = 1;

  [SerializeField]
  [OnValueChanged("UpdateRatio")]
  private RatioType ratioType = RatioType.Isometric;

  private bool useCustomRatio => ratioType == RatioType.Custom;

  [SerializeField]
  [OnValueChanged("UpdateRatio")]
  [ShowIf("useCustomRatio")]
  private float customRatio = 0.5f;

  [SerializeField]
  [OnValueChanged("UpdateRatio")]
  private ZType zType = ZType.Natural;

  [SerializeField]
  [OnValueChanged("UpdateRatio")]
  private float zScale = 1;

  private float getZScale()
  {
    switch (zType)
    {
      case ZType.Natural: return zScale * Mathf.Sqrt(Ratio * Ratio + 1) / (2 * Ratio);
      case ZType.Mechanical: return zScale;
      default: return zScale;
    }
  }

  private void UpdateRatio()
  {
    var grid = GetComponent<Grid>();
    grid.cellLayout = GridLayout.CellLayout.IsometricZAsY;
    grid.cellSize = new Vector3(baseSize, baseSize * Ratio, 2 * getZScale());
    foreach (var isoTransform in GetComponentsInChildren<IsoTransform>())
    {
      isoTransform.UpdatePosition();
    }
  }

  public Vector2 Project(Vector3 isoPos)
  {
    return matrix.MultiplyPoint3x4(isoPos);
  }

  // Need Testing
  public Vector3 Unproject(Vector2 pos)
  {
    return matrix.inverse.MultiplyPoint3x4(pos);
  }
}
