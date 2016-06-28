/* Copyright (C) Miron George - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Miron George <george.miron2003@gmail.com>, 2016
 *
 *
 * History:
 * 09.02.2016    Miron George       Created class.
 */
using AzureDataAccess.Repository.Interfaces;
using AzureDataAccess.Context;
using AzureDataAccess.Repository.Implementations;

namespace AzureDataAccess
{
    /// <summary>
    /// Implementarea nivelului de date
    /// </summary>
    public class AzureDataAccess : IAzureDataAccess
    {
        public IUserRepository UserRepository { get; set; }
        public ICategoryRepository CategoryRepository { get; set; }
        public ITokenRepository TokenRepository { get; set; }
        public IQuestionRepository QuestionRepository { get; set; }
        public IAnswerRepository AnswerRepository { get; set; }
        public IFormRepository FormRepository { get; set; }
        public IVotedFormsRepository VotedFormsRepository { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public AzureDataAccess()
        {
            AzurePollAppDBContext context = new AzurePollAppDBContext();
            UserRepository = new UserRepository(context);
            CategoryRepository = new CategoryRepository(context);
            TokenRepository = new TokenRepository(context);
            QuestionRepository = new QuestionRepository(context);
            AnswerRepository = new AnswerRepository(context);
            FormRepository = new FormRepository(context);
            VotedFormsRepository = new VotedFormsRepository(context);
        }
    }
}
