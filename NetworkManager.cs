using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour
{
    string gameName = "Dodo_Car_Game";
    HostData[] hostData;
    bool refreshing;
    bool hostFull;
    public  GameObject player;
    public GameObject arrow;
    public Transform spawnPoint;
    public Button startServer;
    public Button refresh;
    public Button enter;
    public Text textEnter;
    public Text textStart;
    public Text textRefresh;
    public Text initialized;
   

    void Start()
    {
        enter.image.color = new Color(0, 0, 0, 0);
        textEnter.color = new Color(1, 0, 0, 0);
        initialized.color = new Color(0, 0, 0, 0);
    }

    void SpawnPlayer()
    {
        Network.Instantiate(player,spawnPoint.position, Quaternion.identity,0);
        Network.Instantiate(arrow, spawnPoint.position, Quaternion.identity, 0);
    }

   
    public void StartServer()
    {
        initialized.color = new Color(1, 0, 0, 1);
        Network.InitializeServer(12,24980,!Network.HavePublicAddress());
        MasterServer.RegisterHost(gameName, "Dodo_CarGame", "Testing_Dodo_CarGame");
        
    }

    void OnServerInitialized()
    {  
        SpawnPlayer();
        startServer.image.color = new Color(0, 0, 0, 0);
        refresh.image.color = new Color(0, 0, 0, 0);
        textStart.color = new Color(0, 0, 0, 0);
        textRefresh.color = new Color(0, 0, 0, 0);
        initialized.color = new Color(0, 0, 0, 0);
        refresh.enabled = false;
        startServer.enabled = false;
        refreshing = false;
    }

    IEnumerator Refresh()
    {
        yield return new WaitForSeconds(2f);
    }

    public void  RefreshList()
    {
        MasterServer.RequestHostList(gameName);
        StartCoroutine(Refresh());
        refreshing = true;
    }

    void Update()
    {
        if (refreshing)
        {
            Debug.Log(MasterServer.PollHostList().Length);
            hostData = MasterServer.PollHostList();
            if (hostData.Length > 0)
            {
                initialized.color = new Color(0, 0, 0, 0);
                enter.image.color = new Color(1, 1, 1, 1);
                textEnter.color = new Color(0, 0, 1, 1f);
                startServer.image.color = new Color(0, 0, 0, 0);
                refresh.image.color = new Color(0, 0, 0, 0);
                textStart.color = new Color(0, 0, 0, 0);
                textRefresh.color = new Color(0, 0, 0, 0);
                refresh.enabled = false;
                startServer.enabled = false;
                refreshing = false;
            }
        }
        
    }

    void OnConnectedToServer()          
    {
        SpawnPlayer();
        
        enter.image.color = new Color(0, 0, 0, 0);
        textEnter.color = new Color(0, 0, 0, 0);
        enter.enabled = false;
    }

    public void StartGame()
    {
        if (!Network.isClient && !Network.isServer && hostData.Length > 0)
        {
            Network.Connect(hostData[0]);
            
        }
        
    }
} 
