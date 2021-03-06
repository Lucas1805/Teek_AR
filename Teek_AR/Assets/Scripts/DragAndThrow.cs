﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using MobyShop;
using Assets;
using System.Collections.Generic;
using Ucss;
using Assets.ResponseModels;
using LitJson;
using System;

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

    private int fireballAmount;
    private int iceballAmount;

    public List<GameObject> listDropItem;
    private bool isShowDropItemListAlready = false;
    private int rubyCount = 0;
    private int sapphireCount = 0;
    private int citrineCount = 0;

    public Button resultOKButton;

    public Material fireballMaterial;
    public Material iceballMaterial;

    private float firstTimeDistance;

    private bool isFirstTimeDragonAppear = false;
    private float timeCountdownDragonRespawn;
    private float timeDragonRespawnInMinute = 20f;

    public GameObject HolyBlast;
    private Vector3 hitPosition;
    private float timeWaitForEffectWhenHit = 0;

    public static float DropRateCombo1; //THIS IS THE DEFAULT VALUE IF CANNOT LOAD FROM SERVER
    public static float DropRateCombo2;
    public static float DropRateCombo3;

    //public InputField GravityXInputField;
    //public InputField GravityYInputField;
    //public InputField GravityZInputField;
    //public InputField VelocityFowardInputField;
    //public InputField VelocityUpInputField;
    //public InputField SpeedInputField;

    public GameObject BackgroundPlane;
    private bool isWaitSphereCollider = false;
    private List<GameObject> listSphereCollider = new List<GameObject>();
    private float timeWaitSphereCollider = 0;

    void Start()
    {


        timeCountdownDragonRespawn = timeDragonRespawnInMinute * 60;

        firstTimeDistance = Vector3.Distance(dragon.transform.position, Camera.main.transform.position);

        //fireballMaterial = Resources.Load("FireParticleMeteor2Material", typeof(Material)) as Material;
        //iceballMaterial = Resources.Load("FireParticleSparkMaterial", typeof(Material)) as Material;

        //gameObject.transform.GetChild(0).GetComponent<Renderer>().material = iceballMaterial;
        //gameObject.transform.GetChild(0).GetComponent<Renderer>().material = fireballMaterial;

        fireballAmount = Shop.GetProductClassAmount(ConstantClass.FireBallItemClassName);
        iceballAmount = Shop.GetProductClassAmount(ConstantClass.IceBallItemClassName);

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
            if (fireballAmount <= 0)
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

        GameObject.Find("ShopCanvas").transform.SetParent(GameObject.Find("Canvas").transform, true);

        this.GetComponent<TrailRenderer>().enabled = false;
        this.GetComponent<LineRenderer>().enabled = false;
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
        speedCounter = Vector3.Distance(Input.mousePosition, initialPosition) / dragTotalTime / 100;
        dragTotalTime = 0;

        this.GetComponent<Rigidbody>().useGravity = true;
        //Physics.gravity = new Vector3(float.Parse(GravityXInputField.text),
        //    float.Parse(GravityYInputField.text),
        //    float.Parse(GravityZInputField.text));
        //this.GetComponent<Rigidbody>().velocity +=
        //    this.transform.forward * speedCounter / float.Parse(VelocityFowardInputField.text);
        //this.GetComponent<Rigidbody>().velocity +=
        //    this.transform.up * speedCounter / float.Parse(VelocityUpInputField.text);
        Physics.gravity = new Vector3(0, -50, 0);
        this.GetComponent<Rigidbody>().velocity += this.transform.forward * speedCounter / 2;
        this.GetComponent<Rigidbody>().velocity += this.transform.up * speedCounter / 3;

        float angle = Input.mousePosition.x - Screen.width / 2;
        this.GetComponent<Rigidbody>().velocity += this.transform.right * angle / 30;

        //this.GetComponent<Rigidbody>().velocity *= float.Parse(SpeedInputField.text);
        //Physics.gravity *= float.Parse(SpeedInputField.text);

        if (health.value <= 20 || (health.value <= 50 && CurrentMaterialName() == iceballMaterial.name))
        {
            UpdateTrajectory(initialPosition, this.GetComponent<Rigidbody>().velocity, Physics.gravity, 200);
        }

        dragging = false;
        isThrow = true;

        //ProductInfo fireball = Shop.GetProduct("fireball");
        //if (ball != null)
        {
            if (CurrentMaterialName() == fireballMaterial.name)
            {
                if (fireballAmount > 0)
                {
                    Shop.IncrementProductClassAmount(ConstantClass.FireBallItemClassName, -1);
                    fireballAmount = Shop.GetProductClassAmount(ConstantClass.FireBallItemClassName);
                }
            }
            if (CurrentMaterialName() == iceballMaterial.name)
            {
                if (iceballAmount > 0)
                {
                    Shop.IncrementProductClassAmount(ConstantClass.IceBallItemClassName, -1);
                    iceballAmount = Shop.GetProductClassAmount(ConstantClass.IceBallItemClassName);
                }
            }
        }
        //else Debug.Log("Cannot get product object. Please check for product ID");

        shootSound.SetActive(false);
        shootSound.SetActive(true);

        CallAPIUpdateBallItem();
    }
    // Update is called once per frame
    void Update()
    {
        dragon.transform.LookAt(Camera.main.transform);

        //displayText.text = Vector3.Distance(dragon.transform.position, Camera.main.transform.position).ToString();
        //displayText.text = CurrentMaterialName()
        //    + "\n" + fireballMaterial.name
        //    + "\n" + iceballMaterial.name;
        //displayText.text = "initialPosition: \n" + initialPosition.ToString()
        //    +"\nmousePosition: \n" + Input.mousePosition.ToString();
        //displayText.text = hitPosition.ToString();

        CheckDistanceCameraAndPattern();

        CountdownDragonRespawn();

        fireballAmount = Shop.GetProductClassAmount(ConstantClass.FireBallItemClassName);
        iceballAmount = Shop.GetProductClassAmount(ConstantClass.IceBallItemClassName);

        if (isThrow)
        {
            //ProductInfo fireball = Shop.GetProduct("fireball");

            if (fireballAmount + iceballAmount > 0)
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
                if (fireballAmount + iceballAmount <= 0)
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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 rayPoint = ray.GetPoint(distance);
            transform.position = Vector3.Lerp(this.transform.position, rayPoint, Speed * Time.deltaTime);

            dragTotalTime += Time.deltaTime;

        }
        if (isThrow)
        {
            timeCounter += Time.deltaTime;
            isSpawned = false;
            transform.Rotate(new Vector3(10, 0, 0));

            transform.SetParent(dragon.transform);
        }

        if (isHit)
        {
            HolyBlast.SetActive(false);
            HolyBlast.transform.position = hitPosition;
            HolyBlast.SetActive(true);
            timeWaitForEffectWhenHit += Time.deltaTime;
        }
        if (timeWaitForEffectWhenHit >= 0.1)
        {
            createBall();
            isHit = false;
            timeWaitForEffectWhenHit = 0;
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

                BackgroundPlane.SetActive(true);
            }

            transform.position = new Vector3(0, Screen.height * 2, 0);
        }

        if (!isThrow && fireballAmount <= 0 && CurrentMaterialName() == fireballMaterial.name)
        {
            SwitchBall();
        }
        if (!isThrow && iceballAmount <= 0 && CurrentMaterialName() == iceballMaterial.name)
        {
            SwitchBall();
        }

        GameObject.Find("MenuButton_A").transform.position = new Vector3(115, Screen.height - 90, 0);

        if (isWaitSphereCollider)
        {
            timeWaitSphereCollider += Time.deltaTime;
        }
        if (timeWaitSphereCollider >= 0.1)
        {
            foreach (var item in listSphereCollider)
            {
                Destroy(item);
            }
            //listSphereCollider.Clear();

            timeWaitSphereCollider = 0;
            isWaitSphereCollider = false;
        }

        if (LastHitScript.IsLastHit)
        {
            //GameObject.Find("DisplayText").GetComponent<Text>().text = "Last Hit";
            BackgroundPlane.SetActive(false);
            this.GetComponent<TrailRenderer>().enabled = true;
        }
    }

    void createBall()
    {
        transform.SetParent(GameObject.Find("Camera").transform);
        LastHitScript.IsLastHit = false;
        GameObject.Find("DisplayText").GetComponent<Text>().text = "";
        this.GetComponent<TrailRenderer>().enabled = false;
        this.GetComponent<LineRenderer>().enabled = false;

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
        if (!other.gameObject.name.Contains("sphereCollider_"))
        {
            hitPosition = transform.position;
            isHit = true;

            if (LastHitScript.IsLastHit)
            {
                if (CurrentMaterialName() == fireballMaterial.name)
                {
                    health.value -= 10;
                }
                else
                {
                    health.value -= 10 * 2.5f;
                }

                anim.SetTrigger("Dead");
                //anim.SetTrigger("IsDamaged");

                hitSound.SetActive(false);
                hitSound.SetActive(true);

                //createBall();

                if (health.value <= 0)
                {
                    isDead = true;

                    dragonBackgroundSound.SetActive(false);
                    victorySound.SetActive(true);
                }
                isHit = true;
            }
        }

        if (other.gameObject.CompareTag("Boundary"))
        {
            //createBall();
        }

        if (other.gameObject.CompareTag("Dragon"))
        {
            if (health.value <= 20 || (health.value <= 50 && CurrentMaterialName() == iceballMaterial.name))
            {
                if (LastHitScript.IsLastHit)
                {
                    if (CurrentMaterialName() == fireballMaterial.name)
                    {
                        health.value -= 10;
                    }
                    else
                    {
                        health.value -= 10 * 2.5f;
                    }

                    anim.SetTrigger("Dead");
                    //anim.SetTrigger("IsDamaged");

                    hitSound.SetActive(false);
                    hitSound.SetActive(true);

                    //createBall();

                    if (health.value <= 0)
                    {
                        isDead = true;

                        dragonBackgroundSound.SetActive(false);
                        victorySound.SetActive(true);
                    }
                    isHit = true;
                }
            }

            else
            {
                if (CurrentMaterialName() == fireballMaterial.name)
                {
                    health.value -= 10;
                }
                else
                {
                    health.value -= 10 * 2.5f;
                }

                anim.SetTrigger("Dead");
                //anim.SetTrigger("IsDamaged");

                hitSound.SetActive(false);
                hitSound.SetActive(true);

                //createBall();

                if (health.value <= 0)
                {
                    isDead = true;

                    dragonBackgroundSound.SetActive(false);
                    victorySound.SetActive(true);
                }
                isHit = true;
            }
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

        int ranNumPercent = UnityEngine.Random.Range(1, 101);
        int ranNumInListItem = 0;

        if (ranNumPercent <= DropRateCombo1)
        {
            ranNumInListItem = UnityEngine.Random.Range(0, listItemClone.Count);
            listDropItem[ranNumInListItem].transform.GetChild(1).GetComponent<Text>().text = "x 1";
            listDropItem[ranNumInListItem].SetActive(true);
            switch (ranNumInListItem)
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
        else if (DropRateCombo1 < ranNumPercent && ranNumPercent <= (DropRateCombo1 + DropRateCombo2))
        {
            for (int i = 0; i < 2; i++)
            {
                switch (UnityEngine.Random.Range(0, listItemClone.Count))
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
                switch (UnityEngine.Random.Range(0, listItemClone.Count))
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

    public void ResetGame()
    {
        health.value = 100;
        isDead = false;
        timeCounterDragonDead = 0;

        dragon.SetActive(true);
        healthPanel.SetActive(true);
        resultPanel.SetActive(false);

        isShowDropItemListAlready = false;

        isFirstTimeDragonAppear = false;
        timeCountdownDragonRespawn = timeDragonRespawnInMinute * 60;
    }

    public void CheckDistanceCameraAndPattern()
    {
        float distance = Vector3.Distance(dragon.transform.position, Camera.main.transform.position);
        if (distance != firstTimeDistance && !isDead)
        {
            if (distance < 50)
            {
                dragon.SetActive(false);
                healthPanel.SetActive(false);
            }
            else
            {
                dragon.SetActive(true);
                healthPanel.SetActive(true);
            }
        }
    }

    public void SwitchBall()
    {
        if (!isThrow)
        {
            if (CurrentMaterialName() == fireballMaterial.name)
            {
                gameObject.transform.GetChild(0).GetComponent<Renderer>().material = iceballMaterial;
                return;
            }
            if (CurrentMaterialName() == iceballMaterial.name)
            {
                gameObject.transform.GetChild(0).GetComponent<Renderer>().material = fireballMaterial;
                return;
            }
        }
    }

    public string CurrentMaterialName()
    {
        return gameObject.transform.GetChild(0).GetComponent<Renderer>().material.name.Replace(" (Instance)", "");
    }

    public void CountdownDragonRespawn()
    {
        float distance = Vector3.Distance(dragon.transform.position, Camera.main.transform.position);
        if (firstTimeDistance != distance && dragon.activeSelf)
        {
            isFirstTimeDragonAppear = true;
        }

        if (isFirstTimeDragonAppear)
        {
            timeCountdownDragonRespawn -= Time.deltaTime;
            //displayText.text = timeCountdownDragonRespawn.ToString();
        }

        if (timeCountdownDragonRespawn <= 0)
        {
            ResetGame();
        }
    }

    public void CallAPIUpdateGemAmount()
    {
        HTTPRequest request = new HTTPRequest();
        WWWForm form = new WWWForm();
        //form.AddField("userId", "40efe638-04b6-42aa-81c3-a79b208d75e5");
        //form.AddField("organizerId", 39);
        form.AddField("userId", Decrypt.DecryptString(PlayerPrefs.GetString(ConstantClass.PP_UserIDKey)));
        form.AddField("organizerId", PlayerPrefs.GetInt(ConstantClass.PP_OrganizerId));
        form.AddField("rubyAmount", rubyCount);
        form.AddField("sapphireAmount", sapphireCount);
        form.AddField("citrineAmount", citrineCount);
        request.url = ConstantClass.API_UpdateGemAmount;

        request.stringCallback = new EventHandlerHTTPString(this.OnDoneCallAPIUpdateGemAmount);
        request.onError = new EventHandlerServiceError(MessageHelper.OnError);
        request.onTimeOut = new EventHandlerServiceTimeOut(MessageHelper.OnTimeOut);

        request.formData = form;

        UCSS.HTTP.PostForm(request);
    }

    public void OnDoneCallAPIUpdateGemAmount(string result, string transactionId)
    {
        ResponseModel<CustomerResponseModel> jsonResponse = new ResponseModel<CustomerResponseModel>();
        jsonResponse.Data = new CustomerResponseModel();
        jsonResponse = JsonMapper.ToObject<ResponseModel<CustomerResponseModel>>(result);

        if (jsonResponse.Succeed)
        {
            ResetGame();

            rubyCount = 0;
            sapphireCount = 0;
            citrineCount = 0;

            foreach (var item in listDropItem)
            {
                item.SetActive(false);
            }
        }

        else
        {
            //Show error message
            if (jsonResponse.Errors != null)
                MessageHelper.MessageDialog(ConstantClass.Msg_ErrorTitle, jsonResponse.Message + " " + jsonResponse.Errors[0]);
            else
                MessageHelper.MessageDialog(ConstantClass.Msg_ErrorTitle, jsonResponse.Message);
        }
    }

    public void CallAPIUpdateBallItem()
    {
        HTTPRequest request = new HTTPRequest();
        WWWForm form = new WWWForm();
        form.AddField("userId", Decrypt.DecryptString(PlayerPrefs.GetString(ConstantClass.PP_UserIDKey)));
        form.AddField("organizerId", PlayerPrefs.GetInt(ConstantClass.PP_OrganizerId));
        form.AddField("eventId", PlayerPrefs.GetInt(ConstantClass.PP_EventIDKey));
        form.AddField("price", 0);

        if (CurrentMaterialName() == fireballMaterial.name)
        {
            form.AddField("fireballAmount", 1);
            form.AddField("iceballAmount", 0);
        }
        if (CurrentMaterialName() == iceballMaterial.name)
        {
            form.AddField("fireballAmount", 0);
            form.AddField("iceballAmount", 1);
        }

        request.url = ConstantClass.API_UpdateBallItem;

        request.stringCallback = new EventHandlerHTTPString(this.OnDoneCallAPIUpdateBallItem);
        request.onError = new EventHandlerServiceError(MessageHelper.OnError);
        request.onTimeOut = new EventHandlerServiceTimeOut(MessageHelper.OnTimeOut);

        request.formData = form;

        UCSS.HTTP.PostForm(request);
    }

    public void OnDoneCallAPIUpdateBallItem(string result, string transactionId)
    {
        ResponseModel<CustomerResponseModel> jsonResponse = new ResponseModel<CustomerResponseModel>();
        jsonResponse.Data = new CustomerResponseModel();
        jsonResponse = JsonMapper.ToObject<ResponseModel<CustomerResponseModel>>(result);

        if (jsonResponse.Succeed)
        {
            fireballAmount = (int)jsonResponse.Data.Fireball;
            iceballAmount = (int)jsonResponse.Data.Iceball;
        }
        else
        {
            //restorePurchase(justBoughtProduct, justBoughtProduct.IncrementOnBuy);
            MessageHelper.MessageDialog("Error", jsonResponse.Message);
            Debug.Log(jsonResponse.Message);
        }
    }

    void UpdateTrajectory(Vector3 initialPosition, Vector3 initialVelocity, Vector3 gravity, int numSteps)
    {
        //int numSteps = 20; // for example
        float timeDelta = 1.0f / initialVelocity.magnitude; // for example

        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetVertexCount(numSteps);
        lineRenderer.SetWidth(0, 0);

        Vector3 position = initialPosition;
        Vector3 velocity = initialVelocity;
        for (int i = 0; i < numSteps; ++i)
        {
            lineRenderer.SetPosition(i, position);

            position += velocity * timeDelta + 0.5f * gravity * timeDelta * timeDelta;
            velocity += gravity * timeDelta;

            GameObject sphereCollider = new GameObject("sphereCollider_" + i);
            sphereCollider.AddComponent<SphereCollider>();
            sphereCollider.GetComponent<SphereCollider>().isTrigger = true;
            sphereCollider.AddComponent<Rigidbody>();
            sphereCollider.GetComponent<Rigidbody>().useGravity = false;
            sphereCollider.GetComponent<Rigidbody>().isKinematic = true;
            sphereCollider.AddComponent<LastHitScript>();
            sphereCollider.transform.position = position;

            listSphereCollider.Add(sphereCollider);
            //Destroy(sphereCollider);
        }

        isWaitSphereCollider = true;
    }
}
