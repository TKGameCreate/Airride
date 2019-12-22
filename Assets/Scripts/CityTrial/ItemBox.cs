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
    [Range(1.1f, 5.0f)][SerializeField] private float firstMaterialDiv; //2段階目マテリアルを判定する際に割る数
    [Range(1.1f, 5.0f)][SerializeField] private float secondMaterialDiv; //３段階目マテリアルを判定する際に割る数
    [SerializeField] private float defHP; //初期HP
    [SerializeField] private int maxGenerate;
    [SerializeField] private float destroyTime;
    [SerializeField] private int flashStage;
    [SerializeField] private List<Item> itemList = new List<Item>();
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
    

    private void Start()
    {
        hitPoint = defHP;
    }

    private void Update()
    {
        if(StateManager.State == StateManager.GameState.Game && !destroySet)
        {
            Destroy(gameObject, destroyTime);
            destroySet = true;
        }
        flash.FlashTime(myRenderer);
        if (generate)
        {
            int generateNum = Random.Range(1, maxGenerate + 1); //生成する数をランダムに決める
            for (int i = 0; i < generateNum; i++)
            {
                int index = Random.Range(0, itemList.Count); //生成するアイテムを決める
                var obj = Instantiate(itemList[index], transform.position, Quaternion.identity) as Item; //アイテムの生成
                obj.InstanceVelocity(generateNum, i);
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Front")
        {
            Machine machine = other.transform.parent.gameObject.GetComponent<Machine>();
            if (machine.Player != null)
            {
                hitPoint -= machine.SaveSpeed / 2 * (machine.Status(StatusType.Weight) / 2);
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
    }
}