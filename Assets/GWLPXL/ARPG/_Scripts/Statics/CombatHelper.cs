using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.Abilities.Mods.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Combat.com;
using GWLPXL.ARPGCore.DebugHelpers.com;
using GWLPXL.ARPGCore.StatusEffects.com;
using GWLPXL.ARPGCore.Types.com;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.Statics.com
{

    /// <summary>
    /// helper class for commonly used action during combat, i.e. projectiles, dashing, etc.
    /// </summary>
    public static class CombatHelper
    {
        public static System.Action<GameObject> OnProjectileBlocked;
        public static string AttackValue = "Attack Value";
        public static string Projectile = "Projectile";
        public static string Melee = "Melee";
        public static string ElementalCleave = "Elemental Cleave";
        public static string AoEWeapon = "AoE Weapon";
        public static string AdditionalActor = "Additional Actor Source";
        public static string AdditionalNoActor = "Additional NO Actor Source";
        public static string ElementDamageNoActor = "Element Damage NO Actor";

       
        /// <summary>
        /// used to start a dash
        /// </summary>
        /// <param name="dasher"></param>
        /// <param name="vars"></param>
        public static void DoDash(IActorHub dasher, DashVars vars)
        {
            DashState dash = new DashState(dasher, vars);
        }
        /// <summary>
        /// used to start a leap
        /// </summary>
        /// <param name="leaper"></param>
        /// <param name="direction"></param>
        /// <param name="vars"></param>
        public static void DoLeap(IActorHub leaper, Vector3 direction, LeapVars vars)
        {
            
            LeapState leap = new LeapState(leaper, direction, vars);
        }
        /// <summary>
        /// used to start knockback aoe
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="origin"></param>
        /// <param name="Vars"></param>
        public static void DoKnockbackAOE(IActorHub attacker, Vector3 origin, KnockbackAOEVars Vars)
        {

            Collider[] colliders = Physics.OverlapSphere(origin, Vars.AOE.Radius);
            for (int i = 0; i < colliders.Length; i++)
            {
                IActorHub collateral = colliders[i].GetComponent<IActorHub>();
                if (collateral == null) continue;
                if (CombatHelper.HasSight(attacker, colliders[i].transform, Vars.AOE.PhysicsType, Vars.AOE.Angle) == false) continue;

                Vector3 direction = collateral.MyTransform.position - attacker.MyTransform.position;
                direction.y = 0;
                direction.Normalize();
                KnockbackState state = new KnockbackState(collateral, direction, Vars.Vars);


            }
        }
        /// <summary>
        /// used to start generate resource
        /// </summary>
        /// <param name="target"></param>
        /// <param name="Vars"></param>
        public static void DoGenerateResource(IActorHub target, GenerateRourseOnHitVars Vars)
        {
            int currentMax = target.MyStats.GetRuntimeAttributes().GetResourceMaxValue(Vars.Type);
            int regenAmount = Mathf.FloorToInt(currentMax * Vars.Percent);
            SoTHelper.RegenResource(target, regenAmount, Vars.Type);
            //target.MyStatusEffects.RegenResource(regenAmount, Vars.Type);
            ARPGCore.DebugHelpers.com.ARPGDebugger.DebugMessage("Regen " + regenAmount, target.MyTransform);
        }
        /// <summary>
        /// used to apply dots
        /// </summary>
        /// <param name="target"></param>
        /// <param name="vars"></param>
        public static void DoAddDot(IActorHub target, ModifyResourceVars vars)
        {
            SoTHelper.AddDoT(target, vars);
           // target.MyStatusEffects.AddDoT(vars);

        }

        public static AttackValues GetElementalDamageNoActor(AttackValues values, ElementDamageMultiplierNoActor[] damageArray)//move this, switch to do the damage like usual but just add elemental multiple. maybe add a bool to check? respec iframe? start iframe? shouldnt this just be add dot? yeah
        {

            for (int i = 0; i < damageArray.Length; i++)
            {
                int baseprojectile = damageArray[i].BaseElementDamage;
                ElementType type = damageArray[i].DamageType;
     
                values.ElementAttacks.Add(new ElementAttackResults(type, baseprojectile, ElementDamageNoActor));
            }

            return values;
        }
        /// <summary>
        /// additional damage, no actor
        /// </summary>
        /// <param name="other"></param>
        public static AttackValues GetAdditionalDamageSource(AttackValues values, DamageSourceVars_NoActor Vars)
        {
            PhysicalAttackResults phys = new PhysicalAttackResults(Vars.DamageMultipliers.PhysicalMultipliers.BasePhysicalDamage, AdditionalNoActor);
            values.PhysicalAttack.Add(phys);
            List<ElementAttackResults> ele = new List<ElementAttackResults>(Vars.DamageMultipliers.ElementMultipliers.Length);
            for (int i = 0; i < Vars.DamageMultipliers.ElementMultipliers.Length; i++)
            {
                ele[i] = new ElementAttackResults(Vars.DamageMultipliers.ElementMultipliers[i].DamageType, Vars.DamageMultipliers.ElementMultipliers[i].BaseElementDamage, AdditionalNoActor);
               
            }


            return values;
        }
        /// <summary>
        /// does addition damage source
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="target"></param>
        /// <param name="Vars"></param>
        public static AttackValues GetAdditionalDamageSourceActor(AttackValues values, DamageSourceVars_Actor Vars)
        {
            
            if (Vars.AdditionalDamage.PhysMultipler.PercentOfCasterAttack > 0)
            {
                int add = Vars.AdditionalDamage.PhysMultipler.GetPhysicalDamageAmount(values.Attacker);
                values.PhysicalAttack.Add(new PhysicalAttackResults(add, AdditionalActor));
                
            }


            for (int i = 0; i < Vars.AdditionalDamage.ElementMultiplers.Length; i++)
            {
                int add = Vars.AdditionalDamage.ElementMultiplers[i].GetElementDamageAmount(values.Attacker);
                values.ElementAttacks.Add(new ElementAttackResults(Vars.AdditionalDamage.ElementMultiplers[i].DamageType, add, AdditionalActor));
               
            }

            return values;
        }
        /// <summary>
        /// adds an additional sot source
        /// </summary>
        /// <param name="other"></param>
        /// <param name="Vars"></param>
        public static void DoAddAdditionalSoT(IActorHub other, AdditionalSoTSourceVars Vars)
        {
            if (other.MyStatusEffects == null && Vars.StatusOverTimeOptions.AdditionalDOTs.Length == 0) return;
            for (int i = 0; i < Vars.StatusOverTimeOptions.AdditionalDOTs.Length; i++)
            {
                if (Vars.StatusOverTimeOptions.AdditionalDOTs[i] == null)
                {
                    Debug.LogWarning("Array element at " + i + " on SoTSourceVars is null");
                    continue;
                }
                SoTHelper.AddDoT(other, Vars.StatusOverTimeOptions.AdditionalDOTs[i]);
                //other.MyStatusEffects.AddDoT(Vars.StatusOverTimeOptions.AdditionalDOTs[i]);

            }
            

        }
        /// <summary>
        /// Handles blocked projectile, destroy or disable and raises event with the gameobject if disabled.
        /// </summary>
        /// <param name="projectileOptions"></param>
        /// <param name="projectile"></param>
        public static void HandleProjectileDestroy(ProjectileData projectileOptions, GameObject projectile)
        {
            if (CombatHelper.IsInLayerMask(projectile.gameObject.layer, projectileOptions.ProjectileVars.Blocking) == false) return;

            switch (projectileOptions.ProjectileVars.Block)
            {
                case BlockOptions.Disable:
                    //
                    projectile.gameObject.SetActive(false);
                    OnProjectileBlocked?.Invoke(projectile);
                    break;
                case BlockOptions.Destroy:
                    //
                    UnityEngine.GameObject.Destroy(projectile.gameObject);
                    break;
            }


        }
        public static bool IsInLayerMask(int layer, LayerMask layermask)
        {
            return layermask == (layermask | (1 << layer));
        }
        /// <summary>
        /// raycast detection, making sure no colliders block line of sight.
        /// </summary>
        /// <param name="viewer"></param>
        /// <param name="viewee"></param>
        /// <returns></returns>
        public static bool HasLineOfSight(GameObject viewer, GameObject viewee, LayerMask blockingLayers, float maxRange, int verticalRays = 3, float verticalRayStep = 1)
        {
            Vector3 direction = viewee.transform.position - viewer.transform.position;
            direction.Normalize();
            float verticalstart = 0;
            Ray[] rays = new Ray[verticalRays];
            for (int i = 0; i < rays.Length; i++)
            {
                rays[i] = new Ray(viewer.transform.position + Vector3.up * verticalstart, direction);
                verticalstart += verticalRayStep;
            }
            for (int i = 0; i < rays.Length; i++)
            {
                if (Physics.Raycast(rays[i], maxRange, blockingLayers))
                {
                    return true;
                }
            }

            return false;

        }
        /// <summary>
        /// Returns true if within the angle, false if outside.
        /// </summary>
        /// <param name="damagedealer"></param>

        public static bool HasSight(IActorHub user, Transform target, EditorPhysicsType type, float forwardSightAngle)
        {
            float angle = 0;
            switch (type)
            {
                case EditorPhysicsType.Unity3D:
                    Vector3 targetDir = target.position - user.MyTransform.position;
                    targetDir = targetDir.normalized;

                    float dot = Vector3.Dot(targetDir, user.MyTransform.forward);
                    angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

                    //float whichWay = Vector3.Cross(user.MyTransform.forward, targetDir).y; //Return left or right?
                    //Debug.Log("Left or right " + whichWay);
                    //angle = Vector3.Angle(user.MyTransform.forward, target.transform.position);
                    break;
                case EditorPhysicsType.Unity2D:
                    if (user.MyStateMachine.Get2D() != null)
                    {
                        Vector3 newforward = user.MyStateMachine.Get2D().GetFacingVector();
                        angle = Vector2.Angle(newforward, target.transform.position);
                    }
                    else
                    {
                        angle = Vector2.Angle(user.MyTransform.right, target.transform.position);
                    }
                    break;
            }
            //Debug.Log("Angle" + angle);//testing
            //Debug.Log(angle <= forwardSightAngle);
            return angle <= forwardSightAngle;
        }
        /// <summary>
        /// stat user is the caster, applies an AoE dot 
        /// </summary>
        /// <param name="statUser"></param>
        /// <param name="AoE_Vars"></param>
        /// <param name="DoT_Vars"></param>
        public static void DoAoEDoT(IActorHub statUser, Vector3 origin, AoEVars AoE_Vars, ModifyResourceVars DoT_Vars)
        {

            switch (AoE_Vars.Type)
            {
                case EditorPhysicsType.Unity3D:
                    Collider[] colliders = Physics.OverlapSphere(origin, AoE_Vars.Radius);
                    Debug.Log("Colliders length " + colliders.Length);
                 //   GameObject test = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                   // test.transform.position = statUser.MyTransform.position;
                 //   test.GetComponent<SphereCollider>().radius = AoE_Vars.Radius;
                    for (int i = 0; i < colliders.Length; i++)
                    {
                        Debug.Log("Collider " + colliders[i].name);
                        IActorHub other = colliders[i].GetComponent<IActorHub>();
                        if (other == null) continue;
                        if (other != statUser && other.MyStatusEffects != null)//not null, not myself, haven't already applied
                        {
                            bool hassight = CombatHelper.HasSight(statUser, other.MyTransform, AoE_Vars.Type, AoE_Vars.Angle);
                            Debug.Log("Has sight " + hassight);
                            if (hassight)
                            {
                                SoTHelper.AddDoT(statUser, DoT_Vars);
                                //other.MyStatusEffects.AddDoT(DoT_Vars);
                                Debug.Log("Added dot");
                            }
                            


                        }
                    }
                    break;
                case EditorPhysicsType.Unity2D:
                    Collider2D[] coll2d = Physics2D.OverlapCircleAll(statUser.MyTransform.position, AoE_Vars.Radius);
                    for (int i = 0; i < coll2d.Length; i++)
                    {
                        IActorHub other = coll2d[i].GetComponent<IActorHub>();
                        if (other == null) return;
                        if (other != statUser && other.MyStatusEffects != null)//not null, not myself, haven't already applied
                        {

                            if (CombatHelper.HasSight(statUser, coll2d[i].transform, AoE_Vars.Type, AoE_Vars.Angle))//within angle, apply dot
                            {
                                SoTHelper.AddDoT(statUser, DoT_Vars);
                                //other.MyStatusEffects.AddDoT(DoT_Vars);
                            }


                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// AOE Weapon Damage
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="origin"></param>
        public static void GetAoEWeaponDmg(AttackValues results, IActorHub attacker, Vector3 origin, AoEWeapoNVars Vars)
        {

            int invDamage = 1;
            if (attacker.MyInventory != null)
            {
                invDamage = attacker.MyInventory.GetInventoryRuntime().GetDamageFromEquipment();
            }

            invDamage = Mathf.FloorToInt(invDamage * Vars.PercentOfWpnDmg);

            switch (Vars.PhysicsType)
            {
                case EditorPhysicsType.Unity3D:
                    Collider[] colliders = Physics.OverlapSphere(origin, Vars.Radius);
                    for (int i = 0; i < colliders.Length; i++)
                    {
                        IActorHub receiver = colliders[i].GetComponent<IActorHub>();
                        if (receiver == null) continue;
                        if (receiver.MyHealth != null && receiver.MyHealth != attacker.MyHealth)
                        {
                            //check the angle
                            if (CombatHelper.HasSight(attacker, colliders[i].transform, Vars.PhysicsType, Vars.Angle))
                            {
                                //do the damage.
                                results.PhysicalAttack.Add(new PhysicalAttackResults(invDamage, AoEWeapon));

                            }


                        }

                    }
                    break;
                case EditorPhysicsType.Unity2D:
                    Collider2D[] coll2d = Physics2D.OverlapCircleAll(origin, Vars.Radius);
                    for (int i = 0; i < coll2d.Length; i++)
                    {
                        IActorHub receiver = coll2d[i].GetComponent<IActorHub>();
                        if (receiver == null) continue;
                        if (receiver.MyHealth != null && receiver.MyHealth != attacker.MyHealth)
                        {
                            //check the angle
                            if (CombatHelper.HasSight(attacker, coll2d[i].transform, Vars.PhysicsType, Vars.Angle))
                            {
                                //do the damage.
                                results.PhysicalAttack.Add(new PhysicalAttackResults(invDamage, AoEWeapon));
                            }


                        }
                    }
                    break;
            }
            
        }
        /// <summary>
        /// disables melee damage boxes and the buffs
        /// </summary>
        /// <param name="skillUser"></param>
        /// <param name="buffdic"></param>
        /// <param name="wpnbuffs"></param>
        public static void DisableMeleeWeapons(IActorHub skillUser, MeleeDmgVars vars)
        {
            IMeleeCombatUser melee = skillUser.MyMelee;
            if (melee == null) return;

            if (vars.Buffdic.ContainsKey(melee))
            {
                Transform[] transforms = vars.Buffdic[melee];
                for (int i = 0; i < transforms.Length; i++)
                {

                    transforms[i].GetComponent<IDoDamage>().EnableDamageComponent(false, skillUser);

                }

                for (int i = 0; i < vars.Wpnbuffs.Length; i++)
                {
                    vars.Wpnbuffs[i].Remove(transforms, skillUser);
                }


                vars.Buffdic.Remove(melee);
            }
        }

        /// <summary>
        /// Enables damage boxes and buffs the weapon
        /// </summary>
        /// <param name="skillUser"></param>
        /// <param name="buffdic"></param>
        /// <param name="wpnbuffs"></param>
        /// <param name="meleeDDDic"></param>
        public static void EnableAndBuffMeleeDamageBoxes(IActorHub skillUser, MeleeDmgVars vars)
        {
            //only allow melee users to use this
            IMeleeCombatUser melee = skillUser.MyMelee;
            Transform[] transforms = melee.GetMeleeTransforms();//can be null

            //visual vfx or whatnot
            if (vars.Buffdic.ContainsKey(melee) == false)
            {

                List<Transform> applied = new List<Transform>();
                for (int i = 0; i < transforms.Length; i++)
                {

                    if (transforms[i] == null) continue;

                    ARPGDebugger.CombatDebugMessage("Started cast from " + transforms[i], skillUser.MyTransform);
                    IDoDamage value = melee.GetMeleeDamageBoxes()[i];

                    IDoActorDamage actord = value.GetTransform().GetComponent<IDoActorDamage>();
                    if (actord != null)
                    {
                        ActorDamageData data = null;
                        if (vars.DamageOverride == null)
                        {
                            data = ScriptableObject.Instantiate(actord.GetActorDamageData());
                        }
                        else
                        {
                            data = ScriptableObject.Instantiate(vars.DamageOverride);
                        }


                        //now apply charge percent.
                        ApplyChargeToDamageValues(skillUser, vars.ChargeOptions, data);

                        actord.SetDamageData(data);
                    }

                    IMeleeWeapon meleeW = value.GetTransform().GetComponent<IMeleeWeapon>();
                    if (vars.MeleeOptionsOverride != null && meleeW != null)
                    {
                        meleeW.SetMeleeOptionData(ScriptableObject.Instantiate(vars.MeleeOptionsOverride));
                    }
                    else if (meleeW != null)
                    {
                       //just leave alone
                    }

                    //enable it
                    Transform[] weaposn = new Transform[1] { value.GetTransform() };//applying twice because it's two handed...
                    for (int j = 0; j < vars.Wpnbuffs.Length; j++)
                    {
                        vars.Wpnbuffs[j].Apply(weaposn, skillUser);

                    }

                    
                    //enable the melee dmg box, add to dic
                    value.EnableDamageComponent(true, skillUser);
                    vars.MeleeDDDic[transforms[i]] = value;
                    applied.Add(transforms[i]);
                }

                vars.Buffdic[melee] = applied.ToArray();

            }
        }

        private static void ApplyChargeToDamageValues(IActorHub skillUser, ChargingDamageVars vars, ActorDamageData data)
        {
            //now do the multipliers
            if (vars.MaximumChargePercentPhys > 0)
            {
                float basedmg = data.DamageVar.DamageMultipliers.PhysMultipler.BasePhysicalDamage;
                float newMax = basedmg * vars.MaximumChargePercentPhys;
                float newlerp = Mathf.Lerp(basedmg, newMax, skillUser.MyAbilities.GetRuntimeController().GetChargedAmount());
              //  data.DamageVar.DamageMultipliers.PhysMultipler.SetBaseAmount(Mathf.FloorToInt(newlerp));
            }

            if (vars.MaximumChargePercentElemental > 0)
            {
                for (int j = 0; j < data.DamageVar.DamageMultipliers.ElementMultiplers.Length; j++)
                {
                    ElementDamageMultiplierActor ele = data.DamageVar.DamageMultipliers.ElementMultiplers[j];
                    float basedmg = ele.BaseElementDamage;
                    float newmax = basedmg * vars.MaximumChargePercentElemental;
                    float newlerp = Mathf.Lerp(basedmg, newmax, skillUser.MyAbilities.GetRuntimeController().GetChargedAmount());
                    ele.SetBaseDmgAmount(Mathf.FloorToInt(newlerp));
                }
            }
        }

        public static bool CanMeleeAttack(IActorHub owner, IDoDamage damager, IDoActorDamage actorDmg, IActorHub attacked, IMeleeWeapon meleeOptions)
        {
            return DungeonMaster.Instance.CombatFormulas.GetCombatFormulas().CanMeleeAttack(owner, damager, actorDmg, attacked, meleeOptions);
        }
        /// <summary>
        /// requires rigidbodies
        /// </summary>
        /// <param name="deflector"></param>
        /// <param name="other"></param>
        /// <param name="Vars"></param>
        /// <returns></returns>
        public static bool DoDeflectProjectile(IActorHub deflector, Transform other, DeflectorOnHitVars Vars)
        {
            IProjectile projectile = other.GetComponentInParent<IProjectile>();//if projectile
            if (projectile == null) return false;

            switch (Vars.PhysicsType)//reverse the velocity
            {
                case EditorPhysicsType.Unity3D:
                    Rigidbody body = other.GetComponentInParent<Rigidbody>();
                    if (body != null)
                    {
                        body.velocity = -body.velocity;
                    }
                    break;
                case EditorPhysicsType.Unity2D:
                    Rigidbody2D rb2d = other.GetComponentInParent<Rigidbody2D>();
                    if (rb2d != null)
                    {
                        rb2d.velocity = -rb2d.velocity;
                    }
                    break;
            }


            if (Vars.EnableTakeOver)
            {
                IDoDamage dmg = other.GetComponent<IDoDamage>();
                if (dmg != null)
                {
                    dmg.SetActorOwner(deflector);
                }
            }


            if (Vars.PreventDamageOnSuccess)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// AOE Elemental Damage
        /// </summary>
        /// <param name="caster"></param>
        /// <param name="origin"></param>
        /// <param name="Vars"></param>
        public static void GetElementalCleave(AttackValues results, IActorHub caster, Vector3 origin, AoEWeapoNVars Vars)
        {
            int damage = caster.MyStats.GetRuntimeAttributes().GetElementAttack(Vars.Element);
            float multi = 1;
            if (Vars.PercentOfWpnDmg > 0)
            {
                multi = CombatStats.GetTotalActorDamage(caster.MyTransform.gameObject);//change to weapon dmg
            }
            int scaledDamage = Mathf.FloorToInt(damage * multi);
            Collider[] colliders = Physics.OverlapSphere(origin, Vars.Radius);//this is wrong, add the bonus and then perform the aoe check
            for (int i = 0; i < colliders.Length; i++)
            {
                IActorHub hub = colliders[i].GetComponent<IActorHub>();
                if (hub == null) continue;
                IReceiveDamage receiver = hub.MyHealth;
                if (receiver != null && receiver != caster.MyHealth)
                {
                    if (CombatHelper.HasSight(caster, colliders[i].transform, Vars.PhysicsType, Vars.Angle))
                    {
                        results.ElementAttacks.Add(new ElementAttackResults(Vars.Element, scaledDamage, ElementalCleave));

                    }



                }
            }
        }
        /// <summary>
        /// the shooter version of projectiles, requires somethign to shoot from like a bow
        /// </summary>
        /// <param name="skillUser"></param>
        /// <param name="shooterIndices"></param>
        /// <param name="variables"></param>
        /// <param name="weaponBuffs"></param>
        public static void DoFireShooterProjectile(IActorHub skillUser, ShooterProjectileVars vars)
        {
            if (vars.ShooterIndices.Length == 0) return;

            IProjectileCombatUser projectileUser = skillUser.MyProjectiles;
            //create the projectile object
            Quaternion rot = Quaternion.identity;
            Transform parent = null;
            IShootProjectiles shooter = null;
            List<IDoDamage> _temp = new List<IDoDamage>();
            for (int i = 0; i < vars.ShooterIndices.Length; i++)
            {
                shooter = projectileUser.GetShooter(i);

                if (shooter == null) continue;
                if (shooter.GetShootPoint() == null) continue;

                switch (vars.ProjectileVars.Rotation)
                {
                    case FireRotationParent.FirePoint:
                        parent = projectileUser.GetProjectileFirePoint(i);
                        rot = parent.rotation;
                        break;
                    case FireRotationParent.ParentGameObject:
                        parent = skillUser.MyTransform;
                        rot = skillUser.MyTransform.rotation;
                        break;
                }

                //apply any rot offsets
                Quaternion xoffset = Quaternion.AngleAxis(vars.ProjectileVars.Offsets.XTiltAngle, parent.transform.right);
                rot = xoffset * rot;

                Quaternion yoffset = Quaternion.AngleAxis(vars.ProjectileVars.Offsets.YTiltAngle, parent.transform.up);
                rot = yoffset * rot;

                Quaternion zoffset = Quaternion.AngleAxis(vars.ProjectileVars.Offsets.ZTiltAngle, parent.transform.forward);
                rot = zoffset * rot;

                bool hasNewRot = vars.ProjectileVars.Offsets.XTiltAngle != 0 || vars.ProjectileVars.Offsets.YTiltAngle != 0 || vars.ProjectileVars.Offsets.ZTiltAngle != 0;
                bool hasOffset = vars.ProjectileVars.Offsets.StartOffset != Vector3.zero;
                bool hasOverride = vars.ProjectileVars.ProjectilePrefab != null;

                //choose which prefab to use
                GameObject projective = null;
                if (!hasNewRot && !hasOffset && !hasOverride)
                {
                    projective = shooter.FireProjectile();
                }
                else if (!hasOverride)
                {
                    projective = shooter.FireProjectile(vars.ProjectileVars.Offsets.StartOffset, rot);
                }
                else if (vars.ProjectileVars.ProjectilePrefab != null)
                {
                    projective = shooter.FireProjectile(vars.ProjectileVars.ProjectilePrefab, vars.ProjectileVars.Offsets.StartOffset, rot);
                }
                else
                {
                    projective = shooter.FireProjectile(vars.ProjectileVars.Offsets.StartOffset, rot);
                }

                //get the damage interface
                IDoDamage damage = projective.GetComponent<IDoDamage>();
                ARPGDebugger.CombatDebugMessage("Shoot point " + shooter.GetShootPoint().gameObject.name, skillUser.MyTransform);
                _temp.Add(damage);


            }

            List<Transform> _transforms = new List<Transform>();//get the instances from the interface
            for (int i = 0; i < _temp.Count; i++)
            {
                _transforms.Add(_temp[i].GetTransform());
            }
            //apply the weapon buffs if any
            for (int i = 0; i < vars.weaponBuffs.Length; i++)
            {
                vars.weaponBuffs[i].Apply(_transforms.ToArray(), skillUser);
            }
            //enable the damage component
            for (int i = 0; i < _temp.Count; i++)
            {
                _temp[i].EnableDamageComponent(true, skillUser);
            }
        }
        public static void DoFireAndInIProjectile(IActorHub skillUser, ProjectileVariables vars)
        {
            if (vars.ProjectilePrefab == null) return;//no prefab, no shoot

            IProjectileCombatUser projectileUser = skillUser.MyProjectiles;
            //create the projectile object
            Quaternion rot = Quaternion.identity;
            Transform parent = null;

            switch (vars.Rotation)
            {
                case FireRotationParent.FirePoint:
                    parent = projectileUser.GetProjectileFirePoint();
                    rot = parent.rotation;
                    break;
                case FireRotationParent.ParentGameObject:
                    parent = skillUser.MyTransform;
                    rot = parent.rotation;
                    break;
                case FireRotationParent.TowardsMouse2D:
                    parent = skillUser.MyTransform;
                    Vector2 mousePosOnScreen = (Vector2)DungeonMaster.Instance.GetMainCamera().ScreenToWorldPoint(Input.mousePosition);
                    float angle = Mathf.Atan2(mousePosOnScreen.y - parent.transform.position.y, mousePosOnScreen.x - parent.transform.position.x) * Mathf.Rad2Deg;
                    rot = Quaternion.Euler(new Vector3(0, 0, angle));
                    break;
            }

            Quaternion xoffset = Quaternion.AngleAxis(vars.Offsets.XTiltAngle, parent.transform.right);
            rot = xoffset * rot;

            Quaternion yoffset = Quaternion.AngleAxis(vars.Offsets.YTiltAngle, parent.transform.up);
            rot = yoffset * rot;

            Quaternion zoffset = Quaternion.AngleAxis(vars.Offsets.ZTiltAngle, parent.transform.forward);
            rot = zoffset * rot;

            Vector3 offsetUp = parent.transform.up * vars.Offsets.StartOffset.y;
            Vector3 offsetRight = parent.transform.right * vars.Offsets.StartOffset.x;
            Vector3 offsetForward = parent.transform.forward * vars.Offsets.StartOffset.z;
            Vector3 offset = offsetUp + offsetRight + offsetForward;
            GameObject projective = CreateProjectile(parent.transform.position + offset, rot, vars.ProjectilePrefab);
            //check for damage interface
            IDoDamage[] damage = projective.GetComponents<IDoDamage>();
            //Debug.Log("Added to " + damage.GetTransform().name);

            if (damage.Length > 0)
            {
                IDoActorDamage actord = projective.GetComponent<IDoActorDamage>();
                if (actord != null)
                {
                    //only change the data if we have it charged
                    ActorDamageData data = actord.GetActorDamageData();

                    if (vars.Overrides.DamageOverride != null)
                    {
                        //overrides and charging
                        data = ScriptableObject.Instantiate(vars.Overrides.DamageOverride);
                    }
                    else
                    {
                        data = ScriptableObject.Instantiate(actord.GetActorDamageData());
                    }

                    //now do the multipliers
                    ApplyChargeToDamageValues(skillUser, vars.ChargeOptions, data);

                    actord.SetDamageData(data);



                    if (vars.Overrides.OptionsOverride != null)
                    {
                        IProjectile proj = projective.GetComponent<IProjectile>();
                        if (proj != null)
                        {
                            ProjectileData projoption = ScriptableObject.Instantiate(vars.Overrides.OptionsOverride);
                            proj.SetProjectileData(projoption);
                        }
                    }



                    for (int i = 0; i < damage.Length; i++)
                    {
                        //enable it

                        ARPGDebugger.DebugMessage("Attribute user for projectile " + skillUser, projective);
                        damage[i].EnableDamageComponent(true, skillUser);
                    }

                }
            }
        }

        static GameObject CreateProjectile(Vector3 position, Quaternion rotation, GameObject prefab)
        {
            GameObject newObj = GameObject.Instantiate(prefab, position, rotation);
            return newObj;
        }
    }
}