using System;

namespace Next.PCL.Infra
{
    public class Randomizer
    {
        private static Random _random;


        public static Random Instance => GetRandomizer();

        private static Random GetRandomizer()
        {
            if (_random == null)
                _random = new Random();
            return _random;
        }
    }
}