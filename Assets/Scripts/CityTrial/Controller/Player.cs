using UnityEngine;
using Cinemachine;

public class Player : MonoBehaviour
{
    public enum Condition
    {
        Human,
        Machine
    }

    [SerializeField] private Human human;
    [SerializeField] private Machine defaultMachine;
    [SerializeField] private SpeedMater mater;
    [SerializeField] private CinemachineVirtualCamera startCamera;

    //conditionが切り替わった時に処理させる条件式に使う比較変数
    private Condition changeCondition = Condition.Machine;

    #region プロパティ
    public Machine Machine { set; get; }
    public Machine LastRideMachine { set; get; }
    public Condition PlayerCondition { set; get; } = Condition.Machine;
    #endregion

    private void Start()
    {
        Machine = defaultMachine;
        changeCondition = PlayerCondition;
    }

    // Update is called once per frame
    private void Update()
    {
        if(StateManager.State == StateManager.GameState.Start)
        {
            return;
        }
        if(StateManager.State == StateManager.GameState.Game && startCamera.gameObject.activeSelf)
        {
            startCamera.gameObject.SetActive(false);
        }

        switch (PlayerCondition)
        {
            case Condition.Human:
                human.Controller();
                mater.HumanGage();
                break;
            case Condition.Machine:
                if (Machine != null)
                {
                    mater.MachineGage(Machine);
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

    private void OnDestroy()
    {
        MachineName name = LastRideMachine.MachineStatus.MachineName;
        float[] status = LastRideMachine.StatusList;
        StadiumInstanceMachine.SetMachine(name, status);
    }
}