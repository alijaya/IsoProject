using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using UnityEditor.Tilemaps;

[CustomGridBrush(true, false, false, "Grid Info Brush")]
public class GridInfoBrush : GridBrushBase
{
  public int value = 0;

  public override void Paint(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
  {
    var gi = brushTarget.GetComponent<GridInformation>();
    if (gi != null)
    {
      SetValue(gi, position, value);
    }
  }

  public override void Erase(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
  {
    var gi = brushTarget.GetComponent<GridInformation>();
    if (gi != null)
    {
      EraseValue(gi, position);
    }
  }

  public override void Pick(GridLayout gridLayout, GameObject brushTarget, BoundsInt position, Vector3Int pivot)
  {
    var gi = brushTarget.GetComponent<GridInformation>();
    if (gi != null)
    {
      value = GetValue(gi, position.position);
    }
  }

  private static int GetValue(GridInformation gi, Vector3Int position)
  {
    return gi.GetPositionProperty(position, "value", 0);
  }
  private static void SetValue(GridInformation gi, Vector3Int position, int value)
  {
    gi.SetPositionProperty(position, "value", value);
  }
  private static void EraseValue(GridInformation gi, Vector3Int position)
  {
    gi.ErasePositionProperty(position, "value");
  }
}

[CustomEditor(typeof(GridInfoBrush))]
public class GridInfoBrushEditor : GridBrushEditorBase
{
  public override void OnSceneGUI(GridLayout gridLayout, GameObject brushTarget)
  {
    if (brushTarget == null) return;

    base.OnSceneGUI(gridLayout, brushTarget);

    var gi = brushTarget.GetComponent<GridInformation>();
    if (gi == null) return;

    foreach (var pos in gi.GetAllPositions("value"))
    {
      var value = gi.GetPositionProperty(pos, "value", 0);
      var labelText = value.ToString();
      Handles.Label(gridLayout.CellToLocalInterpolated(pos + new Vector3(.5f, .5f, 0)), labelText);
    }
  }
}