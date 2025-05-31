using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cooltime : MonoBehaviour
{
    public Image coolTimeZ;
    public Image coolTimeX;
    public Image coolTimeC;

    public GameObject readyZImage;
    public GameObject readyXImage;
    public GameObject readyCImage;

    public float cooldownZ = 0.3f;
    public float cooldownX = 0.3f;
    public float cooldownC = 1.0f;

    private float currentTimeZ = 0f;
    private float currentTimeX = 0f;
    private float currentTimeC = 0f;

    private bool isCoolingDownZ = false;
    private bool isCoolingDownX = false;
    private bool isCoolingDownC = false;

    void Start()
    {
        readyZImage.SetActive(true);
        readyXImage.SetActive(true);
        readyCImage.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !isCoolingDownZ)
            UseSkillZ();
        else if (Input.GetKeyDown(KeyCode.X) && !isCoolingDownX)
            UseSkillX();
        else if (Input.GetKeyDown(KeyCode.C) && !isCoolingDownC)
            UseSkillC();

        // Z
        if (isCoolingDownZ)
        {
            currentTimeZ -= Time.deltaTime;
            coolTimeZ.fillAmount = currentTimeZ / cooldownZ;

            if (currentTimeZ <= 0f)
            {
                isCoolingDownZ = false;
                coolTimeZ.fillAmount = 0f;
                readyZImage.SetActive(true);
            }
        }

        // X
        if (isCoolingDownX)
        {
            currentTimeX -= Time.deltaTime;
            coolTimeX.fillAmount = currentTimeX / cooldownX;

            if (currentTimeX <= 0f)
            {
                isCoolingDownX = false;
                coolTimeX.fillAmount = 0f;
                readyXImage.SetActive(true);
            }
        }

        // C
        if (isCoolingDownC)
        {
            currentTimeC -= Time.deltaTime;
            coolTimeC.fillAmount = currentTimeC / cooldownC;

            if (currentTimeC <= 0f)
            {
                isCoolingDownC = false;
                coolTimeC.fillAmount = 0f;
                readyCImage.SetActive(true);
            }
        }
    }

    void UseSkillZ()
    {
        Debug.Log("Z 스킬 사용");
        isCoolingDownZ = true;
        currentTimeZ = cooldownZ;
        coolTimeZ.fillAmount = 1f;
        readyZImage.SetActive(false);
    }

    void UseSkillX()
    {
        Debug.Log("X 스킬 사용");
        isCoolingDownX = true;
        currentTimeX = cooldownX;
        coolTimeX.fillAmount = 1f;
        readyXImage.SetActive(false);
    }

    void UseSkillC()
    {
        Debug.Log("C 스킬 사용");
        isCoolingDownC = true;
        currentTimeC = cooldownC;
        coolTimeC.fillAmount = 1f;
        readyCImage.SetActive(false);
    }
}
