using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JGameData {
    public string version;
    public Construction[] constructions;
    public WorldObject[] worldObjects;
    public Tile[] tiles;
}
