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

    protected Condition condition = Condition.Human;

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
