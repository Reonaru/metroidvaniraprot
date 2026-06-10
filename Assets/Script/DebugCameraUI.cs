using UnityEngine;
using UnityEngine.UI;

public class DebugCameraUI : MonoBehaviour
{
    public Text debugText;
    public CameraManager1 cameraManager;
    public RoomManager roomManager;

    void Update()
    {
        if (debugText == null)
        {
            Debug.LogWarning("❌ debugText が null です");
            return;
        }
        if (cameraManager == null)
        {
            Debug.LogWarning("❌ cameraManager が null です");
            return;
        }
        if (roomManager == null)
        {
            Debug.LogWarning("❌ roomManager が null です");
            return;
        }

        // 現在のカメラ境界線情報を表示
        string info = $@"=== カメラ境界線情報 ===
カメラ位置: {cameraManager.transform.position:F2}
minXLimit: {cameraManager.minXLimit:F2}
maxXStopLimit: {cameraManager.maxXStopLimit:F2}
minYLimit: {cameraManager.minYLimit:F2}
maxYLimit: {cameraManager.maxYLimit:F2}
現在の部屋ID: {roomManager.currentRoomID}";

        debugText.text = info;
    }
}
