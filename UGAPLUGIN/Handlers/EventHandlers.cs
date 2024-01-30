using System.Collections.Generic;
using Exiled.API.Enums;
using PlayerRoles;
using System.Data;
using Exiled.API.Extensions;
using Discord;
using Exiled.API.Features;
using Exiled.Events.Handlers;
using PluginAPI.Core;
using UnityEngine;
using MEC;
using static MapGeneration.ImageGenerator;
using Achievements.Handlers;
using PluginAPI.Events;
using Exiled.Events.EventArgs.Player;
using Exiled.CustomRoles.API;
using System.ComponentModel;
using System.Linq;
using System;
using System.Security.Policy;
using System.Diagnostics.Eventing.Reader;
using PluginAPI.Helpers;

// Right now I'm stuck on Steam ID 64 for KD Ratio. On the server it just gives everyone the same kill/death ratio.
namespace acoolplugin.Handlers
{
    public class EventHandlers
    {
        internal Plugin plugin;
        internal EventHandlers(Plugin plugin) => this.plugin = plugin;
        public bool IsAlive { get; set; }
        public string Nickname { get; set; }
        int Kills = 0;
        int Deaths = 0;
        static void Counter(ref int i) //Using this for counting your deaths, and to count kills!
        {
            i++;
        }
        static void NegCounter(ref int i)
        {
            i--;
        }
        public void OnRoundStart()
        {
            Exiled.API.Features.Log.Info("Round has started!!!");
            Plugin.Instance.DuringRound = true;
            return;
        }
        Dictionary<string, int> killCount = new Dictionary<string, int>()
        {

        };
        Dictionary<string, int> deathCount = new Dictionary<string, int>()
        {

        };
        Dictionary<string, int> tkCount = new Dictionary<string, int>()
        {

        };
        #region 
        public void DCLASSNOTTK(DyingEventArgs ev) //Oh God, what have I done....
        {
            Counter(ref Kills);
            string KD = $"|KD|  {Kills} : {Deaths}  ";
            ev.Player.RankName = "Class-D                         " + KD;
            ev.Attacker.RankColor = "pumpkin";
            ev.Attacker.RankColor.ToString();
        }
        public void DCLASSTK(DyingEventArgs ev)
        {
            NegCounter(ref Kills);
            string KD = $"|KD|  {Kills} : {Deaths}  ";
            ev.Player.RankName = "Class-D                         " + KD;
            ev.Attacker.RankColor = "pumpkin";
            ev.Attacker.RankColor.ToString();
        }
        public void FACILITYGUARDTK(DyingEventArgs ev)
        {
            NegCounter(ref Kills);
            string KD = $"|KD|  {Kills} : {Deaths}  ";
            ev.Player.RankName = "Facility Guard                  " + KD;
          //ev.Player.RankName = "Scientist                       " + KD; I'm commenting here to compare sizes of the strings.
          //ev.Player.RankName = "Nine-Tailed Fox                 " + KD;
          //ev.Player.RankName = "Chaos Insurgency                " + KD;
            ev.Player.RankColor = "nickel";
            ev.Player.RankColor.ToString();
        }
        public void NTFTK(DyingEventArgs ev)
        {
            NegCounter(ref Kills);
            string KD = $"|KD|  {Kills} : {Deaths}  ";
            string NTF = "Nine-Tailed Fox";
            ev.Attacker.RankName = NTF + "                 " + KD;
            ev.Attacker.RankColor = "aqua";
            ev.Attacker.RankColor.ToString();
        }
        public void NTFNOTTK(DyingEventArgs ev)
        {
            Counter(ref Kills);
            string KD = $"|KD|  {Kills} : {Deaths}  ";
            string NTF = "Nine-Tailed Fox";
          //ev.Attacker.RankName = "Nine-Tailed Fox" + KD;
            ev.Attacker.RankName = NTF + "                 " + KD;
            ev.Attacker.RankColor = "aqua";
            ev.Attacker.RankColor.ToString();
        }
        public void CHAOSTK(DyingEventArgs ev)
        {
            NegCounter(ref Kills);
            string KD = $"|KD|  {Kills} : {Deaths}  ";
            string CI = "Chaos Insurgency";
            ev.Attacker.RankName = CI + "                " + KD;
            ev.Attacker.RankColor = "green";
            ev.Attacker.RankColor.ToString();
        }
        public void CHAOSNOTTK(DyingEventArgs ev)
        {
            Counter(ref Kills);
            string KD = $"|KD|  {Kills} : {Deaths}  ";
            string CI = "Chaos Insurgency";
            ev.Attacker.RankName = CI + "                " + KD;
            ev.Attacker.RankColor = "green";
            ev.Attacker.RankColor.ToString();
        }
        public void SCIENTISTTK(DyingEventArgs ev)
        {
            NegCounter(ref Kills);
            string KD = $"|KD|  {Kills} : {Deaths}  ";
            ev.Player.RankName = "Scientist                       " + KD;
            ev.Player.RankColor = "yellow";
            ev.Player.RankColor.ToString();
        }
        public void SCIENTISNOTTTK(DyingEventArgs ev)
        {
            Counter(ref Kills);
            string KD = $"|KD|  {Kills} : {Deaths}  ";
            ev.Player.RankName = "Scientist                       " + KD;
            ev.Player.RankColor = "yellow";
            ev.Player.RankColor.ToString();
        }
        public void OnDying(DyingEventArgs ev)
        {
            string KD = $"|KD|  {Kills} : {Deaths}  ";
            Exiled.API.Features.Log.Info(ev.Player.Nickname + " has died! ");
            if (ev.Attacker.Role == RoleTypeId.ClassD && ev.Player.Role == RoleTypeId.ClassD)
            {
                DCLASSNOTTK(ev);
            }
            if (ev.Player.IsNTF && ev.Attacker.IsNTF)
            {
                NTFTK(ev);
            }
            if (ev.Player.IsNTF && ev.Attacker.Role == RoleTypeId.Scientist)
            {
                SCIENTISTTK(ev);
            }
            if (ev.Player.IsCHI && ev.Attacker.IsCHI)
            {
                CHAOSTK(ev);
            }
            if (ev.Player.Role == RoleTypeId.ClassD && ev.Attacker.IsCHI)
            {
                CHAOSTK(ev);
            }
            if (ev.Player.IsCHI && ev.Attacker.Role == RoleTypeId.ClassD)
            {
                DCLASSTK(ev);
            }
            else
            {
                if (ev.Player.IsNTF)
                {
                    NTFNOTTK(ev);
                }
                if (ev.Player.IsCHI)
                {
                    CHAOSNOTTK(ev);
                }
                if (ev.Player.Role == RoleTypeId.Scientist)
                {
                    SCIENTISNOTTTK(ev);
                }
                if (ev.Player.Role == RoleTypeId.ClassD)
                {
                    DCLASSNOTTK(ev);
                }
                if (ev.Player.Role == RoleTypeId.Scp173)
                {
                    Counter(ref Kills);
                    ev.Player.RankName = "SCP-173                    " + KD;
                    ev.Player.RankColor = "crimson";
                    ev.Player.RankColor.ToString();
                }
                if (ev.Player.Role == RoleTypeId.Scp096)
                {
                    Counter(ref Kills);
                    ev.Player.RankName = "SCP-096                    " + KD;
                    ev.Player.RankColor = "crimson";
                    ev.Player.RankColor.ToString();
                }
                if (ev.Player.Role == RoleTypeId.Scp106)
                {
                    Counter(ref Kills);
                    ev.Player.RankName = "SCP-106                    " + KD;
                    ev.Player.RankColor = "crimson";
                    ev.Player.RankColor.ToString();
                }
                if (ev.Player.Role == RoleTypeId.Scp939)
                {
                    Counter(ref Kills);
                    ev.Player.RankName = "SCP-939                    " + KD;
                    ev.Player.RankColor = "crimson";
                    ev.Player.RankColor.ToString();
                }
                if (ev.Player.Role == RoleTypeId.Scp079)
                {
                    Counter(ref Kills);
                    ev.Player.RankName = "SCP-079                    " + KD;
                    ev.Player.RankColor = "crimson";
                    ev.Player.RankColor.ToString();
                }
                if (ev.Player.Role == RoleTypeId.Scp049)
                {
                    Counter(ref Kills);
                    ev.Player.RankName = "SCP-049                    " + KD;
                    ev.Player.RankColor = "crimson";
                    ev.Player.RankColor.ToString();
                }
                if (ev.Player.Role == RoleTypeId.Scp0492)
                {
                    Counter(ref Kills);
                    ev.Player.RankName = "SCP-049-2                  " + KD;
                    ev.Player.RankColor = "crimson";
                    ev.Player.RankColor.ToString();
                }
                if (ev.Player.Role == RoleTypeId.Scp3114)
                {
                    Counter(ref Kills);
                    ev.Player.RankName = "SCP-3114                   " + KD;
                    ev.Player.RankColor = "crimson";
                    ev.Player.RankColor.ToString();
                }
            }
            Counter(ref Deaths);
            ev.Player.RankName = "Spectator                    " + KD;
            ev.Player.RankColor = null;
            ev.Player.RankColor.ToString();            
        }

        #endregion
        // Spawning in, not dependent on steamid64.
        public void OnSpawning(SpawningEventArgs ev)
        {
            string KD = $"|KD|  {Kills} : {Deaths}  ";
            #region HumanROLES!
            string CI = "Chaos Insurgency";
            string NTF = "Nine-Tailed Fox";
            if (ev.Player.Role == RoleTypeId.Scientist)
            {
                ev.Player.RankName = "Scientist                " + KD;
                ev.Player.RankColor = "yellow";
                ev.Player.RankColor.ToString();
            }
            if (ev.Player.Role == RoleTypeId.ClassD)
            {
                ev.Player.RankName = "Class-D                         " + KD;
              //ev.Player.RankName = "Chaos Insurgency         " + KD;
                ev.Player.RankColor = "pumpkin";
                ev.Player.RankColor.ToString();
            }
            if (ev.Player.IsCHI)
            {
                ev.Player.RankName = CI + "         " + KD;
                ev.Player.RankColor = "green";
                ev.Player.RankColor.ToString();
            }
            if (ev.Player.IsNTF)
            {
                ev.Player.RankName = NTF + "         " + KD;
                ev.Player.RankColor = "aqua";
                ev.Player.RankColor.ToString();
            }
            if (ev.Player.Role == RoleTypeId.FacilityGuard)
            {
                ev.Player.RankName = "Facility Guard            " + KD;
                ev.Player.RankColor = "nickel";
                ev.Player.RankColor.ToString();
            }
            if (ev.Player.Role == RoleTypeId.Tutorial)
            {
                ev.Player.RankName = "Tutorial                  " + KD;
                ev.Player.RankColor = "magenta";
                ev.Player.RankColor.ToString();
            }
            if (ev.Player.Role == RoleTypeId.Overwatch)
            {
                ev.Player.RankName = "Overwatch                 " + KD;
                ev.Player.RankColor = null;
                ev.Player.RankColor.ToString();
            }
            if (ev.Player.Role == RoleTypeId.Spectator)
            {
                ev.Player.RankName = "Spectator                 " + KD;
                ev.Player.RankColor = null;
                ev.Player.RankColor.ToString();
            }
            #endregion
            #region SCP ROLES!
            if (ev.Player.Role == RoleTypeId.Scp173)
            {
                ev.Player.RankName = "SCP-173                    " + KD;
                ev.Player.RankColor = "crimson";
                ev.Player.RankColor.ToString();
            }
            if (ev.Player.Role == RoleTypeId.Scp096)
            {
                ev.Player.RankName = "SCP-096                    " + KD;
                ev.Player.RankColor = "crimson";
                ev.Player.RankColor.ToString();
            }
            if (ev.Player.Role == RoleTypeId.Scp106)
            {
                ev.Player.RankName = "SCP-106                    " + KD;
                ev.Player.RankColor = "crimson";
                ev.Player.RankColor.ToString();
            }
            if (ev.Player.Role == RoleTypeId.Scp939)
            {
                ev.Player.RankName = "SCP-939                    " + KD;
                ev.Player.RankColor = "crimson";
                ev.Player.RankColor.ToString();
            }
            if (ev.Player.Role == RoleTypeId.Scp079)
            {
                ev.Player.RankName = "SCP-079                    " + KD;
                ev.Player.RankColor = "crimson";
                ev.Player.RankColor.ToString();
            }
            if (ev.Player.Role == RoleTypeId.Scp049)
            {
                ev.Player.RankName = "SCP-049                    " + KD;
                ev.Player.RankColor = "crimson";
                ev.Player.RankColor.ToString();
            }
            if (ev.Player.Role == RoleTypeId.Scp0492)
            {
                ev.Player.RankName = "SCP-049-2                  " + KD;
                ev.Player.RankColor = "crimson";
                ev.Player.RankColor.ToString();
            }
            if (ev.Player.Role == RoleTypeId.Scp3114)
            {
                ev.Player.RankName = "SCP-3114                   " + KD;
                ev.Player.RankColor = "crimson";
                ev.Player.RankColor.ToString();
            }
            #endregion      
        }
        public void OnHurting(HurtingEventArgs ev) // Cola gives you health until you take more than two.
        {
            if (!Plugin.Instance.Config.IsEnabled)
                return;
            if (ev.DamageHandler.Type == DamageType.Scp207)
            {
                ev.Player.Heal(2f, overrideMaxHealth: false);
            }
        }
    }
}