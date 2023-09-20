using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Scrapbook : MonoBehaviour
{
    private List<SpawnableObjectSO> spawnableParts;

    private List<List<SpawnableObjectSO>> totUppslag = new List<List<SpawnableObjectSO>>();

    private Dictionary<GameObject, GameObject> buttonPartConnection = new Dictionary<GameObject, GameObject>();

    [SerializeField] GameObject buttonPrefab;
    [SerializeField] RectTransform leftPage;
    [SerializeField] RectTransform rightPage;

    private int pageIndex = 0;

    private void Awake()
    {
        spawnableParts = new List<SpawnableObjectSO>(Resources.LoadAll<SpawnableObjectSO>("SpawnableParts/ScriptableObjects"));
    }

    private void Start()
    {
        CreatePages();
        SetupUppslag();
        gameObject.SetActive(false);
    }

    private void CreatePages()
    {
        int index = 0;

        List<SpawnableObjectSO> page = new List<SpawnableObjectSO>();// create a new page

        foreach (SpawnableObjectSO partSO in spawnableParts)
        {
            if (index >= 36) //36 is the max amount in a uppslag so add to uppslag and clear
            {
                totUppslag.Add(page);
                page = new List<SpawnableObjectSO>();
                index = 0;
            }
            //add item to page
            page.Add(partSO);
            index++;
        }
        if (page.Count > 0)//add the last items to page even if its not a whole page
            totUppslag.Add(page); 
    }

    private void SetupUppslag()
    {
        if (leftPage.childCount != 0)
            foreach (RectTransform child in leftPage) { Destroy(child.gameObject); }
        if (rightPage.childCount != 0)
            foreach (RectTransform child in rightPage) { Destroy(child.gameObject); }

        int spawnedItems = 0;

        RectTransform pageToAddTo = leftPage;

        foreach (SpawnableObjectSO part in totUppslag[pageIndex])
        {
            if (spawnedItems >= 18) //18 is max amount in a page so switch pages
            {
                pageToAddTo = rightPage;
            }

            CreateButton(pageToAddTo, part);

            spawnedItems++;
        }
    }

    private void CreateButton(RectTransform page, SpawnableObjectSO partSO)
    {
        GameObject button = Instantiate(buttonPrefab, page);

        buttonPartConnection.Add(button, partSO.partPrefab);

        button.GetComponent<Button>().onClick.AddListener(() => ObjectSpawner.SpawnObject(buttonPartConnection[button]));

        //button.GetComponent<Image>().sprite = partSO.partSprite;
    }

    public void OpenBook()
    {
        gameObject.SetActive(true);
    }

    [ContextMenu("FlipToNextPage")]
    public void FlipToNextPage()
    {
        pageIndex++;
        pageIndex = Mathf.Clamp(pageIndex, 0, totUppslag.Count - 1);
        SetupUppslag();
    }

    [ContextMenu("FlipToLastPage")]
    public void FlipToLastPage()
    {
        pageIndex--;
        pageIndex = Mathf.Clamp(pageIndex, 0, totUppslag.Count - 1);
        SetupUppslag();
    }

    public void CloseBook()
    {
        gameObject.SetActive(false);
    }
}