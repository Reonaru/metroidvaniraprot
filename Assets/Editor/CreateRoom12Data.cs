using UnityEditor;
using UnityEngine;

public class CreateRoom12Data
{
    [MenuItem("Tools/Create Room12 Data")]
    public static void CreateRoom12Asset()
    {
        // Room12 の RoomData を作成
        RoomData room12 = ScriptableObject.CreateInstance<RoomData>();
        room12.roomID = 12;
        room12.minXBoundary = 398;
        room12.maxXBoundary = 438;
        room12.YBoundary = -15;
        room12.maxYBoundary = 10;
        room12.scrollType = RoomData.ScrollType.Horizontal;

        // ファイルを保存
        string path = "Assets/scriptable/room12.asset";
        AssetDatabase.CreateAsset(room12, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"Room12 Data を作成しました: {path}");
    }
}
