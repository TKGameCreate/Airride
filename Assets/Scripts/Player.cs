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

    [SerializeField] private float hitPoint = 100;

    private Condition condition = Condition.Machine;

    // Update is called once per frame
    void Update()
    {
        Operation();
    }

    protected virtual void Operation()
    {
        Debug.Log("overrideしてください[Player.Operation()]");
    }
}
