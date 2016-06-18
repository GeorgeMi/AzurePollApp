using System;
using BusinessLogic;
using DataTransferObject;
using System.Collections.Generic;
using AzureDataAccess;
using BookLogicTest;
using  NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace BusinessLogicTests
{
    [TestFixture]
    public class FormLogicTest
    {
        IAzureDataAccess _mockDataAccess = new AzureDataAccessMock();
        FormLogic _formLogic;

        [SetUp]
        public void Setup()
        {
            _mockDataAccess = new AzureDataAccessMock();
            _formLogic = new FormLogic(_mockDataAccess);
        }

        [Test]
        public void GetUserForms_Test()
        {
            Assert.AreEqual(1, _formLogic.GetUserForms("user1", 0, 5, "all").Count);
        }

        [Test]
        public void GetUserForms_Test2()
        {
            try
            {
                _formLogic.GetUserForms("user22", 0, 5, "open");
            }
            catch (Exception)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void GetUserForms_Test3()
        {
            Assert.AreEqual(1, _formLogic.GetUserForms("user2", 0, 5, "open").Count);
        }

        [Test]
        public void GetUserForms_Test4()
        {
            Assert.AreEqual(0, _formLogic.GetUserForms("user2", 0, 5, "closed").Count);
        }

        [Test]
        public void GetUserForms_Test5()
        {
            Assert.AreEqual(0, _formLogic.GetUserForms("user2", 0, 5, "asdasd").Count);
        }

        [Test]
        public void GetDetailResultForm_Test()
        {
            Assert.GreaterOrEqual(_formLogic.GetDetailResultForm(1).NrVotes, 1);
        }

        [Test]
        public void GetDetailResultForm_Test2()
        {
            Assert.GreaterOrEqual(_formLogic.GetDetailResultForm(1).Questions.Count, 1);
        }

        [Test]
        public void GetDetailResultForm_Test3()
        {
            try
            {
                _formLogic.GetDetailResultForm(10);
            }
            catch (Exception)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void GetDetailResultForm_Test4()
        {
            try
            {
                _formLogic.GetDetailResultForm(-10);
            }
            catch (Exception)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void GetDetailResultForm_Test5()
        {
            try
            {
                _formLogic.GetDetailResultForm(0);
            }
            catch (Exception)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void GetAllForms_Test()
        {
            Assert.AreEqual(3, _formLogic.GetAllForms("1", "Title", 0, 5, "all").Count);
        }

        [Test]
        public void GetAllForms_Test2()
        {
            Assert.AreEqual(1, _formLogic.GetAllForms("1", "Title", 0, 5, "closed").Count);
        }

        [Test]
        public void GetAllForms_Test3()
        {
            Assert.AreEqual(0, _formLogic.GetAllForms("1", "Title", 0, 5, "cloasdassed").Count);
        }

        [Test]
        public void GetAllForms_Test4()
        {
            Assert.AreEqual(0, _formLogic.GetAllForms("1", "asdads", 0, 5, "all").Count);
        }

        [Test]
        public void GetAllForms_Test5()
        {
            try
            {
                _formLogic.GetAllForms("asd1", "Title", 0, 5, "all");
            }
            catch (Exception)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void GetCategoryForms_Test()
        {
            Assert.AreEqual(1, _formLogic.GetCategoryForms(1, "1", 0, 5, "all").Count);
        }

        [Test]
        public void GetCategoryForms_Test2()
        {
            Assert.AreEqual(0, _formLogic.GetCategoryForms(1, "1", 0, 5, "asdasd").Count);
        }

        [Test]
        public void GetCategoryForms_Test3()
        {
            try
            {
                _formLogic.GetCategoryForms(1, "asdas", 0, 5, "all");
            }
            catch (Exception)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void GetCategoryForms_Test4()
        {
            Assert.AreEqual(0, _formLogic.GetCategoryForms(0, "1", 0, 5, "all").Count);
        }

        [Test]
        public void GetCategoryForms_Test5()
        {
            Assert.AreEqual(0, _formLogic.GetCategoryForms(-200, "1", 0, 5, "all").Count);
        }

        [Test]
        public void GetVotedForms_Test()
        {
            Assert.AreEqual(2, _formLogic.GetVotedForms("user1", 0, 5, "all").Count);
        }

        [Test]
        public void GetVotedForms_Test2()
        {
            Assert.AreEqual(1, _formLogic.GetVotedForms("user1", 0, 5, "open").Count);
        }

        [Test]
        public void GetVotedForms_Test3()
        {
            Assert.AreEqual(1, _formLogic.GetVotedForms("user1", 0, 1, "all").Count);
        }

        [Test]
        public void GetVotedForms_Test4()
        {
            try
            {
                _formLogic.GetVotedForms("asdasd", 0, 1, "all");
            }
            catch (Exception)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void GetVotedForms_Test5()
        {
            Assert.AreEqual(0, _formLogic.GetVotedForms("user1", 0, 1, "asdasd").Count);
        }

        [Test]
        public void GetContentForm_Test()
        {
            Assert.AreEqual(1, _formLogic.GetContentForm(1).Id);
        }

        [Test]
        public void GetContentForm_Test2()
        {
            Assert.IsNotNull(_formLogic.GetContentForm(2));
        }

        [Test]
        public void GetContentForm_Test3()
        {
            try
            {
                _formLogic.GetContentForm(-3);
            }
            catch (Exception)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void GetUsername_Test()
        {
            Assert.AreEqual(0, _formLogic.GetUsername(1).CompareTo("user1"));
        }

        [Test]
        public void GetUsername_Test2()
        {
            Assert.AreEqual(1, _formLogic.GetUsername(1).CompareTo("asdas"));
        }

        [Test]
        public void GetUsername_Test3()
        {
            try
            {
                _formLogic.GetUsername(100);
            }
            catch (Exception)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void GetUsername_Test4()
        {
            try
            {
                _formLogic.GetUsername(-100);
            }
            catch (Exception)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void GetAllQuestionFromForm_Test()
        {
            Assert.AreEqual(2, _formLogic.GetAllQuestionFromForm(1).Count);
        }

        [Test]
        public void GetAllQuestionFromForm_Test2()
        {
            Assert.AreEqual(2, _formLogic.GetAllQuestionFromForm(2).Count);
        }

        [Test]
        public void GetAllQuestionFromForm_Test3()
        {
            Assert.AreEqual(0, _formLogic.GetAllQuestionFromForm(-2).Count);
        }

        [Test]
        public void GetAllAnswerFromQuestion_Test()
        {
            Assert.AreEqual(2, _formLogic.GetAllAnswerFromQuestion(1).Count);
        }

        [Test]
        public void GetAllAnswerFromQuestion_Test2()
        {
            Assert.AreEqual(2, _formLogic.GetAllAnswerFromQuestion(2).Count);
        }

        [Test]
        public void GetAllAnswerFromQuestion_Test3()
        {
            Assert.AreEqual(0, _formLogic.GetAllAnswerFromQuestion(-2).Count);
        }

        [Test]
        public void GetCategory_Test()
        {
            Assert.AreEqual(0, _formLogic.GetCategory(1).CompareTo("category1"));
        }

        [Test]
        public void GetCategory_Test2()
        {
            Assert.AreEqual(-1, _formLogic.GetCategory(1).CompareTo("sadasd"));
        }

        [Test]
        public void GetCategory_Test3()
        {
            try
            {
                _formLogic.GetCategory(-1);
            }
            catch (Exception)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void GetCategory_Test4()
        {
            try
            {
                _formLogic.GetCategory(100);
            }
            catch (Exception)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void GetCategoryID_Test()
        {
            try
            {
                _formLogic.GetCategoryID("asdasd");
            }
            catch (Exception)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void GetCategoryID_Test2()
        {
            Assert.AreEqual(1, _formLogic.GetCategoryID("category1"));
        }

        [Test]
        public void GetQuestionID_Test()
        {
            Assert.AreEqual(1, _formLogic.GetQuestionID("question11", 1));
        }

        [Test]
        public void GetQuestionID_Test2()
        {
            try
            {
                _formLogic.GetQuestionID("asdasd", 1);
            }
            catch (Exception)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void GetQuestionID_Test3()
        {
            try
            {
                _formLogic.GetQuestionID("question11", -1);
            }
            catch (Exception)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void AddForm_Test()
        {
            try
            {
                List<QuestionDTO> q = new List<QuestionDTO>();
                List<AnswerDTO> a = new List<AnswerDTO>();
                a.Add(new AnswerDTO {Answer = "a", AnswerID = 20});

                q.Add(new QuestionDTO {QuestionID = 1, Question = "why?", Answers = a});

                FormDetailDTO f = new FormDetailDTO()
                {
                    Id = 4,
                    Username = "user1",
                    Deadline = "3",
                    Category = "category1",
                    Title = "Title1",
                    State = "open",
                    NrVotes = 100,
                    Questions = q
                };

                _formLogic.AddForm(f);
            }
            catch (Exception)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void GetForm_Test()
        {
            Assert.AreEqual(1,_formLogic.GetForm(1).FormID);
        }

        [Test]
        public void GetForm_Test2()
        {
            try
            {
                int id = _formLogic.GetForm(-1).FormID;
            }
            catch (Exception)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void GetForm_Test3()
        {
            try
            {
                int id = _formLogic.GetForm(100).FormID;
            }
            catch (Exception)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void DeleteForm_Test()
        {
            Assert.AreEqual(3, _formLogic.GetAllForms("1", "Title", 0, 5, "all").Count);
            _formLogic.DeleteForm(1);
            Assert.AreEqual(2, _formLogic.GetAllForms("1", "Title", 0, 5, "all").Count);
        }

        [Test]
        public void DeleteForm_Test2()
        {
            Assert.AreEqual(3, _formLogic.GetAllForms("1", "Title", 0, 5, "all").Count);
            try
            {
                _formLogic.DeleteForm(-1);
            }
            catch (Exception)
            {
                Assert.Pass();
            }

            Assert.AreEqual(3, _formLogic.GetAllForms("1", "Title", 0, 5, "all").Count);
        }

        [Test]
        public void Vote_Test()
        {
            List<VoteDTO> list = new List<VoteDTO>();
            VoteDTO vote = new VoteDTO() {Answer = 1, Question = 1};
            list.Add(vote);

            VoteListDTO voteList = new VoteListDTO() {Answers = list, Username = "user1"};

            Assert.AreEqual(100, _formLogic.GetAllForms("1", "Title", 0, 5, "all")[0].NrVotes);
            _formLogic.Vote(voteList,"1");
            Assert.AreEqual(101, _formLogic.GetAllForms("1", "Title", 0, 5, "all")[0].NrVotes);
        }


    }
}
