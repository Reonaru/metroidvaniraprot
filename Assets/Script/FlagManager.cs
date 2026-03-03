using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagManager : MonoBehaviour
{
    [Header("フラグ設定")]
    [SerializeField] private bool showDebugLog = true;
    
    // フラグを格納する辞書
    private Dictionary<string, bool> flags = new Dictionary<string, bool>();
    
    // シングルトン用の静的参照
    public static FlagManager Instance;
    
    void Awake()
    {
        // シングルトンの設定
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    /// <summary>
    /// フラグをONにする
    /// </summary>
    /// <param name="flagName">フラグ名</param>
    public void SetFlag(string flagName)
    {
        SetFlag(flagName, true);
    }
    
    /// <summary>
    /// フラグを指定した値に設定
    /// </summary>
    /// <param name="flagName">フラグ名</param>
    /// <param name="value">設定する値</param>
    public void SetFlag(string flagName, bool value)
    {
        if (string.IsNullOrEmpty(flagName))
        {
            Debug.LogWarning("フラグ名が空です");
            return;
        }
        
        flags[flagName] = value;
        
        if (showDebugLog)
        {
            Debug.Log($"フラグ '{flagName}' を {value} に設定しました");
        }
    }
    
    /// <summary>
    /// フラグの状態を取得
    /// </summary>
    /// <param name="flagName">フラグ名</param>
    /// <returns>フラグの状態（存在しない場合はfalse）</returns>
    public bool GetFlag(string flagName)
    {
        if (string.IsNullOrEmpty(flagName))
        {
            Debug.LogWarning("フラグ名が空です");
            return false;
        }
        
        if (flags.ContainsKey(flagName))
        {
            return flags[flagName];
        }
        
        return false; // 存在しないフラグはfalse
    }
    
    /// <summary>
    /// フラグがONかどうかをチェック
    /// </summary>
    /// <param name="flagName">フラグ名</param>
    /// <returns>フラグがONの場合はtrue</returns>
    public bool IsFlagOn(string flagName)
    {
        return GetFlag(flagName);
    }
    
    /// <summary>
    /// フラグがOFFかどうかをチェック
    /// </summary>
    /// <param name="flagName">フラグ名</param>
    /// <returns>フラグがOFFの場合はtrue</returns>
    public bool IsFlagOff(string flagName)
    {
        return !GetFlag(flagName);
    }
    
    /// <summary>
    /// フラグをOFFにする
    /// </summary>
    /// <param name="flagName">フラグ名</param>
    public void ClearFlag(string flagName)
    {
        SetFlag(flagName, false);
    }
    
    /// <summary>
    /// フラグをトグル（ON/OFF切り替え）
    /// </summary>
    /// <param name="flagName">フラグ名</param>
    public void ToggleFlag(string flagName)
    {
        bool currentValue = GetFlag(flagName);
        SetFlag(flagName, !currentValue);
    }
    
    /// <summary>
    /// フラグが存在するかチェック
    /// </summary>
    /// <param name="flagName">フラグ名</param>
    /// <returns>フラグが存在する場合はtrue</returns>
    public bool HasFlag(string flagName)
    {
        return flags.ContainsKey(flagName);
    }
    
    /// <summary>
    /// フラグを削除
    /// </summary>
    /// <param name="flagName">フラグ名</param>
    public void RemoveFlag(string flagName)
    {
        if (flags.ContainsKey(flagName))
        {
            flags.Remove(flagName);
            
            if (showDebugLog)
            {
                Debug.Log($"フラグ '{flagName}' を削除しました");
            }
        }
    }
    
    /// <summary>
    /// 全てのフラグをクリア
    /// </summary>
    public void ClearAllFlags()
    {
        flags.Clear();
        
        if (showDebugLog)
        {
            Debug.Log("全てのフラグをクリアしました");
        }
    }
    
    /// <summary>
    /// 設定されているフラグの一覧を取得
    /// </summary>
    /// <returns>フラグ名のリスト</returns>
    public List<string> GetAllFlagNames()
    {
        return new List<string>(flags.Keys);
    }
    
    /// <summary>
    /// ONになっているフラグの一覧を取得
    /// </summary>
    /// <returns>ONのフラグ名のリスト</returns>
    public List<string> GetActiveFlagNames()
    {
        List<string> activeFlags = new List<string>();
        
        foreach (var flag in flags)
        {
            if (flag.Value)
            {
                activeFlags.Add(flag.Key);
            }
        }
        
        return activeFlags;
    }
    
    /// <summary>
    /// 複数のフラグが全てONかチェック
    /// </summary>
    /// <param name="flagNames">チェックするフラグ名の配列</param>
    /// <returns>全てのフラグがONの場合はtrue</returns>
    public bool AreAllFlagsOn(params string[] flagNames)
    {
        foreach (string flagName in flagNames)
        {
            if (!GetFlag(flagName))
            {
                return false;
            }
        }
        return true;
    }
    
    /// <summary>
    /// 複数のフラグのうち1つでもONかチェック
    /// </summary>
    /// <param name="flagNames">チェックするフラグ名の配列</param>
    /// <returns>1つでもフラグがONの場合はtrue</returns>
    public bool IsAnyFlagOn(params string[] flagNames)
    {
        foreach (string flagName in flagNames)
        {
            if (GetFlag(flagName))
            {
                return true;
            }
        }
        return false;
    }
    
    /// <summary>
    /// デバッグ用：全フラグの状態を表示
    /// </summary>
    public void PrintAllFlags()
    {
        Debug.Log("=== フラグ一覧 ===");
        if (flags.Count == 0)
        {
            Debug.Log("設定されているフラグはありません");
            return;
        }
        
        foreach (var flag in flags)
        {
            Debug.Log($"{flag.Key}: {flag.Value}");
        }
    }
}
