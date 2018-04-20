using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Locations;
using StardewValley.Menus;
using StardewValley.Objects;
using xTile;

namespace MapTutorial
{
    class ModEntry : Mod
    {
        public static SaveData Data = new SaveData();

        public override void Entry(IModHelper helper)
        {
            //Adds function to event
            SaveEvents.AfterLoad += MapFixer;
        }

        private void MapFixer(object sender, EventArgs e)
        {
            //this function will run whenever the game loads a save
            //we need to know is sav actually points to a file in a minute
            string sav = Path.Combine(Constants.CurrentSavePath, "custom-farm-type.json");

            //ftype declared for use when custom farm name is found
            var ftype = "";

           //lets see if sav exists so we don't run our code on a regular map
           if (File.Exists(sav))
            {
                //Put the json in the public "Data" variable
                ModEntry.Data = this.Helper.ReadJsonFile<SaveData>(sav);
                //filter though the the FarmTypes object (there's only 1 property in there) 
                foreach(KeyValuePair<string, string> farmType in ModEntry.Data.FarmTypes)
                {
                    //now we can use "Farm Type" outside of this foreach
                    ftype = farmType.Value;
                }

                switch(ftype)
                {
                    //replace the <custom farm id> with wour farm's id (from custom farmtypes)
                    case "<custom farm id>":
                        //Create a variable that will store the location of the affected map
                        //Must be a valid game location
                        //Replace <location> with any valid location
                        GameLocation bStop = Game1.getLocationFromName("<location>");

                        //Remove an existing warp
                        //Replace <x> and <y> with the appropriate coordinates
                        bStop.warps.RemoveAll(a => a.X == <x> && a.Y == <y>);

                        //Add a new warp
                        //Replace the <x>'s and <y>'s - the coordinates before the <location> 
                        //define where the warp is and the coordinates after the <location>
                        //define where the target is on the given <location>
                        bStop.warps.Add(new Warp(<x>, <y>, "<location>", <x>, <y>, false));

                        //Create a new location
                        //I suggest leaving your map in its tbin format
                        //I also suggest keeping maps in the folder where the mod is
                        //This adds the map to a variable called map
                        //replace <map.tbin> with your map file name
                        Map map = this.Helper.Content.Load <Map>("<map.tbin>", ContentSource.ModFolder);

                        //This add makes a location we can add
                        //Make a name for the location and don't put a space in it where <name> is
                        GameLocation extraLoc = new GameLocation(map, "<name>"){ IsOutdoors = true, IsFarm = false };
                        //This actually adds the location passed into it
                        Game1.locations.Add(extraLoc);

                        break;
                    default:
                        break;
                }
            }

        }

    }
}
