using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EastereggTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void playsound()
    {
        GetComponent<AudioSource>().Play();
    }

    public void switchscene()
    {
        SceneManager.LoadScene("SandboxScene");
    }
}
