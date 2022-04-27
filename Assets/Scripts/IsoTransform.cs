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

  public Vector3 IsoWorldMin
  {
    get
    {
      return IsoMin + IsoPosition;
    }
  }

  public Vector3 IsoWorldMax
  {
    get
    {
      return IsoMax + IsoPosition;
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
      var newPos = world.matrix.MultiplyPoint3x4(isoPosition);
      newPos.z = transform.position.z; // keep the z
      transform.position = newPos;
    }
  }
}
