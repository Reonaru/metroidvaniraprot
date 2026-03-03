using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Test_Scribtable")]
public class RoomData : ScriptableObject
{
    public int roomID;
    public int maxXBoundary;
    public int minXBoundary;
    public int YBoundary;
    public int maxYBoundary;
}