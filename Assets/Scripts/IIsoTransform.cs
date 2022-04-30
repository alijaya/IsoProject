using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IIsoTransform
{
  public Vector3 IsoPosition { get; }
  public Vector3 IsoSize { get; }
  public Vector3 IsoAnchor { get; }
  public Vector3 IsoMin { get; }
  public Vector3 IsoMax { get; }
}
