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

    private Dictionary<SpawnableObjectSO, List<SpawnableObjectSO>> RecipePage = new Dictionary<SpawnableObjectSO, List<SpawnableObjectSO>>();

    private Dictionary<GameObject, GameObject> buttonPartConnection = new Dictionary<GameObject, GameObject>();

    [SerializeField] GameObject buttonPrefab;
    [SerializeField] GameObject recipeButtonPrefab;

    [SerializeField] RectTransform recipePage;
    [SerializeField] TextMeshProUGUI recipeName;

    [SerializeField] RectTransform leftPage;
    [SerializeField] RectTransform rightPage;

    private int pageIndex = 0;

    private void Awake()
    {
        spawnableParts = new List<SpawnableObjectSO>(Resources.LoadAll<SpawnableObjectSO>("SpawnableParts/ScriptableObjects"));
        recipeItems = new List<SpawnableObjectSO>(Resources.LoadAll<SpawnableObjectSO>("SpawnableParts/ScriptableObjects/WholeItems"));
    }

    private void Start()
    {
        CreateRecipePages();
        CreatePagesAllItems();
        SetupOpenPage();

        gameObject.SetActive(false);
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
        if (leftPage.childCount != 0)
            foreach (RectTransform child in leftPage) { Destroy(child.gameObject); }
        if (rightPage.childCount != 0)
            foreach (RectTransform child in rightPage) { Destroy(child.gameObject); }

        int spawnedItems = 0;

        RectTransform pageToAddTo = leftPage;
        GameObject buttonToCreate = buttonPrefab;

        if (pageIndex < recipeItems.Count)
        {
            recipePage.gameObject.SetActive(true);
            leftPage.gameObject.SetActive(false);
            pageToAddTo = recipePage;
            buttonToCreate = recipeButtonPrefab;
           // recipeName = totPages[pageIndex,0].;
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
            }

            CreateButton(buttonToCreate, pageToAddTo, part);

            spawnedItems++;
        }
    }

    private void CreateButton(GameObject buttonToSpawn, RectTransform page, SpawnableObjectSO partSO)
    {
        GameObject button = Instantiate(buttonToSpawn, page);

        buttonPartConnection.Add(button, partSO.partPrefab);
        button.GetComponent<Button>().onClick.AddListener(() => ObjectSpawner.SpawnObject(buttonPartConnection[button]));

        //button.GetComponent<Image>().sprite = partSO.partSprite;
    }

    public void FlipToNextPage()
    {
        pageIndex++;
        pageIndex = Mathf.Clamp(pageIndex, 0, totPages.Count - 1);

        SetupOpenPage();
    }

    public void FlipToLastPage()
    {
        pageIndex--;
        pageIndex = Mathf.Clamp(pageIndex, 0, totPages.Count - 1);

        SetupOpenPage();
    }

    private void OnEnable() => OpenBook();
    private void OnDisable() => CloseBook();

    public void OpenBook()
    {
        gameObject.SetActive(true);
    }

    public void CloseBook()
    {
        gameObject.SetActive(false);
    }
}