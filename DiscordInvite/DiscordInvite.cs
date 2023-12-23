using Terraria;
using System;
using TShockAPI;
using TerrariaApi.Server;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace DiscordInvite
{

    [JsonObject]
    public class InviteConfig
    {
        public string InviteMessage { get; set; }
    }

    [ApiVersion(2, 1)]
    public class DiscordInviteGen : TerrariaPlugin
    {
        private static string ConfigPath = Path.Combine(TShock.SavePath, "invite.config");
        private InviteConfig config;

        public override string Name => "Discord Command";
        public override string Author => "Zekevious";
        public override string Description => "Discord chat command";
        public override Version Version => new Version(1, 0, 0);

        public DiscordInviteGen(Main game) : base(game) {}

        public override void Initialize()
        {
            SetupConfig();

            TShockAPI.Commands.ChatCommands.Add(new Command("discord.invite", DiscordCommand, "discord")
            {
                AllowServer = false,
                HelpText = "Join the Discord server."
            });
        }

        private void SetupConfig()
        {
            if (!File.Exists(ConfigPath))
            {
                config = new InviteConfig
                {
                    InviteMessage = "discord.gg/INVITE"
                };
                SaveConfig();
            }
            else
            {
                string configContents = File.ReadAllText(ConfigPath);
                config = JsonConvert.DeserializeObject<InviteConfig>(configContents);
            }
        }

        private void SaveConfig()
        {
            string configContents = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(ConfigPath, configContents);
        }

        private void DiscordCommand(CommandArgs args)
        {
            args.Player.SendInfoMessage(config.InviteMessage);
        }
    }
}
