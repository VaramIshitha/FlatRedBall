﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WpfDataUi;
using WpfDataUi.DataTypes;

namespace OfficialPlugins.VariableDisplay
{
    class RefreshLogic
    {
        static int RefreshesToSkip = 0;

        internal static void RefreshGrid(DataUiGrid grid)
        {
            if (RefreshesToSkip > 0)
            {
                RefreshesToSkip--;
            }
            else
            {
                grid.Refresh();
            }

            foreach (var category in grid.Categories)
            {
                List<InstanceMember> membersToRefresh = new List<InstanceMember>();

                foreach (DataGridItem instanceMember in category.Members)
                {
                    // Not sure why we check if the instanceMember has non-0 count for options.
                    // It could have had 0 before, but after a refresh, it may now have options.
                    bool shouldRefresh = //instanceMember.CustomOptions.Count != 0 &&
                        instanceMember.TypeConverter != null;

                    if (shouldRefresh)
                    {
                        instanceMember.RefreshOptions();
                        membersToRefresh.Add(instanceMember);
                    }
                }

                bool shouldSort = membersToRefresh.Count != 0;

                foreach (var item in membersToRefresh)
                {
                    var index = category.Members.IndexOf(item);
                    category.Members.Remove(item);
                    category.Members.Insert(index, item);
                }
            }
        }

        internal static void IgnoreNextRefresh()
        {
            RefreshesToSkip++;
        }
    }
}
