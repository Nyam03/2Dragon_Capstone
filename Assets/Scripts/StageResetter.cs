using UnityEngine;

public class StageResetter : MonoBehaviour
{
    public StageButton[] stageButtons; // ���� �ִ� ��� StageButton��

    public void ResetAllStages()
    {
        foreach (var stageButton in stageButtons)
        {
            PlayerPrefs.DeleteKey("Stage" + stageButton.stageNumber + "_Clear");
            stageButton.clearMark.SetActive(false); // Ŭ���� ��ũ ��Ȱ��ȭ
        }

        PlayerPrefs.Save();
    }
}
