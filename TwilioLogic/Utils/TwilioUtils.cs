using System;

namespace TwilioLogic.Utils
{
    public static class TwilioUtils
    {

        private static Random Random = new Random();

        public static string CreateSid(string prefix)
        {
            if (prefix.Length != 2)
                throw new ArgumentException("Length must be 2", nameof(prefix));
            var bytes = new byte[16];
            Random.NextBytes(bytes);
            string hex = BitConverter.ToString(bytes).Replace("-", string.Empty);
            return prefix.ToUpper() + hex.ToLower();
        }

        public static string FormatNumber(string number)
            => number;

    }
}
