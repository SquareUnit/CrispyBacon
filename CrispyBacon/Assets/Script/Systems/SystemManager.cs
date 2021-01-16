using UnityEngine;

public class SystemManager : MonoBehaviour
{
    public static SystemManager instance;
    private static readonly object padlock = new object();
    public GameObject riderPrefab;

    private void Start()
    {
        instance = this;

        InitSystems();
        InitPlayers();
        InitUI();
    }

    public void Update()
    {

    }

    private void InitSystems()
    {

    }

    private void InitPlayers()
    {
        Instantiate(riderPrefab, new Vector3(0, 0, 0), Quaternion.identity);
    }

    private void InitUI()
    {

    }
}


/*

GameObject rider = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Rider.prefab", typeof(GameObject)));
rider.transform.position = new Vector3(0, 0, 0);
rider.GetComponent<RiderController>().inputSystem = GetComponent<InputSystem>();

*/