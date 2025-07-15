using System.Collections;
using UnityEngine;

public class BattleLoader : MonoBehaviour
{
    [SerializeField] GameObject templateBattlePanel;
    //[SerializeField] PanelManager panelManager;

    [SerializeField] Transform playerSpot;
    [SerializeField] Transform enemySpot;
    [SerializeField] ActionResolver actionResolver;

    public bool IsStarted = false;

    [Header("½ÇÉ«Ä£°å (ScriptableObject)")]
    public CharacterSO playerSO;
    public CharacterSO enemySO;

    public Character Player;
    public Character Enemy;

    private IEnumerator Start()
    {
        while (RingLibrary.I == null)
        {
            yield return null;
        }
        enemySO = BattleSession.Encounter.Characters[0];

        CharacterSO playerSoCopy = Instantiate(playerSO);
        yield return new WaitUntil(() => GameSystem.I.IsStarted);

        playerSoCopy.RingIds = GameSystem.I.Run.WornRingIds;
        Player = new Character(playerSoCopy);
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

        IsStarted = true;
    }
}
