using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System.Text;
using Assets.ResponseModels;
using System.Collections.Generic;
using LitJson;
using System;
using Assets;

public class ScrollControllerScript : MonoBehaviour {
    private RectTransform panel; // to hold the ScrollPanel
    private RectTransform panelEventDetail;
    //public GameObject[] bttn;
    public RectTransform center;    // center to compare the distance for each button
    //public RectTransform centerEventDetail;

    private float[] distance;   // all button distance to the center
    private float[] distReposition;
    private float[] distanceEventDetail;
    private float[] distRepositionEventDetail;
    private bool dragging = false;  // will be true while we drag the panel
    private bool draggingEventDetail = false;
    //private int bttnDistance;   // will hold the distance between the button
    private int minPanelNum;   // to hold the number of the button with smallest distance to center
    private int minPanelNumEventDetail;
    //private int bttnLength;

    //private bool isOver5Button = false;
    //private int timeOver5Button = 0;
    //private int preMinButtonNum;
    //private bool isAlreadyCount = false;

    private GameObject[] listPanelItem;
    private ArrayList listLine = new ArrayList();
    private Text eventDetailText;

    private GameObject[] listEventDetailPanelItem;
    private int currentMinPanelNum = -1;
    private bool isCurrentMinPanelNum = false;

    private List<EventModel> listEventResponseModel;
    private Sprite eventImageSprite = new Sprite();

    public static int eventId;

    void Start()
    {
        //DialogScript.ConfirmDialog("này thì ConfirmDialog");
        //DialogScript.MessageDialog("này thì MessageDialog");

        ResponseModel<List<EventModel>> jsonResponse = new ResponseModel<List<EventModel>>();

        jsonResponse.Data = new List<EventModel>();

        string wwwText = "{\"Succeed\":true,\"Message\":null,\"Errors\":null,\"Data\":[{\"Description\":\"\",\"Categories\":[\"Hội nghị - Hội thảo\"],\"Id\":3,\"CreatorUserID\":null,\"OrganizerId\":null,\"Name\":\"chiến dịch quảng bá cà phê\",\"ImageUrl\":\"/Cdn/Event/20160906163156PM_f6f34eff-10db-4984-baee-a195c1b61850.jpg\",\"StartDate\":\"\\/Date(1473094800000)\\/\",\"EndDate\":\"\\/Date(1476464340000)\\/\",\"Address\":null,\"Latitude\":0,\"Longitude\":0,\"CreatedTime\":\"\\/Date(-62135596800000)\\/\",\"SeoName\":null,\"IsFree\":true,\"Price\":null,\"Status\":null,\"EnableInteractionPage\":false,\"Active\":false},{\"Description\":\"\",\"Categories\":[\"Âm Nhạc - Giải Trí\"],\"Id\":2,\"CreatorUserID\":null,\"OrganizerId\":null,\"Name\":\"test event hub\",\"ImageUrl\":\"/Cdn/Event/20160905201031PM_e5b972c3-e5b5-497e-8d54-ca78a9660946.jpg\",\"StartDate\":\"\\/Date(1475859600000)\\/\",\"EndDate\":\"\\/Date(1476464340000)\\/\",\"Address\":null,\"Latitude\":0,\"Longitude\":0,\"CreatedTime\":\"\\/Date(-62135596800000)\\/\",\"SeoName\":null,\"IsFree\":true,\"Price\":null,\"Status\":null,\"EnableInteractionPage\":false,\"Active\":false}]}";
        jsonResponse = JsonMapper.ToObject<ResponseModel<List<EventModel>>>(wwwText);
        //jsonResponse = JsonMapper.ToObject<ResponseModel<List<EventResponseModel>>>(www.text);
        listEventResponseModel = jsonResponse.Data;


        //loadParticipateEvent();
        //Load();
        CreateListEventByScript();

        ////bttnLength = bttn.Length;
        distance = new float[listPanelItem.Length];
        distReposition = new float[listPanelItem.Length];

        ////// get distance between button
        ////bttnDistance = (int)Mathf.Abs(bttn[0].GetComponent<RectTransform>().anchoredPosition.x
        ////    - bttn[1].GetComponent<RectTransform>().anchoredPosition.x);

        panel = GameObject.Find("ScrollPanel").GetComponent<RectTransform>();
        //panelEventDetail = GameObject.Find("EventDetailScrollPanel").GetComponent<RectTransform>();

        //eventDetailText = GameObject.Find("EventDetailText").GetComponent<Text>();


    }

    /// <summary>
    /// http://answers.unity3d.com/questions/279750/loading-data-from-a-txt-file-c.html
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public void Load()
    {
        string url = "http://192.168.2.125/Teek/api/event/GetEvents";

        WWW www = new WWW(url);

        StartCoroutine(WaitForRequest(www));
    }

    IEnumerator WaitForRequest(WWW www)
    {
        yield return www;

        if (www.isDone)
        {
            //Check for errors
            if (www.error == null)
            {
                ResponseModel<List<EventResponseModel>> jsonResponse = new ResponseModel<List<EventResponseModel>>();

                jsonResponse.Data = new List<EventResponseModel>();

                string wwwText = "{\"Succeed\":true,\"Message\":null,\"Errors\":null,\"Data\":[{\"Description\":\"\",\"Categories\":[\"Hội nghị - Hội thảo\"],\"Id\":3,\"CreatorUserID\":null,\"OrganizerId\":null,\"Name\":\"chiến dịch quảng bá cà phê\",\"ImageUrl\":\"/Cdn/Event/20160906163156PM_f6f34eff-10db-4984-baee-a195c1b61850.jpg\",\"StartDate\":\"\\/Date(1473094800000)\\/\",\"EndDate\":\"\\/Date(1476464340000)\\/\",\"Address\":null,\"Latitude\":0,\"Longitude\":0,\"CreatedTime\":\"\\/Date(-62135596800000)\\/\",\"SeoName\":null,\"IsFree\":true,\"Price\":null,\"Status\":null,\"EnableInteractionPage\":false,\"Active\":false},{\"Description\":\"\",\"Categories\":[\"Âm Nhạc - Giải Trí\"],\"Id\":2,\"CreatorUserID\":null,\"OrganizerId\":null,\"Name\":\"test event hub\",\"ImageUrl\":\"/Cdn/Event/20160905201031PM_e5b972c3-e5b5-497e-8d54-ca78a9660946.jpg\",\"StartDate\":\"\\/Date(1475859600000)\\/\",\"EndDate\":\"\\/Date(1476464340000)\\/\",\"Address\":null,\"Latitude\":0,\"Longitude\":0,\"CreatedTime\":\"\\/Date(-62135596800000)\\/\",\"SeoName\":null,\"IsFree\":true,\"Price\":null,\"Status\":null,\"EnableInteractionPage\":false,\"Active\":false}]}";
                jsonResponse = JsonMapper.ToObject<ResponseModel<List<EventResponseModel>>>(wwwText);
                //jsonResponse = JsonMapper.ToObject<ResponseModel<List<EventResponseModel>>>(www.text);

                Debug.Log("ABC");

            }
            else
            {
                //showMessage(www.error);
                Debug.Log("WWW Error: " + www.error);
            }
        }

    }


    void Update()
    {
        for (int i = 0; i < listPanelItem.Length; i++)
        {
            distReposition[i] = center.GetComponent<RectTransform>().position.x
                - listPanelItem[i].GetComponent<RectTransform>().position.x;
            distance[i] = Mathf.Abs(distReposition[i]);

        }

        float minDistance = Mathf.Min(distance);    // get the min distance

        for (int a = 0; a < listPanelItem.Length; a++)
        {
            if (minDistance == distance[a])
            {
                minPanelNum = a;
                eventId = listEventResponseModel[a].Id;
                //GameObject.Find("GoToEventDetailText").GetComponent<Text>().text = "go to eventId: "+eventId;
                
            }
        }

        if (currentMinPanelNum != minPanelNum)
        {
            currentMinPanelNum = minPanelNum;
            isCurrentMinPanelNum = true;
        }

        if (isCurrentMinPanelNum)
        {
            isCurrentMinPanelNum = false;
            CreateEventDetailByScript(minPanelNum);
        }

        //GameObject.Find("DebugText").GetComponent<Text>().text = "currentMinPanelNum: " + currentMinPanelNum
        //    + " - minPanelNum: " + minPanelNum + " - isCurrentMinPanelNum: " + isCurrentMinPanelNum;

        //string[] lineOfMinDistancePanel = listLine[minPanelNum].ToString().Split(';');
        //string eventDetailTextContent = "";
        //for (int i = 1; i < lineOfMinDistancePanel.Length; i++)
        //{
        //    eventDetailTextContent += lineOfMinDistancePanel[i] + "\n";
        //}
        ////eventDetailText.text = "này thì EventDetail: \n"+eventDetailTextContent;



        if (!dragging)
        {
            //LerpToBttn(minButtonNum * -bttnDistance);
            LerpToPanel(-listPanelItem[minPanelNum].GetComponent<RectTransform>().anchoredPosition.x);
        }
    }

    void LerpToPanel(float position)
    {
        float newX = Mathf.Lerp(panel.anchoredPosition.x, position, Time.deltaTime * 2.5f);
        Vector2 newPosition = new Vector2(newX, panel.anchoredPosition.y);

        panel.anchoredPosition = newPosition;
    }

    void LerpToPanelEventDetail(float position)
    {
        float newY = Mathf.Lerp(position, panelEventDetail.anchoredPosition.y, Time.deltaTime * 2.5f);
        Vector2 newPosition = new Vector2(panelEventDetail.anchoredPosition.x, newY);

        panelEventDetail.anchoredPosition = newPosition;
    }

    //public void StartDrag()
    //{
    //    dragging = true;
    //}

    //public void EndDrag()
    //{
    //    dragging = false;
    //}

    public void CreateListEventByScript()
    {
        GameObject scrollPanel = GameObject.Find("ScrollPanel");

        //listLine = Load("NayThiLoadFileText.txt");
        //Load();
        listPanelItem = new GameObject[listEventResponseModel.Count];

        for (int i = 0; i < listEventResponseModel.Count; i++)
        {
            GameObject panelItem = new GameObject("PanelItem_" + i);
            panelItem.AddComponent<CanvasRenderer>();
            Image panelItemImage = panelItem.AddComponent<Image>();
            panelItemImage.color = Color.gray;
            string imageUrl = "https://www.google.com.vn/images/branding/googlelogo/2x/googlelogo_color_272x92dp.png";
            //WWW www_loadImage = new WWW(listEventResponseModel[i].ImageUrl);
            WWW www_loadImage = new WWW(imageUrl);
            StartCoroutine(loadImage(www_loadImage));
            panelItemImage.sprite = eventImageSprite;
            panelItem.transform.SetParent(scrollPanel.transform, false);
            panelItem.GetComponent<RectTransform>().sizeDelta = new Vector2(700, 550);
            panelItem.transform.position = new Vector2(scrollPanel.transform.position.x + 975 * i, scrollPanel.transform.position.y);

            GameObject name = new GameObject("Name");
            name.AddComponent<CanvasRenderer>();
            name.transform.SetParent(panelItem.transform, false);
            Text textName = name.AddComponent<Text>();
            textName.text = listEventResponseModel[i].Id.ToString();
            //textName.text = listLine[i].ToString().Split(';')[0];
            //textName.font = Resources.Load<Font>("Fonts/Arial");
            textName.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            textName.color = Color.black;
            textName.fontSize = 55;
            textName.alignment = TextAnchor.MiddleCenter;
            textName.horizontalOverflow = HorizontalWrapMode.Overflow;
            textName.verticalOverflow = VerticalWrapMode.Overflow;

            Button buttonForEvent = panelItem.AddComponent<Button>();
            buttonForEvent.onClick.AddListener(() => new HomeScript().loadEventDetailScene(minPanelNum));

            listPanelItem.SetValue(panelItem, i);
        }
    }

    public void CreateEventDetailByScript(int minPanelNum)
    {
        GameObject eventDetailPanel = GameObject.Find("EventDetailPanel");
        GameObject eventDetailScrollPanel = GameObject.Find("EventDetailScrollPanel");

        foreach (Transform child in eventDetailScrollPanel.transform)
        {
            Destroy(child.gameObject);
        }

        //string[] lineOfMinDistancePanel = listLine[minPanelNum].ToString().Split(';');
        listEventDetailPanelItem = new GameObject[1]; // bo phan tu dau tien

        //for (int i = 1; i < lineOfMinDistancePanel.Length; i++)
        {
            GameObject eventDetailPanelItem = new GameObject("EventDetailPanelItem_");
            eventDetailPanelItem.AddComponent<CanvasRenderer>();
            //Image eventDetailPanelItemImage = eventDetailPanelItem.AddComponent<Image>();
            //eventDetailPanelItemImage.color = Color.white;
            eventDetailPanelItem.transform.SetParent(eventDetailScrollPanel.transform, false);
            //eventDetailPanelItem.GetComponent<RectTransform>().sizeDelta = new Vector2(800, 100);
            eventDetailPanelItem.transform.position = new Vector2(eventDetailPanel.transform.position.x, eventDetailPanel.transform.position.y - 55 * (0 - 1));

            GameObject detail = new GameObject("Detail");
            detail.AddComponent<CanvasRenderer>();
            detail.transform.SetParent(eventDetailPanelItem.transform, false);
            EventModel em = listEventResponseModel[minPanelNum];
            string textDetailContent = "";
            textDetailContent += em.Name + "\n";
            if (em.Description.Length!=0)
            {
                textDetailContent += "Mô tả: "+em.Description + "\n";
            }
            if (em.Categories.Count!=0)
            {
                //textDetailContent += "Thể loại: ";
                foreach (var item in em.Categories)
                {
                    textDetailContent += item + "; ";
                }
                textDetailContent = textDetailContent.Substring(0, textDetailContent.Length - 2);
                textDetailContent += "\n";
            }
            textDetailContent += em.StartDate + "\n";
            //"/Date(1473094800000)/";
            //DateTime dt = new DateTime(long.Parse("1473094800000"));
            //new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(1221818565)
            var d = new DateTime(1245398693390);
            textDetailContent += em.EndDate + "\n";
            if (em.Address!=null)
            {
                textDetailContent += "Địa chỉ: " + em.Address + "\n";
            }
            if (em.IsFree)
            {
                textDetailContent += "0" + "\n";
            }
            else
            {
                textDetailContent += em.Price + "\n";
            }
            Text textDetail = detail.AddComponent<Text>();
            textDetail.text = textDetailContent;
            //textName.font = Resources.Load<Font>("Fonts/Arial");
            textDetail.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            textDetail.color = Color.black;
            textDetail.transform.position = new Vector2(eventDetailPanel.transform.position.x-300, eventDetailPanel.transform.position.y-100);
            textDetail.fontSize = 45;
            textDetail.alignment = TextAnchor.MiddleLeft;
            textDetail.horizontalOverflow = HorizontalWrapMode.Overflow;
            textDetail.verticalOverflow = VerticalWrapMode.Overflow;
            textDetail.lineSpacing = 2;

            listEventDetailPanelItem.SetValue(eventDetailPanelItem, 0);   // tai i chay tu 1 nen phai tru 1
        }
    }

    IEnumerator loadImage(WWW www)
    {
        yield return www;
        eventImageSprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
    }

    void loadParticipateEvent()
    {
        string userID = Decrypt.DecryptString((PlayerPrefs.GetString(ConstantClass.PP_UserIDKey)));

        WWWForm form = new WWWForm();
        form.AddField("userId", userID);

        //SEND POST REQUEST

        WWW www = new WWW(ConstantClass.API_GetParticipateEvents, form);

        StartCoroutine(GetParticipateEventRequest(www));
    }

    IEnumerator GetParticipateEventRequest(WWW www)
    {
        yield return www;

        if (www.isDone)
        {
            //Check for errors
            if (www.error == null)
            {
                ResponseModel<List<EventModel>> jsonResponse = new ResponseModel<List<EventModel>>();
                jsonResponse.Data = new List<EventModel>();
                jsonResponse = JsonMapper.ToObject<ResponseModel<List<EventModel>>>(www.text);

                if (jsonResponse.Succeed)
                {
                    listEventResponseModel = jsonResponse.Data;
                    CreateListEventByScript();
                }
                else
                {
                    
                }
            }
            else
            {
                Debug.Log("WWW Error: " + www.error);
            }
        }

    }

}
