using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Gadge : MonoBehaviour
{
    // requirements
    // have a value, max value, and can be changed from editor
    // spawn the suited sprite to suit the start, body, and end of the meter
    // could change color of the sprite
    // could take an event with a value to change to it
    // could take an increment or decrement event to change accordingly
    // fire an onchange event with the new value


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
        _t = GetComponent<RectTransform>();
        _t.sizeDelta = new(spriteSize.x * maxValue, spriteSize.y);


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
        // create start
        Instantiate(CreateImage(sprites[0], SpriteColor, spriteSize), _t);

        // create middle
        for (int idx = 0; idx < maxValue - 2; idx++)
        {
            Instantiate(CreateImage(sprites[1], SpriteColor, spriteSize), _t);
        }

        // create start
        Instantiate(CreateImage(sprites[2], SpriteColor, spriteSize), _t);
    }

    private void UpdateGadge(int _)
    {
        int m_fullCeils = currentValue, m_emptyCeils = maxValue - currentValue;

        for (int _fullIdx = 0; _fullIdx < m_fullCeils; _fullIdx++)
        {
            var m_image = _t.GetChild(_fullIdx).GetComponent<Image>();
            var m_imageIdx = sprites.IndexOf(m_image.sprite);
            if (m_imageIdx == -1) break;

            if (m_imageIdx > 2)
                m_image.sprite = sprites[m_imageIdx - 3];

        }

        for (int _emptyIdx = m_fullCeils; _emptyIdx < m_fullCeils + m_emptyCeils; _emptyIdx++)
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
        val++;
        OnChange?.Invoke(val);
        return val;
    }

    private int Decrement(int val)
    {
        val--;
        OnChange?.Invoke(val);
        return val;
    }

    private void SetValue(int val)
    {
        currentValue = val;
        OnChange?.Invoke(val);
    }
}
