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
    [SerializeField] private float speed = 5.0f;

    private Condition condition = Condition.Machine;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(condition == Condition.Human)
        {
            //左スティック　キャラクターの移動
            //Aボタン　ジャンプ
            //Aボタン(マシンの近く)　マシンに乗る
        }
        else
        {
            //MachineClassからMachineの操作関数を呼び出す
        }
    }
}
