﻿#region "copyright"

/*
    Copyright Dale Ghent <daleg@elemental.org>

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/
*/

#endregion "copyright"

using DaleGhent.NINA.AstroPhysics.Utility;
using NINA.Core.Utility;
using NINA.Plugin;
using NINA.Plugin.Interfaces;
using NINA.Profile.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace DaleGhent.NINA.AstroPhysics {

    [Export(typeof(IPluginManifest))]
    public class AstroPhysicsTools : PluginBase, ISettings, INotifyPropertyChanged {

        [ImportingConstructor]
        public AstroPhysicsTools() {
            if (Properties.Settings.Default.UpgradeSettings) {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.UpgradeSettings = false;
                CoreUtil.SaveSettings(Properties.Settings.Default);
            }

            APPMExePathDialogCommand = new RelayCommand(OpenAPPMExePathDialog);
            APPMSettingsPathDialogCommand = new RelayCommand(OpenAPPMSettingsPathDialog);
            APPMMapPathDialoggCommand = new RelayCommand(OpenAPPMMapPathDialog);
            ApccExePathDialogCommand = new RelayCommand(OpenApccExePathDialog);
        }

        public string APPMExePath {
            get => Properties.Settings.Default.APPMExePath;
            set {
                Properties.Settings.Default.APPMExePath = value;
                CoreUtil.SaveSettings(Properties.Settings.Default);
                RaisePropertyChanged();
            }
        }

        public string APPMSettingsPath {
            get => Properties.Settings.Default.APPMSettingsPath;
            set {
                Properties.Settings.Default.APPMSettingsPath = value;
                CoreUtil.SaveSettings(Properties.Settings.Default);
                RaisePropertyChanged();
            }
        }

        public string APPMMapPath {
            get => Properties.Settings.Default.APPMMapPath;
            set {
                Properties.Settings.Default.APPMMapPath = value;
                CoreUtil.SaveSettings(Properties.Settings.Default);
                RaisePropertyChanged();
            }
        }

        public string ApccExePath {
            get {
                if (string.IsNullOrEmpty(Properties.Settings.Default.ApccExePath)) {
                    // Find APCC Pro first, then look for Standard
                    if (File.Exists(Properties.Settings.Default.ApccDefaultProPath)) {
                        ApccExePath = Properties.Settings.Default.ApccDefaultProPath;
                    } else if (File.Exists(Properties.Settings.Default.ApccDefaultStandardPath)) {
                        ApccExePath = Properties.Settings.Default.ApccDefaultStandardPath;
                    }
                }

                return Properties.Settings.Default.ApccExePath;
            }
            set {
                Properties.Settings.Default.ApccExePath = value;
                CoreUtil.SaveSettings(Properties.Settings.Default);
                RaisePropertyChanged();
            }
        }

        public uint ApccStartupTimeout {
            get => Properties.Settings.Default.ApccStartupTimeout;
            set {
                Properties.Settings.Default.ApccStartupTimeout = value;
                CoreUtil.SaveSettings(Properties.Settings.Default);
                RaisePropertyChanged();
            }
        }

        public uint ApccDriverConnectTimeout {
            get => Properties.Settings.Default.ApccDriverConnectTimeout;
            set {
                Properties.Settings.Default.ApccDriverConnectTimeout = value;
                CoreUtil.SaveSettings(Properties.Settings.Default);
                RaisePropertyChanged();
            }
        }

        public int DecArcRaSpacing {
            get => Properties.Settings.Default.DecArcRaSpacing;
            set {
                Properties.Settings.Default.DecArcRaSpacing = value;
                CoreUtil.SaveSettings(Properties.Settings.Default);
                RaisePropertyChanged();
            }
        }

        public int DecArcDecSpacing {
            get => Properties.Settings.Default.DecArcDecSpacing;
            set {
                Properties.Settings.Default.DecArcDecSpacing = value;
                CoreUtil.SaveSettings(Properties.Settings.Default);
                RaisePropertyChanged();
            }
        }

        public int DecArcQuantity {
            get => Properties.Settings.Default.DecArcQuantity;
            set {
                Properties.Settings.Default.DecArcQuantity = value;
                CoreUtil.SaveSettings(Properties.Settings.Default);
                RaisePropertyChanged();
            }
        }

        public double HourAngleLeadIn {
            get {
                HoursToDegrees = 15 * Properties.Settings.Default.HourAngleLeadIn;
                return Properties.Settings.Default.HourAngleLeadIn;
            }
            set {
                Properties.Settings.Default.HourAngleLeadIn = value;
                CoreUtil.SaveSettings(Properties.Settings.Default);
                RaisePropertyChanged();
            }
        }

        private double hoursToDegrees = double.NaN;

        public double HoursToDegrees {
            get => hoursToDegrees;
            private set {
                hoursToDegrees = value;
                RaisePropertyChanged();
            }
        }

        public IList<string> PointOrderingStrategyList => Utility.Utility.PointOrderingStrategyList;

        public int PointOrderingStrategy {
            get => Properties.Settings.Default.PointOrderingStrategy;
            set {
                Properties.Settings.Default.PointOrderingStrategy = value;
                CoreUtil.SaveSettings(Properties.Settings.Default);
                RaisePropertyChanged();
            }
        }

        public int PolarPointOrderingStrategy {
            get => Properties.Settings.Default.PolarPointOrderingStrategy;
            set {
                Properties.Settings.Default.PolarPointOrderingStrategy = value;
                CoreUtil.SaveSettings(Properties.Settings.Default);
                RaisePropertyChanged();
            }
        }

        public int PolarProximityLimit {
            get => Properties.Settings.Default.PolarProximityLimit;
            set {
                Properties.Settings.Default.PolarProximityLimit = value;
                CoreUtil.SaveSettings(Properties.Settings.Default);
                RaisePropertyChanged();
            }
        }

        private void OpenAPPMExePathDialog(object obj) {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog {
                FileName = string.Empty,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                Filter = "Any Program|*.exe"
            };

            if (dialog.ShowDialog() == true) {
                APPMExePath = dialog.FileName;
            }
        }

        private void OpenAPPMSettingsPathDialog(object obj) {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog {
                FileName = string.Empty,
                InitialDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"Astro-Physics\APPM"),
                Filter = "APPM Settings File|*.appm"
            };

            if (dialog.ShowDialog() == true) {
                APPMSettingsPath = dialog.FileName;
            }
        }

        private void OpenAPPMMapPathDialog(object obj) {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog {
                FileName = string.Empty,
                InitialDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"Astro-Physics\APPM"),
                Filter = "APPM Mapping File|*.csv"
            };

            if (dialog.ShowDialog() == true) {
                APPMMapPath = dialog.FileName;
            }
        }

        private void OpenApccExePathDialog(object obj) {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog {
                FileName = string.Empty,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                Filter = "Any Program|*.exe"
            };

            if (dialog.ShowDialog() == true) {
                ApccExePath = dialog.FileName;
            }
        }

        public ICommand APPMExePathDialogCommand { get; private set; }
        public ICommand APPMSettingsPathDialogCommand { get; private set; }
        public ICommand APPMMapPathDialoggCommand { get; private set; }
        public ICommand ApccExePathDialogCommand { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}