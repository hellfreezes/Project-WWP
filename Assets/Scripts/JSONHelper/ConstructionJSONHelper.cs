using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ConstructionJSONHelper {
    public string Name;
    public string SpriteName;
    public int TileSizeWidth;
    public int TileSizeHeigth;
    public IntegerJSONHelper[] Resource;
    public FloatJSONHelper[] Param;
    public string[] OnUpdateAction;
}
