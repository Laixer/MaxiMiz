﻿using Maximiz.ViewModels.Enums;
using System;

namespace Maximiz.ViewModels.EntityModels
{

    /// <summary>
    /// Represents an account.
    /// </summary>
    public class AccountModel : EntityModel<Guid>
    {

        /// <summary>
        /// Account name in human readable form.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The publisher to which this accounts belongs.
        /// </summary>
        public Publisher Publisher { get; set; } 

        /// <summary>
        /// Contains the type of account.
        /// 
        /// TODO This seems rather unsafe! A lot to do for our mapper.
        /// The reason I choose this structure is because the database object
        /// layout might change. At the moment our desired 'string' is in the 
        /// details json field of our database. We want to extract the field
        /// and parse it during mapping.
        /// </summary>
        public string AccountTypeString { get; set; }

    }

}
