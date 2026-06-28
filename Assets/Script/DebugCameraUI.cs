using UnityEngine;
using UnityEngine.UI;

public class DebugCameraUI : MonoBehaviour
{
    public Text debugText;
    public CameraManager1 cameraManager;
    public RoomManager roomManager;

    void Start()
    {
        if (debugText == null)
            debugText = GetComponentInChildren<Text>();
        if (cameraManager == null)
            cameraManager = FindObjectOfType<CameraManager1>();
        if (roomManager == null)
            roomManager = FindObjectOfType<RoomManager>();

        if (debugText == null) Debug.LogWarning("❌ debugText が見つかりません");
        if (cameraManager == null) Debug.LogWarning("❌ cameraManager が見つかりません");
        if (roomManager == null) Debug.LogWarning("❌ roomManager が見つかりません");
    }

    void Update()
    {
        if (debugText == null || cameraManager == null || roomManager == null)
            return;

        float screenWidth = Camera.main.orthographicSize * 2 * Camera.main.aspect;

        string info = $@"=== カメラ境界線情報 ===
カメラ位置: {cameraManager.transform.position:F2}
画面幅: {screenWidth:F2}
minXLimit: {cameraManager.minXLimit:F2}
maxXStopLimit: {cameraManager.maxXStopLimit:F2}
minYLimit: {cameraManager.minYLimit:F2}
maxYLimit: {cameraManager.maxYLimit:F2}
現在の部屋ID: {roomManager.currentRoomID}";

        debugText.text = info;
    }
}
