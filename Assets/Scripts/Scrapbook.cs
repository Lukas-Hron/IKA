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

    [SerializeField] Animator bookAnimator;
    [SerializeField] Animator colliderAnimator;

    [SerializeField] GameObject buttonPrefab;
    [SerializeField] GameObject recipeButton;

    [SerializeField] RectTransform recipePage;
    [SerializeField] TextMeshProUGUI recipeName;

    [SerializeField] RectTransform leftPage;
    [SerializeField] RectTransform rightPage;

    private int pageIndex = 0;
    private bool settingUpPage;

    private void Awake()
    {
        spawnableParts = new List<SpawnableObjectSO>(Resources.LoadAll<SpawnableObjectSO>("SpawnableParts/ScriptableObjects"));
        recipeItems = new List<SpawnableObjectSO>(Resources.LoadAll<SpawnableObjectSO>("SpawnableParts/WholeItemsSO"));

        buttonPartConnection.Add(recipeButton, recipeItems[0].partPrefab);
    }

    private void Start()
    {
        CreateRecipePages();
        CreatePagesAllItems();
        SetupOpenPage();

        CloseBook();
    }

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

            if (index >= 36) //36 is the max amount in a uppslag so add to uppslag and clear
            {
                totPages.Add(page);
                page = new List<SpawnableObjectSO>();
                index = 0;
            }
            //add item to page
            page.Add(partSO);
            index++;
        }

        if (page.Count > 0)//add the last items to page even if its not a whole page
            totPages.Add(page);
    }

    private void SetupOpenPage()
    {
        if (settingUpPage) return;
        settingUpPage = true;

        // Reset the pages
        if (leftPage.childCount != 0)
            foreach (RectTransform child in leftPage) { Destroy(child.gameObject); }
        if (rightPage.childCount != 0)
            foreach (RectTransform child in rightPage) { Destroy(child.gameObject); }

        // Reset dictionary and add back the recipebutton
        buttonPartConnection.Clear();
        buttonPartConnection.Add(recipeButton, recipeItems[0].partPrefab);

        int spawnedItems = 0;
        bool isRecipePage = false;

        RectTransform pageToAddTo = leftPage;

        if (pageIndex < recipeItems.Count)
        {
            pageToAddTo = recipePage;
            isRecipePage = true;

            recipePage.gameObject.SetActive(true);
            leftPage.gameObject.SetActive(false);

            recipeName.text = totPages[pageIndex][0].name;

            spawnedItems = 17; //main page should only have the one item on left page
        }
        else
        {
            recipePage.gameObject.SetActive(false);
            leftPage.gameObject.SetActive(true);
        }

        foreach (SpawnableObjectSO part in totPages[pageIndex])
        {
            if (spawnedItems >= 18) //18 is max amount in a page so switch pages
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

        button.GetComponent<Button>().onClick.AddListener(() => ObjectSpawner.SpawnObject(buttonPartConnection[button]));

        //button.GetComponent<Image>().sprite = partSO.partSprite;
    }

    public void FlipToNextPage()
    {
        if (!gameObject.activeInHierarchy) return;

        int pageIndexBefore = pageIndex;
        pageIndex++;

        pageIndex = Mathf.Clamp(pageIndex, 0, totPages.Count - 1);

        if (pageIndex != pageIndexBefore)
            bookAnimator.SetTrigger("FlipToNext");

        StartCoroutine(SetupPageDelay());
    }

    public void FlipToPreviousPage()
    {
        if (!gameObject.activeInHierarchy) return;

        int pageIndexBefore = pageIndex;
        pageIndex--;

        pageIndex = Mathf.Clamp(pageIndex, 0, totPages.Count - 1);

        if (pageIndex != pageIndexBefore)
            bookAnimator.SetTrigger("FlipToPrevious");

        StartCoroutine(SetupPageDelay());
    }

    private IEnumerator SetupPageDelay()
    {
        yield return new WaitForSeconds(0.4f);
        SetupOpenPage();
    }

    private void OnEnable() => OpenBook();
    private void OnDisable() => CloseBook();

    public void OpenBook()
    {
        gameObject.SetActive(true);

        bookAnimator.SetTrigger("Open");
        colliderAnimator.SetTrigger("Open");
    }

    public void CloseBook()
    {
        gameObject.SetActive(false);

        bookAnimator.SetTrigger("Close");
        colliderAnimator.SetTrigger("Close");
    }
}