using Microsoft.Toolkit.Uwp.Notifications;
using System.Collections.Generic;

namespace LightIntensityAnalyzer
{
    public class Notificator
    {
        public static void Display(IEnumerable<string> messages)
        {
            // Requires Microsoft.Toolkit.Uwp.Notifications NuGet package version 7.0 or greater
            //Toast
            var toast = new ToastContentBuilder();
            toast.AddArgument("action", "viewConversation");
            toast.AddArgument("conversationId", 9813);
            toast.AddText("Illumination problems detected!");
            foreach (var m in messages)
                toast.AddText(m);
            toast.Show(); // Not seeing the Show() method? Make sure you have version 7.0, and if you're using .NET 5, your TFM must be net5.0-windows10.0.17763.0 or greater
        }
    }
}
