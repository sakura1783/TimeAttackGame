using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class CharaController : MonoBehaviour
{
    private Animator anim;

    [SerializeField] private float speed = 5f;

    void Start()
    {
        TryGetComponent(out anim);
    }

    void Update()
    {
        ChangeAnimation();
    }

    private void ChangeAnimation()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            anim.SetFloat("X", 0f);
            anim.SetFloat("Y", 1f);

            transform.position += transform.up * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            anim.SetFloat("X", 0f);
            anim.SetFloat("Y", -1f);

            transform.position -= transform.up * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            anim.SetFloat("X", -1f);
            anim.SetFloat("Y", 0f);

            transform.position -= transform.right * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            anim.SetFloat("X", 1f);
            anim.SetFloat("Y", 0f);

            transform.position += transform.right * speed * Time.deltaTime;
        }
    }
}
