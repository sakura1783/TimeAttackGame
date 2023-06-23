using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text txtTime;

    [SerializeField] private GameManager gameManager;

    [SerializeField] private Image imgIntervalAttackTime;

    public void SetUpUI()
    {
        //TODO imgIntervalAttackTimeのFillの最大値を設定
    }

    void Update()
    {
        txtTime.text = gameManager.timer.ToString("n2");

        //imgIntervalAttackTime.fillAmount = bulletGenerator.interval % 2;
    }
}
