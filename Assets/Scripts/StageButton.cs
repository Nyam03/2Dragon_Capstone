using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageButton : MonoBehaviour
{
    public int stageNumber;
    public GameObject clearMark; // üũ ��ũ�� Ŭ���� �ؽ�Ʈ ������Ʈ
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
