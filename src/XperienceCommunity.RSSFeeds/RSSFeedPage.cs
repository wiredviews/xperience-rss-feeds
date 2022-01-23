using System;
using System.Linq;

namespace CMS.DocumentEngine.Types.XperienceCommunity
{
    /// <summary>
    /// Represents a content item of type RSSFeedPage.
    /// </summary>
    public partial class RSSFeedPage : TreeNode
    {
        public partial class RSSFeedPageFields
        {
            private string[] pageTypesSet;

            public string[] PageTypesSet
            {
                get
                {
                    if (pageTypesSet is null)
                    {
                        pageTypesSet = PageTypes
                            .Split(";", StringSplitOptions.RemoveEmptyEntries)
                            .Select(t => t.Trim())
                            .ToArray();
                    }

                    return pageTypesSet;
                }
            }
        }
    }
}
