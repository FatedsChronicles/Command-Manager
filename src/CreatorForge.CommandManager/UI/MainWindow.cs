using System;
using System.Drawing;
using System.Windows.Forms;
using CreatorForge.CommandManager.Core;
using CreatorForge.CommandManager.Theme;

namespace CreatorForge.CommandManager.UI
{
    public sealed class MainWindow : Form
    {
        private readonly object _streamerBotApi;

        public MainWindow(object streamerBotApi)
        {
            _streamerBotApi = streamerBotApi;
            InitializeWindow();
            BuildContent();
        }

        private void InitializeWindow()
        {
            Text = "Creator Forge Command Manager";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(720, 480);
            ClientSize = new Size(960, 620);
            BackColor = UiColors.Background;
            ForeColor = UiColors.Text;
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
        }

        private void BuildContent()
        {
            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = UiColors.Background,
                Padding = new Padding(28),
                ColumnCount = 1,
                RowCount = 4
            };
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 86F));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 1F));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));

            var header = BuildHeader();
            var divider = new Panel { Dock = DockStyle.Fill, BackColor = UiColors.Border };
            var body = BuildBody();
            var footer = BuildFooter();

            root.Controls.Add(header, 0, 0);
            root.Controls.Add(divider, 0, 1);
            root.Controls.Add(body, 0, 2);
            root.Controls.Add(footer, 0, 3);
            Controls.Add(root);
        }

        private Control BuildHeader()
        {
            var panel = new Panel { Dock = DockStyle.Fill, BackColor = UiColors.Background };

            var brand = new Label
            {
                AutoSize = true,
                Location = new Point(0, 4),
                Text = "CREATOR FORGE",
                ForeColor = UiColors.Accent,
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point)
            };

            var title = new Label
            {
                AutoSize = true,
                Location = new Point(0, 27),
                Text = "Command Manager",
                ForeColor = UiColors.Text,
                Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold, GraphicsUnit.Point)
            };

            var version = new Label
            {
                AutoSize = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Text = "v" + UiVersion.Display,
                ForeColor = UiColors.Muted,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point)
            };
            version.Location = new Point(Math.Max(0, panel.Width - version.PreferredWidth), 12);
            panel.Resize += (sender, args) =>
            {
                version.Location = new Point(Math.Max(0, panel.ClientSize.Width - version.PreferredWidth), 12);
            };

            panel.Controls.Add(brand);
            panel.Controls.Add(title);
            panel.Controls.Add(version);
            return panel;
        }

        private Control BuildBody()
        {
            var host = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 26, 0, 26),
                BackColor = UiColors.Background
            };

            var card = new Panel
            {
                Dock = DockStyle.Top,
                Height = 230,
                Padding = new Padding(28),
                BackColor = UiColors.Panel
            };

            var success = new Label
            {
                AutoSize = true,
                Location = new Point(28, 28),
                Text = "Native DLL loaded successfully",
                ForeColor = UiColors.Success,
                Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold, GraphicsUnit.Point)
            };

            var description = new Label
            {
                AutoSize = false,
                Location = new Point(28, 70),
                Size = new Size(720, 58),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Text = "Milestone 3N has opened a native WinForms window on its own UI thread. " +
                       "Command discovery will be connected after this loading boundary passes its live Streamer.bot test.",
                ForeColor = UiColors.Text
            };

            var bridgeStatus = new Label
            {
                AutoSize = true,
                Location = new Point(28, 150),
                Text = _streamerBotApi == null
                    ? "Streamer.bot bridge: unavailable"
                    : "Streamer.bot bridge: received",
                ForeColor = _streamerBotApi == null ? UiColors.Muted : UiColors.Success,
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point)
            };

            card.Controls.Add(success);
            card.Controls.Add(description);
            card.Controls.Add(bridgeStatus);
            host.Controls.Add(card);
            return host;
        }

        private Control BuildFooter()
        {
            return new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Text = "Milestone 3N · Native loading foundation",
                ForeColor = UiColors.Muted
            };
        }
    }
}

