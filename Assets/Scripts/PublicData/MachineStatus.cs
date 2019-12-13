using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MachineStatus : ScriptableObject
{
    public enum Type
    {
        Min = 0,
        Default = 1,
        Max = 2
    }

    [SerializeField] private MachineName machineName; //名前
    [SerializeField] private float[] maxSpeed = new float[3];//最高速度
    [SerializeField] private float[] acceleration = new float[3];//加速度
    [SerializeField] private float[] turning = new float[3];//回転速度
    [SerializeField] private float[] brake = new float[3];//減速度
    [SerializeField] private float[] charge = new float[3];//チャージ量
    [SerializeField] private float[] chargeSpeed = new float[3];//チャージ速度
    [SerializeField] private float[] weight = new float[3];//重さ
    [SerializeField] private float[] flySpeed = new float[3];//滑空速度

    //ALLがある為、基準は２
    //ステータスに干渉するアイテムの数+(独自倍率のものはその)倍率
    private float[] changeNumMag = new float[8]
    {
        2.1f,
        2,
        2,
        3,
        2,
        2,
        2,
        3
    };

    public MachineName MachineName
    {
        get
        {
            return machineName;
        }
    }

    public float WeightChangeMaxSpeed()
    {
        return changeNumMag[0] - 2;
    }

    /// <summary>
    /// 各ステータスの取得
    /// </summary>
    /// <param name="statusType">ステータスタイプ</param>
    /// <param name="type">値のタイプ</param>
    /// <returns>取得したいステータスの値</returns>
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

    /// <summary>
    /// ステータスの上昇値
    /// </summary>
    /// <param name="statusType">ステータスタイプ</param>
    /// <returns>アイテム1つにつき、上昇する値</returns>
    public float PlusStatus(StatusType statusType, float spMag = 0)
    {
        float max = GetStatus(statusType, Type.Max);
        float dNum = GetStatus(statusType, Type.Default);
        float limit = Machine.limitStatus;
        if (spMag <= 0)
        {
            return (max - dNum) / (limit * changeNumMag[(int)statusType]);
        }
        else
        {
            return (max - dNum) / (limit / spMag);
        }
    }

    /// <summary>
    /// ステータス下降値
    /// </summary>
    /// <param name="statusType">ステータスタイプ</param>
    /// <returns>アイテム1つにつき、下降する値</returns>
    public float MinusStatus(StatusType statusType, float spMag = 0)
    {
        float min = GetStatus(statusType, Type.Min);
        float dNum = GetStatus(statusType, Type.Default);
        float limit = Machine.limitStatus;
        if (spMag <= 0)
        {
            return (dNum - min) / (limit * changeNumMag[(int)statusType]);
        }
        else
        {
            return (dNum - min) / (limit * spMag);
        }
    }

    /// <summary>
    /// ゲーム開始時のステータス値
    /// </summary>
    /// <param name="statusType">ステータスタイプ</param>
    /// <returns>ゲーム開始時のステータス(-2)の値</returns>
    public float StartStatus(StatusType statusType)
    {
        float dNum = GetStatus(statusType, Type.Default);
        float down = MinusStatus(statusType);
        return dNum - down * 2;
    }
}