using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text txtTime;

    [SerializeField] private GameManager gameManager;

    [SerializeField] private Image imgIntervalAttackTime;

    private BulletGenerator bulletGenerator;

    public void SetUpUIManager()
    {
        //bulletGenerator = gameManager.CharaController.BulletGenerator;
    }

    void Update()
    {
        txtTime.text = gameManager.timer.ToString("n2");
    }

    public void SetIntervalAttackTime(float interval)
    {
        imgIntervalAttackTime.fillAmount = 0;

        imgIntervalAttackTime.DOFillAmount(1, interval);
    }
}
