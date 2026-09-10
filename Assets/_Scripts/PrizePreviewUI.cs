using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts
{
    public class PrizePreviewUI : MonoBehaviour
    {
        [SerializeField] private PrizePool pool;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private PrizeCatcher catcher;
        [SerializeField] private Transform cardsContainer;

        private readonly Dictionary<uint, GameObject> cardMap = new Dictionary<uint, GameObject>();
        private readonly Dictionary<uint, Image> cardBgMap = new Dictionary<uint, Image>();

        // Theme Colors
        private readonly Color darkNavyBg = new Color(0.075f, 0.059f, 0.204f, 0.9f); // #130F34
        private readonly Color goldenAmber = new Color(0.929f, 0.686f, 0.298f, 1f);  // #EDAF4C
        private readonly Color offWhite = new Color(0.961f, 0.957f, 0.996f, 1f);     // #F5F4FE
        private readonly Color mutedSlate = new Color(0.486f, 0.475f, 0.573f, 1f);   // #7C7992

        private void Awake()
        {
            if (pool == null) pool = FindObjectOfType<PrizePool>();
            if (gameManager == null) gameManager = FindObjectOfType<GameManager>();
            if (catcher == null) catcher = FindObjectOfType<PrizeCatcher>();
        }

        [SerializeField] private float bounceSpeed = 2.5f;
        [SerializeField] private float bounceHeight = 6f;
        private Vector3 initialPanelPosition;
        private bool hasInitializedPos;

        private void Start()
        {
            if (cardsContainer == null) cardsContainer = transform;

            initialPanelPosition = transform.localPosition;
            hasInitializedPos = true;

            SetupContainerLayout();
            PopulateCards();
        }

        private void Update()
        {
            if (hasInitializedPos)
            {
                float yOffset = Mathf.Sin(Time.time * bounceSpeed) * bounceHeight;
                transform.localPosition = initialPanelPosition + new Vector3(0f, yOffset, 0f);
            }
        }

        private void OnEnable()
        {
            if (pool != null) pool.OnPrizeAdded += HandlePrizeAdded;
            if (catcher != null) catcher.OnWin += HandleWin;
            if (gameManager != null) gameManager.StateChanged += HandleStateChanged;
        }

        private void OnDisable()
        {
            if (pool != null) pool.OnPrizeAdded -= HandlePrizeAdded;
            if (catcher != null) catcher.OnWin -= HandleWin;
            if (gameManager != null) gameManager.StateChanged -= HandleStateChanged;
        }

        private void HandlePrizeAdded(Prize prize)
        {
            if (prize == null) return;

            if (cardMap.TryGetValue(prize.timeToWin, out GameObject existingCard))
            {
                UpdateCardContent(existingCard, prize);
            }
            else
            {
                GameObject cardObj = CreatePrizeCard(prize);
                cardMap[prize.timeToWin] = cardObj;
            }
        }

        private void UpdateCardContent(GameObject cardObj, Prize prize)
        {
            if (cardObj == null || prize == null) return;

            // Update Time Badge
            TextMeshProUGUI timeText = cardObj.transform.Find("TimeBadge")?.GetComponent<TextMeshProUGUI>();
            if (timeText != null)
            {
                timeText.text = $"{prize.timeToWin / 1000f:F2}s";
            }

            // Update Icon
            Image iconImg = cardObj.transform.Find("PrizeIcon")?.GetComponent<Image>();
            if (iconImg != null && prize.image != null)
            {
                iconImg.sprite = prize.image;
                iconImg.preserveAspect = true;
            }

            // Update Name
            TextMeshProUGUI nameText = cardObj.transform.Find("PrizeName")?.GetComponent<TextMeshProUGUI>();
            if (nameText != null)
            {
                nameText.text = prize.name;
            }
        }

        private void SetupContainerLayout()
        {
            HorizontalLayoutGroup layout = cardsContainer.GetComponent<HorizontalLayoutGroup>();
            if (layout == null) layout = cardsContainer.gameObject.AddComponent<HorizontalLayoutGroup>();

            layout.spacing = 14f;
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.childControlWidth = false;
            layout.childControlHeight = false;
            layout.childForceExpandWidth = false;
            layout.childForceExpandHeight = false;
        }

        public void PopulateCards()
        {
            if (pool == null) return;

            cardMap.Clear();
            cardBgMap.Clear();

            // Check if scene cards already exist in container
            Dictionary<uint, Transform> existingCards = new Dictionary<uint, Transform>();
            foreach (Transform child in cardsContainer)
            {
                if (child.name.StartsWith("PrizeCard_"))
                {
                    string timeStr = child.name.Replace("PrizeCard_", "");
                    if (uint.TryParse(timeStr, out uint timeVal))
                    {
                        existingCards[timeVal] = child;
                    }
                }
            }

            foreach (var prize in pool.GetAllPrizes())
            {
                if (existingCards.TryGetValue(prize.timeToWin, out Transform existingTransform))
                {
                    GameObject cardObj = existingTransform.gameObject;
                    cardMap[prize.timeToWin] = cardObj;
                    Image bg = cardObj.GetComponent<Image>();
                    if (bg != null) cardBgMap[prize.timeToWin] = bg;

                    UpdateCardContent(cardObj, prize);
                }
                else
                {
                    GameObject cardObj = CreatePrizeCard(prize);
                    cardMap[prize.timeToWin] = cardObj;
                }
            }
        }

        private GameObject CreatePrizeCard(Prize prize)
        {
            GameObject card = new GameObject($"PrizeCard_{prize.timeToWin}");
            card.transform.SetParent(cardsContainer, false);

            RectTransform rt = card.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(115f, 145f);

            Image bg = card.AddComponent<Image>();
            bg.color = darkNavyBg;
            cardBgMap[prize.timeToWin] = bg;

            VerticalLayoutGroup vlg = card.AddComponent<VerticalLayoutGroup>();
            vlg.padding = new RectOffset(8, 8, 10, 10);
            vlg.spacing = 6f;
            vlg.childAlignment = TextAnchor.UpperCenter;
            vlg.childControlWidth = true;
            vlg.childControlHeight = true;
            vlg.childForceExpandWidth = true;
            vlg.childForceExpandHeight = false;

            // 1. Time Badge Text
            GameObject timeBadgeObj = new GameObject("TimeBadge");
            timeBadgeObj.transform.SetParent(card.transform, false);
            LayoutElement timeLe = timeBadgeObj.AddComponent<LayoutElement>();
            timeLe.preferredHeight = 22f;
            timeLe.minHeight = 22f;

            TextMeshProUGUI timeText = timeBadgeObj.AddComponent<TextMeshProUGUI>();
            timeText.text = $"{prize.timeToWin / 1000f:F2}s";
            timeText.fontSize = 14f;
            timeText.fontStyle = FontStyles.Bold;
            timeText.color = goldenAmber;
            timeText.alignment = TextAlignmentOptions.Center;

            // 2. Icon Image
            GameObject iconObj = new GameObject("PrizeIcon");
            iconObj.transform.SetParent(card.transform, false);
            LayoutElement iconLe = iconObj.AddComponent<LayoutElement>();
            iconLe.preferredHeight = 44f;
            iconLe.preferredWidth = 44f;

            Image iconImg = iconObj.AddComponent<Image>();
            if (prize.image != null)
            {
                iconImg.sprite = prize.image;
                iconImg.preserveAspect = true;
            }

            // 3. Prize Name Text
            GameObject nameObj = new GameObject("PrizeName");
            nameObj.transform.SetParent(card.transform, false);
            LayoutElement nameLe = nameObj.AddComponent<LayoutElement>();
            nameLe.preferredHeight = 36f;
            nameLe.minHeight = 36f;

            TextMeshProUGUI nameText = nameObj.AddComponent<TextMeshProUGUI>();
            nameText.text = prize.name;
            nameText.fontSize = 11f;
            nameText.color = offWhite;
            nameText.alignment = TextAlignmentOptions.Center;
            nameText.textWrappingMode = TextWrappingModes.Normal;
            nameText.overflowMode = TextOverflowModes.Ellipsis;

            return card;
        }

        private void HandleWin(Prize prize)
        {
            if (cardMap.TryGetValue(prize.timeToWin, out GameObject card))
            {
                if (cardBgMap.TryGetValue(prize.timeToWin, out Image bg))
                {
                    bg.color = goldenAmber;
                }
                card.transform.localScale = Vector3.one * 1.15f;
            }
        }

        private void HandleStateChanged(GameState state)
        {
            if (state == GameState.Idle)
            {
                foreach (var kvp in cardBgMap)
                {
                    kvp.Value.color = darkNavyBg;
                    if (cardMap.TryGetValue(kvp.Key, out GameObject card))
                    {
                        card.transform.localScale = Vector3.one;
                    }
                }
            }
        }
    }
}
