using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Test_Scribtable")]
public class RoomData : ScriptableObject
{
    public enum ScrollType { Horizontal, Vertical }

    public int roomID;
    public int maxXBoundary;
    public int minXBoundary;
    public int YBoundary;
    public int maxYBoundary;
    public ScrollType scrollType = ScrollType.Horizontal;
}