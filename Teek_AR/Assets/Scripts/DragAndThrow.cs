using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using MobyShop;
using Assets;
using System.Collections.Generic;

public class DragAndThrow : MonoBehaviour
{
    bool dragging = false;
    float distance;
    public float ThrowSpeed;
    public float ArchSpeed;
    public float Speed;
    double timeCounter;
    bool isThrow = false;
    bool isSpawned;
    float speedCounter;
    float dragTotalTime;
    Vector3 initialPosition;
    Quaternion initialRotation;

    int count = 0;
    ArrayList arrayList = new ArrayList();

    private Vector3 distancePokeballRaikou;
    public Text displayText;
    public GameObject dragon;
    private bool isHit;

    private float curveAmount = 0f, curveSpeed = 2f,

minCurveAmountToCurveBall = 1f, maxCurveAmount = 2.5f;

    public GameObject healthPanel;
    public Slider health;
    public GameObject resultPanel;

    Animator anim;
    private double timeCounterDragonDead;
    private bool isDead;

    private AudioSource source;
    //public AudioClip dragonBackgroundSound;
    //public AudioClip victorySound;
    private float volLowRange = .5f;
    private float volHighRange = 1.0f;
    public Slider backgroundSlider;
    public Slider soundEffectSlider;

    public GameObject dragonBackgroundSound;
    public GameObject victorySound;
    public GameObject shootSound;
    public GameObject hitSound;
    
    private int ballAmount;

    public List<GameObject> listDropItem;
    private bool isShowDropItemListAlready = false;

    public Button resultOKButton;

    void Start()
    {
        ballAmount = Shop.GetProductClassAmount(ConstantClass.FireBallItemClassName);

        // lay so banh hien tai
        //Shop.IncrementProductClassAmount(ConstantClass.FireBallItemClassName, -ballAmount + 1); // tang so banh them 1

        initialPosition = transform.position;

        initialRotation = this.GetComponent<Rigidbody>().rotation;
        displayText.text = "";

        anim = dragon.GetComponent<Animator>();
        timeCounterDragonDead = 0;
        isDead = false;

        source = GetComponent<AudioSource>();

        dragonBackgroundSound.SetActive(true);

        //if (ball != null)
        {
            if (ballAmount <= 0)
            {
                hideBall();
            }
            else
            {
                createBall();
            }
        }
        //else Debug.Log("Cannot get product object. Please check for product ID");

        timeCounter += Time.deltaTime;

        // moi vo test thu cai khung hien ket qua drop item
        //ShowDropItem();
    }
    void OnMouseDown()
    {
        distance = Vector3.Distance(transform.position,

        Camera.main.transform.position);
        dragging = true;

    }

    public void OnMouseUp()
    {
        if (dragTotalTime < 0.1)
        {
            dragTotalTime = 0;
            dragging = false;
            isThrow = true;
            return;
        }
        speedCounter = Vector3.Distance(Input.mousePosition,

initialPosition) / dragTotalTime / 100;
        dragTotalTime = 0;
        this.GetComponent<Rigidbody>().useGravity = true;
        this.GetComponent<Rigidbody>().velocity +=

this.transform.forward * speedCounter;
        this.GetComponent<Rigidbody>().velocity += this.transform.up *

speedCounter / 6;
        dragging = false;
        isThrow = true;

        //ProductInfo fireball = Shop.GetProduct("fireball");
        //if (ball != null)
        {
            if (ballAmount > 0)
            {
                Shop.IncrementProductClassAmount(ConstantClass.FireBallItemClassName, -1);
                ballAmount = Shop.GetProductClassAmount

(ConstantClass.FireBallItemClassName);
            }
        }
        //else Debug.Log("Cannot get product object. Please check for product ID");

        shootSound.SetActive(false);
        shootSound.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        displayText.text = Vector3.Distance(dragon.transform.position, transform.position).ToString();

        ballAmount = Shop.GetProductClassAmount(ConstantClass.FireBallItemClassName);
        if (isThrow)
        {
            //ProductInfo fireball = Shop.GetProduct("fireball");

            if (ballAmount > 0)
            {
                if (!isSpawned)
                {
                    Invoke("createBall", 5);
                }

            }
        }
        else
        {
            if (!dragging)
            {
                if (ballAmount <= 0)
                {
                    hideBall();
                }
                else
                {
                    createBall();
                }
            }
        }

        if (dragging)
        {
            Ray ray = Camera.main.ScreenPointToRay

(Input.mousePosition);
            Vector3 rayPoint = ray.GetPoint(distance);
            transform.position = Vector3.Lerp(this.transform.position,

rayPoint, Speed * Time.deltaTime);

            dragTotalTime += Time.deltaTime;

        }
        if (isThrow)
        {
            timeCounter += Time.deltaTime;
            isSpawned = false;
        }

        if (isDead)
        {
            timeCounterDragonDead += Time.deltaTime;
            anim.SetTrigger("Dead");

        }
        if (timeCounterDragonDead >= 2)
        {
            if (!isShowDropItemListAlready)
            {
                dragon.SetActive(false);
                healthPanel.SetActive(false);
                resultPanel.SetActive(true);
                //transform.position = new Vector3(0, Screen.height * 2, 0);

                isShowDropItemListAlready = true;
                ShowDropItem();

                resultOKButton.gameObject.SetActive(true);
            }

            transform.position = new Vector3(0, Screen.height * 2, 0);
        }
    }

    void createBall()
    {
        CancelInvoke();
        transform.position = initialPosition;
        timeCounter = 0;
        isThrow = false;
        this.GetComponent<Rigidbody>().useGravity = false;
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;

        this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        transform.rotation = initialRotation;
        isSpawned = true;
    }

    void hideBall()
    {
        transform.position = new Vector3(0, Screen.height * 2, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Boundary"))
        {
            createBall();
        }

        if (other.gameObject.CompareTag("Dragon"))
        {
            health.value -= 10;
            anim.SetTrigger("Dead");
            //anim.SetTrigger("IsDamaged");

            hitSound.SetActive(false);
            hitSound.SetActive(true);

            createBall();

            if (health.value <= 0)
            {
                isDead = true;

                dragonBackgroundSound.SetActive(false);
                victorySound.SetActive(true);
            }
            isHit = true;
        }
    }

    public void ShowDropItem()
    {
        List<string> listItem = new List<string>();
        listItem.Add("Ruby");
        listItem.Add("Sapphire");
        listItem.Add("Citrine");

        List<string> listItemClone = new List<string>();

        foreach (var item in listItem)
        {
            listItemClone.Add(item);
        }

        float percent1Item = 1;
        float percent2Item = 1;
        //float percent3Item = 98;

        int ranNumPercent = Random.Range(1, 101);
        int ranNumInListItem = 0;
        int rubyCount = 0;
        int sapphireCount = 0;
        int citrineCount = 0;

        if (ranNumPercent <= percent1Item)
        {
            ranNumInListItem = Random.Range(0, listItemClone.Count);
            listDropItem[ranNumInListItem].transform.GetChild(1).GetComponent<Text>().text = "x 1";
            listDropItem[ranNumInListItem].SetActive(true);
        }
        else if (percent1Item < ranNumPercent && ranNumPercent <= (percent1Item + percent2Item))
        {
            for (int i = 0; i < 2; i++)
            {
                switch (Random.Range(0, listItemClone.Count))
                {
                    case 0:
                        rubyCount++;
                        break;
                    case 1:
                        sapphireCount++;
                        break;
                    case 2:
                        citrineCount++;
                        break;
                    default:
                        break;
                }
            }

            if (rubyCount > 0)
            {
                listDropItem[0].transform.GetChild(1).GetComponent<Text>().text = "x " + rubyCount;
                listDropItem[0].SetActive(true);
            }

            if (sapphireCount > 0)
            {
                listDropItem[1].transform.GetChild(1).GetComponent<Text>().text = "x " + sapphireCount;
                listDropItem[1].SetActive(true);
            }

            if (citrineCount > 0)
            {
                listDropItem[2].transform.GetChild(1).GetComponent<Text>().text = "x " + citrineCount;
                listDropItem[2].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                switch (Random.Range(0, listItemClone.Count))
                {
                    case 0:
                        rubyCount++;
                        break;
                    case 1:
                        sapphireCount++;
                        break;
                    case 2:
                        citrineCount++;
                        break;
                    default:
                        break;
                }
            }

            if (rubyCount > 0)
            {
                listDropItem[0].transform.GetChild(1).GetComponent<Text>().text = "x " + rubyCount;
                listDropItem[0].SetActive(true);
            }

            if (sapphireCount > 0)
            {
                listDropItem[1].transform.GetChild(1).GetComponent<Text>().text = "x " + sapphireCount;
                listDropItem[1].SetActive(true);
            }

            if (citrineCount > 0)
            {
                listDropItem[2].transform.GetChild(1).GetComponent<Text>().text = "x " + citrineCount;
                listDropItem[2].SetActive(true);
            }
        }
    }

    public void ClickResultOKButton()
    {
        health.value = 100;
        isDead = false;
        timeCounterDragonDead = 0;

        dragon.SetActive(true);
        healthPanel.SetActive(true);
        resultPanel.SetActive(false);
    }
}
