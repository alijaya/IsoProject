using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using NaughtyAttributes;
using System.Linq;

[DisallowMultipleComponent]
public class IsoTransform : MonoBehaviour, IIsoTransform
{
  public Vector3 IsoPosition
  {
    get
    {
      return IsoLocalPosition;
    }
  }

  [SerializeField]
  [OnValueChanged("UpdatePosition")]
  private Vector3 isoLocalPosition;
  public Vector3 IsoLocalPosition
  {
    get
    {
      return isoLocalPosition;
    }
    set
    {
      isoLocalPosition = value;
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

  public Vector3 IsoLocalMin
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

  public Vector3 IsoLocalMax
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

  public Vector3 IsoMin
  {
    get
    {
      return IsoLocalMin + IsoPosition;
    }
  }

  public Vector3 IsoMax
  {
    get
    {
      return IsoLocalMax + IsoPosition;
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

  public IsoWorld GetWorld()
  {
    return GetComponentInParent<IsoWorld>();
  }

  public void UpdatePosition()
  {
    var world = GetWorld();
    if (world)
    {
      var newPos = world.matrix.MultiplyPoint3x4(isoLocalPosition);
      newPos.z = transform.position.z; // keep the z
      transform.position = newPos;
      if (world.AutoSort) world.Sort();
    }
  }

  // Internal Usage
  [NonSerialized]
  public List<IsoTransform> behind = new List<IsoTransform>();

  [ShowNativeProperty]
  private string behindDebug => String.Join(", ", behind.Select(iso => iso.name));

  [NonSerialized]
  public bool visited = false;
}
