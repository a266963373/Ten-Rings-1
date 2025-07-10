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
    [SerializeField] Transform focusImage;
    private List<Character> characters;

    private void Awake()
    {
        actionPanel.SetActive(false);
        focusImage.gameObject.SetActive(false);
    }

    public void Initialize(List<Character> c)
    {
        characters = c;
    }

    public void Decide(Character actor)
    {
        if (actor.IsPlayerControlled)
        {
            focusImage.SetParent(actionPanel.transform, false);
            actionResolver.Actor = actor;

            foreach (Transform child in actionPanelContent)
            {
                Destroy(child.gameObject);
            }

            foreach (BattleActionSO battleAction in actor.BattleActions)
            {
                ActionButton newActionButton = Instantiate(actionButton, actionPanelContent);
                newActionButton.OnClickAction += ActionButtonOnClick;
                newActionButton.Initialize(battleAction);
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

    private void ActionButtonOnClick(BattleActionSO b, Transform t)
    {
        actionResolver.IsTargetSelectMode = true;
        actionResolver.BattleActionSO = b;
        focusImage.SetParent(t, false);
        focusImage.SetSiblingIndex(0);
        focusImage.gameObject.SetActive(true);
    }

    public void RemoveFocusAction()
    {
        focusImage.gameObject.SetActive(false);
        actionResolver.IsTargetSelectMode = false;
        actionResolver.BattleActionSO = null;
        focusImage.SetParent(actionPanel.transform, false);
    }
}
