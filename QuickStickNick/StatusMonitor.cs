using System;
using System.Collections.Generic;
using System.Threading;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace QuickStickNick
{
    /// <summary>
    /// This class does all the auras monitoring stuff, it tracks auras and updates their status.
    /// </summary>
    public class StatusMonitor
    {
        #region Private attributes
        private IntPtr _GAME_HWND;
        private MainForm _mainForm;
        private bool _aurasCalibrated;
        private Dictionary<AuraNames, Aura> _auras = new Dictionary<AuraNames, Aura>();
        #endregion

        #region P/Invoke stuff
        [DllImportAttribute("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
        #endregion

        #region Constructors and public methods
        public StatusMonitor(IntPtr game_hWnd, MainForm mainForm)
        {
            _GAME_HWND = game_hWnd;
            _mainForm = mainForm;
            //Starts the information gathering thread
            StatusUpdater.DoWork += StatusUpdate;
            StatusUpdater.RunWorkerAsync();

            //BM auras
            _auras.Add(AuraNames.BarbedShot, new Aura(AuraNames.BarbedShot, Classes.BM, 650, 364, 64, 64));
            _auras.Add(AuraNames.BeastCleave, new Aura(AuraNames.BeastCleave, Classes.BM, 650, 428, 64, 64));
            _auras.Add(AuraNames.BeastCleave3, new Aura(AuraNames.BeastCleave3, Classes.BM, 618, 428, 32, 32));
            _auras.Add(AuraNames.BeastCleave4, new Aura(AuraNames.BeastCleave4, Classes.BM, 618, 460, 32, 32));
            _auras.Add(AuraNames.KillCommand, new Aura(AuraNames.KillCommand, Classes.BM, 650, 300, 64, 64));
            _auras.Add(AuraNames.KillCommandBurst, new Aura(AuraNames.KillCommandBurst, Classes.BM, 618, 300, 32, 32));
            _auras.Add(AuraNames.Focus, new Aura(AuraNames.Focus, Classes.BM, 650, 200, 64, 64));
            //Survival auras
            _auras.Add(AuraNames.Focus87, new Aura(AuraNames.Focus87, Classes.Survival, 624, 332, 32, 32));
            _auras.Add(AuraNames.SerpentSting, new Aura(AuraNames.SerpentSting, Classes.Survival, 624, 300, 32, 32));
            _auras.Add(AuraNames.Wildfire, new Aura(AuraNames.Wildfire, Classes.Survival, 624, 364, 32, 32));
            _auras.Add(AuraNames.NotMongooseBite, new Aura(AuraNames.NotMongooseBite, Classes.Survival, 656, 332, 32, 32));
            _auras.Add(AuraNames.NotSerpentStingBomb, new Aura(AuraNames.NotSerpentStingBomb, Classes.Survival, 656, 300, 32, 32));
            //Prot auras
            _auras.Add(AuraNames.Avatar, new Aura(AuraNames.Avatar, Classes.Prot, 656, 300, 32, 32));
            _auras.Add(AuraNames.AvatarNotUp, new Aura(AuraNames.AvatarNotUp, Classes.Prot, 656, 332, 32, 32));
            _auras.Add(AuraNames.DemoShout, new Aura(AuraNames.DemoShout, Classes.Prot, 656, 364, 32, 32));
            _auras.Add(AuraNames.ShieldSlam, new Aura(AuraNames.ShieldSlam, Classes.Prot, 624, 300, 32, 32));
            _auras.Add(AuraNames.ThunderClap, new Aura(AuraNames.ThunderClap, Classes.Prot, 624, 332, 32, 32));
            _auras.Add(AuraNames.IgnorePain, new Aura(AuraNames.IgnorePain, Classes.Prot, 624, 364, 32, 32));
            _auras.Add(AuraNames.NotFreeRevenge, new Aura(AuraNames.NotFreeRevenge, Classes.Prot, 656, 396, 32, 32));
            _auras.Add(AuraNames.RageLowerThan40, new Aura(AuraNames.RageLowerThan40, Classes.Prot, 624, 396, 32, 32));
            _auras.Add(AuraNames.NotVictoryRush, new Aura(AuraNames.NotVictoryRush, Classes.Prot, 624, 238, 32, 32));
            //Arms auras
            _auras.Add(AuraNames.Skullsplitter, new Aura(AuraNames.Skullsplitter, Classes.Arms, 624, 300, 32, 32));
            _auras.Add(AuraNames.NotInForTheKill, new Aura(AuraNames.NotInForTheKill, Classes.Arms, 656, 300, 32, 32));
            _auras.Add(AuraNames.Overpower, new Aura(AuraNames.Overpower, Classes.Arms, 656, 332, 32, 32));
            _auras.Add(AuraNames.MortalStrike, new Aura(AuraNames.MortalStrike, Classes.Arms, 624, 332, 32, 32));
            _auras.Add(AuraNames.NotWW, new Aura(AuraNames.NotWW, Classes.Arms, 624, 364, 32, 32));
            _auras.Add(AuraNames.NotExec, new Aura(AuraNames.NotExec, Classes.Arms, 656, 364, 32, 32));
            _auras.Add(AuraNames.SweepingStrikes, new Aura(AuraNames.SweepingStrikes, Classes.Arms, 624, 396, 32, 32));
            _auras.Add(AuraNames.NotVictoryRushArms, new Aura(AuraNames.NotVictoryRushArms, Classes.Arms, 624, 238, 32, 32));
        }

        /// <summary>
        /// Calibrates all the auras of the selected class
        /// </summary>
        public void Calibrate()
        {
            //Default is BM
            Classes focus;
            if (_mainForm.BMButton.Checked)
            {
                focus = Classes.BM;
            }
            else if (_mainForm.SurvivalButton.Checked)
            {
                focus = Classes.Survival;
            }
            else if (_mainForm.ProtButton.Checked)
            {
                focus = Classes.Prot;
            }
            else /*if (mainForm.ArmsButton.Checked)*/
            {
                focus = Classes.Arms;
            }

            //Calibrates every aura that belongs to the selected class
            foreach (KeyValuePair<AuraNames, Aura> entry in _auras)
            {
                if (entry.Value.GameClass == focus)
                {
                    entry.Value.Calibrate();
                }
            }
            _aurasCalibrated = true;
        }

        /// <summary>
        /// Returns whether the aura is detected on screen or not
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool GetAurasPresence(AuraNames name)
        {
            return _auras[name].Present;
        }

        public bool GameHasFocus()
        {
            //Ignored because of a bug with WoW
            return true;
            return GetForegroundWindow() == _GAME_HWND;
        }

        #endregion

        #region Private methods
        private BackgroundWorker StatusUpdater = new BackgroundWorker();
        private void StatusUpdate(object sender, DoWorkEventArgs e)
        {
            //This is the main status update loop it runs
            while (true)
            {
                //If the game has focus, the auras are calibrated and the bot is turned on
                if (GameHasFocus() && _aurasCalibrated && _mainForm.DoWork)
                {
                    //Default is BM
                    Classes focus;
                    if (_mainForm.BMButton.Checked)
                    {
                        focus = Classes.BM;
                    }
                    else if (_mainForm.SurvivalButton.Checked)
                    {
                        focus = Classes.Survival;
                    }
                    else if (_mainForm.ProtButton.Checked)
                    {
                        focus = Classes.Prot;
                    }
                    else /*if (mainForm.ArmsButton.Checked)*/
                    {
                        focus = Classes.Arms;
                    }

                    //Updates all the auras of the selected class
                    foreach (KeyValuePair<AuraNames, Aura> entry in _auras)
                    {
                        if (entry.Value.GameClass == focus)
                        {
                            entry.Value.Update();
                        }
                    }
                }
                //To make sure the thread sleeps when we're not gathering information, mainForm.DoWork == false
                Thread.Sleep(10);
            }
        }
        #endregion
    }
}

