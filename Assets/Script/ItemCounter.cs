using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCounter : MonoBehaviour
{
    [Header("UI設定")]
    [SerializeField] private Text itemCountText;
    [SerializeField] private string displayFormat = "アイテム: {0}";
    
    [Header("アイテム設定")]
    [SerializeField] private int currentItemCount = 0;
    [SerializeField] private int maxItemCount = 999;
    

    
    void Start()
    {
        // UIテキストの参照がない場合は自動で取得
        if (itemCountText == null)
        {
            itemCountText = GetComponent<Text>();
            if (itemCountText == null)
            {
                itemCountText = GetComponentInChildren<Text>();
            }
        }
        

        
        // 初期表示を更新
        UpdateDisplay();
    }
    
    /// <summary>
    /// アイテムを1つ追加
    /// </summary>
    public void AddItem()
    {
        AddItems(1);
        Debug.Log("1count");
    }
    
    /// <summary>
    /// 指定した数のアイテムを追加
    /// </summary>
    /// <param name="amount">追加するアイテム数</param>
    public void AddItems(int amount)
    {
        if (amount <= 0) return;
        
        int newCount = Mathf.Min(currentItemCount + amount, maxItemCount);
        
        if (newCount != currentItemCount)
        {
            currentItemCount = newCount;
            UpdateDisplay();
        }
    }
    
    /// <summary>
    /// アイテムを1つ減らす
    /// </summary>
    public void RemoveItem()
    {
        RemoveItems(1);
    }
    
    /// <summary>
    /// 指定した数のアイテムを減らす
    /// </summary>
    /// <param name="amount">減らすアイテム数</param>
    public void RemoveItems(int amount)
    {
        if (amount <= 0) return;
        
        int newCount = Mathf.Max(currentItemCount - amount, 0);
        
        if (newCount != currentItemCount)
        {
            currentItemCount = newCount;
            UpdateDisplay();
        }
    }
    
    /// <summary>
    /// アイテム数を直接設定
    /// </summary>
    /// <param name="count">設定するアイテム数</param>
    public void SetItemCount(int count)
    {
        currentItemCount = Mathf.Clamp(count, 0, maxItemCount);
        UpdateDisplay();
    }
    
    /// <summary>
    /// 表示を更新
    /// </summary>
    private void UpdateDisplay()
    {
        if (itemCountText != null)
        {
            itemCountText.text = string.Format(displayFormat, currentItemCount);
        }
    }

    
    /// <summary>
    /// 現在のアイテム数を取得
    /// </summary>
    /// <returns>現在のアイテム数</returns>
    public int GetItemCount()
    {
        return currentItemCount;
    }
    
    /// <summary>
    /// アイテムが最大数に達しているかチェック
    /// </summary>
    /// <returns>最大数に達している場合はtrue</returns>
    public bool IsAtMaxCapacity()
    {
        return currentItemCount >= maxItemCount;
    }
}