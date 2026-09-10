using System.Collections.Generic;
using UnityEngine;

namespace _Scripts
{
    public class Prize
    {
        public string name;
        public Sprite image;
        public uint timeToWin;
    }

    public class PrizePool : MonoBehaviour
    {
        private readonly Dictionary<uint, Prize> prizes = new Dictionary<uint, Prize>();

        public event System.Action<Prize> OnPrizeAdded;
        [SerializeField] private bool winByKosa;

        public void AddPrize(Prize prize)
        {
            prizes[prize.timeToWin] = prize;
            OnPrizeAdded?.Invoke(prize);
        }

        public Prize GetPrize(uint timeGot)
        {
            if (prizes.ContainsKey(timeGot)) return prizes[timeGot];
            else return winByKosa ? prizes[5000] : null;
        }

        public IEnumerable<Prize> GetAllPrizes()
        {
            return prizes.Values;
        }
    }
}