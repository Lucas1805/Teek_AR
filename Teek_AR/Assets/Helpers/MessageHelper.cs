﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MessageHelper : MonoBehaviour {

    public enum DialogResult
    {
        YES,
        NO,
        NULL
    };

    static DialogResult dialogResult;

    static public void ConfirmDialog(string title,string message)
    {
        dialogResult = DialogResult.NULL;

        GameObject ShadePanelTemplate = Resources.Load("prefabs/ShadePanel") as GameObject;
        GameObject ShadePanel = Instantiate(ShadePanelTemplate) as GameObject;
        ShadePanel.transform.SetParent(GameObject.Find("Canvas").transform, false);

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

        GameObject ShadePanelTemplate = Resources.Load("prefabs/ShadePanel") as GameObject;
        GameObject ShadePanel = Instantiate(ShadePanelTemplate) as GameObject;
        ShadePanel.transform.SetParent(GameObject.Find("Canvas").transform, false);

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

    static public void MessageDialog(string title,string message)
    {
        dialogResult = DialogResult.NULL;

        GameObject ShadePanelTemplate = Resources.Load("prefabs/ShadePanel") as GameObject;
        GameObject ShadePanel = Instantiate(ShadePanelTemplate) as GameObject;
        ShadePanel.transform.SetParent(GameObject.Find("Canvas").transform, false);


        GameObject DialogBoxTemplate = Resources.Load("prefabs/Dialog Box") as GameObject;
        GameObject DialogBox = Instantiate(DialogBoxTemplate) as GameObject;
        DialogBoxTemplate sampleButton = DialogBox.GetComponent<DialogBoxTemplate>();
        sampleButton.Title.text = title;
        sampleButton.Message.text = message;
        sampleButton.transform.SetParent(ShadePanel.transform, false);

        GameObject OKButtonTemplate = Resources.Load("prefabs/OKButton") as GameObject;

        GameObject OKButton = Instantiate(OKButtonTemplate) as GameObject;
        OKButton.GetComponent<Button>().onClick.AddListener(() => {
            Destroy(GameObject.Find("ShadePanel(Clone)"));
        });
        OKButton.transform.SetParent(sampleButton.Buttons.transform, false);
    }

    static public void MessageDialog(string message)
    {
        dialogResult = DialogResult.NULL;

        GameObject DialogBoxTemplate = Resources.Load("prefabs/Dialog Box") as GameObject;
        GameObject DialogBox = Instantiate(DialogBoxTemplate) as GameObject;
        DialogBoxTemplate sampleButton = DialogBox.GetComponent<DialogBoxTemplate>();
        sampleButton.Title.text = "";
        sampleButton.Message.text = message;
        sampleButton.transform.SetParent(GameObject.Find("Canvas").transform, false);

        GameObject OKButtonTemplate = Resources.Load("prefabs/OKButton") as GameObject;

        GameObject OKButton = Instantiate(OKButtonTemplate) as GameObject;
        OKButton.transform.SetParent(sampleButton.Buttons.transform, false);

    }


    static public void LoadingDialog(string message)
    {

        GameObject ShadePanelTemplate = Resources.Load("prefabs/ShadePanel") as GameObject;
        GameObject ShadePanel = Instantiate(ShadePanelTemplate) as GameObject;
        ShadePanel.transform.SetParent(GameObject.Find("Canvas").transform, false);

        GameObject LoadingBoxTemplate = Resources.Load("prefabs/Loading Box") as GameObject;
        GameObject LoadingBox = Instantiate(LoadingBoxTemplate) as GameObject;
        LoadingBoxTemplate sampleButton = LoadingBox.GetComponent<LoadingBoxTemplate>();
        sampleButton.transform.SetParent(ShadePanel.transform, false);

    }

    static public void CloseDialog()
    {
        Destroy(GameObject.Find("ShadePanel(Clone)"));
    }
}
