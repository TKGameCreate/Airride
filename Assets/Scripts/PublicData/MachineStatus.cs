using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MachineStatus : ScriptableObject
{
    [SerializeField] private MachineName machineName; //名前
    [SerializeField] private float attack = 10;              //攻撃力
    [SerializeField] private float defense = 1;           //守備力
    [SerializeField] private float maxSpeed = 25;       //最高速度
    [SerializeField] private float acceleration = 1;     //加速
    [SerializeField] private float turning = 15;            //旋回速度
    [SerializeField] private float brake = 1;               //ブレーキ速度倍率
    [SerializeField] private float maxCharge = 1;            //チャージ加速倍率
    [SerializeField] private float chargeSpeed = 1;    //チャージ速度
    [SerializeField] private float weight = 1;           //重さ
    [SerializeField] private float flySpeed = 0;        //飛行時の最高速度
    private float hitPoint = 100;

    public MachineName MachineName { get { return machineName; } }
    public float Attack { set { attack = value; } get { return attack; } }
    public float Defense { set { defense = value; } get { return defense; } }
    public float MaxSpeed { set { maxSpeed = value; } get { return maxSpeed; } }
    public float Acceleration { set { acceleration = value; } get { return acceleration; } }
    public float Turning { set { turning = value; } get { return turning; } }
    public float Brake { set { brake = value; } get { return brake; } }
    public float MaxCharge { set { maxCharge = value; } get { return maxCharge; } }
    public float ChargeSpeed { set { chargeSpeed = value; } get { return chargeSpeed; } }
    public float Weight { set { weight = value; } get { return weight; } }
    public float HitPoint { get { return hitPoint; } }
}
