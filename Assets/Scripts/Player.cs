using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum Condition
    {
        Human,
        Machine
    }

    [SerializeField] private Human human;

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
        switch (condition)
        {
            case Condition.Human:
                human.Controller();
                break;
            case Condition.Machine:
                if (machine != null)
                {
                    machine.Controller();
                }
                break;
            default:
                break;
        }
    }

    private void FixedUpdate()
    {
        Control control;
        switch (condition)
        {
            case Condition.Human:
                control = human;
                break;
            case Condition.Machine:
                control = machine;
                break;
            default:
                control = null;
                return;
        }
        control.FixedController();
    }
}