﻿//--------------------------------------------------------------------------------------------------
// <auto-generated>
//
//     This code was generated by code generator tool.
//
//     To customize the code use your own partial class. For more info about how to use and customize
//     the generated code see the documentation at https://docs.xperience.io/.
//
// </auto-generated>
//--------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using CMS;
using CMS.Base;
using CMS.Helpers;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.XperienceCommunity;

[assembly: RegisterDocumentType(RSSFeedPage.CLASS_NAME, typeof(RSSFeedPage))]

namespace CMS.DocumentEngine.Types.XperienceCommunity
{
    /// <summary>
    /// Represents a content item of type RSSFeedPage.
    /// </summary>
    public partial class RSSFeedPage : TreeNode
    {
        #region "Constants and variables"

        /// <summary>
        /// The name of the data class.
        /// </summary>
        public const string CLASS_NAME = "XperienceCommunity.RSSFeedPage";


        /// <summary>
        /// The instance of the class that provides extended API for working with RSSFeedPage fields.
        /// </summary>
        private readonly RSSFeedPageFields mFields;

        #endregion


        #region "Properties"

        /// <summary>
        /// RSSFeedPageID.
        /// </summary>
        [DatabaseIDField]
        public int RSSFeedPageID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("RSSFeedPageID"), 0);
            }
            set
            {
                SetValue("RSSFeedPageID", value);
            }
        }


        /// <summary>
        /// Title.
        /// </summary>
        [DatabaseField]
        public string RSSFeedPageTitle
        {
            get
            {
                return ValidationHelper.GetString(GetValue("RSSFeedPageTitle"), @"");
            }
            set
            {
                SetValue("RSSFeedPageTitle", value);
            }
        }


        /// <summary>
        /// Feed Description.
        /// </summary>
        [DatabaseField]
        public string RSSFeedPageDescription
        {
            get
            {
                return ValidationHelper.GetString(GetValue("RSSFeedPageDescription"), @"");
            }
            set
            {
                SetValue("RSSFeedPageDescription", value);
            }
        }


        /// <summary>
        /// Limits the RSS feed to only return items for Pages of the selected types.
        /// 
        /// Only Pages that have the URL feature enabled are selectable.
        /// </summary>
        [DatabaseField]
        public string RSSFeedPagePageTypes
        {
            get
            {
                return ValidationHelper.GetString(GetValue("RSSFeedPagePageTypes"), @"");
            }
            set
            {
                SetValue("RSSFeedPagePageTypes", value);
            }
        }


        /// <summary>
        /// Feed Items Count.
        /// </summary>
        [DatabaseField]
        public int RSSFeedPageItemsCount
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("RSSFeedPageItemsCount"), 10);
            }
            set
            {
                SetValue("RSSFeedPageItemsCount", value);
            }
        }


        /// <summary>
        /// Any value greater than 0 will result in the results being cached. A value of 0 will disable caching.
        /// </summary>
        [DatabaseField]
        public int RSSFeedPageCacheLifetimeMinutes
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("RSSFeedPageCacheLifetimeMinutes"), 5);
            }
            set
            {
                SetValue("RSSFeedPageCacheLifetimeMinutes", value);
            }
        }


        /// <summary>
        /// Gets an object that provides extended API for working with RSSFeedPage fields.
        /// </summary>
        [RegisterProperty]
        public RSSFeedPageFields Fields
        {
            get
            {
                return mFields;
            }
        }


        /// <summary>
        /// Provides extended API for working with RSSFeedPage fields.
        /// </summary>
        [RegisterAllProperties]
        public partial class RSSFeedPageFields : AbstractHierarchicalObject<RSSFeedPageFields>
        {
            /// <summary>
            /// The content item of type RSSFeedPage that is a target of the extended API.
            /// </summary>
            private readonly RSSFeedPage mInstance;


            /// <summary>
            /// Initializes a new instance of the <see cref="RSSFeedPageFields" /> class with the specified content item of type RSSFeedPage.
            /// </summary>
            /// <param name="instance">The content item of type RSSFeedPage that is a target of the extended API.</param>
            public RSSFeedPageFields(RSSFeedPage instance)
            {
                mInstance = instance;
            }


            /// <summary>
            /// RSSFeedPageID.
            /// </summary>
            public int ID
            {
                get
                {
                    return mInstance.RSSFeedPageID;
                }
                set
                {
                    mInstance.RSSFeedPageID = value;
                }
            }


            /// <summary>
            /// Title.
            /// </summary>
            public string Title
            {
                get
                {
                    return mInstance.RSSFeedPageTitle;
                }
                set
                {
                    mInstance.RSSFeedPageTitle = value;
                }
            }


            /// <summary>
            /// Feed Description.
            /// </summary>
            public string Description
            {
                get
                {
                    return mInstance.RSSFeedPageDescription;
                }
                set
                {
                    mInstance.RSSFeedPageDescription = value;
                }
            }


            /// <summary>
            /// Limits the RSS feed to only return items for Pages of the selected types.
            /// 
            /// Only Pages that have the URL feature enabled are selectable.
            /// </summary>
            public string PageTypes
            {
                get
                {
                    return mInstance.RSSFeedPagePageTypes;
                }
                set
                {
                    mInstance.RSSFeedPagePageTypes = value;
                }
            }


            /// <summary>
            /// Feed Items Count.
            /// </summary>
            public int ItemsCount
            {
                get
                {
                    return mInstance.RSSFeedPageItemsCount;
                }
                set
                {
                    mInstance.RSSFeedPageItemsCount = value;
                }
            }


            /// <summary>
            /// Any value greater than 0 will result in the results being cached. A value of 0 will disable caching.
            /// </summary>
            public int CacheLifetimeMinutes
            {
                get
                {
                    return mInstance.RSSFeedPageCacheLifetimeMinutes;
                }
                set
                {
                    mInstance.RSSFeedPageCacheLifetimeMinutes = value;
                }
            }
        }

        #endregion


        #region "Constructors"

        /// <summary>
        /// Initializes a new instance of the <see cref="RSSFeedPage" /> class.
        /// </summary>
        public RSSFeedPage() : base(CLASS_NAME)
        {
            mFields = new RSSFeedPageFields(this);
        }

        #endregion
    }
}