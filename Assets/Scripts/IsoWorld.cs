using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

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

  public Vector3 xAxis => new Vector3(baseSize / 2, Ratio * baseSize / 2, 0);
  public Vector3 yAxis => new Vector3(-baseSize / 2, Ratio * baseSize / 2, 0);
  public Vector3 zAxis => new Vector3(0, Ratio * baseSize * getZScale(), 0);

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
