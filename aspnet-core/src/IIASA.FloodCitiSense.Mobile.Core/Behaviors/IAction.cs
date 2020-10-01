using Xamarin.Forms.Internals;

namespace IIASA.FloodCitiSense.Mobile.Core.Behaviors
{
    [Preserve(AllMembers = true)]
    public interface IAction
    {
        bool Execute(object sender, object parameter);
    }
}