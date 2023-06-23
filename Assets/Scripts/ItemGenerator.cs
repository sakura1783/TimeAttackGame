using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    [SerializeField] private Item[] itemsPrefab;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateItem();
        }
    }

    public void GenerateItem()
    {
        int randomIndex = Random.Range(0, itemsPrefab.Length);

        Item item = Instantiate(itemsPrefab[randomIndex]);


    }
}
