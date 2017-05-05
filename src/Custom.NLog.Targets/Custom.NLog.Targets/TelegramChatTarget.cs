using NLog;
using NLog.Config;
using NLog.Targets;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Custom.NLog.Targets
{
    [Target("TelegramChat")]
    public class TelegramChatTarget : TargetWithLayout
    {


        [RequiredParameter]
        public string Token { get; set; }

        [RequiredParameter]
        public string ChatId { get; set; }

        private TelegramBotClient _bot;

        public TelegramChatTarget(string token, string chatId)
        {
            Token = token;
            ChatId = chatId;
        }

        protected override void Write(LogEventInfo logEvent)
        {
            InitBot();
            var logMessage = Layout.Render(logEvent);
            _bot.SendTextMessageAsync(
                ChatId,
                logMessage,
                parseMode: ParseMode.Markdown);
        }

        private void InitBot()
        {
            if (_bot == null)
            {
                _bot = new TelegramBotClient(Token);
            }
            else
            {
                if (LogTargetConfigurationChanged())
                {
                    _bot = null;
                    _bot = new TelegramBotClient(Token);
                }
            }
        }
        private bool LogTargetConfigurationChanged()
        {
            return false;
        }

    }
}
