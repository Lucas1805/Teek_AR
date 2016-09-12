using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DragAndThrow : MonoBehaviour {
    bool dragging = false;
    float distance;
    public float ThrowSpeed;
    public float ArchSpeed;
    public float Speed;
    double timeCounter;
    bool isThrow = false;
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

    void Start()
    {
        initialPosition = transform.position;

        //this.GetComponent<Rigidbody>().freezeRotation = true;

        initialRotation = this.GetComponent<Rigidbody>().rotation;
        //displayText = (Text)gameObject.GetComponent("Text");
        displayText.text = "";

        //this.GetComponent<Rigidbody>().maxAngularVelocity = curveAmount * 8f;
        anim = dragon.GetComponent<Animator>();
        timeCounterDragonDead = 0;
        isDead = false;
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
    }
	// Update is called once per frame
	void Update () {
        //distancePokeballRaikou = transform.position - raikou.transform.position;
        //if (distancePokeballRaikou.x < 0.0)
        //{
        //    displayText.text = "trúng xD";
        //    //raikou.SetActive(false);
        //}
        //else
        //{
        //    displayText.text = distance.ToString();
        //}

        //this.GetComponent<Rigidbody>().maxAngularVelocity = curveAmount * 8f;
        //this.GetComponent<Rigidbody>().angularVelocity = transform.forward * curveAmount * 8f
        //    + this.GetComponent<Rigidbody>().angularVelocity;

        if (dragging)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 rayPoint = ray.GetPoint(distance);
            transform.position = Vector3.Lerp(this.transform.position, rayPoint, Speed * Time.deltaTime);

            dragTotalTime += Time.deltaTime;

            //Vector3 direction = Vector3.right;
            //direction = Camera.main.transform.InverseTransformDirection(direction);

            //this.GetComponent<Rigidbody>().AddForce(direction * curveAmount * Time.deltaTime);
        }
        if (isThrow)
        {
            timeCounter += Time.deltaTime;
            
        }
        if (timeCounter >= 5)
        {
            

            timeCounter = 0;
            isThrow = false;
            transform.position = initialPosition;
            this.GetComponent<Rigidbody>().useGravity = false;
            //this.GetComponent<Rigidbody>().velocity = this.transform.forward * 0;
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;

            this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            //transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            transform.rotation = initialRotation;
            //transform.SetParent(Camera.main.transform);

            dragon.SetActive(true);

            count++;

            if (isHit)
            {
                arrayList.Add(1);
                isHit = false;
                displayText.text = "";
            }
            else
            {
                arrayList.Add(0);
            }
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
            transform.gameObject.SetActive(false);

            resultPanel.SetActive(true);
            milk.SetActive(true);
        }

        
	}

    void OnGUI()
    {
        //if (shouldDisplay)
        {
            // Welcome screen group
            GUI.BeginGroup(new Rect(50, 50, 200, Screen.height));

            // Make a box so you can see where the group is on-screen.
            //GUI.Box(new Rect(0, 0, 300, 20), "");

            string text = "";

            //ArrayList arrayList = new ArrayList();
            //for (int i = 0; i < count; i++)
            //{
            //    arrayList.Add(i);
            //}

            GUI.Box(new Rect(0, 0, 200, 20), "");
            text = "chọi đi xD";
            GUI.Label(new Rect(10, 0, 200, 20), text);

            for (int i = 0; i < count; i++)
            {
                text = "chọi lần " + (i+1) + ": ";
                if (int.Parse(arrayList[i].ToString()) == 0)
                {
                    GUI.color = Color.red;
                    text += "miss 8-}";
                }
                else
                {
                    GUI.color = Color.green;
                    text += "trúng xD";
                }

                GUI.Box(new Rect(0, (i+1) * 21, 200, 20), "");
                GUI.Label(new Rect(10, (i+1) * 21, 200, 20), text);
            }

            //if (count > 0)
            //{
            //    GUI.Box(new Rect(0, count*10, 300, 20), "");
            //    text = "chọi " + count.ToString() + " cái rồi đó xD";
            //    GUI.Label(new Rect(count*10, 0, 300, 20), text);
            //}
            //else
            //{
            //    GUI.Box(new Rect(0, 0, 300, 20), "");
            //    text = "chọi đi xD";
            //    GUI.Label(new Rect(10, 0, 300, 20), text);
            //}
            //GUI.Label(new Rect(10, 0, 650, 300), text);




            // End Welcome group
            GUI.EndGroup();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Dragon"))
        {
            health.value -= 10;
            //anim.SetTrigger("Dead");
            anim.SetTrigger("IsDamaged");

            //if (health.value <=30 )
            //{
            //    health.GetComponent<Renderer>().material.color = Color.red;
            //}

            if (health.value <=0)
            {
                //other.gameObject.SetActive(false);
                //displayText.text = "ye xD";
                //health.gameObject.SetActive(false);
                //dragon.SetActive(false);

                isDead = true;

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

            timeCounter = 5;

            //transform.position = initialPosition;
            //this.GetComponent<Rigidbody>().useGravity = false;
            //this.GetComponent<Rigidbody>().velocity = this.transform.forward * 0;

            

            isHit = true;
        }
    }
}
