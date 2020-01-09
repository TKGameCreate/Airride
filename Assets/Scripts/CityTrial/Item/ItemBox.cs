using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    private const float machineBound = 1000.0f;
    private const float boundUpPower = 0.05f;

    [SerializeField] private Rigidbody rbody;
    [SerializeField] private Renderer myRenderer;
    [SerializeField] private List<Material> materialList = new List<Material>(); //セットするマテリアルのリスト
    [SerializeField] private AudioClip damageSE = null;
    [SerializeField] private AudioClip breakSE = null; 
    [Range(1.1f, 5.0f)][SerializeField] private float firstMaterialDiv; //2段階目マテリアルを判定する際に割る数
    [Range(1.1f, 5.0f)][SerializeField] private float secondMaterialDiv; //３段階目マテリアルを判定する際に割る数
    [SerializeField] private float defHP; //初期HP0.
    [SerializeField] private int maxGenerate;
    [SerializeField] private float destroyTime;
    [SerializeField] private int flashStage;
    [SerializeField] private ItemList itemList;
    private static FlashTime[] flashTime =
    {
        new FlashTime(30.0f, 1.0f),
        new FlashTime(35.0f, 0.5f),
        new FlashTime(40.0f, 0.25f)
    };
    private Flash flash = new Flash(flashTime);
    private float time;
    private float hitPoint; //現在のHP
    private bool generate = false; //アイテム生成フラグ
    private bool destroySet = false;

    public Transform InstancePosition { set; private get; }
    public StageObjectInstance objInstance { set; private get; }

    private void Start()
    {
        hitPoint = defHP;
    }

    private void Update()
    {
        if(StateManager.State == StateManager.GameState.Game && !destroySet)
        {
            StartCoroutine(LimitDestroy());
            destroySet = true;
        }
        flash.FlashTime(myRenderer);
        if (generate)
        {
            int generateNum = Random.Range(1, maxGenerate + 1); ; //生成する数をランダムに決める
            for (int i = 0; i < generateNum; i++)
            {
                int index = Random.Range(0, itemList.ItemLength); //生成するアイテムを決める
                itemList.InstantiateItem(transform.position, index, generateNum, i);
            }
            AudioManager.Instance.PlaySE(breakSE);
            DestroyObject();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Front")
        {
            Machine machine = other.transform.parent.gameObject.GetComponent<Machine>();
            if (machine.Player != null)
            {
                hitPoint -= machine.SaveSpeed / 2 * machine.Status(StatusType.Weight) / 2;
                SetTexture(machine);
                machine.Bound(machineBound, true);
                rbody.AddForce(Vector3.up * machine.SaveSpeed * boundUpPower, ForceMode.Impulse);
            }
        }
    }

    private void SetTexture(Machine machine)
    {
        if (hitPoint < 1)
        {
            generate = true;
            return;
        }
        else if (hitPoint < defHP / firstMaterialDiv && hitPoint > defHP / secondMaterialDiv)
        {
            myRenderer.material = materialList[1];
        }
        else if (hitPoint < defHP / secondMaterialDiv)
        {
            myRenderer.material = materialList[2];
        }
        else
        {
            myRenderer.material = materialList[0];
        }
        AudioManager.Instance.PlaySE(damageSE);
    }

    IEnumerator LimitDestroy()
    {
        yield return new WaitForSeconds(destroyTime);
        DestroyObject();
    }

    private void DestroyObject()
    {
        if (objInstance != null)
        {
            objInstance.AddPosition(InstancePosition);
        }
        Destroy(gameObject);
    }
}