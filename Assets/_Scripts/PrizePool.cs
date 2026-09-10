using System.Collections.Generic;
using UnityEngine;

namespace _Scripts
{
    class Prize
    {
        public string name;
        public string image;
        public uint timeToWin;
    }

    class PrizePool : MonoBehaviour
    {
        private readonly Dictionary<uint, Prize> prizes = new Dictionary<uint, Prize>();

        public void AddPrize(Prize prize)
        {
            prizes.Add(prize.timeToWin, prize);
        }

        public Prize? GetPrize(uint timeGot)
        {
            if (prizes.ContainsKey(timeGot)) return prizes[timeGot];
            else return null;
        }
    }
}