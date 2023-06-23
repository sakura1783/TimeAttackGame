using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CineMachine : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cinemachine;

    [SerializeField] private GameManager gameManager;

    public void SetUpCinemachine(GameManager gameManager)
    {
        this.gameManager = gameManager;

        cinemachine.Follow = this.gameManager.charaController.transform;
    }
}
