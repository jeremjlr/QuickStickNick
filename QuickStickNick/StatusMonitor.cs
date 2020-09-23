﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using System.ComponentModel;
using Emgu.CV.Structure;
using Emgu.CV;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading.Tasks;

namespace QuickStickNick
{
    /// <summary>
    /// This class does all the auras monitoring stuff, it tracks auras and updates their status.
    /// </summary>
    public class StatusMonitor
    {
        #region Private attributes
        private IntPtr GAME_HWND;
        private MainForm mainForm;
        private bool aurasCalibrated;
        private Dictionary<AuraNames, Aura> auras = new Dictionary<AuraNames, Aura>();
        #endregion

        #region P/Invoke stuff
        [DllImportAttribute("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
        #endregion

        #region Constructors and public methods
        public StatusMonitor(IntPtr game_hWnd, MainForm mainForm)
        {
            GAME_HWND = game_hWnd;
            this.mainForm = mainForm;
            //Starts information gathering thread
            StatusUpdater.DoWork += StatusUpdate;
            StatusUpdater.RunWorkerAsync();

            //BM auras
            auras.Add(AuraNames.BarbedShot, new Aura(AuraNames.BarbedShot, Classes.BM, 650, 364, 64, 64));
            auras.Add(AuraNames.BeastCleave, new Aura(AuraNames.BeastCleave, Classes.BM, 650, 428, 64, 64));
            auras.Add(AuraNames.BeastCleave3, new Aura(AuraNames.BeastCleave3, Classes.BM, 618, 428, 32, 32));
            auras.Add(AuraNames.BeastCleave4, new Aura(AuraNames.BeastCleave4, Classes.BM, 618, 460, 32, 32));
            auras.Add(AuraNames.KillCommand, new Aura(AuraNames.KillCommand, Classes.BM, 650, 300, 64, 64));
            auras.Add(AuraNames.KillCommandBurst, new Aura(AuraNames.KillCommandBurst, Classes.BM, 618, 300, 32, 32));
            auras.Add(AuraNames.Focus, new Aura(AuraNames.Focus, Classes.BM, 650, 200, 64, 64));
            //Survival auras
            auras.Add(AuraNames.Focus87, new Aura(AuraNames.Focus87, Classes.Survival, 624, 332, 32, 32));
            auras.Add(AuraNames.SerpentSting, new Aura(AuraNames.SerpentSting, Classes.Survival, 624, 300, 32, 32));
            auras.Add(AuraNames.Wildfire, new Aura(AuraNames.Wildfire, Classes.Survival, 624, 364, 32, 32));
            auras.Add(AuraNames.NotMongooseBite, new Aura(AuraNames.NotMongooseBite, Classes.Survival, 656, 332, 32, 32));
            auras.Add(AuraNames.NotSerpentStingBomb, new Aura(AuraNames.NotSerpentStingBomb, Classes.Survival, 656, 300, 32, 32));
            //Prot auras
            auras.Add(AuraNames.Avatar, new Aura(AuraNames.Avatar, Classes.Prot, 656, 300, 32, 32));
            auras.Add(AuraNames.AvatarNotUp, new Aura(AuraNames.AvatarNotUp, Classes.Prot, 656, 332, 32, 32));
            auras.Add(AuraNames.DemoShout, new Aura(AuraNames.DemoShout, Classes.Prot, 656, 364, 32, 32));
            auras.Add(AuraNames.ShieldSlam, new Aura(AuraNames.ShieldSlam, Classes.Prot, 624, 300, 32, 32));
            auras.Add(AuraNames.ThunderClap, new Aura(AuraNames.ThunderClap, Classes.Prot, 624, 332, 32, 32));
            auras.Add(AuraNames.IgnorePain, new Aura(AuraNames.IgnorePain, Classes.Prot, 624, 364, 32, 32));
            auras.Add(AuraNames.NotFreeRevenge, new Aura(AuraNames.NotFreeRevenge, Classes.Prot, 656, 396, 32, 32));
            auras.Add(AuraNames.RageLowerThan40, new Aura(AuraNames.RageLowerThan40, Classes.Prot, 624, 396, 32, 32));
            auras.Add(AuraNames.NotVictoryRush, new Aura(AuraNames.NotVictoryRush, Classes.Prot, 624, 238, 32, 32));
            //Arms auras
            auras.Add(AuraNames.Skullsplitter, new Aura(AuraNames.Skullsplitter, Classes.Arms, 624, 300, 32, 32));
            auras.Add(AuraNames.NotInForTheKill, new Aura(AuraNames.NotInForTheKill, Classes.Arms, 656, 300, 32, 32));
            auras.Add(AuraNames.Overpower, new Aura(AuraNames.Overpower, Classes.Arms, 656, 332, 32, 32));
            auras.Add(AuraNames.MortalStrike, new Aura(AuraNames.MortalStrike, Classes.Arms, 624, 332, 32, 32));
            auras.Add(AuraNames.NotWW, new Aura(AuraNames.NotWW, Classes.Arms, 624, 364, 32, 32));
            auras.Add(AuraNames.NotExec, new Aura(AuraNames.NotExec, Classes.Arms, 656, 364, 32, 32));
            auras.Add(AuraNames.SweepingStrikes, new Aura(AuraNames.SweepingStrikes, Classes.Arms, 624, 396, 32, 32));
            auras.Add(AuraNames.NotVictoryRushArms, new Aura(AuraNames.NotVictoryRushArms, Classes.Arms, 624, 238, 32, 32));
        }

        public void Calibrate()
        {
            Classes focus = Classes.BM;
            if (mainForm.BMButton.Checked)
            {
                focus = Classes.BM;
            }
            else if (mainForm.SurvivalButton.Checked)
            {
                focus = Classes.Survival;
            }
            else if (mainForm.ProtButton.Checked)
            {
                focus = Classes.Prot;
            }
            else if (mainForm.ArmsButton.Checked)
            {
                focus = Classes.Arms;
            }

            foreach (KeyValuePair<AuraNames, Aura> entry in auras)
            {
                if (entry.Value.GameClass == focus)
                {
                    entry.Value.Calibrate();
                }
            }
                /*if (mainForm.BMButton.Checked)
                {

                    //Kill Command aura
                    bSAura = GetAura(BS_ICON_XPOS, BS_ICON_YPOS);
                    bSAura.Save(calibrationPath + "BS.bmp");
                    //Barbed Shot aura
                    kCAura = GetAura(KC_ICON_XPOS, KC_ICON_YPOS);
                    kCAura.Save(calibrationPath + "KC.bmp");
                    //Beast Cleave aura
                    bCAura = GetAura(BC_ICON_XPOS, BC_ICON_YPOS);
                    bCAura.Save(calibrationPath + "BC.bmp");
                    //Focus aura
                    focusAura = GetAura(FOCUS_ICON_XPOS, FOCUS_ICON_YPOS);
                    focusAura.Save(calibrationPath + "FOCUS.bmp");
                }
                else if (mainForm.ProtButton.Checked)
                {
                    //Avatar aura
                    avatarAura = GetAura(AVATAR_ICON_XPOS, AVATAR_ICON_YPOS);
                    avatarAura.Save(calibrationPath + "AVATAR.bmp");
                    //AvatarUp aura
                    avatarUpAura = GetAura(AVATARUP_ICON_XPOS, AVATARUP_ICON_YPOS);
                    avatarUpAura.Save(calibrationPath + "AVATARUP.bmp");
                }*/
                aurasCalibrated = true;
        }

        public bool GetAurasPresence(AuraNames name)
        {
            return auras[name].Present;
        }

        public bool GameHasFocus()
        {
            //Ignored because of a bug with WoW (fuck off Blizzard)
            return true;
            if (GetForegroundWindow() == GAME_HWND)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Private methods
        private BackgroundWorker StatusUpdater = new BackgroundWorker();
        private void StatusUpdate(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                //If the game has focus and we want to gather information
                if (GameHasFocus() && aurasCalibrated && mainForm.DoWork)
                {
                    Classes focus = Classes.BM;
                    if (mainForm.BMButton.Checked)
                    {
                        focus = Classes.BM;
                    }
                    else if (mainForm.SurvivalButton.Checked)
                    {
                        focus = Classes.Survival;
                    }
                    else if (mainForm.ProtButton.Checked)
                    {
                        focus = Classes.Prot;
                    }
                    else if (mainForm.ArmsButton.Checked)
                    {
                        focus = Classes.Arms;
                    }

                    foreach (KeyValuePair<AuraNames, Aura> entry in auras)
                    {
                        if (entry.Value.GameClass == focus)
                        {
                            //Task.Factory.StartNew(() =>
                            //{
                                entry.Value.Update();
                            //});
                        }
                    }
                }
                stopwatch.Stop();
                Console.WriteLine(stopwatch.ElapsedMilliseconds);
                //Thread.Sleep(15);
            }
        }
        #endregion
    }
}
