using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float timer = 0;

    void Update()
    {
        timer += Time.deltaTime;
    }
}
