using UnityEditor;
using UnityEngine;

public class CreateRoom10Data
{
    [MenuItem("Tools/Create Room10 Data")]
    public static void CreateRoom10Asset()
    {
        // Room10 の RoomData を作成
        RoomData room10 = ScriptableObject.CreateInstance<RoomData>();
        room10.roomID = 10;
        room10.minXBoundary = 340;
        room10.maxXBoundary = 358;  // 画面幅 17.8 に合わせて 340 + 18 = 358
        room10.YBoundary = -15;
        room10.maxYBoundary = 10;
        room10.scrollType = RoomData.ScrollType.Horizontal;

        // ファイルを保存
        string path = "Assets/scriptable/room10.asset";
        AssetDatabase.CreateAsset(room10, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"Room10 Data を作成しました: {path}");
    }
}
