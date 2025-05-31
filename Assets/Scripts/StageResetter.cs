using UnityEngine;

public class StageResetter : MonoBehaviour
{
    public StageButton[] stageButtons; // 씬에 있는 모든 StageButton들

    public void ResetAllStages()
    {
        foreach (var stageButton in stageButtons)
        {
            PlayerPrefs.DeleteKey("Stage" + stageButton.stageNumber + "_Clear");
            stageButton.clearMark.SetActive(false); // 클리어 마크 비활성화
        }

        PlayerPrefs.Save();
    }
}
