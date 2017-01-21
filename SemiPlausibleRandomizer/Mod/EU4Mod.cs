using SemiPlausibleRandomizer.EU4;
using SemiPlausibleRandomizer.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace SemiPlausibleRandomizer.Mod
{
    internal class EU4Mod
    {
        public void Build(World baseWorld, IEnumerable<Province> provinces, IEnumerable<Tuple<int, int>> countrySizeLimits)
        {
            worldBuilder = new WorldBuilder()
            {
                EU4World = baseWorld,
                Provinces = provinces,
                CountrySizeLimits = countrySizeLimits
            };
            worldBuilder.Build();
        }

        public void Save(string modPath, string name, string eu4Version)
        {
            // Create the mod.
            string[] modLines =
            {
                $"name = \"{name}\"",
                $"path = \"mod/{name}\"",
                $"supported_version = \"{eu4Version}\""
            };
            File.WriteAllLines($"{modPath}\\{name}.mod", modLines);
            string ourModPath = $"{modPath}\\{name}";
            Directory.CreateDirectory(ourModPath);
            Directory.CreateDirectory($"{ourModPath}\\history");
            Directory.CreateDirectory($"{ourModPath}\\history\\countries");
            Directory.CreateDirectory($"{ourModPath}\\history\\provinces");
            Directory.CreateDirectory($"{ourModPath}\\common");
            Directory.CreateDirectory($"{ourModPath}\\common\\countries");
            Directory.CreateDirectory($"{ourModPath}\\common\\country_tags");
            Directory.CreateDirectory($"{ourModPath}\\localisation");
            Directory.CreateDirectory($"{ourModPath}\\gfx");
            Directory.CreateDirectory($"{ourModPath}\\gfx\\flags");

            worldBuilder?.Save(ourModPath);
        }

        WorldBuilder worldBuilder = null;
    }
}
