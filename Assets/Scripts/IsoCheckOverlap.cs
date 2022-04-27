using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[ExecuteAlways]
public class IsoCheckOverlap : MonoBehaviour
{
  public IsoTransform compare;
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    var iso = GetComponent<IsoTransform>();
    var sprite = GetComponent<SpriteRenderer>();
    if (iso && compare && sprite)
    {
      if (iso.IsOverlap(compare))
      {
        sprite.color = Color.green;
      }
      else
      {
        sprite.color = Color.white;
      }
    }
  }
}
