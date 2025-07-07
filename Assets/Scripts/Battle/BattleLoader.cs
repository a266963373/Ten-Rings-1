using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleLoader : MonoBehaviour
{
    [SerializeField] GameObject templateBattlePanel;
    //[SerializeField] PanelManager panelManager;

    [SerializeField] Transform playerSpot;
    [SerializeField] Transform enemySpot;
    [SerializeField] ActionResolver actionResolver;

    [Header("½ÇÉ«Ä£°å (ScriptableObject)")]
    public CharacterSO playerSO;
    public CharacterSO enemySO;

    public Character Player;
    public Character Enemy;

    private void Awake()
    {
        enemySO = BattleSession.Encounter.Characters[0];

        Player = new Character(playerSO);
        Enemy = new Character(enemySO);
        Player.IsPlayerSide = true;
        Player.IsPlayerControlled = true;

        CharacterBattlePanel playerPanel = Instantiate(templateBattlePanel, playerSpot).GetComponent<CharacterBattlePanel>();
        CharacterBattlePanel enemyPanel = Instantiate(templateBattlePanel, enemySpot).GetComponent<CharacterBattlePanel>();
        //panelManager.Panels.Add(playerPanel);
        //panelManager.Panels.Add(enemyPanel);
        playerPanel.Character = Player;
        enemyPanel.Character = Enemy;
        playerPanel.Initialize(actionResolver);
        enemyPanel.Initialize(actionResolver);
    }
}
