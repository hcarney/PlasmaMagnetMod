My first KSP plugin, please provide feedback!
If anyone is interested in collaborating on the models/art, let me know!

The plasma magnet (invented by John Slough et al.) is a clever way to make a really light magnetic sail that can ride the solar wind. It maintains a near-constant force for a given "airspeed", regardless of distance from the sun. A further refinement by Jeff Greason is the q-drive, harvesting energy from the plasma magnet and powering electric drives that permit the vehicle to do more than sail directly away from the sun. This mod allows you to do both.

How To:
-It appears in the part list as "QCOR-1 Plasmadasmadyne", under Engine. Shares the model with the 2.5m SAS
-Enable/disable with the button "Plasma Magnet"
-Change power level with the "Plasma Sail Power" slider
-"Hydrocoptic marzelvanes" change whether the force is applied to the part or applied to the ship as a whole
-JEB MODE provides a more Kerbal experience (50x), comparable in magnitude to the difference between IRL ion engines and KSP ion engines (about 460x)
-Alternatively, you may alter the PlasmaMagnetScaleFactor in "plasmaMagnet1.cfg". A realistic drag  (0.1kN/ton) can be achieved by setting PlasmaMagnetScaleFactor=1 (defaulted to 100)
-The plasma magnet will provide EC in proportion to the current drag force, if at least two magnets are active on the craft

Things that are known not to work:
-Does not work in non-physics timewarp
-Not fully integrated into the tech tree, should unlock with Experimental Electrics
-No attempt has been made to balance the ElectricCharge output within the context of KSP yet, it's just raw 1kW=1EC/s. 

Caveats
-Not tested to work with any other mods, yet. I don't anticipate any particular problems, but it's not impossible.
-Drag is realistic (and quite low), increase by enabling JEB MODE (at your own peril)
-The plasma magnet is only available in one size right now, if you want more power/drag try JEB MODE or increase the mass of the part in plasmaMagnet1.cfg
-The plasma magnet only works around things KSP considers to be a star. So far only tested with the default Sun. 

Future Plans
-Art! If you are interested in collaborating on the modeling or texturing work, I would be more than happy to work with you!
-Tech tree integration and progression
-Propellant consumption, plasma magnets need a tiny bit of matter fed into them
-More feedback to the player (current bubble size, drag thrust, etc.)
-Distance requirement for two magnets to produce power
-Stellar config system to support plasma magnets around modded stars, as well as modeling the fall-off in solar wind at the heliopause
-Planetary configs to reflect how different bodies have complex magnetospheres, rather than only working around stars