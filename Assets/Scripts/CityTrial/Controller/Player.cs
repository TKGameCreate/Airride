using UnityEngine;

public class Player : MonoBehaviour
{
    public enum Condition
    {
        Human,
        Machine
    }

    [SerializeField] private Human human;
    [SerializeField] private Machine DefaultMachine;

    //conditionが切り替わった時に処理させる条件式に使う比較変数
    private Condition changeCondition = Condition.Machine;

    #region プロパティ
    public Machine Machine { set; get; }
    public Condition PlayerCondition { set; get; } = Condition.Machine;
    #endregion

    private void Start()
    {
        Machine = DefaultMachine;
        changeCondition = PlayerCondition;
    }

    // Update is called once per frame
    private void Update()
    {
        switch (PlayerCondition)
        {
            case Condition.Human:
                human.Controller();
                break;
            case Condition.Machine:
                if (Machine != null)
                {
                    Machine.Controller();
                }
                break;
            default:
                break;
        }
    }

    private void FixedUpdate()
    {
        Control control;
        switch (PlayerCondition)
        {
            case Condition.Human:
                control = human;
                break;
            case Condition.Machine:
                control = Machine;
                break;
            default:
                control = null;
                return;
        }
        control.FixedController();
    }
}