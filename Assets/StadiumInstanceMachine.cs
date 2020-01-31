using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StadiumInstanceMachine : MonoBehaviour
{
    [SerializeField] private Machine[] machine = { };
    [SerializeField] private Human human = null;
    private static MachineName machineName = default;
    private static float[] machineStatus = { };

    public static void SetMachine(MachineName name, float[] status)
    {
        machineName = name;
        machineStatus = status;
    }

    private void Start()
    {
        var machineObject = Instantiate(machine[(int)machineName]) as Machine;
        machineObject.SetStatus(machineStatus);
    }

    private void MachineInstance()
    {

    }

    private void HumanInstance()
    {
        var humanObject = Instantiate(human) as Human;
    }
}