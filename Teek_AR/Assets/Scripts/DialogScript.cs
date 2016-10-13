using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DialogScript : MonoBehaviour {

    //// Use this for initialization
    //void Start () {

    //}

    //// Update is called once per frame
    //void Update () {

    //}
    public enum DialogResult
    {
        YES,
        NO,
        NULL
    };

    static DialogResult dialogResult;

    static public void ConfirmDialog(string content)
    {
        dialogResult = DialogResult.NULL;

        GameObject confirmDialogBackgroundPanel = new GameObject("ConfirmDialogBackgroundPanel");
        confirmDialogBackgroundPanel.AddComponent<CanvasRenderer>();
        confirmDialogBackgroundPanel.AddComponent<Image>().color = new Color32(0, 0, 0, 100);
        confirmDialogBackgroundPanel.transform.SetParent(GameObject.Find("Canvas").transform, false);
        confirmDialogBackgroundPanel.AddComponent<RectTransform>();
        confirmDialogBackgroundPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);

        GameObject confirmDialogPanel = new GameObject("ConfirmDialogPanel");
        confirmDialogPanel.AddComponent<CanvasRenderer>();
        confirmDialogPanel.AddComponent<Image>().color = Color.black;
        confirmDialogPanel.transform.SetParent(confirmDialogBackgroundPanel.transform, false);
        confirmDialogPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(700, 500);
        //dialogPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(50, 50);

        GameObject contentText = new GameObject("ContentText");
        contentText.AddComponent<CanvasRenderer>();
        contentText.transform.SetParent(confirmDialogPanel.transform, false);
        //contentText.AddComponent<Image>().color = Color.black;
        contentText.AddComponent<RectTransform>();
        contentText.GetComponent<RectTransform>().sizeDelta = new Vector2(600, 300);
        Text contentTextText = contentText.AddComponent<Text>();
        contentTextText.text = content;
        contentTextText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        contentTextText.color = Color.white;
        contentTextText.fontSize = 55;
        contentTextText.alignment = TextAnchor.MiddleCenter;

        GameObject yesButton = new GameObject("YesButton");
        yesButton.AddComponent<CanvasRenderer>();
        yesButton.AddComponent<Image>().color = Color.gray;
        yesButton.transform.SetParent(confirmDialogPanel.transform, false);
        yesButton.AddComponent<RectTransform>();
        yesButton.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 100);
        yesButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-150, -180);
        GameObject yesButtonText = new GameObject("YesButtonText");
        yesButtonText.AddComponent<CanvasRenderer>();
        yesButtonText.transform.SetParent(yesButton.transform, false);
        yesButtonText.AddComponent<RectTransform>();
        yesButtonText.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 100);
        Text yesButtonTextText = yesButtonText.AddComponent<Text>();
        yesButtonTextText.text = "Yes";
        yesButtonTextText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        yesButtonTextText.color = Color.white;
        yesButtonTextText.fontSize = 55;
        yesButtonTextText.alignment = TextAnchor.MiddleCenter;
        Button buttonForYesButton = yesButton.AddComponent<Button>();
        buttonForYesButton.onClick.AddListener(() => ClickYesButton());

        GameObject noButton = new GameObject("NoButton");
        noButton.AddComponent<CanvasRenderer>();
        noButton.AddComponent<Image>().color = Color.gray;
        noButton.transform.SetParent(confirmDialogPanel.transform, false);
        noButton.AddComponent<RectTransform>();
        noButton.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 100);
        noButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(150, -180);
        GameObject noButtonText = new GameObject("NoButtonText");
        noButtonText.AddComponent<CanvasRenderer>();
        noButtonText.transform.SetParent(noButton.transform, false);
        noButtonText.AddComponent<RectTransform>();
        noButtonText.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 100);
        Text noButtonTextText = noButtonText.AddComponent<Text>();
        noButtonTextText.text = "No";
        noButtonTextText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        noButtonTextText.color = Color.white;
        noButtonTextText.fontSize = 55;
        noButtonTextText.alignment = TextAnchor.MiddleCenter;
        Button buttonForNoButton = noButton.AddComponent<Button>();
        buttonForNoButton.onClick.AddListener(() => ClickNoButton());

        //return resultText;
    }

    static void ClickYesButton()
    {
        Destroy(GameObject.Find("ConfirmDialogBackgroundPanel"));
        dialogResult = DialogResult.YES;
    }

    static void ClickNoButton()
    {
        Destroy(GameObject.Find("ConfirmDialogBackgroundPanel"));
        dialogResult = DialogResult.NO;
    }

    static public DialogResult GetDialogResult()
    {
        return dialogResult;
    }

    static public void MessageDialog(string content)
    {
        dialogResult = DialogResult.NULL;

        GameObject messageDialogBackgroundPanel = new GameObject("MessageDialogBackgroundPanel");
        messageDialogBackgroundPanel.AddComponent<CanvasRenderer>();
        messageDialogBackgroundPanel.AddComponent<Image>().color = new Color32(0, 0, 0, 100);
        messageDialogBackgroundPanel.transform.SetParent(GameObject.Find("Canvas").transform, false);
        messageDialogBackgroundPanel.AddComponent<RectTransform>();
        messageDialogBackgroundPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);

        GameObject messageDialogPanel = new GameObject("MessageDialogPanel");
        messageDialogPanel.AddComponent<CanvasRenderer>();
        messageDialogPanel.AddComponent<Image>().color = Color.black;
        messageDialogPanel.transform.SetParent(messageDialogBackgroundPanel.transform, false);
        messageDialogPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(700, 500);
        //dialogPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(50, 50);

        GameObject contentText = new GameObject("ContentText");
        contentText.AddComponent<CanvasRenderer>();
        contentText.transform.SetParent(messageDialogPanel.transform, false);
        //contentText.AddComponent<Image>().color = Color.black;
        contentText.AddComponent<RectTransform>();
        contentText.GetComponent<RectTransform>().sizeDelta = new Vector2(600, 300);
        Text contentTextText = contentText.AddComponent<Text>();
        contentTextText.text = content;
        contentTextText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        contentTextText.color = Color.white;
        contentTextText.fontSize = 55;
        contentTextText.alignment = TextAnchor.MiddleCenter;

        GameObject oKButton = new GameObject("OKButton");
        oKButton.AddComponent<CanvasRenderer>();
        oKButton.AddComponent<Image>().color = Color.gray;
        oKButton.transform.SetParent(messageDialogPanel.transform, false);
        oKButton.AddComponent<RectTransform>();
        oKButton.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 100);
        oKButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -180);
        GameObject oKButtonText = new GameObject("OKButtonText");
        oKButtonText.AddComponent<CanvasRenderer>();
        oKButtonText.transform.SetParent(oKButton.transform, false);
        oKButtonText.AddComponent<RectTransform>();
        oKButtonText.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 100);
        Text oKButtonTextText = oKButtonText.AddComponent<Text>();
        oKButtonTextText.text = "OK";
        oKButtonTextText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        oKButtonTextText.color = Color.white;
        oKButtonTextText.fontSize = 55;
        oKButtonTextText.alignment = TextAnchor.MiddleCenter;
        Button buttonForOKButton = oKButton.AddComponent<Button>();
        buttonForOKButton.onClick.AddListener(() => Destroy(messageDialogBackgroundPanel));
    }
}
