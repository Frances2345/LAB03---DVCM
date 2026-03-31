using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Action OnPlayerWin;
    public Func<int> OnPlayerGetSmt;

    public PlayerController player;
    void Start()
    {
        OnPlayerWin += DestroyAllEnemys;
        OnPlayerWin += ShowVictoryUI;
        OnPlayerGetSmt += SimpleReturn;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPlayerFinishMatch()
    {
        OnPlayerWin?.Invoke();
        OnPlayerGetSmt?.Invoke();
    }

    public void DestroyAllEnemys()
    {
        Debug.Log("Destroy All Enemies");
    }

    public void ShowVictoryUI()
    {
        Debug.Log("ShowVictoryUI");
    }

    public int SimpleReturn()
    {
        return 12;
    }

}