using MoneyChange.ViewModels;

namespace MoneyChange.Infrastructure
{
    public class InstanceLocator
    {
        public MainViewModel Main
        {
            get;
            set;
        }
        public InstanceLocator()
        {
            Main = new MainViewModel();
        }
    }
}
