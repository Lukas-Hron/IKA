using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class Scrapbook : MonoBehaviour
{
    private List<SpawnableObjectSO> spawnableParts;
    private List<SpawnableObjectSO> recipeItems;

    private List<List<SpawnableObjectSO>> totPages = new List<List<SpawnableObjectSO>>();

    private Dictionary<GameObject, GameObject> buttonPartConnection = new Dictionary<GameObject, GameObject>();

    private ObjectSpawner objectSpawner;

    [SerializeField] GameObject canvas;

    [SerializeField] Animator bookAnimator;
    [SerializeField] Animator bookAnimator2;

    [SerializeField] GameObject buttonPrefab;
    [SerializeField] GameObject recipeButton;

    [SerializeField] RectTransform recipePage;
    [SerializeField] TextMeshProUGUI recipeName;
    [SerializeField] Image recipeDecorImage;

    [SerializeField] RectTransform leftPage;
    [SerializeField] RectTransform rightPage;

    [SerializeField] Button flipNextButton;
    [SerializeField] Button flipPreviousButton;

    private SoundPlayer soundScript;

    private int pageIndex = 0;
    private int allPartsPages = 0;

    private int maxItemsPerPage = 10;
    private bool settingUpPage;

    private void Awake()
    {
        spawnableParts = new List<SpawnableObjectSO>(Resources.LoadAll<SpawnableObjectSO>("SpawnableParts/ScriptableObjects"));
        recipeItems = new List<SpawnableObjectSO>(Resources.LoadAll<SpawnableObjectSO>("SpawnableParts/WholeItemsSO"));

        buttonPartConnection.Add(recipeButton, recipeItems[0].partPrefab);
        objectSpawner = GetComponent<ObjectSpawner>();

        soundScript = GetComponent<SoundPlayer>();

        CreateRecipePages();
        CreatePagesAllItems();
        SetBlankPages();
    }

    private void Start()
    {
        flipPreviousButton.interactable = false;
        canvas.SetActive(false);
    }

    public void OpenBook()
    {
        Invoke(nameof(FlipPage), 0.4f);
        Invoke(nameof(ShowCanvas), 0.4f);

        bookAnimator.SetTrigger("Open");
        bookAnimator2.SetTrigger("LiftUp");

        soundScript.PlayAudio(1);
        SetBlankPages();

        Destroy(GameObject.Find("CheckHover"));
    }

    private void ShowCanvas() => canvas.SetActive(true);

    private void CreateRecipePages()
    {
        List<SpawnableObjectSO> page = new List<SpawnableObjectSO>();// create a new page

        foreach (SpawnableObjectSO mainItem in recipeItems)
        {
            //add the main item to a page
            page.Add(mainItem);
            foreach (SpawnableObjectSO partSO in spawnableParts)
            {
                if (partSO.name.Contains(mainItem.name))
                    page.Add(partSO);
            }

            if (page.Count > 0) //add both the main item and the parts as a page
                totPages.Add(page);

            page = new List<SpawnableObjectSO>(); //Reset page
        }
    }

    private void CreatePagesAllItems()
    {
        int index = 0;

        List<SpawnableObjectSO> page = new List<SpawnableObjectSO>();// create a new page

        foreach (SpawnableObjectSO partSO in spawnableParts)
        {
            if (index >= maxItemsPerPage * 2) //10 is the max amount in a page so 20 in uppslag so add to pages and clear
            {
                totPages.Add(page);
                page = new List<SpawnableObjectSO>();
                index = 0;
                allPartsPages++;
            }
            //add item to page
            page.Add(partSO);
            index++;
        }

        //add the last items to page even if its not a whole page
        if (page.Count > 0)
        {
            totPages.Add(page);
            allPartsPages++;
        }
    }

    private void SetBlankPages()
    {
        // Reset the pages
        if (leftPage.childCount != 0)
            foreach (RectTransform child in leftPage) { Destroy(child.gameObject); }
        if (rightPage.childCount != 0)
            foreach (RectTransform child in rightPage) { Destroy(child.gameObject); }

        // Reset dictionary and add back the recipebutton
        buttonPartConnection.Clear();
        buttonPartConnection.Add(recipeButton, recipeItems[0].partPrefab);
    }

    public void SetupOpenPage()
    {
        if (settingUpPage) return;
        settingUpPage = true;

        int spawnedItems = 0;
        bool isRecipePage = false;
        RectTransform pageToAddTo = leftPage;

        if (pageIndex < recipeItems.Count)// first pages are recipes then do all items
        {
            pageToAddTo = recipePage;
            isRecipePage = true;

            recipePage.gameObject.SetActive(true);
            leftPage.gameObject.SetActive(false);

            recipeName.text = totPages[pageIndex][0].name;
            recipeDecorImage.sprite = totPages[pageIndex][0].wholeItemDecorSprite;

            spawnedItems = maxItemsPerPage - 1; //main page should only have the one item on left page
        }
        else
        {
            recipePage.gameObject.SetActive(false);
            leftPage.gameObject.SetActive(true);
        }

        foreach (SpawnableObjectSO part in totPages[pageIndex])
        {
            if (spawnedItems >= maxItemsPerPage) //18 is max amount in a page so switch pages
            {
                pageToAddTo = rightPage;
                isRecipePage = false;
            }
            CreateButton(buttonPrefab, pageToAddTo, part, isRecipePage);

            spawnedItems++;
        }

        settingUpPage = false;
    }

    private void CreateButton(GameObject buttonToSpawn, RectTransform page, SpawnableObjectSO partSO, bool isRecipePage)
    {
        GameObject button;

        if (isRecipePage)
        {
            button = recipeButton;
            buttonPartConnection[button] = partSO.partPrefab;
        }
        else
        {
            button = Instantiate(buttonToSpawn, page);
            buttonPartConnection.Add(button, partSO.partPrefab);
        }

        button.GetComponent<Button>().onClick.AddListener(() => objectSpawner.SpawnObject(buttonPartConnection[button]));
        button.GetComponent<Image>().sprite = partSO.partSprite;
    }

    public void FlipToNextPage()
    {
        SetBlankPages();

        pageIndex++;
        pageIndex = Mathf.Clamp(pageIndex, 0, totPages.Count - 1);

        flipNextButton.interactable = (pageIndex == totPages.Count - 1) ? false : true;
        flipPreviousButton.interactable = true;

        bookAnimator.SetTrigger("FlipToNext");

        soundScript.PlayAudio(0);
        Invoke(nameof(FlipPage), 0.4f);
    }

    public void FlipToPreviousPage()
    {
        SetBlankPages();

        pageIndex--;
        pageIndex = Mathf.Clamp(pageIndex, 0, totPages.Count - 1);

        flipPreviousButton.interactable = (pageIndex == 0) ? false : true;
        flipNextButton.interactable = true;

        bookAnimator.SetTrigger("FlipToPrevious");

        soundScript.PlayAudio(0);
        Invoke(nameof(FlipPage), 0.4f);
    }

    private void FlipPage() => SetupOpenPage();


    //public void CloseBook()
    //{
    //    CancelInvoke();
    //    canvas.SetActive(false);
    //    Debug.Log("close");
    //    bookAnimator.SetTrigger("Close");
    //    bookAnimator2.SetTrigger("PutDown");

    //    soundScript.PlayAudio(2);
    //}
}