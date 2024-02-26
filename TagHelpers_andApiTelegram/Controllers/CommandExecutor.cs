using TagHelpers_andApiTelegram.Controllers.Commands;
using Telegram.Bot.Types;

namespace TagHelpers_andApiTelegram.Controllers
{
    public class CommandExecutor : ITelegramUpdateLister
    {

        private List<ICommand> commands;
        private IListener? listener = null;


        public CommandExecutor()
        {
            commands = new List<ICommand>()
            {
                new RegisterCommand(this), new ChatCommand(this), new LoginCommand(this)
            };
        }

        public async Task GetUpdate(Update update)
        {

            if (listener == null)
            {
                await ExecuteCommand(update);
            }
            else
            {
                await listener.GetUpdate(update);
            }


        }

        private async Task ExecuteCommand(Update update)
        {
            Message msg = update.Message;


            foreach (var command in commands)
            {
                if (command.Name == msg.Text)
                {
                    await command.ExecuteAsync(update);
                }
            }
        }

        public void StartListen(IListener newListener)
        {
            listener = newListener;
        }

        public void StopListen()
        {
            listener = null;
        }
    }
}
