using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Gadge : MonoBehaviour
{
    [SerializeField] private int currentValue, maxValue;
    [SerializeField, Tooltip("3 full, 3 empty")] private List<Sprite> sprites;
    [SerializeField] private Vector2 spriteSize = new(32, 32);
    [SerializeField] private Color SpriteColor;
    public UnityAction<int> ChangeToValue;
    [SerializeField] private UnityEvent<int> OnChange;
    [SerializeField] private UnityEvent IncrementValue, DecrementValue;

    private RectTransform _t;
    void Start()
    {
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

        // create start
        Instantiate(CreateImage(sprites[0], SpriteColor, spriteSize), transform);

        // create middle
        for (int idx = 0; idx < maxValue - 2; idx++)
        {
            Instantiate(CreateImage(sprites[1], SpriteColor, spriteSize), transform);
        }

        // create start
        Instantiate(CreateImage(sprites[2], SpriteColor, spriteSize), transform);
    }

    private void SetMeterSize()
    {
        _t = GetComponent<RectTransform>();
        _t.sizeDelta = new(spriteSize.x * maxValue, spriteSize.y);
    }

    private void UpdateGadge(int full_val)
    {
        for (int _fullIdx = 0; _fullIdx < full_val; _fullIdx++)
        {
            var m_image = _t.GetChild(_fullIdx).GetComponent<Image>();
            var m_imageIdx = sprites.IndexOf(m_image.sprite);
            if (m_imageIdx == -1) break;

            if (m_imageIdx > 2)
                m_image.sprite = sprites[m_imageIdx - 3];

        }

        for (int _emptyIdx = full_val; _emptyIdx < maxValue; _emptyIdx++)
        {
            var m_image = _t.GetChild(_emptyIdx).GetComponent<Image>();
            var m_imageIdx = sprites.IndexOf(m_image.sprite);
            if (m_imageIdx == -1) break;

            if (m_imageIdx < 3)
                m_image.sprite = sprites[m_imageIdx + 3];
        }

    }

    private GameObject CreateImage(Sprite sprite, Color color, Vector2 size)
    {

        var m_CreatedImage = new GameObject();
        var m_transform = m_CreatedImage.AddComponent<RectTransform>();
        m_transform.sizeDelta = size;

        m_CreatedImage.AddComponent<CanvasRenderer>();

        var m_ImageComponent = m_CreatedImage.AddComponent<Image>();
        m_ImageComponent.sprite = sprite;
        m_ImageComponent.color = color;

        return m_CreatedImage;

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
