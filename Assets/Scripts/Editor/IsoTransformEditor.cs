using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NaughtyAttributes.Editor;

[CustomEditor(typeof(IsoTransform))]
public class IsoTransformEditor : NaughtyInspector
{
  private IsoTransform isoTransform;
  private Grid grid;
  private Transform transform;
  private Vector3 pos;
  private Vector3 xAxis;
  private Vector3 yAxis;
  private Vector3 zAxis;
  private Matrix4x4 matrix;
  private Vector3 anchor;
  private Vector3 xSize;
  private Vector3 ySize;
  private Vector3 zSize;
  protected override void OnEnable()
  {
    base.OnEnable();
    Tools.hidden = true;
  }

  protected override void OnDisable()
  {
    base.OnDisable();
    Tools.hidden = false;
  }

  public void OnSceneGUI()
  {
    isoTransform = target as IsoTransform;
    grid = isoTransform.GetGrid();
    transform = isoTransform.transform;
    pos = transform.position;
    xAxis = grid.CellToLocal(Vector3Int.right);
    yAxis = grid.CellToLocal(Vector3Int.up);
    zAxis = grid.CellToLocal(Vector3Int.forward);
    zAxis.z = 0;

    matrix = new Matrix4x4(xAxis, yAxis, zAxis, Vector4.zero);


    anchor = pos + matrix.MultiplyPoint3x4(isoTransform.IsoAnchor);
    var size = isoTransform.IsoSize;
    xSize = xAxis * size.x;
    ySize = yAxis * size.y;
    zSize = zAxis * size.z;

    var tempColor = Handles.color;
    DrawPositionHandles();
    DrawBound();
    DrawBoundHandles();
    Handles.color = tempColor;
  }

  public void DrawPositionHandles()
  {
    var recordObjects = new Object[] { isoTransform, transform };

    Handles.color = Handles.xAxisColor;
    EditorGUI.BeginChangeCheck();
    var newPosX = Handles.Slider(pos, xAxis);
    if (EditorGUI.EndChangeCheck())
    {
      Undo.RecordObjects(recordObjects, "Change Iso X");
      isoTransform.IsoPosition += Vector3.right * Vector3.Dot(newPosX - pos, xAxis);
    }

    Handles.color = Handles.yAxisColor;
    EditorGUI.BeginChangeCheck();
    var newPosY = Handles.Slider(pos, yAxis);
    if (EditorGUI.EndChangeCheck())
    {
      Undo.RecordObjects(recordObjects, "Change Iso Y");
      isoTransform.IsoPosition += Vector3.up * Vector3.Dot(newPosY - pos, yAxis);
    }

    Handles.color = Handles.zAxisColor;
    EditorGUI.BeginChangeCheck();
    var newPosZ = Handles.Slider(pos, zAxis);
    if (EditorGUI.EndChangeCheck())
    {
      Undo.RecordObjects(recordObjects, "Change Iso Z");
      isoTransform.IsoPosition += Vector3.forward * Vector3.Dot(newPosZ - pos, zAxis);
    }
  }

  public void DrawBound()
  {
    Handles.color = Handles.selectedColor;

    Handles.DrawLine(anchor, anchor + xSize);
    Handles.DrawLine(anchor, anchor + ySize);
    Handles.DrawLine(anchor, anchor + zSize);

    Handles.DrawLine(anchor + xSize + zSize, anchor + xSize);
    Handles.DrawLine(anchor + xSize + zSize, anchor + zSize);
    Handles.DrawLine(anchor + xSize + zSize, anchor + xSize + ySize + zSize);

    Handles.DrawLine(anchor + ySize + zSize, anchor + ySize);
    Handles.DrawLine(anchor + ySize + zSize, anchor + zSize);
    Handles.DrawLine(anchor + ySize + zSize, anchor + xSize + ySize + zSize);

    Handles.color = Handles.secondaryColor;

    Handles.DrawLine(anchor + xSize + ySize, anchor + xSize);
    Handles.DrawLine(anchor + xSize + ySize, anchor + ySize);
    Handles.DrawLine(anchor + xSize + ySize, anchor + xSize + ySize + zSize);
  }
  public void DrawBoundHandles()
  {
    var handleScale = .05f;
    Vector3 pos;
    Vector3 newPos;
    float size;

    Handles.color = Handles.xAxisColor;
    EditorGUI.BeginChangeCheck();
    pos = anchor + ySize / 2 + zSize / 2;
    size = HandleUtility.GetHandleSize(pos) * handleScale;
    newPos = Handles.Slider(pos, -xAxis, size, Handles.DotHandleCap, 0);
    if (EditorGUI.EndChangeCheck())
    {
      Undo.RecordObject(isoTransform, "Change Iso Min X");
      isoTransform.IsoMin += Vector3.right * Vector3.Dot(newPos - pos, xAxis);
    }

    Handles.color = Handles.xAxisColor;
    EditorGUI.BeginChangeCheck();
    pos = anchor + ySize / 2 + zSize / 2 + xSize;
    size = HandleUtility.GetHandleSize(pos) * handleScale;
    newPos = Handles.Slider(pos, xAxis, size, Handles.DotHandleCap, 0);
    if (EditorGUI.EndChangeCheck())
    {
      Undo.RecordObject(isoTransform, "Change Iso Max X");
      isoTransform.IsoMax += Vector3.right * Vector3.Dot(newPos - pos, xAxis);
    }

    Handles.color = Handles.yAxisColor;
    EditorGUI.BeginChangeCheck();
    pos = anchor + xSize / 2 + zSize / 2;
    size = HandleUtility.GetHandleSize(pos) * handleScale;
    newPos = Handles.Slider(pos, -yAxis, size, Handles.DotHandleCap, 0);
    if (EditorGUI.EndChangeCheck())
    {
      Undo.RecordObject(isoTransform, "Change Iso Min Y");
      isoTransform.IsoMin += Vector3.up * Vector3.Dot(newPos - pos, yAxis);
    }

    Handles.color = Handles.yAxisColor;
    EditorGUI.BeginChangeCheck();
    pos = anchor + xSize / 2 + zSize / 2 + ySize;
    size = HandleUtility.GetHandleSize(pos) * handleScale;
    newPos = Handles.Slider(pos, yAxis, size, Handles.DotHandleCap, 0);
    if (EditorGUI.EndChangeCheck())
    {
      Undo.RecordObject(isoTransform, "Change Iso Max Y");
      isoTransform.IsoMax += Vector3.up * Vector3.Dot(newPos - pos, yAxis);
    }

    Handles.color = Handles.zAxisColor;
    EditorGUI.BeginChangeCheck();
    pos = anchor + xSize / 2 + ySize / 2;
    size = HandleUtility.GetHandleSize(pos) * handleScale;
    newPos = Handles.Slider(pos, -zAxis, size, Handles.DotHandleCap, 0);
    if (EditorGUI.EndChangeCheck())
    {
      Undo.RecordObject(isoTransform, "Change Iso Min Z");
      isoTransform.IsoMin += Vector3.forward * Vector3.Dot(newPos - pos, zAxis);
    }

    Handles.color = Handles.zAxisColor;
    EditorGUI.BeginChangeCheck();
    pos = anchor + xSize / 2 + ySize / 2 + zSize;
    size = HandleUtility.GetHandleSize(pos) * handleScale;
    newPos = Handles.Slider(pos, zAxis, size, Handles.DotHandleCap, 0);
    if (EditorGUI.EndChangeCheck())
    {
      Undo.RecordObject(isoTransform, "Change Iso Max Z");
      isoTransform.IsoMax += Vector3.forward * Vector3.Dot(newPos - pos, zAxis);
    }

  }

}
