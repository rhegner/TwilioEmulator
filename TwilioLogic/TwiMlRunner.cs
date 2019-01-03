using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TwilioLogic
{
    class TwiMlRunner
    {

        public async Task RunTwiMl(XElement root, CancellationToken ct)
        {
            foreach (var element in root.Elements())
            {
                if (element.Name == "Say")
                    await HandleSay(element, ct);
                else if (element.Name == "Play")
                    await HandlePlay(element, ct);
                else if (element.Name == "Dial")
                    await HandleDial(element, ct);
                else if (element.Name == "Pause")
                    await HandlePause(element, ct);
                else
                    throw new NotSupportedException($"Unsupported verb {element.Name}");
            }
        }

        private async Task HandleSay(XElement sayElement, CancellationToken ct)
        {
            var text = sayElement.Value;
            if (!int.TryParse(sayElement.Attribute("loop")?.Value, out var loop))
                loop = 1;
            for (int i=0; i<loop; i++)
            {
                await ReportActivity($"Say: {text}");
                await Task.Delay(2000, ct);
            }
        }

        private async Task HandlePlay(XElement playElement, CancellationToken ct)
        {
            var text = playElement.Value;
            if (!int.TryParse(playElement.Attribute("loop")?.Value, out var loop))
                loop = 1;
            for (int i = 0; i < loop; i++)
            {
                await ReportActivity($"Play: {text}");
                await Task.Delay(2000, ct);
            }
        }

        private async Task HandleDial(XElement dialElement, CancellationToken ct)
        {

        }

        private async Task HandlePause(XElement pauseElement, CancellationToken ct)
        {
            if (!int.TryParse(pauseElement.Attribute("loop")?.Value, out var pause))
                pause = 1;
            await ReportActivity($"Pausing for {pause} seconds");
            await Task.Delay(TimeSpan.FromSeconds(pause), ct);
        }

        private Task ReportActivity(string activity)
        {
            return Task.CompletedTask;
        }
    }
}
