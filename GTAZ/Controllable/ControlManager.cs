﻿using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System;
using GTA;
using GTA.Math;
using GTA.Native;

namespace GTAZ.Controllable {

    /// <summary>
    /// Manages the wrapped Entities that exist in the GTA world.
    /// </summary>
    public class ControlManager {

        private readonly List<ControllableEntity> _elements;

        public ControlManager() {
            _elements = new List<ControllableEntity>();
        }

        public ControlManager(int capacity) {
            _elements = new List<ControllableEntity>(capacity);
        }

        /// <summary>
        /// Creates a prop with the specified wrapper, model, position and rotation.
        /// </summary>
        /// <param name="cprop">The wrapper to wrap around the newly spawned Prop.</param>
        /// <param name="model">The model of the newly spawned Prop.</param>
        /// <param name="position">The position of where to spawn the Prop.</param>
        /// <param name="rotation">The rotation of the newly spawned Prop.</param>
        /// <param name="dynamic">Whether the Prop should be dynamic (true) or static (false).</param>
        /// <param name="ground">Whether to place the newly spawned Prop on the ground properly.</param>
        /// <returns></returns>
        public Prop CreateProp(ControllableProp cprop, Model model, Vector3 position, Vector3 rotation, bool dynamic, bool ground) {

            var var1 = World.CreateProp(model, position, rotation, dynamic, true);
            var var2 = cprop.Control(var1);

            if (var2 == null) {
                return null;
            }

            Add(var2);
            return var1;

        }

        /// <summary>
        /// Creates a prop with dynamic physics using the specified wrapper, model, position and rotation.
        /// </summary>
        /// <param name="cprop">The wrapper to wrap around the newly spawned Prop.</param>
        /// <param name="model">The model of the newly spawned Prop.</param>
        /// <param name="position">The position of where to spawn the Prop.</param>
        /// <param name="rotation">The rotation of the newly spawned Prop.</param>
        /// <param name="ground">Whether to place the newly spawned Prop on the ground properly.</param>
        /// <returns></returns>
        public Prop CreateDynamicProp(ControllableProp cprop, Model model, Vector3 position, Vector3 rotation,
            bool ground) {
            return CreateProp(cprop, model, position, rotation, true, ground);
        }

        /// <summary>
        /// Creates a prop with static physics using the specified wrapper, model, position and rotation.
        /// </summary>
        /// <param name="cprop">The wrapper to wrap around the newly spawned Prop.</param>
        /// <param name="model">The model of the newly spawned Prop.</param>
        /// <param name="position">The position of where to spawn the Prop.</param>
        /// <param name="rotation">The rotation of the newly spawned Prop.</param>
        /// <param name="ground">Whether to place the newly spawned Prop on the ground properly.</param>
        /// <returns></returns>
        public Prop CreateStaticProp(ControllableProp cprop, Model model, Vector3 position, Vector3 rotation,
            bool ground) {
            return CreateProp(cprop, model, position, rotation, false, ground);
        }

        /// <summary>
        /// Creates a ped with the specified wrapper, model and at the specified position.
        /// </summary>
        /// <param name="cped">The wrapper to wrap around the newly spawned Ped.</param>
        /// <param name="model">The model/skin of the newly spawned Ped.</param>
        /// <param name="position">The position of where to spawn the Ped.</param>
        /// <returns></returns>
        public Ped CreatePed(ControllablePed cped, PedHash model, Vector3 position) {

            var var1 = World.CreatePed(model, position);
            var var2 = cped.Control(var1);

            if (var2 == null) {
                return null;
            }

            Add(var2);
            return var1;

        }

        /// <summary>
        /// Creates a vehicle with the specified wrapper, model and at the specified position and pointing in the specified heading.
        /// </summary>
        /// <param name="cveh">The wrapper to wrap around the newly spawned Vehicle.</param>
        /// <param name="model">The model/skin of the newly spawned Vehicle.</param>
        /// <param name="position">The position of where to spawn the Vehicle.</param>
        /// <param name="heading">The heading in degrees of the newly spawned Vehicle.</param>
        /// <returns></returns>
        public Vehicle CreateVehicle(ControllableVehicle cveh, VehicleHash model, Vector3 position, float heading = 0) {

            var var1 = World.CreateVehicle(model, position, heading);
            var var2 = cveh.Control(var1);

            if (var2 == null) {
                return null;
            }

            Add(var2);
            UI.Notify(LivingVehicles.ToArray().Length + " vehicles now exist");

            return var1;

        }

        /// <summary>
        /// Runs the KeyDown event on every Entity in this ControlManager.
        /// </summary>
        /// <param name="e">The KeyEventArgs of the KeyDown event.</param>
        /// <returns></returns>
        public ControlManager KeyDown(KeyEventArgs e) {
            Entities.ForEach(entity => entity.KeyDown(e));
            return this;
        }

        /// <summary>
        /// Returns whether this ControlManager contains the specified ControllableEntity.
        /// </summary>
        /// <param name="cped">The ControllableEntity to look for.</param>
        /// <returns></returns>
        public bool Contains(ControllableEntity cped) {
            return Contains(cped.Entity);
        }

        /// <summary>
        /// Returns whether this ControlManager contains a wrapper with the specified Entity.
        /// </summary>
        /// <param name="entity">The Entity to look for.</param>
        /// <returns></returns>
        public bool Contains(Entity entity) {
            return _elements.Any(e => e.Entity == entity);
        }

        /// <summary>
        /// Returns whether this ControlManager contains a wrapper with an Entity with the specified GroupId.
        /// </summary>
        /// <param name="groupId">The GroupId of the Entity to look for.</param>
        /// <returns></returns>
        public bool Contains(string groupId) {
            return _elements.Any(e => e.GroupId == groupId);
        }

        /// <summary>
        /// Returns whether this ControlManager contains a wrapper with an Entity with the specified UniqueId.
        /// </summary>
        /// <param name="uid">The EntityId of the Entity to look for.</param>
        /// <returns></returns>
        public bool Contains(int uid) {
            return _elements.Any(e => e.UniqueId == uid);
        }

        /// <summary>
        /// Returns a collection of wrappers that have an Entity with the specified GroupId.
        /// </summary>
        /// <param name="groupId">The GroupId of the Entity to look for.</param>
        /// <returns></returns>
        public IEnumerable<ControllableEntity> Get(string groupId) {
            return _elements.Where(e => e.GroupId == groupId);
        }

        /// <summary>
        /// Returns a collection of wrappers that have an Entity with the specified UniqueId.
        /// </summary>
        /// <param name="uid">The UniqueId of the Entity to look for.</param>
        /// <returns></returns>
        public ControllableEntity Get(int uid) {
            return _elements.Where(e => e.UniqueId == uid).ToArray()[0];
        }

        /// <summary>
        /// Returns a collection of wrappers with an active Entity.
        /// </summary>
        public IEnumerable<ControllableEntity> ActiveEntities {
            get { return _elements.Where(e => e.IsActive); }
        }

        /// <summary>
        /// Returns a collection of wrappers with an inactive Entity.
        /// </summary>
        public IEnumerable<ControllableEntity> InactiveEntities {
            get { return _elements.Where(e => !e.IsActive); }
        }

        /// <summary>
        /// Returns a collection of wrappers with a living Entity.
        /// </summary>
        public IEnumerable<ControllableEntity> LivingEntities {
            get { return _elements.Where(e => e.IsActive && e.Entity.IsAlive); }
        }

        /// <summary>
        /// Returns a collection of wrappers with a dead Entity.
        /// </summary>
        public IEnumerable<ControllableEntity> DeadEntities {
            get { return _elements.Where(e => e.IsActive && e.Entity.IsDead); }
        }

        /// <summary>
        /// Returns a collection of wrappers with a living Ped.
        /// </summary>
        public IEnumerable<ControllableEntity> LivingPeds {
            get {
                return Peds.Where(e => e.IsActive);
            }
        }

        /// <summary>
        /// Returns a collection of wrappers with a living Vehicle.
        /// </summary>
        public IEnumerable<ControllableEntity> LivingVehicles {
            get {
                return Vehicles.Where(v => v.IsActive);
            }
        }

        public void AssignRandomId(ControllableEntity entity) {

            var rand = new Random(Game.GameTime);
            var uniqueId = rand.Next(0, 1000000);

            if (Contains(uniqueId)) {
                return;
            }

            entity.UniqueId = uniqueId;

        }

        /// <summary>
        /// Adds the specified ControllableEntity to this ControlManager.
        /// </summary>
        /// <param name="entity">The ControllableEntity to add.</param>
        /// <returns></returns>
        public ControlManager Add(ControllableEntity entity) {

            if (!Contains(entity.UniqueId))

                _elements.Add(entity);
                Main.WindowManager.RefreshAllMenus();

            return this;

        }

        /// <summary>
        /// Removes the specified ControllableEntity from this ControlManager.
        /// </summary>
        /// <param name="entity">The ControllableEntity to remove.</param>
        /// <returns></returns>
        public ControlManager Remove(ControllableEntity entity) {

            if (Contains(entity)) {

                _elements.Remove(entity);
                Main.WindowManager.RefreshAllMenus();

            }

            return this;

        }

        /// <summary>
        /// Removes a ControllableEntity with an Entity with the specified UniqueId.
        /// </summary>
        /// <param name="uid">The UniqueId of the Entity to remove.</param>
        /// <returns></returns>
        public ControlManager Remove(int uid) {
            if (Contains(uid)) {
                _elements.Remove(_elements.Where(e => e.UniqueId == uid).ToArray()[0]);
                Main.WindowManager.RefreshAllMenus();
            }
            return this;
        }

        public ControlManager RemoveAndDeleteAll() {

            _elements.ForEach(e => {

                e.Entity.MarkAsNoLongerNeeded();
                if (e.Entity.IsPersistent) e.Entity.IsPersistent = false;
                e.Entity.Delete();

            });

            _elements.Clear();

            return this;

        }

        public bool AliveForMilliseconds(int uid, float milliseconds) {
            return _elements.Where(e => e.UniqueId == uid).ToArray()[0].SecondsAlive >= milliseconds;
        }

        public bool AliveForSeconds(int uid, float seconds) {
            return AliveForMilliseconds(uid, seconds * 1000);
        }

        /// <summary>
        /// Returns a collection of all the Peds within this ControlManager.
        /// </summary>
        public IEnumerable<ControllableEntity> Peds {
            get { return _elements.Where(e => e.GroupId.Contains("PED")); }
        }

        /// <summary>
        /// Returns a collection of all the Vehicles within this ControlManager.
        /// </summary>
        public IEnumerable<ControllableEntity> Vehicles {
            get { return _elements.Where(e => e.GroupId.Contains("VEHICLE")); }
        } 

        /// <summary>
        /// Returns a list of all the ControllableEntities within this ControlManager.
        /// </summary>
        public List<ControllableEntity> Entities {
            get { return _elements.Where(e => e != null).ToList(); }
        }

        /// <summary>
        /// Returns the amount of Peds within this ControlManager.
        /// </summary>
        public int PedCount {
            get { return Peds.ToList().Count; }   
        }

        /// <summary>
        /// Returns the amount of Vehicles within this ControlManager.
        /// </summary>
        public int VehicleCount {
            get { return Vehicles.ToList().Count; }
        }

        /// <summary>
        /// Returns the amount of Entities within this ControlManager.
        /// </summary>
        public int EntityCount {
            get { return Entities.Count; }
        }

        /// <summary>
        /// This has to be fired every tick within the Script. 
        /// <para>This ticks every single wrapped Entity within this ControlManager.</para>
        /// </summary>
        public void Tick() {
            Entities.ForEach(e => e.OnTick());
        }

        public string[] ToArray() {

            string[] result = new string[_elements.Count];
            for (var i = 0; i < _elements.Count; i++) {
                var entity = _elements[i];
                result[i] = "UID: " + entity.UniqueId + " | GID: " + entity.GroupId;
            }

            return result;

        }

    }

}
