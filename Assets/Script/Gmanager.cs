using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum GameState{
	Start,
	Prepare,
	Playing,
	End
}

public class Gmanager : MonoBehaviour
{


    
    public static Gmanager Instance;
    private Dictionary<string, bool> flags = new Dictionary<string, bool>();
    private GameState currentGameState;

    public Text label;
    public Button button;
	public GameObject shatter;

    public int enemydefeat = 0;
    
	private RoomManager roomManager;

[SerializeField] private Transform[] checkPoints; 
    private int currentCheckPointIndex = 0;

    void Awake(){
        Instance = this;
        SetCurrentState(GameState.Start);

    }
    // Start is called before the first frame update

// 外からこのメソッドを使って状態を変更
	public void SetCurrentState(GameState state){
		currentGameState = state;
		OnGameStateChanged (currentGameState);
	}

	// 状態が変わったら何をするか
	void OnGameStateChanged(GameState state){
		switch (state) {
		case GameState.Start:

			break;
		case GameState.Prepare:
			StartCoroutine (PrepareCoroutine ());
			break;
		case GameState.Playing:
			PlayingAction ();
			break;
		case GameState.End:
			EndAction ();
			break;
		default:
			break;
		}
	}

	// Startになったときの処理


	// Prepareになったときの処理
	IEnumerator PrepareCoroutine(){
		label.text = "3";
		yield return new WaitForSeconds(1);
		label.text = "2";
		yield return new WaitForSeconds(1);
		label.text = "1";
		yield return new WaitForSeconds(1);
		label.text = "";
		SetCurrentState (GameState.Playing);
	}
	// Playingになったときの処理
	void PlayingAction(){
		label.text = "ゲーム中";
	}
	// Endになったときの処理
	void EndAction(){
	}

    public void Enemydefeatfun(){
        enemydefeat += 1;
        Debug.Log(enemydefeat);
    }

	public void shatter_up()
	{
		Destroy(shatter);
	}


    public void SetFlag(string key, bool value)
{
    // 1. すでにその名前のフラグが存在するか確認する
    if (flags.ContainsKey(key)) 
    {
        // 存在するなら、値を上書きする
        flags[key] = value;
    }
    else 
    {
        // 存在しないなら、新しくリストに追加する
        flags.Add(key, value);
    }

}

public bool GetFlag(string key)
{
    // 辞書にその名前のフラグがあるか確認
    if (flags.ContainsKey(key))
    {
        return flags[key]; // あればその値を返す(true or false)
    }
    
    // 💡 盲点：まだ登録されていないフラグを聞かれたら、
    // まだクリアしていない（false）とみなすのが論理的に安全
    return false;
}


public void SetCheckPoint(int index)
{
    if (index < checkPoints.Length)
    {
        currentCheckPointIndex = index;
        Debug.Log($"チェックポイント{index}に更新");
    }
}
public Vector3 GetCurrentCheckPoint()
{
    if (checkPoints.Length > 0)
        return checkPoints[currentCheckPointIndex].position;
    return Vector3.zero;
}

}
