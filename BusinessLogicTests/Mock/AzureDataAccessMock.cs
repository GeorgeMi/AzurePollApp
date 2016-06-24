/* Copyright (C) Miron George - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Miron George <george.miron2003@gmail.com>, 2016
 *
 *
 * History:
 * 16.06.2016    Miron George       Created class.
 */

using System;
using AzureDataAccess;
using AzureDataAccess.Repository.Interfaces;
using System.Linq;
using Entities;
using System.Collections.Generic;
using System.Linq.Expressions;
using Moq;

namespace BookLogicTest
{
    public class AzureDataAccessMock : IAzureDataAccess
    {
        public IUserRepository UserRepository { get; set; }
        public ICategoryRepository CategoryRepository { get; set; }
        public ITokenRepository TokenRepository { get; set; }
        public IQuestionRepository QuestionRepository { get; set; }
        public IAnswerRepository AnswerRepository { get; set; }
        public IFormRepository FormRepository { get; set; }
        public IVotedFormsRepository VotedFormsRepository { get; set; }

        public AzureDataAccessMock()
        {
            /* 
             *  Data Lists
             */

            List<User> usersList = new List<User>
            {
                new User {UserID = 1, Username = "user1", Password = "pass1", Email = "user1@email.com",Role = "admin",Verified = "yes"},
                new User {UserID = 2, Username = "user2", Password = "pass2", Email = "user2@email.com",Role = "user",Verified = "yes"},
                new User {UserID = 3, Username = "user3", Password = "pass3", Email = "user3@email.com",Role = "user",Verified = "no"}
            };

            List<Form> formsList = new List<Form>
            {
               new Form {FormID = 1, UserID = 1, CreatedDate = System.DateTime.Now, CategoryID = 1, Title = "Title1",State = "open",NrVotes = 100,Deadline = System.DateTime.Now.AddDays(10)},
               new Form {FormID = 2, UserID = 2, CreatedDate = System.DateTime.Now, CategoryID = 2, Title = "Title2",State = "open",NrVotes = 150,Deadline = System.DateTime.Now.AddDays(10)},
               new Form {FormID = 3, UserID = 3, CreatedDate = System.DateTime.Now.AddDays(-20), CategoryID = 3, Title = "Title3",State = "closed",NrVotes = 50,Deadline = System.DateTime.Now.AddDays(-10)}
            };

            List<Category> categoriesList = new List<Category>
            {
                new Category {CategoryID = 1, Name = "category1"},
                new Category {CategoryID = 2, Name = "category2"},
                new Category {CategoryID = 3, Name = "category3"}
            };

            List<Token> tokensList = new List<Token>
            {
                new Token {TokenID = 1, UserID = 1, TokenString = "1"},
                new Token {TokenID = 2, UserID = 2, TokenString = "2"},
                new Token {TokenID = 3, UserID = 3, TokenString = "3"},
            };

            List<Question> questionsList = new List<Question>
            {
                new Question {QuestionID = 1, FormID = 1, Content = "question11"},
                new Question {QuestionID = 2, FormID = 1, Content = "question12"},
                new Question {QuestionID = 3, FormID = 2, Content = "question21"},
                new Question {QuestionID = 4, FormID = 2, Content = "question22"},
                new Question {QuestionID = 5, FormID = 3, Content = "question31"},
                new Question {QuestionID = 6, FormID = 3, Content = "question32"}
            };

            List<Answer> answersList = new List<Answer>
            {
                new Answer {AnswerID = 1, QuestionID = 1, Content = "answer111", NrVotes = 5},
                new Answer {AnswerID = 2, QuestionID = 1, Content = "answer112", NrVotes = 15},
                new Answer {AnswerID = 3, QuestionID = 2, Content = "answer121", NrVotes = 25},
                new Answer {AnswerID = 4, QuestionID = 2, Content = "answer122", NrVotes = 5},
                new Answer {AnswerID = 5, QuestionID = 3, Content = "answer211", NrVotes = 15},
                new Answer {AnswerID = 6, QuestionID = 3, Content = "answer212", NrVotes = 25},
                new Answer {AnswerID = 7, QuestionID = 4, Content = "answer221", NrVotes = 5},
                new Answer {AnswerID = 8, QuestionID = 4, Content = "answer222", NrVotes = 15},
                new Answer {AnswerID = 9, QuestionID = 5, Content = "answer311", NrVotes = 25},
                new Answer {AnswerID = 10, QuestionID = 5, Content = "answer312", NrVotes = 5},
                new Answer {AnswerID = 11, QuestionID = 6, Content = "answer321", NrVotes = 15},
                new Answer {AnswerID = 12, QuestionID = 6, Content = "answer322", NrVotes = 25},

            };

            List<VotedForm> votedFormsList = new List<VotedForm>
            {
                new VotedForm {UserID = 1, FormID = 2},
                new VotedForm {UserID = 1, FormID = 3},
                new VotedForm {UserID = 2, FormID = 1},
                new VotedForm {UserID = 2, FormID = 2},
            };

            foreach (var form in formsList)
            {
                form.Category = categoriesList.Where(c => c.CategoryID == form.CategoryID).FirstOrDefault();
                form.User = usersList.Where(u => u.UserID == form.UserID).FirstOrDefault();
            }

            foreach (var question in questionsList)
            {
                question.Form = formsList.Where(f => f.FormID == question.FormID).FirstOrDefault();
            }

            foreach (var answer in answersList)
            {
                answer.Question = questionsList.Where(q => q.QuestionID == answer.QuestionID).FirstOrDefault();
            }

            foreach (var token in tokensList)
            {
                token.User = usersList.Where(u => u.UserID == token.UserID).FirstOrDefault();
            }

            foreach (var votedForm in votedFormsList)
            {
                votedForm.Form = formsList.Where(f => f.FormID == votedForm.FormID).FirstOrDefault();
            }

            /* 
             *  GetAll
             *
             */

            Mock<IUserRepository> userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(m => m.GetAll()).Returns(usersList.AsQueryable());

            Mock< ICategoryRepository > categoryRepositoryMock = new Mock<ICategoryRepository>();
            categoryRepositoryMock.Setup(m => m.GetAll()).Returns(categoriesList.AsQueryable());

            Mock<ITokenRepository> tokenRepositoryMock = new Mock<ITokenRepository>();
            tokenRepositoryMock.Setup(m => m.GetAll()).Returns(tokensList.AsQueryable());

            Mock<IQuestionRepository> questionRepositoryMock = new Mock<IQuestionRepository>();
            questionRepositoryMock.Setup(m => m.GetAll()).Returns(questionsList.AsQueryable());

            Mock<IAnswerRepository> answerRepositoryMock = new Mock<IAnswerRepository>();
            answerRepositoryMock.Setup(a => a.GetAll()).Returns(answersList.AsQueryable());

            Mock<IFormRepository> formRepositoryMock = new Mock<IFormRepository>();
            formRepositoryMock.Setup(m => m.GetAll()).Returns(formsList.AsQueryable());

            Mock<IVotedFormsRepository> votedFormRepositoryMock = new Mock<IVotedFormsRepository>();
            votedFormRepositoryMock.Setup(m => m.GetAll()).Returns(votedFormsList.AsQueryable());

            /* 
             *  FindAllBy
             *
             */
            userRepositoryMock.Setup(m => m.FindAllBy(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns<Expression<Func<User, bool>>>(usersList.AsQueryable().Where);

            formRepositoryMock.Setup(m => m.FindAllBy(It.IsAny<Expression<Func<Form, bool>>>()))
                .Returns<Expression<Func<Form, bool>>>(predicate => formsList.AsQueryable().Where(predicate));

            answerRepositoryMock.Setup(m => m.FindAllBy(It.IsAny<Expression<Func<Answer, bool>>>()))
                .Returns<Expression<Func<Answer, bool>>>(answersList.AsQueryable().Where);

            questionRepositoryMock.Setup(m => m.FindAllBy(It.IsAny<Expression<Func<Question, bool>>>()))
                .Returns<Expression<Func<Question, bool>>>(predicate => questionsList.AsQueryable().Where(predicate));

            tokenRepositoryMock.Setup(m => m.FindAllBy(It.IsAny<Expression<Func<Token, bool>>>()))
                .Returns<Expression<Func<Token, bool>>>(tokensList.AsQueryable().Where);

            votedFormRepositoryMock.Setup(m => m.FindAllBy(It.IsAny<Expression<Func<VotedForm, bool>>>()))
                .Returns<Expression<Func<VotedForm, bool>>>(predicate => votedFormsList.AsQueryable().Where(predicate));

            categoryRepositoryMock.Setup(m => m.FindAllBy(It.IsAny<Expression<Func<Category, bool>>>()))
                .Returns<Expression<Func<Category, bool>>>(predicate => categoriesList.AsQueryable().Where(predicate));

            /* 
             *  FindFirstBy
             *
             */

            formRepositoryMock.Setup(m => m.FindFirstBy(It.IsAny<Expression<Func<Form, bool>>>()))
                .Returns<Expression<Func<Form, bool>>>(predicate => formsList.AsQueryable().Where(predicate).FirstOrDefault());

           categoryRepositoryMock.Setup(m => m.FindFirstBy(It.IsAny<Expression<Func<Category, bool>>>()))
                .Returns<Expression<Func<Category, bool>>>(predicate => categoriesList.AsQueryable().Where(predicate).FirstOrDefault());

           userRepositoryMock.Setup(m => m.FindFirstBy(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns<Expression<Func<User, bool>>>(predicate => usersList.AsQueryable().Where(predicate).FirstOrDefault());

           questionRepositoryMock.Setup(m => m.FindFirstBy(It.IsAny<Expression<Func<Question, bool>>>()))
                .Returns<Expression<Func<Question, bool>>>(predicate => questionsList.AsQueryable().Where(predicate).FirstOrDefault());

           answerRepositoryMock.Setup(m => m.FindFirstBy(It.IsAny<Expression<Func<Answer, bool>>>()))
                .Returns<Expression<Func<Answer, bool>>>(predicate => answersList.AsQueryable().Where(predicate).FirstOrDefault());

           tokenRepositoryMock.Setup(m => m.FindFirstBy(It.IsAny<Expression<Func<Token, bool>>>()))
                .Returns<Expression<Func<Token, bool>>>(predicate => tokensList.AsQueryable().Where(predicate).FirstOrDefault());

           votedFormRepositoryMock.Setup(m => m.FindFirstBy(It.IsAny<Expression<Func<VotedForm, bool>>>()))
                .Returns<Expression<Func<VotedForm, bool>>>(predicate => votedFormsList.AsQueryable().Where(predicate).FirstOrDefault());
            
            /* 
             *  Add 
             *
             */

            formRepositoryMock.Setup(x => x.Add(It.IsAny<Form>())).Callback(new Action<Form>(f =>
            {
                f.FormID = formsList.Last().FormID + 1;
                formsList.Add(f);
            }));

            questionRepositoryMock.Setup(m => m.Add(It.IsAny<Question>())).Callback(new Action<Question>(f =>
            {
                f.QuestionID = questionsList.Last().QuestionID + 1 ;
                questionsList.Add(f);
            }));

            answerRepositoryMock.Setup(m => m.Add(It.IsAny<Answer>())).Callback(new Action<Answer> (f => {
                f.AnswerID = answersList.Last().AnswerID + 1;
                answersList.Add(f);
            }));

            categoryRepositoryMock.Setup(m => m.Add(It.IsAny<Category>())).Callback(new Action<Category> (f => {
                categoriesList.Add(f);
            }));

            tokenRepositoryMock.Setup(m => m.Add(It.IsAny<Token>())).Callback(new Action<Token> (f => {
                tokensList.Add(f);
            }));

            userRepositoryMock.Setup(m => m.Add(It.IsAny<User>())).Callback(new Action<User> (f => {
                f.UserID = usersList.Last().UserID + 1;
                usersList.Add(f);
            }));

            /* 
             *  Delete 
             *
             */

            formRepositoryMock.Setup(x => x.Delete(It.IsAny<Form>())).Callback(new Action<Form>(x =>
            {
                var i = formsList.FindIndex(f => f.FormID.Equals(x.FormID));
                formsList.RemoveAt(i);
            }));

            questionRepositoryMock.Setup(x => x.Delete(It.IsAny<Question>())).Callback(new Action<Question>(x =>
            {
                var i = questionsList.FindIndex(f => f.QuestionID.Equals(x.QuestionID));
                questionsList.RemoveAt(i);
            }));

            answerRepositoryMock.Setup(x => x.Delete(It.IsAny<Answer>())).Callback(new Action<Answer>(x =>
            {
                var i = answersList.FindIndex(f => f.AnswerID.Equals(x.AnswerID));
                answersList.RemoveAt(i);
            }));

            categoryRepositoryMock.Setup(x => x.Delete(It.IsAny<Category>())).Callback(new Action<Category>(x =>
            {
                var i = categoriesList.FindIndex(f => f.CategoryID.Equals(x.CategoryID));
                categoriesList.RemoveAt(i);
            }));

            tokenRepositoryMock.Setup(x => x.Delete(It.IsAny<Token>())).Callback(new Action<Token>(x =>
            {
                var i = tokensList.FindIndex(f => f.TokenID.Equals(x.TokenID));
                tokensList.RemoveAt(i);
            }));

            userRepositoryMock.Setup(x => x.Delete(It.IsAny<User>())).Callback(new Action<User>(x =>
            {
                var i = usersList.FindIndex(f => f.UserID.Equals(x.UserID));
                usersList.RemoveAt(i);
            }));

            /* 
             *  Vote 
             *
             */
            
            formRepositoryMock.Setup(x => x.AddVote(It.IsAny<int>())).Callback(new Action<int>(x =>
             {
                var i = formsList.FindIndex(q => q.FormID.Equals(x));
                formsList[i].NrVotes = formsList[i].NrVotes + 1;
            }));
            
            UserRepository = userRepositoryMock.Object;
            FormRepository = formRepositoryMock.Object;
            CategoryRepository = categoryRepositoryMock.Object;
            TokenRepository = tokenRepositoryMock.Object;
            QuestionRepository = questionRepositoryMock.Object;
            AnswerRepository = answerRepositoryMock.Object;
            VotedFormsRepository = votedFormRepositoryMock.Object;
        }
    }
}

