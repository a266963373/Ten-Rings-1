using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleLoader : MonoBehaviour
{
    public static BattleLoader I { get; private set; }
    [SerializeField] GameObject templateBattlePanel;
    [SerializeField] Transform playerSpot;
    [SerializeField] Transform enemySpot;
    [SerializeField] ActionResolver actionResolver;

    public bool IsStarted = false;

    [Header("角色模板 (ScriptableObject)")]
    public CharacterSO playerSO;
    public List<CharacterSO> enemySOs = new(); // 支持多个敌人

    public Character Player;
    public List<Character> Characters = new();  // living characters
    public List<Character> Allies
    {
        get { return Characters.Where(c => c.IsPlayerSide).ToList();}
    }

    public List<Character> Enemies
    {
        get { return Characters.Where(c => !c.IsPlayerSide).ToList();}
    }
    public List<Character> Deads = new();

    private void Awake()
    {
        I = this;
    }

    private IEnumerator Start()
    {
        while (RingLibrary.I == null)
        {
            yield return null;
        }

        // 加载所有敌人角色模板
        enemySOs = BattleSession.Encounter.Characters;

        CharacterSO playerSoCopy = Instantiate(playerSO);
        yield return new WaitUntil(() => GameSystem.I.IsStarted);

        playerSoCopy.RingIds = BattleSession.IsDebug ? GameSystem.I.CurrentSave.WornRingIds : GameSystem.I.Run.WornRingIds;
        LoadCharacterSO(playerSoCopy, isPlayerSide: true);

        // 初始化所有敌人
        Enemies.Clear();
        foreach (var enemySO in enemySOs)
        {
            LoadCharacterSO(enemySO, isPlayerSide: false);
        }

        IsStarted = true;
    }

    public void LoadCharacterSO(CharacterSO characterSO, bool isPlayerSide = false)
    {
        if (characterSO == null) return;
        Character newCharacter = new(characterSO)
        {
            IsPlayerSide = isPlayerSide,
        };
        LoadCharacter(newCharacter);
    }

    public void LoadCharacter(Character character)
    {
        if (character == null) return;
        Transform spawnSpot;
        if (character.IsPlayerSide)
        {
            //Allies.Add(character);
            spawnSpot = playerSpot;
            //character.Allies = Allies;
        }
        else
        {
            //Enemies.Add(character);
            spawnSpot = enemySpot;
            //character.Allies = Enemies;
        }
        Characters.Add(character);
        CharacterBattlePanel characterBattlePanel = Instantiate(templateBattlePanel, spawnSpot).GetComponent<CharacterBattlePanel>();
        characterBattlePanel.Character = character;
        characterBattlePanel.Initialize(actionResolver);
    }

    public void DestroyCharacter(Character character)
    {
        if (character == null) return;

        // 1. 从列表移除
        Characters.Remove(character);
        Deads.Add(character);

        // 2. 销毁对应的面板
        Transform parent = character.IsPlayerSide ? playerSpot : enemySpot;
        foreach (Transform child in parent)
        {
            CharacterBattlePanel panel = child.GetComponent<CharacterBattlePanel>();
            if (panel != null && panel.Character == character)
            {
                Destroy(child.gameObject);
                break;
            }
        }
    }
}
