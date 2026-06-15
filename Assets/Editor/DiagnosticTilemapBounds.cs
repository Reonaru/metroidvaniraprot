using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Diagnostic tool to inspect Tilemap boundary extraction methods.
/// Helps determine whether to use cellBounds or SpriteRenderer.bounds for room automation.
/// Usage: Select a GameObject with Tilemap, then Tools > Diagnostic > Inspect Tilemap Bounds
/// </summary>
public class DiagnosticTilemapBounds
{
    [MenuItem("Tools/Diagnostic/Inspect Tilemap Bounds")]
    public static void InspectTilemapBounds()
    {
        GameObject selectedObject = Selection.activeGameObject;
        if (selectedObject == null)
        {
            EditorUtility.DisplayDialog("Error", "Please select a GameObject with a Tilemap component", "OK");
            return;
        }

        Tilemap tilemap = selectedObject.GetComponent<Tilemap>();
        if (tilemap == null)
        {
            EditorUtility.DisplayDialog("Error", $"Selected object '{selectedObject.name}' does not have a Tilemap component", "OK");
            return;
        }

        Debug.Log("============================================");
        Debug.Log($"📍 Tilemap Diagnostic Report: {tilemap.name}");
        Debug.Log("============================================");

        // 1. Tilemap cellBounds (tile coordinates)
        BoundsInt cellBounds = tilemap.cellBounds;
        Debug.Log($"\n📊 cellBounds (Tile Coordinates):");
        Debug.Log($"  x min: {cellBounds.xMin}");
        Debug.Log($"  x max: {cellBounds.xMax}");
        Debug.Log($"  y min: {cellBounds.yMin}");
        Debug.Log($"  y max: {cellBounds.yMax}");
        Debug.Log($"  size: {cellBounds.size}");

        // Convert cellBounds to world coordinates
        Vector3 cellMin = tilemap.CellToWorld(new Vector3Int(cellBounds.xMin, cellBounds.yMin, 0));
        Vector3 cellMax = tilemap.CellToWorld(new Vector3Int(cellBounds.xMax, cellBounds.yMax, 0));
        Debug.Log($"\n🌍 cellBounds converted to World Coordinates:");
        Debug.Log($"  world min: ({cellMin.x:F2}, {cellMin.y:F2})");
        Debug.Log($"  world max: ({cellMax.x:F2}, {cellMax.y:F2})");

        // 2. Tilemap.localBounds (local space - need to convert to world)
        Bounds localBounds = tilemap.localBounds;
        Debug.Log($"\n📦 tilemap.localBounds (Local Space):");
        Debug.Log($"  center: ({localBounds.center.x:F2}, {localBounds.center.y:F2})");
        Debug.Log($"  min: ({localBounds.min.x:F2}, {localBounds.min.y:F2})");
        Debug.Log($"  max: ({localBounds.max.x:F2}, {localBounds.max.y:F2})");
        Debug.Log($"  size: ({localBounds.size.x:F2}, {localBounds.size.y:F2})");

        // Convert local bounds to world space
        Vector3 worldMin = tilemap.transform.TransformPoint(localBounds.min);
        Vector3 worldMax = tilemap.transform.TransformPoint(localBounds.max);
        Debug.Log($"\n🌍 localBounds converted to World Space:");
        Debug.Log($"  world min: ({worldMin.x:F2}, {worldMin.y:F2})");
        Debug.Log($"  world max: ({worldMax.x:F2}, {worldMax.y:F2})");

        // 3. Check for SpriteRenderer alternative
        TilemapRenderer tilemapRenderer = selectedObject.GetComponent<TilemapRenderer>();
        if (tilemapRenderer != null)
        {
            Debug.Log($"\n🎨 TilemapRenderer detected:");
            Debug.Log($"  Material: {tilemapRenderer.material.name}");
        }

        // 4. Recommended extraction for RoomData
        Debug.Log($"\n✅ Recommended Extraction for RoomData:");
        Debug.Log($"  minXBoundary: {worldMin.x:F2}");
        Debug.Log($"  maxXBoundary: {worldMax.x:F2}");
        Debug.Log($"  YBoundary: {worldMin.y:F2}");
        Debug.Log($"  maxYBoundary: {worldMax.y:F2}");

        Debug.Log("\n============================================");
        Debug.Log("💡 Notes:");
        Debug.Log("- tilemap.localBounds gives local-space coordinates");
        Debug.Log("- Must convert to world space using transform.TransformPoint()");
        Debug.Log("- cellBounds gives tile-cell coordinates (need CellToWorld conversion)");
        Debug.Log("- For RoomData, use world-space bounds from localBounds conversion");
        Debug.Log("============================================\n");

        EditorUtility.DisplayDialog(
            "Tilemap Diagnostic Complete",
            $"Inspection complete for '{tilemap.name}'\nCheck Console for detailed output",
            "OK"
        );
    }

    [MenuItem("Tools/Diagnostic/List All Tilemaps in Scene")]
    public static void ListAllTilemaps()
    {
        Tilemap[] allTilemaps = Object.FindObjectsOfType<Tilemap>();

        if (allTilemaps.Length == 0)
        {
            EditorUtility.DisplayDialog("Info", "No Tilemaps found in scene", "OK");
            return;
        }

        Debug.Log("============================================");
        Debug.Log($"📋 Found {allTilemaps.Length} Tilemap(s) in scene:");
        Debug.Log("============================================");

        foreach (Tilemap tilemap in allTilemaps)
        {
            Bounds localBounds = tilemap.localBounds;
            Vector3 worldMin = tilemap.transform.TransformPoint(localBounds.min);
            Vector3 worldMax = tilemap.transform.TransformPoint(localBounds.max);
            Debug.Log($"\n🗺️ {tilemap.name}");
            Debug.Log($"  Parent: {tilemap.transform.parent?.name ?? "Root"}");
            Debug.Log($"  Position: ({tilemap.transform.position.x:F2}, {tilemap.transform.position.y:F2})");
            Debug.Log($"  World Bounds: min=({worldMin.x:F2},{worldMin.y:F2}) max=({worldMax.x:F2},{worldMax.y:F2})");
            Debug.Log($"  Local Bounds Size: ({localBounds.size.x:F2}, {localBounds.size.y:F2})");
        }

        Debug.Log("\n============================================\n");
    }
}
