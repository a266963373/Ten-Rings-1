using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ActionDecider : MonoBehaviour
{
    [SerializeField] GameObject actionPanel; // 包含按钮的面板
    [SerializeField] Transform actionPanelContent;
    [SerializeField] ActionButton actionButton;
    [SerializeField] ActionResolver actionResolver;
    [SerializeField] Transform focusImage;
    private List<Character> characters => BattleSystem.I.Characters;
    private Character currentCharacter;

    private void Awake()
    {
        actionPanel.SetActive(false);
        focusImage.gameObject.SetActive(false);
    }

    public void Decide(Character actor)
    {
        currentCharacter = actor;
        if (actor.IsPlayerControlled)
        {
            PlayerDecide(actor);
        }
        else
        {
            NpcDecide(actor);
        }
    }

    private void PlayerDecide(Character actor)
    {
        focusImage.SetParent(actionPanel.transform, false);
        actionResolver.Actor = actor;

        foreach (Transform child in actionPanelContent)
        {
            Destroy(child.gameObject);
        }

        int currentMp = actor.Stats.GetStat(StatType.MP);

        foreach (BattleActionSO battleAction in actor.BattleActions)
        {
            ActionButton newActionButton = Instantiate(actionButton, actionPanelContent);
            newActionButton.OnClickAction += ActionButtonOnClick;
            newActionButton.Initialize(battleAction);

            // 设置按钮可用性
            bool canAfford = battleAction.ManaCostRunTime <= currentMp;
            newActionButton.SetInteractable(canAfford);
        }
        actionPanel.SetActive(true);
    }

    private void NpcDecide(Character actor)
    {
        Character chosenTarget = null;
        BattleActionSO chosenAction = null;

        // 1. 获取可支付的技能
        int currentMp = actor.Stats.GetStat(StatType.MP);
        List<BattleActionSO> affordableActions = actor.BattleActions
            .Where(a => a.ManaCostRunTime <= currentMp)
            .ToList();

        // 2. See if there is action that finds favorate target
        foreach (var action in affordableActions)
        {
            chosenTarget = FindChosenTarget(actor, action, action.Favorate);
            if (chosenTarget != null)
            {
                chosenAction = action;
                break; // 找到一个合适的目标就可以了
            }
        }

        if (chosenTarget == null)
        {
            // if no favorate action, randomly choose one except the first two (basic attack and defend); 
            // get rid of favorate actions
            affordableActions = affordableActions
                .Where(a => a.Favorate == TargetType.None)
                .ToList();
            // if not enough actions, use the first one (usually basic attack)
            chosenAction = affordableActions.Count > 2
                ? affordableActions[UnityEngine.Random.Range(2, affordableActions.Count)]
                : actor.BattleActions[0];

            chosenTarget = FindChosenTarget(actor, chosenAction, chosenAction.Prefer);

            // 如果没选到目标，随便选一个角色
            if (chosenTarget == null)
            {
                Debug.Log("Has not found preferred target!");
                if (characters.Count > 0)
                    chosenTarget = characters[UnityEngine.Random.Range(0, characters.Count)];
                else
                    chosenTarget = actor;
            }
        }

        actionResolver.BattleActionSO = chosenAction;
        actionResolver.Actor = actor;
        actionResolver.Target = chosenTarget;
        StartCoroutine(actionResolver.StartResolve());
    }

    private Character FindChosenTarget(Character actor, BattleActionSO action, TargetType targetType)
    {
        Character chosenTarget = null;
        var candidates = characters;

        switch (targetType)
        {
            case TargetType.None:
                break;
            case TargetType.Killable:
                chosenTarget = FindKillableTarget(actor, action, candidates);
                break;
            case TargetType.WithoutMyStatus:
                if (action.Statuses[0].IsBuff)
                    chosenTarget = actor.Allies.Where(c => !c.Statuses.Any(s => s.Id == action.Statuses[0].Id))
                        .FirstOrDefault();
                else
                    chosenTarget = actor.Enemies.Where(c => !c.Statuses.Any(s => s.Id == action.Statuses[0].Id))
                        .FirstOrDefault();
                break;
            case TargetType.LowestPercentHpAlly:
                chosenTarget = actor.Allies
                    .OrderBy(c => (float)c.Stats.GetStat(StatType.HP) / c.Stats.GetStat(StatType.MHP))
                    .FirstOrDefault();
                break;
            case TargetType.Enemy:
                var enemies = actor.Enemies;
                if (enemies.Count > 0)
                    chosenTarget = enemies[UnityEngine.Random.Range(0, enemies.Count)];
                break;
            case TargetType.Ally:
                var allies = actor.Allies;
                if (allies.Count > 0)
                    chosenTarget = allies[UnityEngine.Random.Range(0, allies.Count)];
                break;
            case TargetType.Self:
                chosenTarget = actor;
                break;
            default:
                if (candidates.Count > 0)
                    chosenTarget = candidates[UnityEngine.Random.Range(0, candidates.Count)];
                break;
            }
        return chosenTarget;
    }

    private void ActionButtonOnClick(BattleActionSO b, Transform t)
    {
        actionResolver.IsTargetSelectMode = true;
        actionResolver.BattleActionSO = b;
        focusImage.SetParent(t, false);
        focusImage.SetSiblingIndex(0);
        focusImage.gameObject.SetActive(true);

        if (b.Must == TargetType.Self)
        {   // this won't be taunted
            actionResolver.SelectTarget(currentCharacter);
        }
        else if (b.Must == TargetType.Field)
        {
            actionResolver.SelectTarget(null);
        }
        else if (b is InvokeActionSO ia && ia.IsActionInvoked)
        {
            actionResolver.SelectTarget(ia.Target);
        }
        else if (currentCharacter.TauntedApplier != null)
        {
            actionResolver.SelectTarget(currentCharacter.TauntedApplier);
        }
    }

    public void RemoveFocusAction()
    {
        focusImage.gameObject.SetActive(false);
        actionResolver.IsTargetSelectMode = false;
        actionResolver.BattleActionSO = null;
        focusImage.SetParent(actionPanel.transform, false);
    }

    private Character FindKillableTarget(Character actor, BattleActionSO actionSO, List<Character> candidates)
    {
        foreach (var target in candidates)
        {
            if (target == null || target.IsDead || target == actor) continue;
            var action = actionSO.GetAction(actor, target);
            int scale = actor.Stats.GetStat(action.Scale);
            int damage = Mathf.RoundToInt(action.Damage.Value * scale * 0.01f);
            if (target.Stats.GetStat(StatType.HP) <= damage)
            {
                return target; // 找到第一个能杀的
            }
        }
        return null;
    }
}
