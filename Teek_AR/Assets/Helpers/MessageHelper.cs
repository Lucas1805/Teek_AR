using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets;
using UnityEngine.SceneManagement;

public class MessageHelper : MonoBehaviour
{



    public enum DialogResult
    {
        YES,
        NO,
        NULL
    };

    static DialogResult dialogResult;

    static public void ConfirmDialog(string title, string message)
    {

        dialogResult = DialogResult.NULL;

        GameObject ShadePanel = GameObject.Find("ShadePanel(Clone)");
        if (ShadePanel == null)
        {
            GameObject ShadePanelTemplate = Resources.Load("prefabs/ShadePanel") as GameObject;
            ShadePanel = Instantiate(ShadePanelTemplate) as GameObject;
            ShadePanel.transform.SetParent(GameObject.Find("Canvas").transform, false);
        }
        else
        {
            Destroy(ShadePanel.transform.GetChild(0));
        }


        GameObject DialogBoxTemplate = Resources.Load("prefabs/Dialog Box") as GameObject;
        GameObject DialogBox = Instantiate(DialogBoxTemplate) as GameObject;
        DialogBoxTemplate sampleButton = DialogBox.GetComponent<DialogBoxTemplate>();
        sampleButton.Title.text = title;
        sampleButton.Message.text = message;

        sampleButton.transform.SetParent(ShadePanel.transform, false);

        GameObject YesButtonTemplate = Resources.Load("prefabs/YesButton") as GameObject;

        GameObject YesButton = Instantiate(YesButtonTemplate) as GameObject;

        YesButton.GetComponent<Button>().onClick.AddListener(() => ClickYesButton());
        YesButton.transform.SetParent(sampleButton.Buttons.transform, false);


        GameObject NoButtonTemplate = Resources.Load("prefabs/NoButton") as GameObject;
        GameObject NoButton = Instantiate(NoButtonTemplate) as GameObject;
        NoButton.GetComponent<Button>().onClick.AddListener(() => ClickNoButton());
        NoButton.transform.SetParent(sampleButton.Buttons.transform, false);
    }
    static public void ConfirmDialog(string message)
    {
        dialogResult = DialogResult.NULL;

        GameObject ShadePanel = GameObject.Find("ShadePanel(Clone)");
        if (ShadePanel == null)
        {
            GameObject ShadePanelTemplate = Resources.Load("prefabs/ShadePanel") as GameObject;
            ShadePanel = Instantiate(ShadePanelTemplate) as GameObject;
            ShadePanel.transform.SetParent(GameObject.Find("Canvas").transform, false);
        }
        else
        {
            Destroy(ShadePanel.transform.GetChild(0).gameObject);
        }


        GameObject DialogBoxTemplate = Resources.Load("prefabs/Dialog Box") as GameObject;
        GameObject DialogBox = Instantiate(DialogBoxTemplate) as GameObject;
        DialogBoxTemplate sampleButton = DialogBox.GetComponent<DialogBoxTemplate>();
        sampleButton.Message.text = message;

        sampleButton.transform.SetParent(ShadePanel.transform, false);

        GameObject YesButtonTemplate = Resources.Load("prefabs/YesButton") as GameObject;

        GameObject YesButton = Instantiate(YesButtonTemplate) as GameObject;

        YesButton.GetComponent<Button>().onClick.AddListener(() => ClickYesButton());
        YesButton.transform.SetParent(sampleButton.Buttons.transform, false);


        GameObject NoButtonTemplate = Resources.Load("prefabs/NoButton") as GameObject;
        GameObject NoButton = Instantiate(NoButtonTemplate) as GameObject;
        NoButton.GetComponent<Button>().onClick.AddListener(() => ClickNoButton());
        NoButton.transform.SetParent(sampleButton.Buttons.transform, false);
    }

    static void ClickYesButton()
    {
        Destroy(GameObject.Find("ShadePanel(Clone)"));
        dialogResult = DialogResult.YES;
    }

    static void ClickNoButton()
    {
        Destroy(GameObject.Find("ShadePanel(Clone)"));
        dialogResult = DialogResult.NO;
    }

    static public DialogResult GetDialogResult()
    {
        return dialogResult;
    }

    static public void MessageDialog(string title, string message)
    {
        dialogResult = DialogResult.NULL;

        GameObject ShadePanel = GameObject.Find("ShadePanel(Clone)");
        if (ShadePanel == null)
        {
            GameObject ShadePanelTemplate = Resources.Load("prefabs/ShadePanel") as GameObject;
            ShadePanel = Instantiate(ShadePanelTemplate) as GameObject;
            ShadePanel.transform.SetParent(GameObject.Find("Canvas").transform, false);
        }
        else
        {
            Destroy(ShadePanel.transform.GetChild(0).gameObject);
        }
        GameObject DialogBoxTemplate = Resources.Load("prefabs/Dialog Box") as GameObject;
        GameObject DialogBox = Instantiate(DialogBoxTemplate) as GameObject;
        DialogBoxTemplate sampleButton = DialogBox.GetComponent<DialogBoxTemplate>();
        sampleButton.Title.text = title;
        sampleButton.Message.text = message;
        sampleButton.transform.SetParent(ShadePanel.transform, false);

        GameObject OKButtonTemplate = Resources.Load("prefabs/OKButton") as GameObject;

        GameObject OKButton = Instantiate(OKButtonTemplate) as GameObject;
        OKButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            Destroy(GameObject.Find("ShadePanel(Clone)"));
        });
        OKButton.transform.SetParent(sampleButton.Buttons.transform, false);
    }

    static public void MessageDialog(string message)
    {
        dialogResult = DialogResult.NULL;

        GameObject ShadePanel = GameObject.Find("ShadePanel(Clone)");
        if (ShadePanel == null)
        {
            GameObject ShadePanelTemplate = Resources.Load("prefabs/ShadePanel") as GameObject;
            ShadePanel = Instantiate(ShadePanelTemplate) as GameObject;
            ShadePanel.transform.SetParent(GameObject.Find("Canvas").transform, false);
        }
        else
        {
            Destroy(ShadePanel.transform.GetChild(0).gameObject);
        }
        GameObject DialogBoxTemplate = Resources.Load("prefabs/Dialog Box") as GameObject;
        GameObject DialogBox = Instantiate(DialogBoxTemplate) as GameObject;
        DialogBoxTemplate sampleButton = DialogBox.GetComponent<DialogBoxTemplate>();
        sampleButton.Title.text = "";
        sampleButton.Message.text = message;
        sampleButton.transform.SetParent(ShadePanel.transform, false);

        GameObject OKButtonTemplate = Resources.Load("prefabs/OKButton") as GameObject;

        GameObject OKButton = Instantiate(OKButtonTemplate) as GameObject;
        OKButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            Destroy(GameObject.Find("ShadePanel(Clone)"));
        });
        OKButton.transform.SetParent(sampleButton.Buttons.transform, false);

    }

    static public void SuccessDialog(string title, string message)
    {
        dialogResult = DialogResult.NULL;

        GameObject ShadePanel = GameObject.Find("ShadePanel(Clone)");
        if (ShadePanel == null)
        {
            GameObject ShadePanelTemplate = Resources.Load("prefabs/ShadePanel") as GameObject;
            ShadePanel = Instantiate(ShadePanelTemplate) as GameObject;
            ShadePanel.transform.SetParent(GameObject.Find("Canvas").transform, false);
        }
        else
        {
            Destroy(ShadePanel.transform.GetChild(0).gameObject);
        }
        GameObject DialogBoxTemplate = Resources.Load("prefabs/Success Dialog") as GameObject;
        GameObject DialogBox = Instantiate(DialogBoxTemplate) as GameObject;
        DialogBoxTemplate sampleButton = DialogBox.GetComponent<DialogBoxTemplate>();
        sampleButton.Title.text = title;
        sampleButton.Message.text = message;
        sampleButton.transform.SetParent(ShadePanel.transform, false);

        GameObject OKButtonTemplate = Resources.Load("prefabs/SuccessOKButton") as GameObject;

        GameObject OKButton = Instantiate(OKButtonTemplate) as GameObject;
        OKButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            Destroy(GameObject.Find("ShadePanel(Clone)"));
        });
        OKButton.transform.SetParent(sampleButton.Buttons.transform, false);
    }

    static public void SuccessDialog(string message)
    {
        dialogResult = DialogResult.NULL;

        GameObject ShadePanel = GameObject.Find("ShadePanel(Clone)");
        if (ShadePanel == null)
        {
            GameObject ShadePanelTemplate = Resources.Load("prefabs/ShadePanel") as GameObject;
            ShadePanel = Instantiate(ShadePanelTemplate) as GameObject;
            ShadePanel.transform.SetParent(GameObject.Find("Canvas").transform, false);
        }
        else
        {
            Destroy(ShadePanel.transform.GetChild(0).gameObject);
        }
        GameObject DialogBoxTemplate = Resources.Load("prefabs/Success Dialog") as GameObject;
        GameObject DialogBox = Instantiate(DialogBoxTemplate) as GameObject;
        DialogBoxTemplate sampleButton = DialogBox.GetComponent<DialogBoxTemplate>();
        sampleButton.Title.text = "";
        sampleButton.Message.text = message;
        sampleButton.transform.SetParent(ShadePanel.transform, false);

        GameObject OKButtonTemplate = Resources.Load("prefabs/SuccessOKButton") as GameObject;

        GameObject OKButton = Instantiate(OKButtonTemplate) as GameObject;
        OKButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            Destroy(GameObject.Find("ShadePanel(Clone)"));
        });
        OKButton.transform.SetParent(sampleButton.Buttons.transform, false);

    }

    static public void ErrorDialog(string title, string message)
    {
        dialogResult = DialogResult.NULL;

        GameObject ShadePanel = GameObject.Find("ShadePanel(Clone)");
        if (ShadePanel == null)
        {
            GameObject ShadePanelTemplate = Resources.Load("prefabs/ShadePanel") as GameObject;
            ShadePanel = Instantiate(ShadePanelTemplate) as GameObject;
            ShadePanel.transform.SetParent(GameObject.Find("Canvas").transform, false);
        }
        else
        {
            Destroy(ShadePanel.transform.GetChild(0).gameObject);
        }
        GameObject DialogBoxTemplate = Resources.Load("prefabs/Error Dialog") as GameObject;
        GameObject DialogBox = Instantiate(DialogBoxTemplate) as GameObject;
        DialogBoxTemplate sampleButton = DialogBox.GetComponent<DialogBoxTemplate>();
        sampleButton.Title.text = title;
        sampleButton.Message.text = message;
        sampleButton.transform.SetParent(ShadePanel.transform, false);

        GameObject OKButtonTemplate = Resources.Load("prefabs/ErrorOKButton") as GameObject;

        GameObject OKButton = Instantiate(OKButtonTemplate) as GameObject;
        OKButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            Destroy(GameObject.Find("ShadePanel(Clone)"));
        });
        OKButton.transform.SetParent(sampleButton.Buttons.transform, false);
    }

    static public void ErrorDialog(string message)
    {
        dialogResult = DialogResult.NULL;

        GameObject ShadePanel = GameObject.Find("ShadePanel(Clone)");
        if (ShadePanel == null)
        {
            GameObject ShadePanelTemplate = Resources.Load("prefabs/ShadePanel") as GameObject;
            ShadePanel = Instantiate(ShadePanelTemplate) as GameObject;
            ShadePanel.transform.SetParent(GameObject.Find("Canvas").transform, false);
        }
        else
        {
            Destroy(ShadePanel.transform.GetChild(0).gameObject);
        }
        GameObject DialogBoxTemplate = Resources.Load("prefabs/Error Dialog") as GameObject;
        GameObject DialogBox = Instantiate(DialogBoxTemplate) as GameObject;
        DialogBoxTemplate sampleButton = DialogBox.GetComponent<DialogBoxTemplate>();
        sampleButton.Title.text = "";
        sampleButton.Message.text = message;
        sampleButton.transform.SetParent(ShadePanel.transform, false);

        GameObject OKButtonTemplate = Resources.Load("prefabs/ErrorOKButton") as GameObject;

        GameObject OKButton = Instantiate(OKButtonTemplate) as GameObject;
        OKButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            Destroy(GameObject.Find("ShadePanel(Clone)"));
        });
        OKButton.transform.SetParent(sampleButton.Buttons.transform, false);

    }

    public static void OnError(string error, string transactionId)
    {
        LoadingManager.hideLoadingIndicator(GameObject.Find("Loading Panel"));
        MessageHelper.ErrorDialog(ConstantClass.Msg_ErrorTitle, error);
        Debug.Log("WWW Error: " + error);
    }
    public static void OnTimeOut(string transactionId)
    {
        LoadingManager.hideLoadingIndicator(GameObject.Find("Loading Panel"));
        MessageHelper.ErrorDialog(ConstantClass.Msg_TimeOut);
        Debug.Log(ConstantClass.Msg_TimeOut);
    }
}
