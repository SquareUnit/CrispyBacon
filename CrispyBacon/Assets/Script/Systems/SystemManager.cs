using UnityEngine;

namespace MainSystem
{
    public class SystemManager : MonoBehaviour
    {
        private SystemManager instance;
        private static readonly object padlock = new object();
        public GameObject riderPrefab;

        private void Start()
        {
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

        #region Properties
        public SystemManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = this;
                            DontDestroyOnLoad(this);
                        }
                    }
                }
                return instance;
            }
        }
        #endregion
    }
}

/*

GameObject rider = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Rider.prefab", typeof(GameObject)));
rider.transform.position = new Vector3(0, 0, 0);
rider.GetComponent<RiderController>().inputSystem = GetComponent<InputSystem>();

*/