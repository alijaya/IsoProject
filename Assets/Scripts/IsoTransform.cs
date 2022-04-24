using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[DisallowMultipleComponent]
public class IsoTransform : MonoBehaviour
{
  [SerializeField]
  [OnValueChanged("UpdatePosition")]
  private Vector3 isoPosition;
  public Vector3 IsoPosition
  {
    get
    {
      return isoPosition;
    }
    set
    {
      isoPosition = value;
      UpdatePosition();
    }
  }

  [SerializeField]
  private Vector3 isoSize;
  public Vector3 IsoSize
  {
    get
    {
      return isoSize;
    }
    set
    {
      isoSize = value;
    }
  }

  [SerializeField]
  private Vector3 isoAnchor;
  public Vector3 IsoAnchor
  {
    get
    {
      return isoAnchor;
    }
    set
    {
      isoAnchor = value;
    }
  }

  public Vector3 IsoMin
  {
    get
    {
      return isoAnchor;
    }
    set
    {
      isoSize = isoAnchor + isoSize - value;
      isoAnchor = value;
    }
  }

  public Vector3 IsoMax
  {
    get
    {
      return isoAnchor + isoSize;
    }
    set
    {
      isoSize = value - isoAnchor;
    }
  }

  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }

  public Grid GetGrid()
  {
    return GetComponentInParent<Grid>();
  }

  public void UpdatePosition()
  {
    var grid = GetGrid();
    if (grid)
    {
      var xAxis = grid.CellToLocal(Vector3Int.right);
      var yAxis = grid.CellToLocal(Vector3Int.up);
      var zAxis = grid.CellToLocal(Vector3Int.forward);
      zAxis.z = 0;
      transform.position = xAxis * isoPosition.x + yAxis * isoPosition.y + zAxis * isoPosition.z;
    }
  }

  //private void OnValidate()
  //{
  //    UpdatePosition();
  //}
}
