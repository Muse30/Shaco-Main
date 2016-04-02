using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Drawing;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using EloBuddy.SDK.Events;

namespace Shaco
{
    public static class Program
    {
        public static AIHeroClient player = ObjectManager.Player;
        public static Item botrk = new Item((int)ItemId.Blade_of_the_Ruined_King);
        public static Item tiamat = new Item((int)ItemId.Blade_of_the_Ruined_King);
        public static Item hydra = new Item((int)ItemId.Blade_of_the_Ruined_King);
        public static Item titanic = new Item((int)ItemId.Blade_of_the_Ruined_King);
        public static Item randuins = new Item((int)ItemId.Blade_of_the_Ruined_King);
        public static Item odins = new Item((int)ItemId.Blade_of_the_Ruined_King);
        public static Item bilgewater = new Item((int)ItemId.Blade_of_the_Ruined_King);
        public static Item hexgun = new Item((int)ItemId.Blade_of_the_Ruined_King);
        public static Item Dfg = new Item((int)ItemId.Blade_of_the_Ruined_King);
        public static Item Bft = new Item((int)ItemId.Blade_of_the_Ruined_King);
        public static Item Ludens = new Item((int)ItemId.Blade_of_the_Ruined_King);
        public static Item Muramana = new Item((int)ItemId.Blade_of_the_Ruined_King);
        public static Item Muramana2 = new Item((int)ItemId.Blade_of_the_Ruined_King);
        public static Item sheen = new Item((int)ItemId.Blade_of_the_Ruined_King);
        public static Item gaunlet = new Item((int)ItemId.Blade_of_the_Ruined_King);
        public static Item trinity = new Item((int)ItemId.Blade_of_the_Ruined_King);
        public static Item lich = new Item((int)ItemId.Blade_of_the_Ruined_King);
        public static Item youmuu = new Item((int)ItemId.Blade_of_the_Ruined_King);
        public static Item frost = new Item((int)ItemId.Blade_of_the_Ruined_King);
        public static Item mountain = new Item((int)ItemId.Blade_of_the_Ruined_King);
        public static Item solari = new Item((int)ItemId.Blade_of_the_Ruined_King);
        public static Item Qss = new Item((int)ItemId.Blade_of_the_Ruined_King);
        public static Item Mercurial = new Item((int)ItemId.Blade_of_the_Ruined_King);
        public static Item Dervish = new Item((int)ItemId.Blade_of_the_Ruined_King);
        public static Item Zhonya = new Item((int)ItemId.Blade_of_the_Ruined_King);
        public static Item Woooglet = new Item((int)ItemId.Blade_of_the_Ruined_King);
        public static bool QssUsed;
        public static float MuramanaTime;
        public static Obj_AI_Base hydraTarget;
        public static bool CloneDelay;
        public static int CloneRange = 2300;
        public static int LastAATick;
        public static Item Hydra, Tiamat;
        public static Spell.Targeted Q, E, R;
        public static Spell.Skillshot W;
        public static Spell.Targeted Ignite;
        public static Spell.Active R2;
        public static Menu ShacoMenu, ComboMenu, HarassMenu, LaneClearMenu, MiscMenu, DrawingsMenu, SmiteMenu, escapeMenu;
        private static CheckBox _useq;
        private static Slider _useqmin;
        private static CheckBox _usew;
        private static CheckBox _useecombo;
        private static CheckBox _useeharass;
        private static CheckBox _user;
        private static CheckBox _usercc;
        private static CheckBox _moveclone;
        private static CheckBox _jukefleewithclone;
        private static Slider _jukefleepercentage;
        private static CheckBox _waitforstealth;
        private static CheckBox _useignite;
        private static CheckBox _ksq;
        private static KeyBind _automoveclone;
        private static CheckBox _ks;
        private static CheckBox _useitems;
        private static KeyBind _stackbox;
        private static CheckBox _drawq;
        private static CheckBox _drawqtimer;
        private static CheckBox _draww;
        private static CheckBox _drawe;
        private static CheckBox _drawdamagebar;
        private static CheckBox _evadeQ;
        private static KeyBind _evadeR;
        private static CheckBox _evade;
        public static Text StealthTimer;
        public static float DeceiveTime;
        public static double TotalDamage = 0;
        public static Spell.Targeted SmiteSpell;
        public static Obj_AI_Base Monster;
        public class LastSpellCast
        {
            public int CastTick;
            public SpellSlot Slot = SpellSlot.Unknown;
        }


        public static Text SmiteStatus = new Text("",
            new Font(FontFamily.GenericSansSerif, 9, FontStyle.Bold));

        public static AIHeroClient Player
        {
            get { return ObjectManager.Player; }
        }

        public static Obj_AI_Base Clone;

        public static Obj_AI_Minion Clone1
        {
            get
            {
                Obj_AI_Minion Clone = null;
                if (Player.Spellbook.GetSpell(SpellSlot.R).Name != "hallucinateguide") return null;
                return ObjectManager.Get<Obj_AI_Minion>().FirstOrDefault(m => m.Name == Player.Name && !m.IsMe);
            }
        }

        public static bool ShacoStealth
        {
            get { return Player.HasBuff("Deceive"); }
        }

        public static int GameTimeTickCount
        {
            get { return (int)(Game.Time * 1000); }
        }

        public static void Main()
        {
            Loading.OnLoadingComplete += OnLoad;
        }



        private static void OnLoad(EventArgs args)
        {
            if (Player.Hero != Champion.Shaco) return;
            Chat.Print("Shaco Ported", System.Drawing.Color.Red);
            Q = new Spell.Targeted(SpellSlot.Q, 400);
            W = new Spell.Skillshot(SpellSlot.W, 425, SkillShotType.Circular);
            E = new Spell.Targeted(SpellSlot.E, 625);
            R = new Spell.Targeted(SpellSlot.R, 2300);
            R2 = new Spell.Active(SpellSlot.R, 200);
            StealthTimer = new Text("", new Font("Verdana", 25F, FontStyle.Bold));
          
            ShacoMenu = MainMenu.AddMenu("Shaco Ported", "shaco");
            //Drawings Menu
            DrawingsMenu = ShacoMenu.AddSubMenu("Drawings", "drawingsmenu");
            DrawingsMenu.AddGroupLabel("Drawings Settings");
            DrawingsMenu.AddSeparator();
            _drawq = DrawingsMenu.Add("drawq", new CheckBox("Draw Q"));
            _draww = DrawingsMenu.Add("draww", new CheckBox("Draw W"));
            _drawe = DrawingsMenu.Add("drawe", new CheckBox("Draw E"));
            _drawdamagebar = DrawingsMenu.Add("drawdamagebar", new CheckBox("Draw HP bar damage indicator"));
            _drawqtimer = DrawingsMenu.Add("drawqtimer", new CheckBox("Draw Q timer"));
            DrawingsMenu.AddSeparator();

            ComboMenu = ShacoMenu.AddSubMenu("Combo", "Combo");
            ComboMenu.AddSeparator();
            //Combo Menu
            _useitems = ComboMenu.Add("useitems", new CheckBox("Use Items"));
            _useq = ComboMenu.Add("useqcombo", new CheckBox("Use Q"));
            _useqmin = ComboMenu.Add("useminrange", new Slider("Q min range", 200, 0, 400));
            _usew = ComboMenu.Add("usewcombo", new CheckBox("Use W"));
            _useecombo = ComboMenu.Add("useecombo", new CheckBox("Use E"));
            _user = ComboMenu.Add("usercombo", new CheckBox("Use R"));
            _moveclone = ComboMenu.Add("useclone", new CheckBox("Move clone on combo"));
            _usercc = ComboMenu.Add("usercc", new CheckBox("Dodge targeted cc"));
            _waitforstealth = ComboMenu.Add("waitforstealth", new CheckBox("Dont use Spells in Q"));
            _useignite = ComboMenu.Add("useignite", new CheckBox("Use ignite"));

            //Harass Menu
            HarassMenu = ShacoMenu.AddSubMenu("Harass", "harassmenu");
            HarassMenu.AddGroupLabel("Harass Settings");
            HarassMenu.AddSeparator();
            _useeharass = HarassMenu.Add("useeharass", new CheckBox("Use E Harass"));
            //Misc Menu
            MiscMenu = ShacoMenu.AddSubMenu("Clone Menu", "miscmenu");
            MiscMenu.AddGroupLabel("Misc Settings");
            MiscMenu.AddSeparator();
            _stackbox = MiscMenu.Add("stackbox",
    new KeyBind("Put box down", false, KeyBind.BindTypes.HoldActive, "G".ToCharArray()[0]));
            MiscMenu.Add("cloneorb",
             new KeyBind("Switch clone mode", false, KeyBind.BindTypes.PressToggle, "T".ToCharArray()[0]));
            MiscMenu.AddSeparator();

            StringList(MiscMenu, "Clonetarget", "Clone Priority",
                new[] { "Target Selector", "Lowest health", "Closest to you" }, 0);
            StringList(MiscMenu, "clonemode", "Clone mode",
                new[] { "Attack priority target", "Follow mouse" }, 0);
            MiscMenu.Add("followmouse", new CheckBox("Follow mouse when there's no target"));         
            MiscMenu.AddSeparator();
            _jukefleewithclone = MiscMenu.Add("jukefleewithclone", new CheckBox("Juke flee with clone"));
            _jukefleepercentage = MiscMenu.Add("jukefleeminpercentage", new Slider("Juke flee health percent", 15));
            MiscMenu.Add("ultwall", new CheckBox("Use ult to jump wall when fleeing"));
            _ksq = MiscMenu.Add("ksq", new CheckBox("KS E"));
            _ks = MiscMenu.Add("ks", new CheckBox("KS QE"));





            escapeMenu = ShacoMenu.AddSubMenu("Escape", "escapesmenu");
            _evadeQ = escapeMenu.Add("Escape", new CheckBox("Use Q to flee"));
            _evade = escapeMenu.Add("Evade", new CheckBox("Evade With Ultimate"));
            escapeMenu.AddSeparator();
            escapeMenu.AddGroupLabel("To use Flee use your keybind in the orbwalker menu");
           


            if (HasSpell("summonersmite"))
            {
               Smitemethod();
            }

            var slot = Player.GetSpellSlotFromName("summonerdot");
            if (slot != SpellSlot.Unknown)
            {
                Ignite = new Spell.Targeted(slot, 600);
            }
            Tiamat = new Item((int)ItemId.Tiamat_Melee_Only, 420);
            Hydra = new Item((int)ItemId.Ravenous_Hydra_Melee_Only, 420);


            GameObject.OnCreate += GameObject_OnCreate;
            GameObject.OnDelete += GameObject_OnDelete;
            DamageIndicator.Initialize();
            Game.OnUpdate += Game_OnGameUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
            Orbwalker.OnPostAttack += OrbwalkingOnAfterAttack;

        }


        private static void Combo(AIHeroClient target)
        {
            if (target == null)
            {
                return;
            }


            var cmbDmg = ComboDamage(target);

            if (ComboConfig.UseItems)
            {
                UseItems(target, cmbDmg);
            }

            var dist = (float)(Q.Range + Player.MoveSpeed * 2.5);
            if (Clone != null && !CloneDelay && MiscConfig.Moveclone && MiscConfig.JukeFleePercentage != Player.HealthPercent && MiscConfig.JukeFleeWithClone | !MiscConfig.JukeFleeWithClone)
            {
                moveClone();
            }
            if ((ComboConfig.Waitforstealth && ShacoStealth && cmbDmg < target.Health) ||
                !Orbwalker.CanMove)
            {
                return;
            }
            if (ComboConfig.UseQ && Q.IsReady() &&
                Game.CursorPos.Distance(target.Position) < 250 && target.Distance(Player) < dist &&
                (target.Distance(Player) >= ComboConfig.UseQMin ||
                 (cmbDmg > target.Health && Player.CountEnemiesInRange(2000) == 1)))
            {
                if (target.Distance(Player) < Q.Range)
                {
                    var pos = Predict(target, true, 0.5f);
                    Q.Cast(pos);
                }
                else
                {
                    if (!CheckWalls(target) || GetPath(Player, target.Position) < dist)
                    {
                        Q.Cast(
                            Player.Position.ExtendVector3(target.Position, Q.Range));
                    }
                }
            }
            if (ComboConfig.UseW && W.IsReady() &&
                target.Health > cmbDmg && Player.Distance(target) < W.Range)
            {
                HandleW(target);
            }
            if (ComboConfig.UseE && E.IsInRange(target) && E.IsReady())
            {
                E.Cast(target);
            }

            if (ComboConfig.UseR && R.IsReady() && Clone == null && target.HealthPercent < 75 &&
                cmbDmg < target.Health && target.HealthPercent > cmbDmg && target.HealthPercent > 25 && target.IsValidTarget(E.Range))
            {
                R2.Cast();
            }
        }




        private static void Harass()
        {
            Obj_AI_Base target = TargetSelector.GetTarget(E.Range, DamageType.Physical);
            if (target == null)
            {
                return;
            }
            if (HarassConfig.UseEHarass && E.IsInRange(target) && target.IsValidTarget(E.Range))
            {
                E.Cast(target);
            }
        }


 

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (Player.IsDead) return;
            var heropos = Drawing.WorldToScreen(ObjectManager.Player.Position);
            var buff = Player.GetBuff("Deceive");
            if (buff != null && DrawingConfig.DrawDeceiveTimer)
            {
                StealthTimer.Color = System.Drawing.Color.Red;
                StealthTimer.TextValue = Convert.ToString(Convert.ToInt32(buff.EndTime - Game.Time));
                StealthTimer.Position = Player.Position.WorldToScreen();
                StealthTimer.Draw();
            }
            var color = new ColorBGRA(255, 255, 255, 255);
            if (DrawingConfig.DrawQ && Q.IsReady())
            {
                Circle.Draw(color, Q.Range, Player.Position);
            }

            if (DrawingConfig.DrawW && W.IsReady())
            {
                Circle.Draw(color, W.Range, Player.Position);
            }
            if (DrawingConfig.DrawE && E.IsReady())
            {
                Circle.Draw(color, E.Range, Player.Position);
            }

            if (MiscMenu["cloneorb"].Cast<KeyBind>().CurrentValue && ObjectManager.Player.Level >= 6 && R.IsLearned)
            {
                Drawing.DrawText(heropos.X - 80, heropos.Y + 40, System.Drawing.Color.White, "Clone mode: Attack priority target");

            }
            if (!MiscMenu["cloneorb"].Cast<KeyBind>().CurrentValue && ObjectManager.Player.Level >= 6 && R.IsLearned)
            {
                Drawing.DrawText(heropos.X - 40, heropos.Y + 20, System.Drawing.Color.White, "Clone mode: Follow Mouse");
            }


        }

        private static void HandleW(Obj_AI_Base target)
        {
            var turret =
                ObjectManager.Get<Obj_AI_Turret>()
                    .OrderByDescending(t => t.Distance(target))
                    .FirstOrDefault(t => t.IsEnemy && t.Distance(target) < 3000 && !t.IsDead);
            if (turret != null)
            {
                CastW(target, target.Position, turret.Position);
            }
            else
            {
                if (target.IsMoving)
                {
                    var pred = W.GetPrediction(target);
                    if (pred.HitChance >= HitChance.Dashing)
                    {
                        CastW(target, target.Position, pred.UnitPosition);
                    }
                }
                else
                {
                    W.Cast(Player.Position.ExtendVector3(target.Position, W.Range - Player.Distance(target)));
                }
            }
        }

        private static void CastW(Obj_AI_Base target, Vector3 from, Vector3 to)
        {
            var positions = new List<Vector3>();

            for (var i = 1; i < 11; i++)
            {
                positions.Add(from.ExtendVector3(to, 42 * i));
            }
            var best =
                positions.OrderByDescending(p => p.Distance(target.Position))
                    .FirstOrDefault(
                        p => !p.IsWall() && p.Distance(Player.Position) < W.Range && p.Distance(target.Position) > 350);
            if (best != null && best.IsValid())
            {
                W.Cast(best);
            }
        }

        public static int TickCount
        {
            get
            {
                return Environment.TickCount & int.MaxValue;
            }
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            try
            {
                if (Player.HealthPercent <= MiscConfig.JukeFleePercentage && R.IsReady())
                {
                    var pet = Clone; //Player.Pet as Obj_AI_Base;      
                    if (Clone == null && R2.IsReady())
                    {
                        R2.Cast();
                    }
                    EloBuddy.Player.IssueOrder(
                        GameObjectOrder.MovePet,
                        (pet.Position - Player.Position).Normalized()
                )
                    ;
                }
            }
            catch { }

            if (MiscMenu["cloneorb"].Cast<KeyBind>().CurrentValue)
            {
                MiscMenu["clonemode"].Cast<Slider>().CurrentValue = 0;
            }

            else if (!MiscMenu["cloneorb"].Cast<KeyBind>().CurrentValue)
            {
                MiscMenu["clonemode"].Cast<Slider>().CurrentValue = 1;
            }
            if (!MiscConfig.JukeFleeWithClone)
            {
                _jukefleepercentage.CurrentValue = 0;
            }
            AIHeroClient target = TargetSelector.GetTarget(
                Q.Range + Player.MoveSpeed * 3, DamageType.Physical);
            if (ShacoStealth && target != null && target.Health > ComboDamage(target) &&
                IsFacing(target, Player.Position) &&
                Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Combo)
            {
                Orbwalker.DisableAttacking = true;
            }
            else
            {
                Orbwalker.DisableAttacking = false;
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                Combo(target);
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
            {
                Escape();
            }

       

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) || Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                /*Clear(); */
            }

            else if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass)) Harass();
            foreach (var enemy in EntityManager.Heroes.Enemies)
            {
                if (Ignite != null && ComboConfig.Useignite)
                {
                    if (Ignite.IsReady() &&
                        Player.GetSummonerSpellDamage(enemy, DamageLibrary.SummonerSpells.Ignite) >=
                        enemy.TotalShieldHealth() && enemy.Health < 50 + 20 * Player.Level - (enemy.HPRegenRate / 5 * 3))
                    {
                        Ignite.Cast(enemy);
                    }
                }
            }

            if (E.IsReady())
            {
                var ksTarget =
                    EntityManager.Heroes.Enemies.FirstOrDefault(
                        h =>
                            h.IsValidTarget() && !h.Buffs.Any(b => invulnerable.Contains(b.Name)) &&
                            h.Health < GetDamage(SpellSlot.E, h));
                if (ksTarget != null)
                {
                    if ((MiscConfig.ks || MiscConfig.ksq) &&
                        E.IsInRange(ksTarget) && E.IsReady() &&
                        Player.Mana > Player.Spellbook.GetSpell(SpellSlot.E).SData.Mana)
                    {
                        E.Cast(ksTarget);
                    }
                    if (Q.IsReady() && MiscConfig.ks &&
                        ksTarget.Distance(Player) < Q.Range + E.Range && ksTarget.Distance(Player) > E.Range &&
                        !Player.Position.Extend(ksTarget.Position, Q.Range).IsWall() &&
                        Player.Mana >
                        Player.Spellbook.GetSpell(SpellSlot.Q).SData.Mana +
                        Player.Spellbook.GetSpell(SpellSlot.E).SData.Mana)
                    {
                        Q.Cast(Player.Position.ExtendVector3(ksTarget.Position, Q.Range));
                    }
                }

                if (MiscConfig.Stackbox && W.IsReady())
                {
                    var box =
                        ObjectManager.Get<Obj_AI_Minion>()
                            .Where(m => m.Distance(Player) < W.Range && m.Name == "Jack In The Box" && !m.IsDead)
                            .OrderBy(m => m.Distance(Game.CursorPos))
                            .FirstOrDefault();

                    if (box != null)
                    {
                        W.Cast(box.Position);
                    }
                    else
                    {
                        if (Player.Distance(Game.CursorPos) < W.Range)
                        {
                            W.Cast(Game.CursorPos);
                        }
                        else
                        {
                            W.Cast(Player.Position.ExtendVector3(Game.CursorPos, W.Range));
                        }
                    }
                }
            }
            if (Clone != null && !CloneDelay && MiscConfig.JukeFleePercentage != Player.HealthPercent && MiscConfig.JukeFleeWithClone | !MiscConfig.JukeFleeWithClone)
            {
                moveClone();
            }
        }
       

public static void StringList(Menu menu, string uniqueId, string displayName, string[] values, int defaultValue)
        {
            var mode = menu.Add(uniqueId, new Slider(displayName, defaultValue, 0, values.Length - 1));
            mode.DisplayName = displayName + ": " + values[mode.CurrentValue];
            mode.OnValueChange +=
                delegate (ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
                {
                    sender.DisplayName = displayName + ": " + values[args.NewValue];
                };
        }

        private static void moveClone()
        {
            AIHeroClient Gtarget = TargetSelector.GetTarget(CloneRange, DamageType.Physical);
            if (MiscMenu["cloneorb"].Cast<KeyBind>().CurrentValue)
            {
                if (MiscMenu["followmouse"].Cast<CheckBox>().CurrentValue && Clone != null && Gtarget == null && !Clone.Spellbook.IsAutoAttacking &&
                             (MiscConfig.JukeFleePercentage >= Player.HealthPercent && !MiscConfig.JukeFleeWithClone || MiscConfig.JukeFleePercentage <= Player.HealthPercent && !MiscConfig.JukeFleeWithClone || MiscConfig.JukeFleePercentage < Player.HealthPercent && MiscConfig.JukeFleeWithClone))
                {
                    R.Cast(Game.CursorPos);
                }
                if (Gtarget != null && CanCloneAttack(Clone1) &&
                              (MiscConfig.JukeFleePercentage >= Player.HealthPercent && !MiscConfig.JukeFleeWithClone || MiscConfig.JukeFleePercentage <= Player.HealthPercent && !MiscConfig.JukeFleeWithClone || MiscConfig.JukeFleePercentage < Player.HealthPercent && MiscConfig.JukeFleeWithClone))
                {
                    switch (MiscMenu["Clonetarget"].Cast<Slider>().CurrentValue)
                    {
                        case 0:
                            Gtarget = TargetSelector.GetTarget(2300, DamageType.Physical);
                            break;
                        case 1:
                            Gtarget =
                                ObjectManager.Get<AIHeroClient>()
                                    .Where(i => i.IsEnemy && !i.IsDead && Player.Distance(i) <= CloneRange)
                                    .OrderBy(i => i.Health).FirstOrDefault();

                            break;
                        case 2:
                            Gtarget =
                                ObjectManager.Get<AIHeroClient>()
                                    .Where(i => i.IsEnemy && !i.IsDead && Player.Distance(i) <= CloneRange)
                                    .OrderBy(i => Player.Distance(i))
                                    .FirstOrDefault();
                            break;
                    }
                    if (Clone != null && Orbwalker.LastTarget != Gtarget && Orbwalker.LastTarget is Obj_AI_Base && Orbwalker.LastTarget.IsValid && !Clone.Spellbook.IsAutoAttacking && CanCloneAttack(Clone1) && (MiscConfig.JukeFleePercentage >= Player.HealthPercent && !MiscConfig.JukeFleeWithClone || MiscConfig.JukeFleePercentage <= Player.HealthPercent && !MiscConfig.JukeFleeWithClone || MiscConfig.JukeFleePercentage < Player.HealthPercent && MiscConfig.JukeFleeWithClone))
                    {
                        Obj_AI_Base target = (Obj_AI_Base)Orbwalker.LastTarget;
                        R.Cast(target);
                        CloneDelay = true;
                        Core.DelayAction(() => CloneDelay = false, 200);
                    }
                    if (Clone != null && Gtarget != null && Gtarget.IsValid && !Clone.Spellbook.IsAutoAttacking && CanCloneAttack(Clone1) && (MiscConfig.JukeFleePercentage >= Player.HealthPercent && !MiscConfig.JukeFleeWithClone || MiscConfig.JukeFleePercentage <= Player.HealthPercent && !MiscConfig.JukeFleeWithClone || MiscConfig.JukeFleePercentage < Player.HealthPercent && MiscConfig.JukeFleeWithClone))
                    {
                        if (CanCloneAttack(Clone1))
                        {
                            R.Cast(Gtarget);

                        }
                        else if (Player.HealthPercent > 25)
                        {
                            var prediction = Prediction.Position.PredictUnitPosition(Gtarget, 2);
                            R.Cast(
                                Gtarget.Position.ExtendVector3(prediction.To3DPlayer(), Gtarget.GetAutoAttackRange()));
                        }
                        CloneDelay = true;
                        Core.DelayAction(() => CloneDelay = false, 200);
                    }
                }
            }
            if (!MiscMenu["cloneorb"].Cast<KeyBind>().CurrentValue)
            {
                if (Clone != null && !Clone.Spellbook.IsAutoAttacking && (MiscConfig.JukeFleePercentage >= Player.HealthPercent && !MiscConfig.JukeFleeWithClone || MiscConfig.JukeFleePercentage <= Player.HealthPercent && !MiscConfig.JukeFleeWithClone || MiscConfig.JukeFleePercentage < Player.HealthPercent && MiscConfig.JukeFleeWithClone))
                {
                    if (Gtarget != null && Clone.IsInRange(Gtarget, ObjectManager.Player.GetAutoAttackRange()) && CanCloneAttack(Clone1))
                    { }
                    else
                    {
                        R.Cast(Game.CursorPos);
                    }
                }
            }
        }

        public static bool CanCloneAttack(Obj_AI_Minion clone)
        {
            if (clone != null)
            {
                return GameTimeTickCount >=
                       LastAATick + Game.Ping + 100 + (clone.AttackDelay - clone.AttackCastDelay) * 1000;
            }
            return false;
        }

        private static bool CheckWalls(Obj_AI_Base target)
        {
            var step = Player.Distance(target) / 15;
            for (var i = 1; i < 16; i++)
            {
                if (Player.Position.Extend(target.Position, step * i).IsWall())
                {
                    return true;
                }
            }
            return false;
        }

        public static Vector3 Predict(Obj_AI_Base target, bool serverPos, float distance)
        {
            var enemyPos = serverPos ? target.ServerPosition : target.Position;
            var myPos = serverPos ? Player.ServerPosition : Player.Position;

            return enemyPos + Vector3.Normalize(enemyPos - myPos) * distance;
        }

        public static float GetDamage(SpellSlot spell, Obj_AI_Base target)
        {
            var ap = Player.FlatMagicDamageMod + Player.BaseAbilityDamage;
            if (spell == SpellSlot.Q)
            {
                if (!Q.IsReady())
                    return 0;
                return Player.CalculateDamageOnUnit(target, DamageType.Magical, 75f + 40f * (Q.Level - 1) + 45 / 100 * ap);
            }
            if (spell == SpellSlot.W)
            {
                if (!W.IsReady())
                    return 0;
                return Player.CalculateDamageOnUnit(target, DamageType.Magical, 90f + 45f * (W.Level - 1) + 90 / 100 * ap);
            }
            if (spell == SpellSlot.E)
            {
                if (!E.IsReady())
                    return 0;
                return Player.CalculateDamageOnUnit(target, DamageType.Magical, 55f + 25f * (E.Level - 1) + 55 / 100 * ap);
            }
            if (spell == SpellSlot.R)
            {
                if (!R.IsReady())
                    return 0;
                return Player.CalculateDamageOnUnit(target, DamageType.Magical, 150f + 100f * (R.Level - 1) + 50 / 100 * ap);
            }

            return 0;
        }

        private static float ComboDamage(Obj_AI_Base hero)
        {
            double damage = 0;

            if (Q.IsReady())
            {
                damage += Player.GetSpellDamage(hero, SpellSlot.Q);
            }
            if (E.IsReady())
            {
                damage += Player.GetSpellDamage(hero, SpellSlot.E);
            }

            damage += GetItemsDamage(hero);

            var ignitedmg = Player.GetSummonerSpellDamage(hero, DamageLibrary.SummonerSpells.Ignite);
            if (Player.Spellbook.CanUseSpell(Player.GetSpellSlotFromName("summonerdot")) == SpellState.Ready &&
                hero.Health < damage + ignitedmg)
            {
                damage += ignitedmg;
            }

            return (float)damage;
        }

        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base hero, GameObjectProcessSpellCastEventArgs args)
        {
            if (EscapeConfig.Evade) return;
            if (hero.IsAlly) return;
            if (!hero.IsValid()) return;

            //Need to calc Delay/Time for misille to hit !

            if (DangerDB.TargetedList.Contains(args.SData.Name))
            {
                if (args.Target.IsMe)
                    R.Cast();
            }

            if (DangerDB.CircleSkills.Contains(args.SData.Name))
            {
                if (player.Distance(args.End) < args.SData.LineWidth)
                    R.Cast();
            }

            if (DangerDB.Skillshots.Contains(args.SData.Name))
            {
                if (new Geometry.Polygon.Rectangle(args.Start, args.End, args.SData.LineWidth).IsInside(player))
                {
                    R.Cast();
                }
                if (Clone != null)
            {
                var clone = ObjectManager.Get<Obj_AI_Minion>().FirstOrDefault(m => m.Name == Player.Name && !m.IsMe);

                if (args == null || clone == null)
                {
                    return;
                }
                if (hero.NetworkId != clone.NetworkId)
                {
                    return;
                }
                LastAATick = GameTimeTickCount;
            }

            if (args == null || hero == null)
            {
                return;
            }
            if (ComboConfig.UseRcc && hero is AIHeroClient && hero.IsEnemy &&
                Player.Distance(hero) < Q.Range &&
                isDangerousSpell(
                    args.SData.Name, args.Target as AIHeroClient, hero, args.End, float.MaxValue))
            {
                R2.Cast();
            }
        }
        }

        public static bool HasSpell(string s)
        {
            return EloBuddy.Player.Spells.FirstOrDefault(o => o.SData.Name.Contains(s)) != null;
        }

        private static void OrbwalkingOnAfterAttack(AttackableUnit target, EventArgs args)
        {

            if (target is AIHeroClient)
            {
                var t = target as AIHeroClient;
                if (!t.IsValidTarget())
                {
                    return;
                }

                if (Hydra.IsReady() && Hydra.IsOwned() && Player.Distance(target) < Hydra.Range)
                {
                    Hydra.Cast();
                }
                else if (Tiamat.IsReady() && Tiamat.IsOwned() && Player.Distance(target) < Tiamat.Range)
                {
                    Tiamat.Cast();
                }
            }
        }


        public static float GetPath(AIHeroClient hero, Vector3 b)
        {
            var path = hero.GetPath(b);
            var lastPoint = path[0];
            var distance = 0f;
            foreach (var point in path.Where(point => !point.Equals(lastPoint)))
            {
                distance += lastPoint.Distance(point);
                lastPoint = point;
            }
            return distance;
        }
        public static class EscapeConfig
        {
      
            public static bool Evade
            {
                get { return _evade.CurrentValue; }
            }
        }


        public static class DrawingConfig
        {
            public static bool DrawQ
            {
                get { return _drawq.CurrentValue; }
            }

            public static bool DrawW
            {
                get { return _draww.CurrentValue; }
            }

            public static bool DrawE
            {
                get { return _drawe.CurrentValue; }
            }


            public static bool DrawDamageBar
            {
                get { return _drawdamagebar.CurrentValue; }
            }

            public static bool DrawDeceiveTimer
            {
                get { return _drawqtimer.CurrentValue; }
            }
        }

        public static class MiscConfig
        {
            public static bool Stackbox
            {
                get { return _stackbox.CurrentValue; }
            }

            public static bool clonemode
            {
                get { return MiscMenu["clonemode"].Cast<KeyBind>().CurrentValue; }
            }

            public static bool JukeFleeWithClone
            {
                get { return _jukefleewithclone.CurrentValue; }
            }

            public static int JukeFleePercentage
            {
                get { return _jukefleepercentage.CurrentValue; }
            }

            public static bool Moveclone
            {
                get { return _moveclone.CurrentValue; }
            }

            public static bool ksq
            {
                get { return _ksq.CurrentValue; }
            }

            public static bool ks
            {
                get { return _ks.CurrentValue; }
            }
        }

        public static class HarassConfig
        {
            public static bool UseEHarass
            {
                get { return _useeharass.CurrentValue; }
            }
        }

        public static class ComboConfig
        {
            public static bool UseQ
            {
                get { return _useq.CurrentValue; }
            }

            public static int UseQMin
            {
                get { return _useqmin.CurrentValue; }
            }

            public static bool UseW
            {
                get { return _usew.CurrentValue; }
            }

            public static bool UseE
            {
                get { return _useecombo.CurrentValue; }
            }

            public static bool UseR
            {
                get { return _user.CurrentValue; }
            }

            public static bool Waitforstealth
            {
                get { return _waitforstealth.CurrentValue; }
            }

            public static bool Useignite
            {
                get { return _useignite.CurrentValue; }
            }

            public static bool UseRcc
            {
                get { return _usercc.CurrentValue; }
            }

            public static bool UseItems
            {
                get { return _useitems.CurrentValue; }
            }
        }
    

    public static readonly string[] SmiteableUnits =
         {
            "SRU_Red", "SRU_Blue", "SRU_Dragon", "SRU_Baron",
            "SRU_Gromp", "SRU_Murkwolf", "SRU_Razorbeak",
            "SRU_Krug", "Sru_Crab"
        };

    private static readonly int[] SmiteRed = { 3715, 1415, 1414, 1413, 1412 };
    private static readonly int[] SmiteBlue = { 3706, 1403, 1402, 1401, 1400 };

    public static void Smitemethod()
    {
        SmiteMenu = ShacoMenu.AddSubMenu("Smite", "Smite");
        SmiteMenu.AddSeparator();
        SmiteMenu.Add("smiteActive",
            new KeyBind("Smite Active (toggle)", true, KeyBind.BindTypes.PressToggle, 'H'));
        SmiteMenu.AddSeparator();
        SmiteMenu.Add("useSlowSmite", new CheckBox("KS with Blue Smite"));
        SmiteMenu.Add("comboWithDuelSmite", new CheckBox("Combo with Red Smite"));
        SmiteMenu.AddSeparator();
        SmiteMenu.AddGroupLabel("Camps");
        SmiteMenu.AddLabel("Epics");
        SmiteMenu.Add("SRU_Baron", new CheckBox("Baron"));
        SmiteMenu.Add("SRU_Dragon", new CheckBox("Dragon"));
        SmiteMenu.AddLabel("Buffs");
        SmiteMenu.Add("SRU_Blue", new CheckBox("Blue"));
        SmiteMenu.Add("SRU_Red", new CheckBox("Red"));
        SmiteMenu.AddLabel("Small Camps");
        SmiteMenu.Add("SRU_Gromp", new CheckBox("Gromp", false));
        SmiteMenu.Add("SRU_Murkwolf", new CheckBox("Murkwolf", false));
        SmiteMenu.Add("SRU_Krug", new CheckBox("Krug", false));
        SmiteMenu.Add("SRU_Razorbeak", new CheckBox("Razerbeak", false));
        SmiteMenu.Add("Sru_Crab", new CheckBox("Skuttles", false));

        Game.OnUpdate += SmiteEvent;
    }

    public static void SetSmiteSlot()
    {
        SpellSlot smiteSlot;
        if (SmiteBlue.Any(x => Player.InventoryItems.FirstOrDefault(a => a.Id == (ItemId)x) != null))
            smiteSlot = Player.GetSpellSlotFromName("s5_summonersmiteplayerganker");
        else if (
            SmiteRed.Any(
                x => Player.InventoryItems.FirstOrDefault(a => a.Id == (ItemId)x) != null))
            smiteSlot = Player.GetSpellSlotFromName("s5_summonersmiteduel");
        else
            smiteSlot = Player.GetSpellSlotFromName("summonersmite");
        SmiteSpell = new Spell.Targeted(smiteSlot, 500);
    }

    public static int GetSmiteDamage()
    {
        var level = Player.Level;
        int[] smitedamage =
        {
                20*level + 370,
                30*level + 330,
                40*level + 240,
                50*level + 100
            };
        return smitedamage.Max();
    }

    private static void SmiteEvent(EventArgs args)
    {
        SetSmiteSlot();
        if (!SmiteSpell.IsReady() || Player.IsDead) return;
        if (SmiteMenu["smiteActive"].Cast<KeyBind>().CurrentValue)
        {
            var unit =
                EntityManager.MinionsAndMonsters.Monsters
                    .Where(
                        a =>
                            SmiteableUnits.Contains(a.BaseSkinName) && a.Health < GetSmiteDamage() &&
                            SmiteMenu[a.BaseSkinName].Cast<CheckBox>().CurrentValue)
                    .OrderByDescending(a => a.MaxHealth)
                    .FirstOrDefault();

            if (unit != null)
            {
                SmiteSpell.Cast(unit);
                return;
            }
        }
        if (SmiteMenu["useSlowSmite"].Cast<CheckBox>().CurrentValue &&
            SmiteSpell.Handle.Name == "s5_summonersmiteplayerganker")
        {
            foreach (
                var target in
                    EntityManager.Heroes.Enemies
                        .Where(h => h.IsValidTarget(SmiteSpell.Range) && h.Health <= 20 + 8 * Player.Level))
            {
                SmiteSpell.Cast(target);
                return;
            }
        }
        if (SmiteMenu["comboWithDuelSmite"].Cast<CheckBox>().CurrentValue &&
            SmiteSpell.Handle.Name == "s5_summonersmiteduel" &&
            Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
        {
            foreach (
                var target in
                    EntityManager.Heroes.Enemies
                        .Where(h => h.IsValidTarget(SmiteSpell.Range)).OrderByDescending(TargetSelector.GetPriority)
                )
            {
                SmiteSpell.Cast(target);
                return;
            }
        }
    }
        public static void UseItems(Obj_AI_Base target, float comboDmg = 0f, bool cleanseSpell = false)
        {
            hydraTarget = target;

            if (Item.HasItem(randuins.Id) && Item.CanUseItem(randuins.Id))
            {
                if (target != null && player.Distance(target) < randuins.Range &&
                    player.CountEnemiesInRange(randuins.Range) >= 2)
                {
                    Item.UseItem(randuins.Id);
                }
            }
            if (target != null && Item.HasItem(odins.Id) &&
                Item.CanUseItem(odins.Id))
            {
                var odinDmg = player.GetItemDamage(target, ItemId.Odyns_Veil);

                if (odinDmg > target.Health)
                {
                    odins.Cast(target);
                }

            }
            if (Item.HasItem(bilgewater.Id) &&
                Item.CanUseItem(bilgewater.Id))
            {
                var bilDmg = player.GetItemDamage(target, ItemId.Bilgewater_Cutlass);
                if (bilDmg > target.Health)
                {
                    bilgewater.Cast(target);
                }

            }
            if (target != null && Item.HasItem(botrk.Id) &&
                Item.CanUseItem(botrk.Id))
            {
                var botrDmg = player.GetItemDamage(target, ItemId.Blade_of_the_Ruined_King);

                if (botrDmg > target.Health)
                {
                    botrk.Cast(target);
                }

            }
            if (target != null && Item.HasItem(hexgun.Id) &&
                Item.CanUseItem(hexgun.Id))
            {
                var hexDmg = player.GetItemDamage(target, ItemId.Hextech_Gunblade);

                if (hexDmg > target.Health)
                {
                    hexgun.Cast(target);
                }

            }
            if (
                ((!MuramanaEnabled && 40 < player.ManaPercent) ||
                 (MuramanaEnabled && 40 > player.ManaPercent)))
            {
                if (Muramana.IsOwned() && Muramana.IsReady())
                {
                    Muramana.Cast();
                }
                if (Muramana2.IsOwned() && Muramana2.IsReady())
                {
                    Muramana2.Cast();
                }
            }
            MuramanaTime = System.Environment.TickCount;
            if (Item.HasItem(youmuu.Id) && Item.CanUseItem(youmuu.Id) &&
                target != null && player.Distance(target) < player.AttackRange + 50 && target.HealthPercent < 65)
            {
                youmuu.Cast();
            }

            if (Item.HasItem(frost.Id) && Item.CanUseItem(frost.Id) && target != null)
            {
                if (player.Distance(target) < frost.Range &&
                    (2 <= target.CountEnemiesInRange(225f) &&
                     ((target.Health / target.MaxHealth * 100f) < 40 && true ||
                      !true)))
                {
                    frost.Cast(target);
                }
            }
            if (Item.HasItem(solari.Id) && Item.CanUseItem(solari.Id))
            {
                if ((2 <= player.CountAlliesInRange(solari.Range) &&
                     2 <= player.CountEnemiesInRange(solari.Range)) ||
                    ObjectManager.Get<Obj_AI_Base>()
                        .FirstOrDefault(
                            h => h.IsAlly && !h.IsDead && solari.IsInRange(h) && CheckCriticalBuffs(h)) !=
                    null)
                {
                    solari.Cast();
                }
            }

            UseCleanse(cleanseSpell);

        }

        public static bool MuramanaEnabled
        {
            get { return player.HasBuff("Muramana"); }
        }

        public static void UseCleanse(bool cleanseSpell)
        {
            if (QssUsed)
            {
                return;
            }
            if (Item.CanUseItem(Qss.Id) && Item.HasItem(Qss.Id))
            {
                Cleanse(Qss);
            }
            if (Item.CanUseItem(Mercurial.Id) && Item.HasItem(Mercurial.Id))
            {
                Cleanse(Mercurial);
            }
            if (Item.CanUseItem(Dervish.Id) && Item.HasItem(Dervish.Id))
            {
                Cleanse(Dervish);
            }
        }


        private static void Cleanse(Item Item)
        {
            var delay = 600;
            foreach (var buff in player.Buffs)
            {
                if (buff.Type == BuffType.Slow)
                {
                    CastQSS(delay, Item);
                    return;
                }
                if (buff.Type == BuffType.Blind)
                {
                    CastQSS(delay, Item);
                    return;
                }
                if (buff.Type == BuffType.Silence)
                {
                    CastQSS(delay, Item);
                    return;
                }
                if (buff.Type == BuffType.Snare)
                {
                    CastQSS(delay, Item);
                    return;
                }
                if (buff.Type == BuffType.Stun)
                {
                    CastQSS(delay, Item);
                    return;
                }
                if (buff.Type == BuffType.Charm)
                {
                    CastQSS(delay, Item);
                    return;
                }
                if (buff.Type == BuffType.Taunt)
                {
                    CastQSS(delay, Item);
                    return;
                }
                if ((buff.Type == BuffType.Fear || buff.Type == BuffType.Flee))
                {
                    CastQSS(delay, Item);
                    return;
                }
                if (buff.Type == BuffType.Suppression)
                {
                    CastQSS(delay, Item);
                    return;
                }
                if (buff.Type == BuffType.Polymorph)
                {
                    CastQSS(delay, Item);
                    return;
                }
                switch (buff.Name)
                {
                    case "zedulttargetmark":
                        CastQSS(2900, Item);
                        break;
                    case "VladimirHemoplague":
                        CastQSS(4900, Item);
                        break;
                    case "MordekaiserChildrenOfTheGrave":
                        CastQSS(delay, Item);
                        break;
                    case "urgotswap2":
                        CastQSS(delay, Item);
                        break;
                    case "skarnerimpale":
                        CastQSS(delay, Item);
                        break;
                    case "poppydiplomaticimmunity":
                        CastQSS(delay, Item);
                        break;
                }
            }
        }

        private static void CastQSS(int delay, Item item)
        {
            QssUsed = true;
            Core.DelayAction(
            () =>
            {
                Item.UseItem(item.Id, player);
                QssUsed = false;
            }, delay);
            return;

        }


        public static void castHydra(Obj_AI_Base target)
        {
            if (target != null && player.Distance(target) < hydra.Range && !player.Spellbook.IsAutoAttacking)
            {
                if (Item.HasItem(tiamat.Id) && Item.CanUseItem(tiamat.Id))
                {
                    Item.UseItem(tiamat.Id);
                }
                if (Item.HasItem(hydra.Id) && Item.CanUseItem(hydra.Id))
                {
                    Item.UseItem(hydra.Id);
                }
            }
        }


        public static float GetItemsDamage(Obj_AI_Base target)
        {
            double damage = 0;
            if (Item.HasItem(odins.Id) && Item.CanUseItem(odins.Id))
            {
                damage += player.GetItemDamage(target, ItemId.Odyns_Veil);
            }
            if (Item.HasItem(hexgun.Id) && Item.CanUseItem(hexgun.Id))
            {
                damage += player.GetItemDamage(target, ItemId.Hextech_Gunblade);
            }
            var ludenStacks = player.Buffs.FirstOrDefault(buff => buff.Name == "itemmagicshankcharge");
            if (ludenStacks != null && (Item.HasItem(Ludens.Id) && ludenStacks.Count == 100))
            {
                damage += player.CalculateDamageOnUnit(target, DamageType.Magical, Convert.ToInt32(100 + player.FlatMagicDamageMod * 0.15));
            }
            if (Item.HasItem(lich.Id) && Item.CanUseItem(lich.Id))
            {
                damage += player.CalculateDamageOnUnit(target, DamageType.Magical, Convert.ToInt32(player.BaseAttackDamage * 0.75 + player.FlatMagicDamageMod * 0.5));
            }

            if (Item.HasItem(tiamat.Id) && Item.CanUseItem(tiamat.Id))
            {
                damage += player.GetItemDamage(target, ItemId.Tiamat_Melee_Only);
            }
            if (Item.HasItem(hydra.Id) && Item.CanUseItem(hydra.Id))
            {
                damage += player.GetItemDamage(target, ItemId.Ravenous_Hydra_Melee_Only);
            }
            if (Item.HasItem(bilgewater.Id) && Item.CanUseItem(bilgewater.Id))
            {
                damage += player.GetItemDamage(target, ItemId.Bilgewater_Cutlass);
            }
            if (Item.HasItem(botrk.Id) && Item.CanUseItem(botrk.Id))
            {
                damage += player.GetItemDamage(target, ItemId.Blade_of_the_Ruined_King);
            }
            if (Item.HasItem(sheen.Id) && (Item.CanUseItem(sheen.Id) || player.HasBuff("sheen")))
            {
                damage += player.CalculateDamageOnUnit(target, DamageType.Physical, player.BaseAttackDamage);
            }
            if (Item.HasItem(gaunlet.Id) && Item.CanUseItem(gaunlet.Id))
            {
                damage += player.CalculateDamageOnUnit(target, DamageType.Physical, Convert.ToInt32(player.BaseAttackDamage * 1.25));
            }
            if (Item.HasItem(trinity.Id) && Item.CanUseItem(trinity.Id))
            {
                damage += player.CalculateDamageOnUnit(target, DamageType.Physical, Convert.ToInt32(player.BaseAttackDamage * 2));
            }
            return (float)damage;
        }

        public static LastSpellCast LastSpell = new LastSpellCast();
        public static List<LastSpellCast> LastSpellsCast = new List<LastSpellCast>();

        private static void GameObject_OnCreate(GameObject sender, EventArgs args)
        {
            if (sender == null || !sender.IsValid || !sender.Name.Equals(Player.Name))
            {
                return;
            }

            Clone = sender as Obj_AI_Base;
        }

        private static void GameObject_OnDelete(GameObject sender, EventArgs args)
        {
            if (sender == null || !sender.IsValid || !sender.Name.Equals(Player.Name))
            {
                return;
            }

            Clone = null;
        }

        public static void Escape()
        {
            var enemy =
                EntityManager.Heroes.Enemies.Where(
                    hero =>
                        hero.IsValidTarget(Q.Range) && Q.IsReady());
            var x = Player.Position.Extend(Game.CursorPos, 300);
            if (Q.IsReady() && !Player.IsDashing()) EloBuddy.Player.CastSpell(SpellSlot.Q, Game.CursorPos);
        }

        public static Obj_AI_Base getClone()
        {
            Obj_AI_Base Clone = null;
            foreach (var unit in ObjectManager.Get<Obj_AI_Base>().Where(clone => !clone.IsMe && clone.Name == player.Name))
            {
                Clone = unit;
            }

            return Clone;

        }


        public static bool isDangerousSpell(string spellName,
            Obj_AI_Base target,
            Obj_AI_Base hero,
            Vector3 end,
            float spellRange)
        {
            if (spellName == "CurseofTheSadMummy")
            {
                if (player.Distance(hero.Position) <= 600f)
                {
                    return true;
                }
            }
            if (IsFacing(target, player.Position) &&
                (spellName == "EnchantedCrystalArrow" || spellName == "rivenizunablade" ||
                 spellName == "EzrealTrueshotBarrage" || spellName == "JinxR" || spellName == "sejuaniglacialprison"))
            {
                if (player.Distance(hero.Position) <= spellRange - 60)
                {
                    return true;
                }
            }
            if (spellName == "InfernalGuardian" || spellName == "UFSlash" ||
                (spellName == "RivenW" && player.HealthPercent < 25))
            {
                if (player.Distance(end) <= 270f)
                {
                    return true;
                }
            }
            if (spellName == "BlindMonkRKick" || spellName == "SyndraR" || spellName == "VeigarPrimordialBurst" ||
                spellName == "AlZaharNetherGrasp" || spellName == "LissandraR")
            {
                if (target.IsMe)
                {
                    return true;
                }
            }
            if (spellName == "TristanaR" || spellName == "ViR")
            {
                if (target.IsMe || player.Distance(target.Position) <= 100f)
                {
                    return true;
                }
            }
            if (spellName == "GalioIdolOfDurand")
            {
                if (player.Distance(hero.Position) <= 600f)
                {
                    return true;
                }
            }
            if (target != null && target.IsMe)
            {
                if (isTargetedCC(spellName) && spellName != "NasusW" && spellName != "ZedUlt")
                {
                    return true;
                }
            }
            return false;
        }

        public static bool CheckCriticalBuffs(Obj_AI_Base i)
        {
            foreach (BuffInstance buff in i.Buffs)
            {
                if (i.Health <= 6 * ObjectManager.Player.Level && dotsSmallDmg.Contains(buff.Name))
                {
                    return true;
                }
                if (i.Health <= 12 * ObjectManager.Player.Level && dotsMedDmg.Contains(buff.Name))
                {
                    return true;
                }
                if (i.Health <= 25 * ObjectManager.Player.Level && dotsHighDmg.Contains(buff.Name))
                {
                    return true;
                }
            }
            return false;
        }

        private static List<string> dotsHighDmg =
            new List<string>(
                new string[]
                {
                    "karthusfallenonecastsound", "CaitlynAceintheHole", "zedulttargetmark", "timebombenemybuff",
                    "VladimirHemoplague"
                });

        private static List<string> dotsMedDmg =
            new List<string>(
                new string[]
                {
                    "summonerdot", "cassiopeiamiasmapoison", "cassiopeianoxiousblastpoison", "bantamtraptarget",
                    "explosiveshotdebuff", "swainbeamdamage", "SwainTorment", "AlZaharMaleficVisions",
                    "fizzmarinerdoombomb"
                });

        private static List<string> dotsSmallDmg =
            new List<string>(
                new string[]
                { "deadlyvenom", "toxicshotparticle", "MordekaiserChildrenOfTheGrave", "dariushemo", "brandablaze" });

        public static bool IsFacing(Obj_AI_Base source, Vector3 target, float angle = 90)
        {
            if (source == null || !target.IsValid())
            {
                return false;
            }
            return
                (double)
                    Geometry.AngleBetween(
                        Geometry.Perpendicular(Extensions.To2D(source.Direction)), Extensions.To2D(target - source.Position)) <
                angle;
        }

        public static List<string> TargetedCC =
            new List<string>(
                new string[]
                {
                    "TristanaR", "BlindMonkRKick", "AlZaharNetherGrasp", "VayneCondemn", "JayceThunderingBlow", "Headbutt",
                    "Drain", "BlindingDart", "RunePrison", "IceBlast", "Dazzle", "Fling", "MaokaiUnstableGrowth",
                    "MordekaiserChildrenOfTheGrave", "ZedUlt", "LuluW", "PantheonW", "ViR", "JudicatorReckoning",
                    "IreliaEquilibriumStrike", "InfiniteDuress", "SkarnerImpale", "SowTheWind", "PuncturingTaunt",
                    "UrgotSwap2", "NasusW", "VolibearW", "Feast", "NocturneUnspeakableHorror", "Terrify", "VeigarPrimordialBurst"
                });

        public static List<string> invulnerable =
            new List<string>(
                new string[]
                {
                    "sionpassivezombie", "willrevive", "BraumShieldRaise", "UndyingRage", "PoppyDiplomaticImmunity",
                    "LissandraRSelf", "JudicatorIntervention", "ZacRebirthReady", "AatroxPassiveReady", "Rebirth",
                    "alistartrample", "NocturneShroudofDarknessShield", "SpellShield"
                });

        public static bool isTargetedCC(string Spellname)
        {
            return TargetedCC.Contains(Spellname);
        }
    }
}
