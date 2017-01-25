using UnityEngine;
#if !UNITY_5_2
using UnityEngine.SceneManagement;
#endif
using System.Collections;
using System.Collections.Generic;

public class NewSpawnPool : MonoBehaviour {

    public static NewSpawnPool instance;

    public SpawnPoolSettings[] settings;

    public bool shouldPreload;

	bool splashScreenDone;
	bool loadingDone;
    bool didPreload;

    private Dictionary<string, List<GameObject>> spawnPoolObjects;

	// Use this for initialization
	void Start () {
        if (instance != null)
            Destroy(gameObject);
        else {
            DontDestroyOnLoad(gameObject);
            spawnPoolObjects = new Dictionary<string, List<GameObject>>();
            instance = this;
        }
	}
	
	// Update is called once per frame
	void Update () {
	    if(!didPreload && shouldPreload)
        {
            didPreload = true;
            StartCoroutine(preload());
        }
	}

    IEnumerator preload()
    {
        yield return null;
        for (int k = 0; k < settings.Length; k++)
        {
            SpawnPoolSettings setting = settings[k];
            for (int i = 0; i < setting.spawnItemList.Count; i++)
            {
                SpawnItem item = setting.spawnItemList[i];
                for (int j = 0; j < item.amount; j++)
                {
                    createGameObject(item.prefab);
                }
            }
        }
        yield return null;
		if(splashScreenDone){
            LoadNextScene();
		}
		loadingDone = true;
    }

    GameObject createGameObject(GameObject prefab)
    {
        GameObject gameObject = Instantiate(prefab);
        gameObject.SetActive(true);
        gameObject.SetActive(false);
        gameObject.transform.parent = transform;
        addToPool(prefab.name, gameObject);
        return gameObject;
    }

    void addToPool(string name, GameObject gameObject)
    {
        if(!spawnPoolObjects.ContainsKey(name))
        {
            spawnPoolObjects.Add(name, new List<GameObject>());
        }
        spawnPoolObjects[name].Add(gameObject);
    }

    public GameObject getGameObject(GameObject prefab)
    {
        if (spawnPoolObjects.ContainsKey(prefab.name))
        {
            for (int i = 0; i < spawnPoolObjects[prefab.name].Count; i++)
            {
                if (!spawnPoolObjects[prefab.name][i].activeInHierarchy)
                {
                    spawnPoolObjects[prefab.name][i].SetActive(true);
                    return spawnPoolObjects[prefab.name][i];
                }
            }
        }
        GameObject spawned = createGameObject(prefab);
        spawned.SetActive(true);
        return spawned; 
    }

    public void DisableAllGameObjects()
    {
        foreach (List<GameObject> spawnedObjects in spawnPoolObjects.Values)
        {
            for (int i = 0; i < spawnedObjects.Count; i++)
            {
                GameObject spawnedObject = spawnedObjects[i];
                spawnedObject.SetActive(false);
            }
        }
    }

    void LoadNextScene()
    {
        if (NiceSceneTransition.instance != null) {
            NiceSceneTransition.instance.LoadScene("GamePlay");
        }
        else {
#if UNITY_5_2
			Application.LoadLevel("GamePlay");
#else
            SceneManager.LoadScene("GamePlay", LoadSceneMode.Single);
#endif
        }
    }

	public void SplashScreenDone()
	{
		splashScreenDone = true;
		if(loadingDone){
            LoadNextScene();
        }
	}
}
