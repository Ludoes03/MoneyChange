using System.Globalization;

namespace MoneyChange.Interfaces
{
    public interface ILocalize
    {
        CultureInfo GetCurrentCultureInnfo();
        void SetLocale(CultureInfo ci);
    }
}
