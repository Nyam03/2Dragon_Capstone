using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageButton : MonoBehaviour
{
    public int stageNumber;
    public GameObject clearMark; // 체크 마크나 클리어 텍스트 오브젝트
    void Start()
    {
        bool isCleared = PlayerPrefs.GetInt("Stage" + stageNumber + "_Clear", 0) == 1;
        clearMark.SetActive(isCleared); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
