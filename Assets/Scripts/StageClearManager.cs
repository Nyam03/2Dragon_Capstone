using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageClearManager : MonoBehaviour
{
    public int stageNumber;

   /* void Awake()
    {
#if UNITY_EDITOR
        PlayerPrefs.DeleteAll();  // 에디터에서만 초기화
        PlayerPrefs.Save();
#endif
    } */
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClearStage()
    {
        PlayerPrefs.SetInt("Stage" + stageNumber + "_Clear", 1);
        PlayerPrefs.Save();
        
    }
}
