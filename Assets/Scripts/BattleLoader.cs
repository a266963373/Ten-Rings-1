using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleLoader : MonoBehaviour
{
    [SerializeField] GameObject templateBattlePanel;
    [SerializeField] Transform playerSpot;
    [SerializeField] Transform enemySpot;

    [Header("½ÇÉ«Ä£°å (ScriptableObject)")]
    public CharacterSO playerSO;
    public CharacterSO enemySO;

    public Character Player;
    public Character Enemy;

    private void Awake()
    {
        Player = new Character(playerSO);
        Enemy = new Character(enemySO);
        CharacterBattlePanel playerPanel = Instantiate(templateBattlePanel, playerSpot).GetComponent<CharacterBattlePanel>();
        CharacterBattlePanel enemyPanel = Instantiate(templateBattlePanel, enemySpot).GetComponent<CharacterBattlePanel>();
        playerPanel.Character = Player;
        enemyPanel.Character = Enemy;
        playerPanel.Initialize();
        enemyPanel.Initialize();
    }
}
