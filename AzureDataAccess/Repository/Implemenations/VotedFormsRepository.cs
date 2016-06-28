/* Copyright (C) Miron George - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Miron George <george.miron2003@gmail.com>, 2016
 */

using Entities;
using AzureDataAccess.Repository.Interfaces;
using AzureDataAccess.Context;

namespace AzureDataAccess.Repository.Implementations
{
    /// <summary>
    /// Repository implementare interfata "IVotedFormsRepository"
    /// </summary>
    public class VotedFormsRepository : GenericRepository<VotedForm>, IVotedFormsRepository
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public VotedFormsRepository(AzurePollAppDBContext context) : base(context)
        {
        }
    }
}
