using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MachineStatus : ScriptableObject
{
    [SerializeField] private string machineName; //名前
    [SerializeField] private float hitPoint;            //HP
    [SerializeField] private float attack;              //攻撃力
    [SerializeField] private float defense;           //守備力
    [SerializeField] private float acceleration;     //加速
    [SerializeField] private float turning;           //旋回
    [SerializeField] private float charge;           //チャージ速度
    [SerializeField] private float weight;           //重さ(飛行力)

    public string MachineName { get { return machineName; } }
    public float HitPoint { set { hitPoint = value; } get { return hitPoint; } }
    public float Attack { set { attack = value; } get { return attack; } }
    public float Defense { set { defense = value; } get { return defense; } }
    public float Acceleration { set { acceleration = value; } get { return acceleration; } }
    public float Turning { set { turning = value; } get { return turning; } }
    public float Charge { set { charge = value; } get { return charge; } }
    public float Weight { set { weight = value; } get { return weight; } }
}
