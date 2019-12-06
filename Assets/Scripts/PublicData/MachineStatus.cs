using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MachineStatus : ScriptableObject
{
    public enum Type
    {
        Min = 0,
        Max = 1,
        Default = 2
    }

    [SerializeField] private MachineName machineName; //名前
    [SerializeField] private float[] maxSpeed;//最高速度
    [SerializeField] private float[] acceleration;//加速度
    [SerializeField] private float[] turning;//回転速度
    [SerializeField] private float[] brake;//減速度
    [SerializeField] private float[] charge;//チャージ量
    [SerializeField] private float[] chargeSpeed;//チャージ速度
    [SerializeField] private float[] weight;//重さ
    [SerializeField] private float[] flySpeed;//滑空速度

    public MachineName MachineName
    {
        get
        {
            return machineName;
        }
    }

    public float GetStatus(StatusType statusType,Type type)
    {
        int num = (int)type;
        switch (statusType)
        {
            case StatusType.MaxSpeed:
                return maxSpeed[num];
            case StatusType.Acceleration:
                return acceleration[num];
            case StatusType.Turning:
                return turning[num];
            case StatusType.Brake:
                return brake[num];
            case StatusType.Charge:
                return charge[num];
            case StatusType.ChargeSpeed:
                return chargeSpeed[num];
            case StatusType.Weight:
                return weight[num];
            case StatusType.FlySpeed:
                return flySpeed[num];
            default:
                return 0;
        }
    }
}