﻿using System.Windows.Forms;
using GTA;
using GTAZ.Controllable;

namespace GTAZ.Vehicles {

    public class TeamVehicle : ControllableVehicle {

        public TeamVehicle(int uid) : base("TEAM_VEHICLE", new VehicleProperties {

            Teleport = false,
            AttachBlip = true,

            BlipColor = BlipColor.Blue,
            IsDrivable = true,

            PrimaryColor = VehicleColor.MetallicBlack,
            SecondaryColor = VehicleColor.MetallicBlack,

            NumberPlateText = "",
            WindowTint = VehicleWindowTint.PureBlack,
            IsEngineRunning = true,
            SmashWindows = false,
            EngineHealth = 100,
            BurstTires = false


        }) {}

        #region

        //protected override void OnEntityDead() {
        //}

        //protected override void OnPlayerKeyDown(KeyEventArgs e) {
        //}

        //protected override void OnEntityAliveUpdate(int tick) {
        //}

        //protected override void OnEntityInitialize() {
        //}

        //protected override void OnEntityPlayerNearbyUpdate(int tick) {
        //}

        //protected override void OnEntityPedNearby(Ped ped) {
        //}

        //protected override void OnEntityPlayerNearby() {
        //}

        //protected override void OnEntityAlive() {
        //}

        #endregion

    }

}
