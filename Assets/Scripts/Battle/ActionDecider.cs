using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ActionDecider : MonoBehaviour
{
    [SerializeField] GameObject actionPanel; // 包含按钮的面板
    [SerializeField] Transform actionPanelContent;
    [SerializeField] ActionButton actionButton;
    [SerializeField] ActionResolver actionResolver;
    private List<Character> characters;

    private void Awake()
    {
        actionPanel.SetActive(false);
    }

    public void Initialize(List<Character> c)
    {
        characters = c;
    }

    public void Decide(Character actor)
    {
        if (actor.IsPlayerControlled)
        {
            actionResolver.Actor = actor;

            foreach (Transform child in actionPanelContent)
            {
                Destroy(child.gameObject);
            }

            foreach (BattleActionSO battleAction in actor.BattleActions)
            {
                ActionButton newActionButton = Instantiate(actionButton, actionPanelContent);
                newActionButton.Initialize(battleAction, actionResolver);
            }
            actionPanel.SetActive(true);
        }
        else
        {
            actionResolver.BattleActionSO = actor.BattleActions[0];
            actionResolver.Actor = actor;
            actionResolver.Target = characters.Where(c => c != actor).FirstOrDefault();
            actionResolver.StartResolve();
        }
    }
}
