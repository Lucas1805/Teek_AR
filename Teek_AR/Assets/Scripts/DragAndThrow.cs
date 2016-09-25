using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using MobyShop;

public class DragAndThrow : MonoBehaviour {
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

    private float curveAmount = 0f, curveSpeed = 2f, minCurveAmountToCurveBall = 1f, maxCurveAmount = 2.5f;

    public GameObject healthPanel;
    public Slider health;
    public GameObject resultPanel;
    public GameObject milk;

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

    void Start()
    {
        ProductInfo fireball = Shop.GetProduct("fireball");
        initialPosition = transform.position;
        //this.GetComponent<Rigidbody>().freezeRotation = true;

        initialRotation = this.GetComponent<Rigidbody>().rotation;
        //displayText = (Text)gameObject.GetComponent("Text");
        displayText.text = "";

        //this.GetComponent<Rigidbody>().maxAngularVelocity = curveAmount * 8f;
        anim = dragon.GetComponent<Animator>();
        timeCounterDragonDead = 0;
        isDead = false;

        source = GetComponent<AudioSource>();

        dragonBackgroundSound.SetActive(true);

        if (fireball != null)
        {
            if(fireball.Value <= 0)
            {
                transform.position = new Vector3(0, Screen.height * 2, 0);
               
            } else
            {
                createBall();
            }
        }
        else Debug.Log("Cannot get product object. Please check for product ID");

        timeCounter += Time.deltaTime;
    }
    void OnMouseDown()
    {
        distance = Vector3.Distance(transform.position, Camera.main.transform.position);
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
        speedCounter = Vector3.Distance(Input.mousePosition, initialPosition)/ dragTotalTime / 100;
        dragTotalTime = 0;
        this.GetComponent<Rigidbody>().useGravity = true;
        this.GetComponent<Rigidbody>().velocity += this.transform.forward * speedCounter;
        this.GetComponent<Rigidbody>().velocity += this.transform.up * speedCounter / 6;
        dragging = false;
        isThrow = true;

        ProductInfo fireball = Shop.GetProduct("fireball");
        if (fireball != null)
        {
            if (fireball.Value > 0)
            {
                fireball.Value = fireball.Value - 1;
            }
        }
        else Debug.Log("Cannot get product object. Please check for product ID");

        //source.PlayOneShot(shootSound, soundEffectSlider.value);
        //shootSound.SetActive(true);
        shootSound.SetActive(false);

        shootSound.SetActive(true);
    }
	// Update is called once per frame
	void Update () {
        if(!dragging || isThrow)
        {

            ProductInfo fireball = Shop.GetProduct("fireball");

            if (fireball.Value > 0)
            {
                if (!isSpawned)
                {
                    Invoke("createBall", 5);
                }
                
            }
        }


            //if (timeCounter >= 5)
            //{
            //    createBall();
            //    dragon.SetActive(true);

            //}

        if (dragging)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 rayPoint = ray.GetPoint(distance);
            transform.position = Vector3.Lerp(this.transform.position, rayPoint, Speed * Time.deltaTime);

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
        if (timeCounterDragonDead>=2)
        {
            dragon.SetActive(false);

            healthPanel.SetActive(false);
            //transform.gameObject.SetActive(false);

            resultPanel.SetActive(true);
            milk.SetActive(true);

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
        //this.GetComponent<Rigidbody>().velocity = this.transform.forward * 0;
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;

        this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        //transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        transform.rotation = initialRotation;
        //transform.SetParent(Camera.main.transform);
        isSpawned = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Dragon"))
        {
            health.value -= 10;
            anim.SetTrigger("Dead");
            //anim.SetTrigger("IsDamaged");

            hitSound.SetActive(false);
            hitSound.SetActive(true);
            
            if (health.value <=0)
            {
                //other.gameObject.SetActive(false);
                //displayText.text = "ye xD";
                //health.gameObject.SetActive(false);
                //dragon.SetActive(false);

                
                isDead = true;
                //dragonBackgroundSound.UnloadAudioData();
                //source.PlayOneShot(victorySound, backgroundSlider.value);

                dragonBackgroundSound.SetActive(false);
                victorySound.SetActive(true);
                

                //anim.SetTrigger("Dead");
                //anim.SetTrigger("Dead");
                //anim.SetTrigger("Dead");

                //other.gameObject.SetActive(false);

                //healthPanel.SetActive(false);
                //transform.gameObject.SetActive(false);

                //resultPanel.SetActive(true);
                //milk.SetActive(true);
            }
            //other.gameObject.SetActive(false);
            //isThrow = false;

            //displayText.text = "trúng xD";

            //timeCounter = 5;

            //transform.position = initialPosition;
            //this.GetComponent<Rigidbody>().useGravity = false;
            //this.GetComponent<Rigidbody>().velocity = this.transform.forward * 0;

            

            isHit = true;
            
        }
    }
}
