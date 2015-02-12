// Copyright 2014 - 2014 Esk0r
// Config.cs is part of Evade.
// 
// Evade is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Evade is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Evade. If not, see <http://www.gnu.org/licenses/>.

#region

using System.Drawing;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;

#endregion

namespace Evade
{
    internal static class Config
    {
        public const bool PrintSpellData = false;
        public const bool TestOnAllies = false;
        public const int SkillShotsExtraRadius = 9;
        public const int SkillShotsExtraRange = 20;
        public const int GridSize = 10;
        public const int ExtraEvadeDistance = 15;
        public const int DiagonalEvadePointsCount = 7;
        public const int DiagonalEvadePointsStep = 20;

        public const int CrossingTimeOffset = 250;

        public const int EvadingFirstTimeOffset = 250;
        public const int EvadingSecondTimeOffset = 0;

        public const int EvadingRouteChangeTimeOffset = 250;

        public const int EvadePointChangeInterval = 300;
        public static int LastEvadePointChangeT = 0;

        public static Menu Menu;

        public static void CreateMenu()
        {
            Menu = new Menu("Kacis", "Evade", true);

            //Create the evade spells submenus.
            var evadeSpells = new Menu("Kacis Yetenekleri", "evadeSpells");
            foreach (var spell in EvadeSpellDatabase.Spells)
            {
                var subMenu = new Menu(spell.Name, spell.Name);

                subMenu.AddItem(
                    new MenuItem("DangerLevel" + spell.Name, "Tehlike Seviyesi").SetValue(
                        new Slider(spell.DangerLevel, 5, 1)));

                if (spell.IsTargetted && spell.ValidTargets.Contains(SpellValidTargets.AllyWards))
                {
                    subMenu.AddItem(new MenuItem("WardJump" + spell.Name, "Ziplama").SetValue(true));
                }

                subMenu.AddItem(new MenuItem("Enabled" + spell.Name, "Aktif").SetValue(true));

                evadeSpells.AddSubMenu(subMenu);
            }
            Menu.AddSubMenu(evadeSpells);

            //Create the skillshots submenus.
            var skillShots = new Menu("Kacilacak Yetenekler", "Skillshots");

            foreach (var hero in ObjectManager.Get<Obj_AI_Hero>())
            {
                if (hero.Team != ObjectManager.Player.Team || Config.TestOnAllies)
                {
                    foreach (var spell in SpellDatabase.Spells)
                    {
                        if (spell.ChampionName.ToLower() == hero.ChampionName.ToLower())
                        {
                            var subMenu = new Menu(spell.MenuItemName, spell.MenuItemName);

                            subMenu.AddItem(
                                new MenuItem("DangerLevel" + spell.MenuItemName, "Tehlike Seviyesi").SetValue(
                                    new Slider(spell.DangerValue, 5, 1)));

                            subMenu.AddItem(
                                new MenuItem("IsDangerous" + spell.MenuItemName, "Tehlikeli").SetValue(
                                    spell.IsDangerous));

                            subMenu.AddItem(new MenuItem("Draw" + spell.MenuItemName, "Cizim").SetValue(true));
                            subMenu.AddItem(new MenuItem("Enabled" + spell.MenuItemName, "Aktif").SetValue(true));

                            skillShots.AddSubMenu(subMenu);
                        }
                    }
                }
            }

            Menu.AddSubMenu(skillShots);

            var shielding = new Menu("Ally shielding", "Kalkan");

            foreach (var ally in ObjectManager.Get<Obj_AI_Hero>())
            {
                if (ally.IsAlly && !ally.IsMe)
                {
                    shielding.AddItem(
                        new MenuItem("shield" + ally.ChampionName, "Kalkan " + ally.ChampionName).SetValue(true));
                }
            }
            Menu.AddSubMenu(shielding);

            var collision = new Menu("Carpisma", "Collision");
            collision.AddItem(new MenuItem("MinionCollision", "Minyon ile Carpisma").SetValue(false));
            collision.AddItem(new MenuItem("HeroCollision", "Sampiyon ile Carpisma").SetValue(false));
            collision.AddItem(new MenuItem("YasuoCollision", "Yasuo duvari ile carpisma").SetValue(true));
            collision.AddItem(new MenuItem("EnableCollision", "Aktif").SetValue(true));
            //TODO add mode.
            Menu.AddSubMenu(collision);

            var drawings = new Menu("Drawings", "Drawings");
            drawings.AddItem(new MenuItem("EnabledColor", "Aktif buyu renkleri").SetValue(Color.White));
            drawings.AddItem(new MenuItem("DisabledColor", "Devre disi buyu renkleri").SetValue(Color.Red));
            drawings.AddItem(new MenuItem("MissileColor", "Mermi,ok renkleri").SetValue(Color.LimeGreen));
            drawings.AddItem(new MenuItem("Border", "Cizim kalinligi").SetValue(new Slider(1, 5, 1)));

            drawings.AddItem(new MenuItem("EnableDrawings", "Aktif").SetValue(true));
            Menu.AddSubMenu(drawings);

            var misc = new Menu("Misc", "Misc");
            misc.AddItem(new MenuItem("DisableFow", "Savas sisinden gelen yetenekler devre disi").SetValue(false));
            misc.AddItem(new MenuItem("ShowEvadeStatus", "Kacis Durumunu Goster").SetValue(false));
            Menu.AddSubMenu(misc);

            Menu.AddItem(
                new MenuItem("Enabled", "Aktif").SetValue(new KeyBind("K".ToCharArray()[0], KeyBindType.Toggle, true)));

            Menu.AddItem(
                new MenuItem("OnlyDangerous", "Yalniz tehlikeli yeteneklerden kac").SetValue(new KeyBind(32, KeyBindType.Press)));

            Menu.AddToMainMenu();
        }
    }
}
