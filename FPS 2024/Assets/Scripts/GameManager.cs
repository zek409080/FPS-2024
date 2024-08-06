using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviourPunCallbacks
{
    #region Singleton

    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    #endregion

    const string playerPrefabPath = "Prefabs/Player";

    [SerializeField]UIManager manager;

    int playersInGame;
    List<PlayerController> playerList = new List<PlayerController>();
    PlayerController playerLocal;

    

    private void Start()
    {
        photonView.RPC("AddPlayer", RpcTarget.AllBuffered);
        timer = timerbase;

    }
    private void Update()
    {
        if(SceneManager.GetActiveScene().name == "SampleScene")
        {
            photonView.RPC("TimerGameOverRPC", RpcTarget.All);
        }   
        
    }

    private void CreatePlayer()
    {
        PlayerController player = NetworkManager.instance.Instantiate(playerPrefabPath, new Vector3(30, 1, 30), Quaternion.identity).GetComponent<PlayerController>();
        player.photonView.RPC("Initialize", RpcTarget.All);
    }

    [PunRPC]
    private void AddPlayer()
    {
        playersInGame++;
        if (playersInGame == PhotonNetwork.PlayerList.Length)
        {
            CreatePlayer();
        }
    }

    [SerializeField] float timerbase;
    float timer;

    [PunRPC]
    void TimerGameOverRPC()
    {
        timer -= Time.deltaTime;
        manager.UpdateTextTimer(timer);
        if (timer < 0)
        {
            Time.timeScale = 0;
        }
    }
}