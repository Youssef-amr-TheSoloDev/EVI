using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
[RequireComponent(typeof(HorizontalLayoutGroup), typeof(RectTransform))]
public class Gadge : MonoBehaviour
{
    [SerializeField] private int currentValue, maxValue;
    [SerializeField, Tooltip("full -> empty")] private List<Sprite> sprites;
    [SerializeField] private Vector2 spriteSize = new(32, 32);
    [SerializeField] private float spritesSpacing = -5f;
    [SerializeField] private Color SpriteColor;
    public UnityAction<int> ChangeToValue;
    [SerializeField] private UnityEvent<int> OnChange;
    [SerializeField] private UnityEvent IncrementValue, DecrementValue;

    private RectTransform _t;
    void Start()
    {
        currentValue = currentValue > maxValue ? maxValue : currentValue;

        SpawnGadge();

        OnChange.AddListener(UpdateGadge);
        IncrementValue.AddListener(() =>
        {
            currentValue = Increment(currentValue);
        });

        DecrementValue.AddListener(() =>
        {
            currentValue = Decrement(currentValue);
        });
    }

    [ContextMenu("spawn meter")]
    private void SpawnGadge()
    {
        // set meter size
        SetMeterSize();

        // create world object
        GameObject m_imageObject = new("ceil", new Type[]
        {
            typeof(RectTransform),
            typeof(CanvasRenderer),
            typeof(Image)
        });

        // add start ceil
        Instantiate(SetupImage(m_imageObject, sprites[0], SpriteColor, spriteSize), transform);

        // add middle ceils
        for (int idx = 0; idx < maxValue - 2; idx++)
        {
            Instantiate(SetupImage(m_imageObject, sprites[0], SpriteColor, spriteSize), transform);
        }

        // add start ceil
        Instantiate(SetupImage(m_imageObject, sprites[0], SpriteColor, spriteSize), transform);

        // destroy world created object
        Destroy(m_imageObject);

        // update empty and full ceils
        UpdateGadge(currentValue);
    }

    private void SetMeterSize()
    {
        _t = GetComponent<RectTransform>();
        _t.sizeDelta = new(spriteSize.x * maxValue, spriteSize.y);

        GetComponent<HorizontalLayoutGroup>().spacing = spritesSpacing * maxValue;
    }

    private void UpdateGadge(int full_val)
    {
        for (int _fullIdx = 0; _fullIdx < full_val; _fullIdx++)
        {
            var m_image = _t.GetChild(_fullIdx).GetComponent<Image>();
            var m_imageIdx = sprites.IndexOf(m_image.sprite);
            if (m_imageIdx == -1) break;

            if (m_imageIdx > (sprites.Count / 2) - 1)
                m_image.sprite = sprites[m_imageIdx - sprites.Count / 2];

        }

        for (int _emptyIdx = full_val; _emptyIdx < maxValue; _emptyIdx++)
        {
            var m_image = _t.GetChild(_emptyIdx).GetComponent<Image>();
            var m_imageIdx = sprites.IndexOf(m_image.sprite);
            if (m_imageIdx == -1) break;

            if (m_imageIdx < sprites.Count / 2)
                m_image.sprite = sprites[m_imageIdx + sprites.Count / 2];
        }

    }

    private GameObject SetupImage(GameObject Image, Sprite sprite, Color color, Vector2 size)
    {
        var m_transform = Image.GetComponent<RectTransform>();
        m_transform.sizeDelta = size;

        var m_ImageComponent = Image.GetComponent<Image>();
        m_ImageComponent.sprite = sprite;
        m_ImageComponent.color = color;

        return Image;
    }

    private int Increment(int val)
    {
        val = Math.Clamp(++val, 0, maxValue);
        OnChange?.Invoke(val);
        return val;
    }

    private int Decrement(int val)
    {
        val = Math.Clamp(--val, 0, maxValue);
        OnChange?.Invoke(val);
        return val;
    }

    private void SetValue(int val)
    {
        currentValue = val;
        OnChange?.Invoke(val);
    }
}
