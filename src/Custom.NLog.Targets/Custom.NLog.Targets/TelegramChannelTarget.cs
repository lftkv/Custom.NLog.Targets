using NLog;
using NLog.Config;
using NLog.Targets;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Custom.NLog.Targets
{
    [Target("TelegramChannel")]
    public class TelegramChannelTarget : TargetWithLayout
    {
        [RequiredParameter]
        public string Token { get; set; }

        [RequiredParameter]
        public string ChannelName { get; set; }

        private TelegramBotClient _bot;

        public TelegramChannelTarget(string token, string channelname)
        {
            Token = token;
            ChannelName = channelname;
        }

        protected override void Write(LogEventInfo logEvent)
        {
            InitBot();
            var logMessage = Layout.Render(logEvent);
            _bot.SendTextMessageAsync(
                ChannelName,
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
