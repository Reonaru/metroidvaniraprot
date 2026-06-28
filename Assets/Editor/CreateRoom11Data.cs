using UnityEditor;
using UnityEngine;

public class CreateRoom11Data
{
    [MenuItem("Tools/Create Room11 Data")]
    public static void CreateRoom11Asset()
    {
        // Room11 の RoomData を作成
        RoomData room11 = ScriptableObject.CreateInstance<RoomData>();
        room11.roomID = 11;
        room11.minXBoundary = 358;
        room11.maxXBoundary = 398;
        room11.YBoundary = -15;
        room11.maxYBoundary = 10;
        room11.scrollType = RoomData.ScrollType.Horizontal;

        // ファイルを保存
        string path = "Assets/scriptable/room11.asset";
        AssetDatabase.CreateAsset(room11, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"Room11 Data を作成しました: {path}");
    }
}
