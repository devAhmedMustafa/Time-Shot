using UnityEngine;

namespace _Scripts
{
    public class PrizePoolFiller : MonoBehaviour
    {
        [SerializeField] private PrizePool pool;
        [SerializeField] private Sprite[] prizeSprites;

        private void Start()
        {
            AddPrizeWithImage("Warm-up Winner", 5000, 0);
            AddPrizeWithImage("Quick Draw", 10000, 1);
            AddPrizeWithImage("Steady Hands", 15000, 2);
            AddPrizeWithImage("Time Keeper", 30000, 3);
            AddPrizeWithImage("Master of Time", 60000, 4);
        }

        private void AddPrizeWithImage(string name, uint timeToWin, int spriteIndex)
        {
            Sprite prizeSprite = null;
            if (prizeSprites != null && spriteIndex < prizeSprites.Length && prizeSprites[spriteIndex] != null)
            {
                prizeSprite = prizeSprites[spriteIndex];
            }
            else
            {
                prizeSprite = CreateDefaultPrizeSprite(timeToWin);
            }

            pool.AddPrize(new Prize
            {
                name = name,
                timeToWin = timeToWin,
                image = prizeSprite
            });
        }

        private Sprite CreateDefaultPrizeSprite(uint timeToWin)
        {
            Texture2D texture = new Texture2D(64, 64);
            Color mainColor = GetColorForTime(timeToWin);
            Color darkBorder = new Color(0.04f, 0.035f, 0.12f, 1f);

            for (int y = 0; y < 64; y++)
            {
                for (int x = 0; x < 64; x++)
                {
                    float dx = x - 31.5f;
                    float dy = y - 31.5f;
                    float distSq = dx * dx + dy * dy;

                    if (distSq <= 24 * 24)
                    {
                        texture.SetPixel(x, y, mainColor);
                    }
                    else if (distSq <= 28 * 28)
                    {
                        texture.SetPixel(x, y, darkBorder);
                    }
                    else
                    {
                        texture.SetPixel(x, y, Color.clear);
                    }
                }
            }
            texture.Apply();
            return Sprite.Create(texture, new Rect(0, 0, 64, 64), new Vector2(0.5f, 0.5f));
        }

        private Color GetColorForTime(uint timeMs)
        {
            switch (timeMs)
            {
                case 5000: return new Color(0.93f, 0.69f, 0.30f); // Warm Amber
                case 10000: return new Color(0.35f, 0.75f, 0.95f); // Tech Cyan
                case 15000: return new Color(0.40f, 0.85f, 0.50f); // Emerald Green
                case 30000: return new Color(0.70f, 0.45f, 0.95f); // Royal Purple
                case 60000: return new Color(0.95f, 0.35f, 0.45f); // Crimson Star
                default: return new Color(0.93f, 0.69f, 0.30f);
            }
        }
    }
}