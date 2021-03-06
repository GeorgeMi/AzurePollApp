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
    /// Repository implementare interfata "IQuestionRepository"
    /// </summary>
    public class QuestionRepository : GenericRepository<Question>, IQuestionRepository
    {
        /// <summary>
        /// Constructor
        /// </summary> 
        public QuestionRepository(AzurePollAppDBContext context) : base(context)
        {
        }
    }
}
