using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public List<PlayerBehaviour> Players;

    public HorizontalLayoutGroup ListOfPlayersLife;
    public PlayerLifeUI PlayerLifeUIPrefab;

    public Dictionary<PlayerBehaviour, PlayerLifeUI> PlayerLifeUIList = new Dictionary<PlayerBehaviour, PlayerLifeUI>();

    // Start is called before the first frame update
    void Start()
    {
        int i = 1;
        foreach(var player in Players)
        {
            PlayerLifeUI life = Instantiate(PlayerLifeUIPrefab, ListOfPlayersLife.transform);
            PlayerLifeUIList.Add(player, life);
            life.Setup(player, i);
            ++i;
        }

        PlayerLifeUIPrefab.gameObject.SetActive(false);
    }

    internal void OnPlayerHit(PlayerBehaviour player)
    {
        PlayerLifeUIList[player].UpdateLife(player.Life);
    }
}
