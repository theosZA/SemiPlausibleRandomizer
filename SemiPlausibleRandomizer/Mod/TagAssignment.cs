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
        public void AssignTags(IEnumerable<CountryBuilder> countries)
        {
            foreach (var country in countries)
            {
                // Find a tag which has this country's capital province as a province assignment.
                var matchingElements = tagAssignments.Where(i => i.Value.Exists(j => j.type == AssignmentType.Province && j.provinceID.HasValue && j.provinceID == country.Capital.Key));
                if (matchingElements.Count() > 0)
                {
                    country.Tag = matchingElements.First().Key;
                }
            }
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
