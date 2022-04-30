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
    var minA = isoA.IsoMin;
    var maxA = isoA.IsoMax;
    var minB = isoB.IsoMin;
    var maxB = isoB.IsoMax;
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
    var xBehind = isoB.IsoMax.x - isoA.IsoMin.x;
    var yBehind = isoB.IsoMax.y - isoA.IsoMin.y;
    var zBehind = isoA.IsoMax.z - isoB.IsoMin.z;
    var xFront = isoA.IsoMax.x - isoB.IsoMin.x;
    var yFront = isoA.IsoMax.y - isoB.IsoMin.y;
    var zFront = isoB.IsoMax.z - isoA.IsoMin.z;
    var minBehind = Mathf.Min(xBehind, yBehind, zBehind);
    var minFront = Mathf.Min(xFront, yFront, zFront);
    return minBehind < minFront;
  }

  public static bool IsInFront(this IsoTransform isoA, IsoTransform isoB)
  {
    return IsBehind(isoB, isoA);
  }
}