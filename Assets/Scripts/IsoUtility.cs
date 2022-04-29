using UnityEngine;

public static class IsoUtility
{

  // min should <= max
  public static bool IsOverlap(this (float min, float max) rangeA, (float min, float max) rangeB)
  {
    return rangeA.min < rangeB.max && rangeB.min < rangeA.max;
  }
  public static bool IsOverlap(this IsoTransform isoA, IsoTransform isoB)
  {
    var world = isoA.GetWorld();
    var scale = world.GetZScale();
    var minA = isoA.IsoWorldMin;
    var maxA = isoA.IsoWorldMax;
    var minB = isoB.IsoWorldMin;
    var maxB = isoB.IsoWorldMax;
    minA.z *= scale;
    maxA.z *= scale;
    minB.z *= scale;
    maxB.z *= scale;
    var xyA = (minA.x - maxA.y, maxA.x - minA.y);
    var xyB = (minB.x - maxB.y, maxB.x - minB.y);
    var yzA = (minA.y + minA.z, maxA.y + maxA.z);
    var yzB = (minB.y + minB.z, maxB.y + maxB.z);
    var zxA = (minA.z + minA.x, maxA.z + maxA.x);
    var zxB = (minB.z + minB.x, maxB.z + maxB.x);
    return IsOverlap(xyA, xyB) && IsOverlap(yzA, yzB) && IsOverlap(zxA, zxB);
  }

  public static bool IsBehind(this IsoTransform isoA, IsoTransform isoB)
  {
    var xBehind = isoB.IsoWorldMax.x - isoA.IsoWorldMin.x;
    var yBehind = isoB.IsoWorldMax.y - isoA.IsoWorldMin.y;
    var zBehind = isoA.IsoWorldMax.z - isoB.IsoWorldMin.z;
    var xFront = isoA.IsoWorldMax.x - isoB.IsoWorldMin.x;
    var yFront = isoA.IsoWorldMax.y - isoB.IsoWorldMin.y;
    var zFront = isoB.IsoWorldMax.z - isoA.IsoWorldMin.z;
    var minBehind = Mathf.Min(xBehind, yBehind, zBehind);
    var minFront = Mathf.Min(xFront, yFront, zFront);
    return minBehind < minFront;
  }

  public static bool IsInFront(this IsoTransform isoA, IsoTransform isoB)
  {
    return IsBehind(isoB, isoA);
  }
}