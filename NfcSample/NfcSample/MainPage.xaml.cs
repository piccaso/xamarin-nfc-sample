﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.NFC;
using Xamarin.Forms;

namespace NfcSample {
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage {
        public MainPage() {
            InitializeComponent();
        }

        protected override void OnAppearing() {
            base.OnAppearing();
        }

        private void AppendText(string text) {
            if(string.IsNullOrEmpty(text)) return;
            if (Editor.Text.Length > 0) {
                Editor.Text += $"{Environment.NewLine}{text}";
            } else {
                Editor.Text = text;
            }
        }

        private bool _nfcEnabled;
        private void NfcSetup() {
            AppendText($"CrossNFC.IsSupported = {CrossNFC.IsSupported}");
            AppendText($"CrossNFC.Current.IsAvailable = {CrossNFC.Current.IsAvailable}");
            AppendText($"CrossNFC.Current.IsEnabled = {CrossNFC.Current.IsEnabled}");

            if (!CrossNFC.IsSupported || !CrossNFC.Current.IsAvailable || !CrossNFC.Current.IsEnabled) {
                return;
            }
            if (_nfcEnabled) {
                CrossNFC.Current.OnMessageReceived -= CurrentOnOnMessageReceived;
                CrossNFC.Current.StopListening();
            }

            CrossNFC.Current.OnMessageReceived += CurrentOnOnMessageReceived;
            CrossNFC.Current.StartListening();
            _nfcEnabled = true;
        }

        private void CurrentOnOnMessageReceived(ITagInfo tagInfo) {
            AppendText("MessageReceived");
            AppendText($"SerialNumber = {tagInfo.SerialNumber}");
            var id = NFCUtils.ByteArrayToHexString(tagInfo.Identifier);
            AppendText($"Identifier = {id}");
            AppendText($"tagInfo.IsEmpty = {tagInfo.IsEmpty}");
            AppendText($"tagInfo.IsSupported = {tagInfo.IsSupported}");
            AppendText($"tagInfo.IsWritable = {tagInfo.IsWritable}");
            if (tagInfo.Records != null) {
                foreach (var record in tagInfo.Records) {
                    if (record.Payload != null && record.Payload.Any()) {
                        var payload = NFCUtils.ByteArrayToHexString(record.Payload);
                        AppendText($"record.Payload = {payload}");
                    }
                }
            }
            Device.BeginInvokeOnMainThread(() => {
                CrossNFC.Current.StartListening();
            });
        }
        private void EnableButtonClicked(object sender, EventArgs e) {
            try {
                NfcSetup();
            } catch (Exception ex) {
                AppendText(ex.ToString());
            }
        }
    }
}
