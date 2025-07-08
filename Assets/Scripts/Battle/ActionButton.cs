using UnityEngine;
using UnityEngine.Localization.Components;

public class ActionButton : MonoBehaviour
{
    private BattleActionSO battleAction;
    [SerializeField] LocalizeStringEvent localizeStringEvent;
    private ActionResolver resolver;

    public void Initialize(BattleActionSO b, ActionResolver r)
    {
        battleAction = b;
        resolver = r;
        localizeStringEvent.StringReference.SetReference("Battle Action", b.Name);
    }

    public void OnClick()
    {
        resolver.IsTargetSelectMode = true;
        resolver.BattleActionSO = battleAction;
    }
}
