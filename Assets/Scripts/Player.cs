using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public enum Condition
    {
        Human,
        Machine
    }

    [SerializeField] private Human human;
    [SerializeField] private GameObject machineText;

    //conditionが切り替わった時に処理させる条件式に使う比較変数
    private Condition changeCondition = Condition.Human;

    private Machine machine;
    private Condition condition = Condition.Human;

    #region プロパティ
    public Machine Machine
    {
        set
        {
            machine = value;
        }
        get
        {
            return machine;
        }
    }

    public Condition PlayerCondition
    {
        set
        {
            condition = value;
        }
        get
        {
            return condition;
        }
    }
    #endregion

    private void Start()
    {
        changeCondition = condition;
    }

    // Update is called once per frame
    private void Update()
    {
        //テキストの表示/非表示
        if(changeCondition != condition)
        {
            changeCondition = condition;
            switch (condition)
            {
                case Condition.Human:
                    machineText.SetActive(false);
                    break;
                case Condition.Machine:
                    machineText.SetActive(true);
                    break;
                default:
                    break;
            }
        }

        switch (condition)
        {
            case Condition.Human:
                human.Controller();
                break;
            case Condition.Machine:
                if (machine != null)
                {
                    machine.TextDisplay();
                    machine.Controller();
                }
                break;
            default:
                break;
        }
    }

    private void FixedUpdate()
    {
        if (condition == Condition.Human)
        {
            human.FixedController();
        }
    }
}
