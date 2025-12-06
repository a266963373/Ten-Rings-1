using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FieldSystem : MonoBehaviour
{
    [SerializeField] Transform TerrainContainer;
    [SerializeField] Transform WeatherContainer;
    [SerializeField] FieldButton TemplateFieldButton;

    public static FieldSystem I;
    private void Awake()
    {
        I = this;
        TemplateFieldButton.gameObject.SetActive(false);
    }

    private readonly List<Field> Fields = new();

    private void UpdateUI()
    {
        // 清空容器
        foreach (Transform child in TerrainContainer)
            Destroy(child.gameObject);
        foreach (Transform child in WeatherContainer)
            Destroy(child.gameObject);

        // 遍历所有场地，实例化按钮并分配到容器
        foreach (var field in Fields)
        {
            Transform parent = field.Type == FieldType.Terrain ? TerrainContainer : WeatherContainer;
            var btn = Instantiate(TemplateFieldButton, parent);
            btn.gameObject.SetActive(true);
            btn.Setup(field);
        }
    }

    public void AddField(Field f)
    {
        var existing = Fields.Find(field => field.Id == f.Id);

        if (existing == null)
        {
            if (f.Duration > 0)
            {
                Fields.Add(f);
            }
        }
        else
        {
            existing.Duration += f.Duration;
        }

        UpdateUI();
    }

    public void OnWorldTurn()
    {
        Trigger(TimingType.OnWorldTurn);

        foreach (var field in Fields.ToList())
        {
            field.Duration -= 1;
            if (field.Duration <= 0)
            {
                Fields.Remove(field);
            }
        }

        UpdateUI();
    }

    public void Trigger(TriggerType triggerType, BattleAction ba)
    {
        foreach (var field in Fields)
        {
            foreach (var triggerEffect in field.TriggerEffects)
            {
                if (triggerEffect.Trigger == triggerType)
                {
                    triggerEffect.Effect?.Invoke(ba);
                }
            }
        }
    }

    private void Trigger(TimingType timing)
    {
        foreach (var field in Fields)
        {
            foreach (var triggerEffect in field.TriggerEffects)
            {
                if (triggerEffect.Timing == timing)
                {
                    triggerEffect.EffectOnTime?.Invoke();
                }
            }
        }
    }

    public int Trigger(BattleActionSO ba, IntModType intMod)
    {
        int value;
        if (intMod == IntModType.ManaCost)
        {
            value = ba.ManaCost;
        }
        else
        {
            value = 0;
        }

        foreach (var field in Fields)
        {
            foreach (var te in field.TriggerEffects)
            {
                if (te.IntMod == intMod && te.EffectOnInt != null)
                {
                    value = te.EffectOnInt.Invoke(ba);
                }
            }
        }
        return value;
    }
}
