using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Bottle : MonoBehaviour
{
    #region MainData
    public GameObject GameController;
    Rigidbody2D RB;
    ConstantForce2D CF;

    public float StartForce = 6;

    public int Reward;
    bool IsOpened = false;
    #endregion

    #region GameLogic
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        RB.AddForce(transform.up * StartForce, ForceMode2D.Impulse);

        CF = GetComponent<ConstantForce2D>();
        CF.torque = Random.Range(-700, 700);

        Destroy(gameObject, 3);
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Blade" && !IsOpened && GameController.GetComponent<scr_GameController>().GameIsStarted)
        {
            IsOpened = true;

            GameController.GetComponent<scr_GameController>().AddScore(Reward);

            Transform childCap = transform.GetChild(0);

            childCap.parent = null;
            childCap.GetComponent<ConstantForce2D>().torque = Random.Range(-400, 400);
            childCap.gameObject.GetComponent<Rigidbody2D>().simulated = true;
            //childCap.gameObject.GetComponent<Rigidbody2D>().AddRelativeForce(transform.up * 20, ForceMode2D.Impulse);

        }
        else if (col.tag == "DefeatLine" && !IsOpened)
        {
            if (GameController.GetComponent<scr_GameController>().GameIsStarted)
                GameController.GetComponent<scr_GameController>().Defeat();
        }
    }
    #endregion
}
