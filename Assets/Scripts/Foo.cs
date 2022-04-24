using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class Foo : MonoBehaviour
{
  [OnValueChanged("OnTestChanged")]
  public int test;

  void OnTestChanged()
  {
    Debug.Log(test);
  }

  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }
}
