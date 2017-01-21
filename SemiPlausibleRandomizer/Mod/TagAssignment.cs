using Pfarah;
using SemiPlausibleRandomizer.EU4;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SemiPlausibleRandomizer.Mod
{
    /// <summary>
    /// Holds details about how existing tags should be allocated to generated countries.
    /// </summary>
    internal class TagAssignment
    {
        public void LoadFromFile(string fileName)
        {
            var assignmentsRecord = ParaValue.LoadText(fileName) as ParaValue.Record;
            foreach (var tagAssignment in assignmentsRecord.properties)
            {
                var tag = tagAssignment.Item1;
                var assignmentDetails = new List<AssignmentDetail>();
                foreach (var assignmentOptions in (tagAssignment.Item2 as ParaValue.Record).properties)
                {
                    assignmentDetails.Add(new AssignmentDetail()
                    {
                        type = StringToAssignmentType(assignmentOptions.Item1),
                        key = (assignmentOptions.Item2 as ParaValue.String)?.Item,
                        provinceID = (int?)(assignmentOptions.Item2 as ParaValue.Number)?.Item,
                    });
                }
                tagAssignments[tag] = assignmentDetails;
            }
        }

        /// <summary>
        /// For each country provided, see if we can assign it one of our tags.
        /// </summary>
        /// <param name="countries">Countries that we wish to substitute with existing tags.</param>
        /// <param name="referenceWorld">World object that can be used to look up more information for tag assignment.</param>
        public void AssignTags(IEnumerable<CountryBuilder> countries, World referenceWorld)
        {
            var usedTags = new HashSet<string>();
            foreach (var country in countries)
            {
                // Does the country occupy 75%+ of its capital region?
                var region = referenceWorld.GetRegionContainingProvince(country.Capital.Key);
                var regionProvinces = referenceWorld.GetProvincesInRegion(region.Key);
                var countryProvincesInRegion = regionProvinces.Intersect(country.Provinces);
                if (countryProvincesInRegion.Count() >= 0.75 * regionProvinces.Count())
                {
                    // Try find a region tag.
                    var regionTag = FindMatchingTag(AssignmentType.Region, region.Key);
                    if (regionTag != null && !usedTags.Contains(regionTag))
                    {
                        country.Tag = regionTag;
                        usedTags.Add(regionTag);
                        continue;
                    }
                }

                // Does the country occupy 75%+ of its capital area?
                var area = referenceWorld.GetAreaContainingProvince(country.Capital.Key);
                var areaProvinces = referenceWorld.GetProvincesInArea(area.Key);
                var countryProvincesInArea = areaProvinces.Intersect(country.Provinces);
                if (countryProvincesInArea.Count() >= 0.75 * areaProvinces.Count())
                {
                    // Try find an area tag.
                    var areaTag = FindMatchingTag(AssignmentType.Area, area.Key);
                    if (areaTag != null && !usedTags.Contains(areaTag))
                    {
                        country.Tag = areaTag;
                        usedTags.Add(areaTag);
                        continue;
                    }
                }

                // Does the country have 75%+ of its culture?
                var culture = country.Capital.Culture;
                var cultureProvinces = referenceWorld.GetProvincesWithCulture(culture);
                var countryCultureProvinces = cultureProvinces.Intersect(country.Provinces);
                if (countryCultureProvinces.Count() >= 0.75 * cultureProvinces.Count())
                {
                    // Try to find a culture tag.
                    var cultureTag = FindMatchingTag(AssignmentType.Culture, culture);
                    if (cultureTag != null && !usedTags.Contains(cultureTag))
                    {
                        country.Tag = cultureTag;
                        usedTags.Add(cultureTag);
                        continue;
                    }
                }

                // Try find a province tag.
                var provinceTag = FindMatchingProvinceTag(country.Capital.Key);
                if (provinceTag != null && !usedTags.Contains(provinceTag))
                {
                    country.Tag = provinceTag;
                    usedTags.Add(provinceTag);
                    continue;
                }
            }
        }

        string FindMatchingTag(AssignmentType type, string key)
        {
            foreach (var tagAssignment in tagAssignments)
            {
                foreach (var assignment in tagAssignment.Value)
                {
                    if (assignment.type == type && assignment.key == key)
                    {
                        return tagAssignment.Key;
                    }
                }
            }
            return null;
        }

        string FindMatchingProvinceTag(int provinceID)
        {
            foreach (var tagAssignment in tagAssignments)
            {
                foreach (var assignment in tagAssignment.Value)
                {
                    if (assignment.type == AssignmentType.Province && assignment.provinceID == provinceID)
                    {
                        return tagAssignment.Key;
                    }
                }
            }
            return null;
        }

        enum AssignmentType
        {
            Province,
            Area,
            Region,
            Culture,
            CultureGroup
        }
        AssignmentType StringToAssignmentType(string value)
        {
            switch (value)
            {
                case "province": return AssignmentType.Province;
                case "area": return AssignmentType.Area;
                case "region": return AssignmentType.Region;
                case "culture": return AssignmentType.Culture;
                case "culture_group": return AssignmentType.CultureGroup;
                default:
                    throw new Exception($"Unrecognized tag assignment type: {value}");
            }
        }

        struct AssignmentDetail
        {
            public AssignmentType type;
            public string key;
            public int? provinceID;
        }
        Dictionary<string, List<AssignmentDetail>> tagAssignments = new Dictionary<string, List<AssignmentDetail>>();
    }
}
