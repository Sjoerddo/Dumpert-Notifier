﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace DumpertNotifier
{
    public partial class DnForm : Form
    {
        private readonly Uri _homepage = new Uri("http://www.dumpert.nl");
        private readonly FeedManager _manager;
        private DateTime _startTime = DateTime.Now;
        private Uri _notificationUrl;

        public DnForm()
        {
            _manager = new FeedManager();
            InitializeComponent();
        }

        private void _notifyIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            Process.Start((_notificationUrl ?? _homepage).ToString());
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            var items = _manager.GetNewItems(_startTime);
            var count = items.Count;

            if (count == 0) return;

            var first = items.First();
            _startTime = first.PublishDate.DateTime;
            _notificationUrl = _manager.GetFirstUrlFromItem(first);

            _notifyIcon.ShowBalloonTip(5000, (count == 1) ? "Nieuw filmpje!" : $"{count} nieuwe filmpjes!",
                $"{first.Title.Text}\n{first.Summary.Text}\n{_startTime.ToShortTimeString()}",
                ToolTipIcon.Info);
            _notifyIcon.Text = $@"Laatste filmpje: {_startTime.ToShortTimeString()}";
        }

        private void quitMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}