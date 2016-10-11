using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets;

public class EventDetailScript : MonoBehaviour
{
    public Sprite sprite;
    public GameObject loadingPanel;

    private GameObject[] listPanelItem;
    private ArrayList listLine = new ArrayList();



    public ArrayList Load(string fileName)
    {
        ArrayList listLine = new ArrayList();
        string line;
        StreamReader theReader = new StreamReader(fileName, Encoding.Default);
        using (theReader)
        {
            do
            {
                line = theReader.ReadLine();

                if (line != null)
                {
                    listLine.Add(line);
                }
            }
            while (line != null);
            theReader.Close();
            return listLine;
        }
    }
    // Use this for initialization
    void Start()
    {
        //Populate Point Reward Data
        GameObject PointScrollView = GameObject.Find("PointScrollView");
        listLine = Load("NayThiLoadFileText.txt");
        listPanelItem = new GameObject[listLine.Count];
        PointScrollView.GetComponent<RectTransform>().sizeDelta = new Vector2(0, (listLine.Count) * 178);
        for (int i = 0; i < listLine.Count; i++)
        {
            //Panel Item
            GameObject panelItem = new GameObject("PointRewardItem_" + i);
            panelItem.AddComponent<CanvasRenderer>();
            Image panelItemImage = panelItem.AddComponent<Image>();
            panelItemImage.color = new Color32(255, 255, 255, 255);
            //panelItemImage.sprite = sprite;
            //panelItemImage.type = Image.Type.Sliced;

            //Item
            GameObject item = new GameObject("Item_" + i);
            Image ava = item.AddComponent<Image>();
            //Sprite spr = new Sprite();
            
            ava.color = new Color32(255, 255, 255, 100);
            ava.GetComponent<RectTransform>().sizeDelta = new Vector2(178, 178);
            ava.transform.position = new Vector2(-272, 0);

            GameObject point = new GameObject();
            Text txt = point.AddComponent<Text>();
            txt.text = "20";
            txt.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            txt.fontSize = 30;
            txt.color = new Color32(50, 50, 50, 255);
            var txtRectTransform = txt.GetComponent<RectTransform>();
            txtRectTransform.position = new Vector3(0.1236534f, 24.934f, 0);//set position txtPoint
            txtRectTransform.localScale = new Vector3(1.657835f, 1.657835f, 1.657835f); //set scale txtPoint
            txt.alignment = TextAnchor.MiddleCenter;
            txt.transform.SetParent(item.transform, false);

            GameObject points = new GameObject();
            Text txt2 = points.AddComponent<Text>();
            txt2.text = "points";
            txt2.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            txt2.fontSize = 35;
            txt2.color = new Color32(50, 50, 50, 255);
            var txt2RectTransform = txt2.GetComponent<RectTransform>();
            txt2RectTransform.position = new Vector3(2.098083e-05f, -23f, 0);//set position txtPoint
            txt2RectTransform.localScale = new Vector3(1f, 1f, 1f); //set scale txtPoint
            txt2.alignment = TextAnchor.MiddleCenter;
            txt2.transform.SetParent(item.transform, false);

            GameObject reward = new GameObject();
            Text txt3 = reward.AddComponent<Text>();
            txt3.text = "Một ly cà phê";
            txt3.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            txt3.fontSize = 14;
            txt3.color = new Color32(50, 50, 50, 255);
            var txt3RectTransform = txt3.GetComponent<RectTransform>();
            txt3RectTransform.position = new Vector3(91.75f, 0.09746552f, 0);//set position txtPoint
            txt3RectTransform.sizeDelta = new Vector2(179f, 58.479f);
            txt3RectTransform.localScale = new Vector3(3.040522f, 3.040522f, 3.040523f); //set scale txtPoint
            txt3.alignment = TextAnchor.MiddleCenter;
            item.transform.SetParent(panelItem.transform, false);
            txt3.transform.SetParent(panelItem.transform, false);






            panelItem.transform.SetParent(PointScrollView.transform, false);
            panelItem.GetComponent<RectTransform>().sizeDelta = new Vector2(800, 178);
            panelItem.transform.localPosition = new Vector2(0, -89 - (178 * i));

            //    //GameObject name = new GameObject("Name");
            //    //name.AddComponent<CanvasRenderer>();
            //    //name.transform.SetParent(panelItem.transform, false);
            //    //Text textName = name.AddComponent<Text>();
            //    //textName.text = listLine[i].ToString().Split(';')[0];
            //    ////textName.font = Resources.Load<Font>("Fonts/Arial");
            //    //textName.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            //    //textName.color = Color.black;
            //    //textName.fontSize = 55;
            //    //textName.alignment = TextAnchor.MiddleCenter;
            //    //textName.horizontalOverflow = HorizontalWrapMode.Overflow;
            //    //textName.verticalOverflow = VerticalWrapMode.Overflow;

            //    listPanelItem.SetValue(panelItem, i);
        }

        //Populate Point Reward Data
        GameObject ComboScrollView = GameObject.Find("ComboScrollView");

        listPanelItem = new GameObject[listLine.Count];
        ComboScrollView.GetComponent<RectTransform>().sizeDelta = new Vector2(0, (listLine.Count) * 178);

        for (int i = 0; i < listLine.Count; i++)
        {
            //Panel Item
            GameObject panelItem = new GameObject("ComboRewardItem_" + i);
            
            panelItem.AddComponent<CanvasRenderer>();
            Image panelItemImage = panelItem.AddComponent<Image>();
            panelItemImage.color = new Color32(255, 255, 255, 255);
            //panelItemImage.sprite = sprite;
            //panelItemImage.type = Image.Type.Sliced;

            //Item
            GameObject item = new GameObject("Item_" + i);
            Image ava = item.AddComponent<Image>();
            //Sprite spr = new Sprite();

            ava.color = new Color32(255, 255, 255, 100);
            ava.GetComponent<RectTransform>().sizeDelta = new Vector2(178, 178);
            ava.transform.position = new Vector2(-272, 0);

            GameObject point = new GameObject();
            Text txt = point.AddComponent<Text>();
            txt.text = "40";
            txt.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            txt.fontSize = 30;
            txt.color = new Color32(50, 50, 50, 255);
            var txtRectTransform = txt.GetComponent<RectTransform>();
            txtRectTransform.position = new Vector3(0.1236534f, 24.934f, 0);//set position txtPoint
            txtRectTransform.localScale = new Vector3(1.657835f, 1.657835f, 1.657835f); //set scale txtPoint
            txt.alignment = TextAnchor.MiddleCenter;
            txt.transform.SetParent(item.transform, false);

            GameObject points = new GameObject();
            Text txt2 = points.AddComponent<Text>();
            txt2.text = "points";
            txt2.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            txt2.fontSize = 35;
            txt2.color = new Color32(50, 50, 50, 255);
            var txt2RectTransform = txt2.GetComponent<RectTransform>();
            txt2RectTransform.position = new Vector3(2.098083e-05f, -23f, 0);//set position txtPoint
            txt2RectTransform.localScale = new Vector3(1f, 1f, 1f); //set scale txtPoint
            txt2.alignment = TextAnchor.MiddleCenter;
            txt2.transform.SetParent(item.transform, false);

            GameObject reward = new GameObject();
            Text txt3 = reward.AddComponent<Text>();
            txt3.text = "Một ly cà phê";
            txt3.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            txt3.fontSize = 14;
            txt3.color = new Color32(50, 50, 50, 255);
            var txt3RectTransform = txt3.GetComponent<RectTransform>();
            txt3RectTransform.position = new Vector3(91.75f, 0.09746552f, 0);//set position txtPoint
            txt3RectTransform.sizeDelta = new Vector2(179f, 58.479f);
            txt3RectTransform.localScale = new Vector3(3.040522f, 3.040522f, 3.040523f); //set scale txtPoint
            txt3.alignment = TextAnchor.MiddleCenter;
            item.transform.SetParent(panelItem.transform, false);
            txt3.transform.SetParent(panelItem.transform, false);






            panelItem.transform.SetParent(ComboScrollView.transform, false);
            panelItem.GetComponent<RectTransform>().sizeDelta = new Vector2(800, 178);
            panelItem.transform.localPosition = new Vector2(0, -89 - (178 * i));

            //    //GameObject name = new GameObject("Name");
            //    //name.AddComponent<CanvasRenderer>();
            //    //name.transform.SetParent(panelItem.transform, false);
            //    //Text textName = name.AddComponent<Text>();
            //    //textName.text = listLine[i].ToString().Split(';')[0];
            //    ////textName.font = Resources.Load<Font>("Fonts/Arial");
            //    //textName.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            //    //textName.color = Color.black;
            //    //textName.fontSize = 55;
            //    //textName.alignment = TextAnchor.MiddleCenter;
            //    //textName.horizontalOverflow = HorizontalWrapMode.Overflow;
            //    //textName.verticalOverflow = VerticalWrapMode.Overflow;

            //    listPanelItem.SetValue(panelItem, i);
        }
        //Populate Point Reward Data
        GameObject ShopScrollView = GameObject.Find("ShopScrollView");

        listPanelItem = new GameObject[listLine.Count];
        ShopScrollView.GetComponent<RectTransform>().sizeDelta = new Vector2(0, (listLine.Count) * 89);

        for (int i = 0; i < listLine.Count; i++)
        {
            //Panel Item
            GameObject panelItem = new GameObject("ShopItem_" + i);

            panelItem.AddComponent<CanvasRenderer>();
            Image panelItemImage = panelItem.AddComponent<Image>();
            panelItemImage.color = new Color32(255, 255, 255, 255);
            //panelItemImage.sprite = sprite;
            //panelItemImage.type = Image.Type.Sliced;

           
            GameObject reward = new GameObject();
            Text txt3 = reward.AddComponent<Text>();
            txt3.text = "Một ly cà phê";
            txt3.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            txt3.fontSize = 12;
            txt3.color = new Color32(50, 50, 50, 255);
            var txt3RectTransform = txt3.GetComponent<RectTransform>();
            txt3RectTransform.localPosition = new Vector2(-70, 0);//set position txtPoint
            txt3RectTransform.sizeDelta = new Vector2(170, 27);
            txt3RectTransform.localScale = new Vector2(3.040522f, 3.040522f); //set scale txtPoint
            txt3.alignment = TextAnchor.MiddleLeft;
            //item.transform.SetParent(panelItem.transform, false);
            txt3.transform.SetParent(panelItem.transform, false);






            panelItem.transform.SetParent(ShopScrollView.transform, false);
            panelItem.GetComponent<RectTransform>().sizeDelta = new Vector2(800, 89);
            panelItem.transform.localPosition = new Vector2(0, -44.5f - (89 * i));

            //    //GameObject name = new GameObject("Name");
            //    //name.AddComponent<CanvasRenderer>();
            //    //name.transform.SetParent(panelItem.transform, false);
            //    //Text textName = name.AddComponent<Text>();
            //    //textName.text = listLine[i].ToString().Split(';')[0];
            //    ////textName.font = Resources.Load<Font>("Fonts/Arial");
            //    //textName.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            //    //textName.color = Color.black;
            //    //textName.fontSize = 55;
            //    //textName.alignment = TextAnchor.MiddleCenter;
            //    //textName.horizontalOverflow = HorizontalWrapMode.Overflow;
            //    //textName.verticalOverflow = VerticalWrapMode.Overflow;

            //    listPanelItem.SetValue(panelItem, i);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void PlayGame()
    {
        LoadingManager.showLoadingIndicator(loadingPanel);
        //Save event ID to PlayerPrefs
        SceneManager.LoadSceneAsync(ConstantClass.GameSceneName);
    }
}
