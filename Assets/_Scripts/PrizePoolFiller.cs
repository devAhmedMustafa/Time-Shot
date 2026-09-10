using UnityEngine;

namespace _Scripts
{
    class PrizePoolFiller : MonoBehaviour
    {
        [SerializeField] private PrizePool pool;
        private void Start() {
            pool.AddPrize(new Prize
            {
                name = "Warm-up Winner",
                timeToWin = 5000
            });
            pool.AddPrize(new Prize
            {
                name = "Quick Draw",
                timeToWin = 10000
            });
            pool.AddPrize(new Prize
            {
                name = "Steady Hands",
                timeToWin = 15000
            });
            pool.AddPrize(new Prize
            {
                name = "Time Keeper",
                timeToWin = 30000
            });
            pool.AddPrize(new Prize
            {
                name = "Master of Time",
                timeToWin = 60000
            });
        }
    }
}