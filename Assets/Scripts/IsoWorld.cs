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
  public Vector2 zAxis => new Vector2(0, Ratio * baseSize * GetZScale());

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

  public bool AutoSort = true;

  public float GetZScale()
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
    grid.cellSize = new Vector3(baseSize, baseSize * Ratio, 2 * GetZScale());

    var tempAutoSort = AutoSort;
    AutoSort = false; // force disable sort
    foreach (var isoTransform in GetComponentsInChildren<IsoTransform>())
    {
      isoTransform.UpdatePosition();
    }
    AutoSort = tempAutoSort;
    Sort();
  }

  [HideIf("AutoSort")]
  [Button]
  public void Sort()
  {
    var isoTransforms = GetComponentsInChildren<IsoTransform>();

    // reset
    foreach (var iso in isoTransforms)
    {
      iso.behind.Clear();
      iso.visited = false;
    }

    // create relation, naive solution, O(N^2)
    for (var i = 0; i < isoTransforms.Length; i++)
    {
      var isoA = isoTransforms[i];
      for (var j = i + 1; j < isoTransforms.Length; j++)
      {
        var isoB = isoTransforms[j];
        if (isoA.IsInFront(isoB))
        {
          isoA.behind.Add(isoB);
        }
        else
        {
          isoB.behind.Add(isoA);
        }
      }
    }

    // toposort, O(N)

    var depth = 0;
    foreach (var iso in isoTransforms)
    {
      if (iso.visited) continue;
      visit(iso);
    }

    void visit(IsoTransform iso)
    {
      if (iso.visited) return;
      iso.visited = true;
      foreach (var behind in iso.behind)
      {
        visit(behind);
      }

      // set depth
      var newPos = iso.transform.position;
      newPos.z = (depth++) * (-1);
      iso.transform.position = newPos;
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
