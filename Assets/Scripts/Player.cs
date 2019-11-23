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

    // Update is called once per frame
    private void Update()
    {
        switch (condition)
        {
            case Condition.Human:
                human.Controller();
                break;
            case Condition.Machine:
                machine.Controller();
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
