using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UniLinq;
using UnityEngine;
using static ProceduralSpaceObject;

namespace KSPDemo1
{

    public class PlasmaMagnetModule : PartModule
    {
        //#region KSP Fields
        [KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiActiveUnfocused = true, guiName = "Plasma Magnet"), UI_Toggle(disabledText = "Disabled", enabledText = "Enabled", affectSymCounterparts = UI_Scene.All)]
        public bool PlasmaMagnetEnabled = false;

        [KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiActiveUnfocused = true, guiName = "Hydrocoptic marzelvanes"), UI_Toggle(disabledText = "Shipwide", enabledText = "Force on generator", affectSymCounterparts = UI_Scene.All)]
        public bool partOrShipwide = false;

        [KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiActiveUnfocused = true, guiName = "JEB MODE"), UI_Toggle(disabledText = "Disabled", enabledText = "Enabled", affectSymCounterparts = UI_Scene.All)]
        public bool jebMode = false;

        [KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiActiveUnfocused = true, guiName = "Plasma Sail Power"), UI_FloatRange(minValue = 0, maxValue = 100, stepIncrement = 1, affectSymCounterparts = UI_Scene.All)]
        public float PlasmaMagnetSlider = 100f;

        [KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiActiveUnfocused = true, guiName = "Apparent Windspeed")]
        public int airspeedMagSimple;

        [KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiActiveUnfocused = true, guiName = "Excitation Power (EC)")]
        public float excitationPower = 0f;

        [KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiActiveUnfocused = true, guiName = "Windmill Power Harvested (EC)")]
        public float dragPower = 0f;

        [KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiActiveUnfocused = true, guiName = "# of Active Magnets")]
        public int count = 0;

        [KSPField]
        public string requiredResourceName = "ElectricCharge";

        [KSPField]
        public float PlasmaMagnetScaleFactor;

        [KSPField]
        public bool IsInEditor = false;


        //         #endregion

        //ModuleResourceConverter
        //ModuleGenerator
        //protected List<PlasmaMagnetModule> plasmaMagnets = null;
        private float lastUpdate = 0.0f;
        private float lastFixedUpdate = 0.0f;
        private float logInterval = 0.1f;
        private bool justActivated = false;
        private List<Part> partList;
        // private PartModuleList moduleList;

        public void FixedUpdate()
        {
            //if (vessel.GetConnectedResourceTotals(,     ){}
            if (PlasmaMagnetEnabled)
            {
                if (!justActivated)
                {
                    count = 1;
                    foreach (var module in vessel.FindPartModulesImplementing<PlasmaMagnetModule>())
                    {
                        if (module.justActivated)
                            count++;
                    }
                    justActivated = true;
                    //part.RequestResource("ElectricCharge", ((double) dragPower -(double) excitationPower)* -1 * TimeWarp.fixedDeltaTime); //applies drag power
                    // part.RequestResource("ElectricCharge", ((double)dragPower) * -1 * TimeWarp.fixedDeltaTime); //applies drag power
                }
            }
            else
            {
                if (justActivated)
                {
                    count--;
                    justActivated = false;
                }

            }
        }

        public override void OnStart(StartState state)
        {
            Debug.Log("Yo");
            part.force_activate();
            if (state == StartState.Editor) { IsInEditor = true; }

            ////plasmaMagnets = new List<PlasmaMagnetModule>();
            //partList = vessel.parts;
            //count = 0;
            //for (int i = 0; i < partList.Count; i++)
            //{
            //    PartModuleList moduleList =partList[i].Modules;
            //    for (int j = 0; j < moduleList.Count; j++)
            //    {
            //        if (moduleList[j] is PlasmaMagnetModule)
            //        {
            //            //bool thisOneActive = partList[i].PlasmaMagnetModule.PlasmaMagnetEnabled;
            //            //if (thisOneActive)

            //                    count++;

            //        }
            //    }

            //}

        }
        public override void OnUpdate()
        {
            //if (!PlasmaMagnetEnabled) { return; };
            //  if ((Time.time - lastUpdate) > logInterval)
            {

                // Debug.Log(plasmaMagnets.Count);
                if (!isReady()) { return; }
                ;
                //if (FlightGlobals.getMainBody().isStar)
                if (!PlasmaMagnetEnabled) { return; }
                ;
            }
        }
        public override void OnFixedUpdate()
        {

            if (!isReady()) { return; }
            ;
            if (!PlasmaMagnetEnabled) { return; }
            ;
            if (!CheckBodyHasWind()) { return; }
            ;
            float bullshittium = 1 * PlasmaMagnetScaleFactor;//scaling power for testing purposes
            Int32 windspeed = 500000;
            windspeed = ScaleWindWithAltitude() * windspeed;


            if (jebMode) { bullshittium = bullshittium * 50; }
            ;
            float sliderPower = PlasmaMagnetSlider * 0.01f;
            //if (vessel.GetHeightFromSurface > ) {windspeed = 0; }; //implement heliopause here
            if (IsInEditor) { Debug.Log("I Am Therefore I Am"); }
            else { Debug.Log("Altitude " + TrueAlt()); }

            Vector3d apparentWindspeed = TransformVectorInverse(vessel.obt_velocity, vessel) - new Vector3d(0, windspeed, 0);///vector addition time, static 500km/s right now but could try other things
            double airspeedMag = apparentWindspeed.magnitude;
            float magnetForce = (float)airspeedMag;
            float magnetAcceleration;
            airspeedMagSimple = (int)airspeedMag;
            float sailSize = part.mass;//should be more like 0.1 N/kg,
                                       // real scale of 0.1kN/t = 0.0000000000004f //0.1f before
                                       // magnetForce = -magnetForce * part.mass * 0.1f * sliderPower * bullshittium;/// 0.1kN/t when at "rest", so ~500km/s*(partMasskg), want to add throttle here
                                       //might be off by factor of 1000x because things are actually in kN, not N
                                       // Vector3 magnetDragVector = Vector3.Normalize(apparentWindspeed) * magnetForce;// new Vector3(0f, magnetForce, 0f);///TransformVector uses a different direction convention
                                       // double dragPower = airspeedMag * magnetDragVector.magnitude;
                                       //trying something different
            if (!partOrShipwide)
            {// change velocity instead

                magnetForce = -magnetForce * part.mass * 0.1f * sliderPower * bullshittium;/// 0.1kN/t when at "rest", so ~500km/s*(partMasskg), want to add throttle here                
                Debug.Log("Magnet force " + magnetForce * TimeWarp.fixedDeltaTime);

                magnetForce = magnetForce / ((float)vessel.totalMass * 1000);// calculated acceleration
                Vector3 magnetDragVector = Vector3.Normalize(apparentWindspeed) * magnetForce / 500 * TimeWarp.fixedDeltaTime;// new Vector3(0f, magnetForce, 0f);///TransformVector uses a different direction convention

                magnetAcceleration = (magnetForce / ((float)vessel.totalMass * 1000)) / 500;// calculated acceleration
                                                                                            //Debug.Log("Magnet acceleration " + magnetAcceleration * TimeWarp.fixedDeltaTime);
                                                                                            //Debug.Log("fixedDeltaTime" + TimeWarp.fixedDeltaTime);


                Vector3 magnetVectorAligned = TransformVector(magnetDragVector, vessel);
                vessel.ChangeWorldVelocity(magnetVectorAligned);
                //Debug.Log("Magnet vector " + magnetVectorAligned);
                //now do power
                excitationPower = (float)((airspeedMag * (magnetForce / 5.055) * -1 * TimeWarp.fixedDeltaTime * 0.0002) / bullshittium);
                // Debug.Log("Normalized wind " + Vector3.Normalize(apparentWindspeed));               
                //Debug.Log("fixedDeltaTime" + TimeWarp.fixedDeltaTime);
                //Debug.Log("magnetvectorAligned " + magnetVectorAligned.magnitude);
                if (count > 1)//excitation power should be about 1/5000 drag power
                {
                    dragPower = (float)((airspeedMag * (magnetForce / 5.055) * -1 * TimeWarp.fixedDeltaTime) / bullshittium);// makes power//might need conversion factor
                }
                else dragPower = 0;
            }

            if (partOrShipwide)
            {//apply force to just the part itself
                magnetForce = -magnetForce * part.mass * 0.1f * sliderPower * bullshittium;/// 0.1kN/t when at "rest", so ~500km/s*(partMasskg), want to add throttle here
                //might be off by factor of 1000x because things are actually in kN, not N
                Debug.Log("Magnet force " + magnetForce * TimeWarp.fixedDeltaTime);
                Debug.Log("fixedDeltaTime" + TimeWarp.fixedDeltaTime);

                Vector3 magnetDragVector = Vector3.Normalize(apparentWindspeed) * magnetForce / 500 * 1 / 1000;// new Vector3(0f, magnetForce, 0f);///TransformVector uses a different direction convention
                //double dragPower = airspeedMag * magnetDragVector.magnitude;
                Vector3 magnetVectorAligned = TransformVector(magnetDragVector, vessel);
                part.rb.AddForce(magnetVectorAligned, ForceMode.Force);
                Debug.Log("magnetVectorAligned" + magnetVectorAligned);
                //now do power
                excitationPower = (float)((airspeedMag * (magnetForce / 500) * -1 * TimeWarp.fixedDeltaTime * 0.002 * 0.1 * 0.009) / bullshittium);
                if (count > 1)
                {
                    dragPower = (float)((airspeedMag * (magnetForce / 500) * -1 * TimeWarp.fixedDeltaTime * 0.1 * 0.09) / bullshittium);// makes power//might need conversion factor
                }
                else dragPower = 0;
            }

            var required = TimeWarp.deltaTime * excitationPower;
            if (ResourceAvailable(requiredResourceName) > required)
            {
                RequestResource(requiredResourceName, required);
                RequestResource(requiredResourceName, TimeWarp.deltaTime * dragPower * -1);
            }
            else
                OnNoPower();

            if ((Time.time - lastUpdate) > logInterval)
            {
                //MonoUtilities.RefreshContextWindows(part);//display data might not work??
                lastUpdate = Time.time;
                //TransformVectorInverse(vessel.obt_velocity, vessel) + new Vector3d(0, 500000, 0) 
                //Debug.Log("magnetForce " + magnetForce);
                //Debug.Log("Vessel mass " + vessel.totalMass);
                //Debug.Log("AirspeedMag " + airspeedMag + "| part mass" + part.mass);
                ////Debug.Log("Added Force " + magnetDragVector);
                //Debug.Log("Changed velocity by " + dragAcceleration);
                // Debug.Log("Aligned " + magnetVectorAligned);
            }

        }
        //input vector is in the local coordinate frame, where x = north/south, y = up/down, z = east/west
        public static Vector3 TransformVector(Vector3 vec, Vessel vessel)
        {
            Matrix4x4 Vesselframe = Matrix4x4.identity;
            Vesselframe.SetColumn(0, (Vector3)vessel.north);
            Vesselframe.SetColumn(1, (Vector3)vessel.upAxis);
            Vesselframe.SetColumn(2, (Vector3)vessel.east);
            return Vesselframe * vec;
        }
        //input global, output local
        public static Vector3 TransformVectorInverse(Vector3 vec, Vessel vessel)
        {
            Matrix4x4 Vesselframe = Matrix4x4.identity;
            Vesselframe.SetColumn(0, (Vector3)vessel.north);
            Vesselframe.SetColumn(1, (Vector3)vessel.upAxis);
            Vesselframe.SetColumn(2, (Vector3)vessel.east);
            return Vesselframe.inverse * vec;
        }
        private bool isReady()
        {
            if (HighLogic.LoadedSceneIsEditor || this.vessel == null)
            {
                return false;
            }
            if (this.vessel != FlightGlobals.ActiveVessel)
            {
                return false;
            }
            return true;
        }

        private double ResourceAvailable(string resource)
        {
            var res = PartResourceLibrary.Instance.GetDefinition(resource);
            double amount, maxAmount;
            part.GetConnectedResourceTotals(res.id, out amount, out maxAmount);
            return amount;
        }

        private float RequestResource(string resource, double amount)
        {
            var res = PartResourceLibrary.Instance.GetDefinition(resource);
            return (float)this.part.RequestResource(res.id, amount);
        }
        private bool CheckBodyHasWind()
        {
            if (!(FlightGlobals.getMainBody().isStar))
            {
                PlasmaMagnetEnabled = false;
                ScreenMessages.PostScreenMessage("Solar wind becalmed by planetary magnetosphere, shutting down");
                return (false);
            }
            else { return (true); }
            //only works around stars
        }
        private int ScaleWindWithAltitude()//just a placeholder for now, should check body config
        {
            double TerminationShockAlt = 1300000000000;
            if (TrueAlt() > TerminationShockAlt) { return (0); }
            else { return (1); }
            ;
        }
        public double TrueAlt()
        {
            Vector3 pos = this.part.transform.position; //or this.vessel.GetWorldPos3D()
            double ASL = FlightGlobals.getAltitudeAtPos(pos);
            if (this.vessel.mainBody.pqsController == null) { return ASL; }
            double terrainAlt = this.vessel.pqsAltitude;
            if (this.vessel.mainBody.ocean && terrainAlt <= 0) { return ASL; }
            ; //Checks for oceans
            return ASL - terrainAlt;
        }
        private void OnNoPower()
        {
            //ScreenMessages.PostScreenMessage(($ "Not enough {requiredResourceName}"), 5f, ScreenMessageStyle.UPPER_CENTER);
            PlasmaMagnetEnabled = false;
        }
    }


}

