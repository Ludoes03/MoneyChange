using System;

namespace MoneyChange.Helpers
{
    public class PlatformCulture
    {
        public PlatformCulture(string platformCultureString)
        {
            if(string.IsNullOrEmpty(platformCultureString))
            {
                throw new ArgumentException("Expected culture identifier", nameof(platformCultureString));
            }

            PlatformString = platformCultureString.Replace("_", "-");
            var dashIndex = PlatformString.IndexOf("-", StringComparison.Ordinal);
            if(dashIndex > 0 )
            {
                var parts = PlatformString.Split('-');
                LenguageCode = parts[0];
                LocaleCode = parts[1];
            }
            else
            {
                LenguageCode = PlatformString;
                LocaleCode = "";
            }

        }

        public string PlatformString { get; private set; }
        public string LenguageCode { get; private set; }
        public string LocaleCode { get; private set; }

        public override string ToString()
        {
            return PlatformString;
        }
    }
}
