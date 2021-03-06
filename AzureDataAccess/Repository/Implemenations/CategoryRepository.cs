﻿/* Copyright (C) Miron George - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Miron George <george.miron2003@gmail.com>, 2016
 *
 * Role:
 * Repository implementation. 
 *
 * History:
 * 09.02.2016    Miron George       Created class.
 */
using Entities;
using AzureDataAccess.Repository.Interfaces;
using AzureDataAccess.Context;

namespace AzureDataAccess.Repository.Implementations
{
    /// <summary>
    /// Repository implementare interfata "ICategoryRepository"
    /// </summary>
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CategoryRepository(AzurePollAppDBContext context) : base(context)
        {
        }
    }
}
