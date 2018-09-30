using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoopChangeView : MonoBehaviour {

    public GameObject otherScript;
    public ArrayList textures = new ArrayList();
    private int currentViewIndex = 0;
    public GameObject theEarth;

    public float lastRan = Time.time;

    private void changeTexture(Texture2D texture)
    {
        Renderer renderer = GetComponent<Renderer>();

        Debug.Log(renderer.material.mainTexture);

        renderer.material.mainTexture = texture;
    }

    public static Texture2D LoadPNG(string filePath)
    {

        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            Debug.Log(fileData);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }
        return tex;
    }

    public void Awake()
    {
        string[] imagePaths = { "Assets/one.jpg", "Assets/two.jpg", "Assets/three.jpg", "Assets/four.jpg", "Assets/five.jpg" };

        foreach (string path in imagePaths)
        {
            textures.Add(LoadPNG(path));
            Debug.Log(textures[textures.Count - 1]);
        }

        Debug.Log("This ran.");
        //changeTexture((Texture2D)textures[2]);
        //changeTexture((Texture2D)textures[4]);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(lastRan + 10 < Time.time)
        {
            currentViewIndex += 1;
            if (currentViewIndex > textures.Count - 1)
            {
                currentViewIndex = 0;
            }
            
            changeTexture((Texture2D)textures[currentViewIndex]);

            lastRan = Time.time;
        }
	}
}
