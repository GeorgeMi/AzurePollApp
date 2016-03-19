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
    public class QuestionRepository : GenericRepository<Question>, IQuestionRepository
    {
        public QuestionRepository(AzurePollAppDBContext context) : base(context)
        {

        }
        public void AddVote(int id)
        {
            Question q = Context.Questions.Find(id);
            q.NrVotes = q.NrVotes + 1;

            Context.SaveChanges();
        }
    }
}
