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
    var minA = isoA.IsoWorldMin;
    var maxA = isoA.IsoWorldMax;
    var minB = isoB.IsoWorldMin;
    var maxB = isoB.IsoWorldMax;
    var xyA = (minA.x - maxA.y, maxA.x - minA.y);
    var xyB = (minB.x - maxB.y, maxB.x - minB.y);
    var yzA = (minA.y + minA.z, maxA.y + maxA.z);
    var yzB = (minB.y + minB.z, maxB.y + maxB.z);
    var zxA = (minA.z + minA.x, maxA.z + maxA.x);
    var zxB = (minB.z + minB.x, maxB.z + maxB.x);
    return IsOverlap(xyA, xyB) && IsOverlap(yzA, yzB) && IsOverlap(zxA, zxB);
  }
}