/* Copyright (C) Miron George - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Miron George <george.miron2003@gmail.com>, 2016
 */
using AzureDataAccess;
using DataTransferObject;
using Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BusinessLogic
{
    /// <summary>
    /// Logica gestionarii sondajelor
    /// </summary>
    public class FormLogic
    {
        private IAzureDataAccess _dataAccess;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="objDataAccess"></param>
        public FormLogic(IAzureDataAccess objDataAccess)
        {
            // Primesc obiectul, nu e treaba FormLogic ce dataAccess se foloseste
            // Unity are grija de dependency injection
            _dataAccess = objDataAccess;
        }

        /// <summary>
        /// Returnez toate formurile unui user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="page"></param>
        /// <param name="per_page"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public List<FormDTO> GetUserForms(string username, int page, int per_page, string state)
        {
            if (username == null)
                throw new MissingFieldException();
           
            int userID = _dataAccess.UserRepository.FindFirstBy(user => user.Username.Equals(username)).UserID;
            List<Form> formList;
            List<FormDTO> formDtoList = new List<FormDTO>();
            FormDTO formDTO;

            if (state == "all")
            {
                formList = _dataAccess.FormRepository.FindAllBy(form => form.UserID == userID).OrderByDescending(form => form.CreatedDate).ToList();
            }
            else
            {
                formList = _dataAccess.FormRepository.FindAllBy(form => form.UserID == userID && form.State == state ).OrderByDescending(form => form.CreatedDate).ToList();
            }

            formList = formList.Skip(page * per_page).Take(per_page).ToList();

            foreach (Form f in formList)
            {
                formDTO = new FormDTO();
                formDTO.Title = f.Title;
                formDTO.State = f.State;
                formDTO.CreatedDate = f.CreatedDate.ToString();
                formDTO.Deadline = f.Deadline.ToShortDateString();
                formDTO.Category = _dataAccess.CategoryRepository.FindFirstBy(category => category.CategoryID == f.CategoryID).Name;
                formDTO.Username = _dataAccess.UserRepository.FindFirstBy(user => user.UserID == f.UserID).Username;
                formDTO.Id = f.FormID;
                formDTO.NrVotes = f.NrVotes;
                formDtoList.Add(formDTO);
            }

            return formDtoList.ToList();
        }

        /// <summary>
        /// Returnarea rezultatul unui sondaj cu tot cu intrebari si raspunsuri
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public VoteResultDetailDTO GetDetailResultForm(int id)
        {
            VoteResultDetailDTO voteResult = new VoteResultDetailDTO();
            voteResult.Questions = new List<VoteQuestionResultDetailDTO>();
            voteResult.Title = _dataAccess.FormRepository.FindFirstBy(f => f.FormID == id).Title;

            List<Question> questionList = _dataAccess.QuestionRepository.FindAllBy(q => q.FormID == id).ToList();
            List<Answer> answerList;

            VoteQuestionResultDetailDTO questionDTO;
            VoteAnswerDetailResultDTO answerDTO;

            foreach (Question q in questionList)
            {
                questionDTO = new VoteQuestionResultDetailDTO();
                questionDTO.Answers = new List<VoteAnswerDetailResultDTO>();
                questionDTO.Question = q.Content;
                answerList = _dataAccess.AnswerRepository.FindAllBy(a => a.QuestionID == q.QuestionID).ToList();

                foreach (Answer a in answerList)
                {
                    answerDTO = new VoteAnswerDetailResultDTO();
                    answerDTO.Answer = a.Content;
                    answerDTO.AnswerNrVotes = Decimal.ToInt32(a.NrVotes);
                    questionDTO.Answers.Add(answerDTO);
                }

                voteResult.NrVotes = _dataAccess.FormRepository.FindFirstBy(f => f.FormID == q.FormID).NrVotes;
                voteResult.Questions.Add(questionDTO);
            }

            return voteResult;
        }

        /// <summary>
        /// Returnarea tuturor sondajelor care contin searchedName
        /// </summary>
        /// <param name="token"></param>
        /// <param name="searchedName"></param>
        /// <param name="page_nr"></param>
        /// <param name="per_page"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public List<FormDTO> GetAllForms(string token, string searchedName, int page_nr, int per_page, string state)
        {
            List<Form> formList;
            List<FormDTO> formDtoList = new List<FormDTO>();
            FormDTO formDTO;
            int userID;

            if (state == "all")
            {
                formList = _dataAccess.FormRepository.GetAll().Where(form => form.Category.Name.Contains(searchedName) ||
                form.User.Username.Contains(searchedName) || form.Title.Contains(searchedName)).OrderByDescending(form => form.CreatedDate).ToList();
            }
            else
            {
               formList = _dataAccess.FormRepository.GetAll().Where(form => (form.Category.Name.Contains(searchedName) ||
               form.User.Username.Contains(searchedName) || form.Title.Contains(searchedName)) && form.State.Equals(state)).OrderByDescending(form => form.CreatedDate).ToList();
            }

            formList = formList.Skip(page_nr * per_page).Take(per_page).ToList();

            userID = _dataAccess.TokenRepository.FindFirstBy(user => user.TokenString.Equals(token)).UserID;

            foreach (Form f in formList)
            {
                formDTO = new FormDTO();
                formDTO.Title = f.Title;
                formDTO.State = f.State;
                formDTO.CreatedDate = f.CreatedDate.ToString();
                formDTO.Deadline = f.Deadline.ToShortDateString();
                formDTO.Category = _dataAccess.CategoryRepository.FindFirstBy(category => category.CategoryID == f.CategoryID).Name;
                formDTO.Username = _dataAccess.UserRepository.FindFirstBy(user => user.UserID == f.UserID).Username;
                formDTO.Id = f.FormID;
                formDTO.NrVotes = f.NrVotes;
                formDTO.Voted = true;

                try
                {
                    userID = _dataAccess.VotedFormsRepository.FindFirstBy(voted => voted.FormID == f.FormID && voted.UserID == userID).UserID;
                }
                catch
                {
                    //daca userul a votat sondajul deja, nu il va mai putea vota
                    formDTO.Voted = false;
                }

                formDtoList.Add(formDTO);
            }

            return formDtoList.ToList();

        }

        /// <summary>
        /// Returnarea tuturor sondajelor dintr-o categorie
        /// </summary>
        /// <param name="categoryID"></param>
        /// <param name="token"></param>
        /// <param name="page"></param>
        /// <param name="per_page"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public List<FormDTO> GetCategoryForms(int categoryID, string token, int page, int per_page, string state)
        {
            List<Form> formList;
            List<FormDTO> formDtoList = new List<FormDTO>();
            FormDTO formDTO;
            int userID;

            if (state == "all")
            {
                formList = _dataAccess.FormRepository.FindAllBy(f => f.CategoryID == categoryID).OrderByDescending(f => f.CreatedDate).ToList();
            }
            else
            {
                formList = _dataAccess.FormRepository.FindAllBy(f => f.CategoryID == categoryID && f.State == state).OrderByDescending(f => f.CreatedDate).ToList();
            }

            formList = formList.Skip(page * per_page).Take(per_page).ToList();
            userID = _dataAccess.TokenRepository.FindFirstBy(user => user.TokenString.Equals(token)).UserID;

            foreach (Form f in formList)
            {
                formDTO = new FormDTO();
                formDTO.Title = f.Title;
                formDTO.State = f.State;
                formDTO.CreatedDate = f.CreatedDate.ToString();
                formDTO.Deadline = f.Deadline.ToShortDateString();
                formDTO.Category = _dataAccess.CategoryRepository.FindFirstBy(category => category.CategoryID == f.CategoryID).Name;
                formDTO.Username = _dataAccess.UserRepository.FindFirstBy(user => user.UserID == f.UserID).Username;
                formDTO.Id = f.FormID;
                formDTO.NrVotes = f.NrVotes;
                formDTO.Voted = true;

                try
                {
                    userID = _dataAccess.VotedFormsRepository.FindFirstBy(voted => voted.FormID == f.FormID && voted.UserID == userID).UserID;
                }
                catch
                {
                    //daca userul a votat sondajul deja, nu il va mai putea vota
                    formDTO.Voted = false;
                }

                formDtoList.Add(formDTO);
            }
            return formDtoList.ToList();
        }

        /// <summary>
        /// Returnarea tuturor sondajelor votate de catre un user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="page"></param>
        /// <param name="per_page"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public List<FormDTO> GetVotedForms(string username, int page, int per_page, string state)
        {
            int userID = _dataAccess.UserRepository.FindFirstBy(user => user.Username.Equals(username)).UserID;
            List<Form> formList = new List<Form>();
            List<VotedForm> votedFormsList;
            List<FormDTO> formDtoList = new List<FormDTO>();
            FormDTO formDTO;
            Form form;

            if (state == "all")
            {
                votedFormsList = _dataAccess.VotedFormsRepository.FindAllBy(voted => voted.UserID == userID).OrderByDescending(voted => voted.Form.CreatedDate).ToList();
            }
            else
            {
                votedFormsList = _dataAccess.VotedFormsRepository.FindAllBy(voted => voted.UserID == userID && voted.Form.State == state).OrderByDescending(voted => voted.Form.CreatedDate).ToList();
            }

            votedFormsList = votedFormsList.Skip(page * per_page).Take(per_page).ToList();

            foreach (VotedForm votedForm in votedFormsList)
            {
                form = _dataAccess.FormRepository.FindFirstBy(f => f.FormID == votedForm.FormID);
                formList.Add(form);
            }

            foreach (Form f in formList)
            {
                formDTO = new FormDTO();
                formDTO.Title = f.Title;
                formDTO.State = f.State;
                formDTO.CreatedDate = f.CreatedDate.ToString();
                formDTO.Deadline = f.Deadline.ToShortDateString();
                formDTO.Category = _dataAccess.CategoryRepository.FindFirstBy(category => category.CategoryID == f.CategoryID).Name;
                formDTO.Username = _dataAccess.UserRepository.FindFirstBy(user => user.UserID == f.UserID).Username;
                formDTO.Id = f.FormID;
                formDTO.NrVotes = f.NrVotes;
                formDTO.Voted = true;

                formDtoList.Add(formDTO);
            }

            return formDtoList.ToList();
        }

        /// <summary>
        /// Returnarea tuturor sondajelor
        /// </summary>
        /// <param name="token"></param>
        /// <param name="page"></param>
        /// <param name="per_page"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public List<FormDTO> GetAllForms(string token, int page, int per_page, string state)
        {
            List<Form> formList;
            FormDTO formDTO;
            List<FormDTO> formDtoList;
            int userID;

            if (state == "all")
            {
                formList = _dataAccess.FormRepository.GetAll().OrderByDescending(form => form.CreatedDate).ToList();
            }
            else
            {
                formList = _dataAccess.FormRepository.GetAll().Where(form => form.State == state).OrderByDescending(form => form.CreatedDate).ToList();
            }

            formList = formList.Skip(page * per_page).Take(per_page).ToList();
            formDtoList = new List<FormDTO>();

            userID = _dataAccess.TokenRepository.FindFirstBy(user => user.TokenString.Equals(token)).UserID;

            foreach (Form f in formList)
            {
                formDTO = new FormDTO();
                formDTO.Title = f.Title;
                formDTO.State = f.State;
                formDTO.CreatedDate = f.CreatedDate.ToString();
                formDTO.Deadline = f.Deadline.ToShortDateString();
                formDTO.Category = _dataAccess.CategoryRepository.FindFirstBy(category => category.CategoryID == f.CategoryID).Name;
                formDTO.Username = _dataAccess.UserRepository.FindFirstBy(user => user.UserID == f.UserID).Username;
                formDTO.Id = f.FormID;
                formDTO.NrVotes = f.NrVotes;
                formDTO.Voted = true;

                try
                {
                    userID = _dataAccess.VotedFormsRepository.FindFirstBy(voted => voted.FormID == f.FormID && voted.UserID == userID).UserID;
                }
                catch
                {
                    // Daca userul a votat sondajul deja sau sondajul este inchis, nu il va mai putea vota
                    if (formDTO.State != "closed")
                    {
                        formDTO.Voted = false;
                    }
                }

                formDtoList.Add(formDTO);
            }
            return formDtoList.ToList();
        }

        /// <summary>
        /// Returnarea continutului unui sondaj
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FormDetailDTO GetContentForm(int id)
        {
            FormDetailDTO formDTO = new FormDetailDTO();
            Form f = _dataAccess.FormRepository.FindFirstBy(form => form.FormID == id);

            formDTO.Title = f.Title;
            formDTO.CreatedDate = f.CreatedDate.ToString();
            formDTO.Deadline = f.Deadline.ToString();
            formDTO.State = f.State;
            formDTO.Category = GetCategory(f.CategoryID);
            formDTO.Questions = GetAllQuestionFromForm(id);
            formDTO.Username = GetUsername(f.UserID);
            formDTO.Id = f.FormID;
            formDTO.NrVotes = f.NrVotes;
            return formDTO;
        }

        /// <summary>
        /// Cautare nume userului dupa id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetUsername(int id)
        {
            return _dataAccess.UserRepository.FindFirstBy(user => user.UserID == id).Username;
        }

        /// <summary>
        /// Returnare lista intrebari care apartin unui sondaj
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<QuestionDTO> GetAllQuestionFromForm(int id)
        {
            List<QuestionDTO> questionDTOList = new List<QuestionDTO>();
            QuestionDTO questionDTO;
            List<Question> questionList = _dataAccess.QuestionRepository.FindAllBy(question => question.FormID == id).ToList();

            foreach (Question q in questionList)
            {
                questionDTO = new QuestionDTO();
                questionDTO.Question = q.Content;
                questionDTO.QuestionID = q.QuestionID;
                questionDTO.Answers = GetAllAnswerFromQuestion(q.QuestionID);

                questionDTOList.Add(questionDTO);
            }
            return questionDTOList;
        }

        /// <summary>
        /// Returnare lista raspunsuri care apartin unei intrebari
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<AnswerDTO> GetAllAnswerFromQuestion(int id)
        {
            List<AnswerDTO> answerDTOList = new List<AnswerDTO>();
            AnswerDTO answerDTO;
            List<Answer> answerList = _dataAccess.AnswerRepository.FindAllBy(answer => answer.QuestionID == id).ToList();

            foreach (Answer a in answerList)
            {
                answerDTO = new AnswerDTO();
                answerDTO.Answer = a.Content;
                answerDTO.AnswerID = a.AnswerID;
                answerDTOList.Add(answerDTO);
            }
            return answerDTOList;
        }

        /// <summary>
        /// Cautare nume categorie dupa id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetCategory(int id)
        {
            return _dataAccess.CategoryRepository.FindFirstBy(cat => cat.CategoryID == id).Name;
        }

        /// <summary>
        /// Cautare id categorie dupa nume
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int GetCategoryID(string name)
        {
            return _dataAccess.CategoryRepository.FindFirstBy(cat => cat.Name.Equals(name)).CategoryID;
        }

        /// <summary>
        /// Cautare id intrebare
        /// </summary>
        /// <param name="content"></param>
        /// <param name="formID"></param>
        /// <returns></returns>
        public int GetQuestionID(string content, int formID)
        {
            return _dataAccess.QuestionRepository.FindFirstBy(q => q.Content.Equals(content) && q.FormID == formID).QuestionID;
        }

        /// <summary>
        /// Adaugare categorie
        /// </summary>
        /// <param name="name"></param>
        public void AddCategory(string name)
        {
            Category category = new Category();
            category.Name = name;
            _dataAccess.CategoryRepository.Add(category);
        }

        /// <summary>
        /// Adaugare sondaj
        /// </summary>
        /// <param name="formDTO"></param>
        public void AddForm(FormDetailDTO formDTO)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            // string format = "ddd MMM d HH:mm:ss GMT";
            if (string.IsNullOrWhiteSpace(formDTO.Category) || string.IsNullOrWhiteSpace(formDTO.Username) || formDTO.Questions.Capacity <= 1)
                throw new MissingMemberException("Values cannot be null");

            Form form = new Form() { CreatedDate = DateTime.Now, Deadline = DateTime.Now.AddDays(7 * Int32.Parse(formDTO.Deadline)), State = formDTO.State, Title = formDTO.Title };
            Question q;
            Answer a;
            form.CategoryID = GetCategoryID(formDTO.Category);
            form.UserID = GetUserID(formDTO.Username);
            form.NrVotes = 0;

            _dataAccess.FormRepository.Add(form);

            // Adaugare intrebare
            int formID = GetFormID(form.Title, form.UserID, form.CreatedDate); //Preiau id-ul formului
            int questionID;
            foreach (QuestionDTO questionDTO in formDTO.Questions)
            {
                q = new Question();
                q.FormID = form.FormID;
                q.Content = questionDTO.Question;
               
                _dataAccess.QuestionRepository.Add(q);

                // Adaugare raspuns
                questionID = GetQuestionID(q.Content, formID);
                foreach (AnswerDTO answerDTO in questionDTO.Answers)
                {
                    a = new Answer();
                    a.Content = answerDTO.Answer;
                    a.NrVotes = 0;
                    a.QuestionID = questionID;

                    _dataAccess.AnswerRepository.Add(a);
                }
            }
        }
        /// <summary>
        /// Returnare sondaj dupa id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Form GetForm(int id)
        {
            return _dataAccess.FormRepository.FindFirstBy(form => form.FormID == id);
        }

        /// <summary>
        /// Stergere sondaj
        /// </summary>
        /// <param name="id"></param>
        public void DeleteForm(int id)
        {
            Form f = _dataAccess.FormRepository.FindFirstBy(form => form.FormID == id);
            _dataAccess.FormRepository.Delete(f);
        }

        /// <summary>
        /// Returnare id sondaj
        /// </summary>
        /// <param name="title"></param>
        /// <param name="userID"></param>
        /// <param name="createdDate"></param>
        /// <returns></returns>
        public int GetFormID(string title, int userID, DateTime createdDate)
        {
            DateTime date = createdDate.AddSeconds(-5);
           
            return _dataAccess.FormRepository.FindFirstBy(form => form.Title.Equals(title) && form.UserID == userID && form.CreatedDate >= date).FormID;
        }

        /// <summary>
        ///  Returnare id user
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public int GetUserID(string username)
        {
            return _dataAccess.UserRepository.FindFirstBy(user => user.Username.Equals(username)).UserID;
        }

        /// <summary>
        /// Returnare id sondaj daca acesta apartine userului care are tokenul usertToken
        /// </summary>
        /// <param name="formID"></param>
        /// <param name="userToken"></param>
        /// <returns></returns>
        public int FormIdCreatedbyUserId(int formID, string userToken)
        {
            int userID = _dataAccess.TokenRepository.FindFirstBy(token => token.TokenString.Equals(userToken)).UserID;
            return _dataAccess.FormRepository.FindFirstBy(form => form.FormID == formID && form.UserID == userID).FormID;
        }

        /// <summary>
        /// Votare sondaj
        /// </summary>
        /// <param name="voteListDTO"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public VoteResultDTO Vote(VoteListDTO voteListDTO, string token)
        {
            int userID = _dataAccess.UserRepository.FindFirstBy(user => user.Username.Equals(voteListDTO.Username)).UserID;
            int questionID, formID;

            // Testeaza daca tokenul si userul care a votat coincid
            if (userID == _dataAccess.TokenRepository.FindFirstBy(user => user.TokenString.Equals(token)).UserID)
            {
               
                questionID = voteListDTO.Answers[0].Question;

                formID = _dataAccess.QuestionRepository.FindFirstBy(question => question.QuestionID == questionID).FormID;

                if (
                    _dataAccess.VotedFormsRepository.FindAllBy(
                        votedForm => votedForm.FormID == formID && votedForm.UserID == userID).ToList().Count > 0)
                {
                    throw new Exception("Poll already voted");
                }

                if (_dataAccess.FormRepository.FindFirstBy((form => form.FormID == formID)).State == "closed")
                {
                    throw new Exception("Closed polls cannot be voted");
                }
              
                // Incrementez numarul de voturi pentru fiecare intrebare si raspuns
                foreach (VoteDTO voteDTO in voteListDTO.Answers)
                {
                    _dataAccess.AnswerRepository.AddVote(voteDTO.Answer);
                }
                _dataAccess.FormRepository.AddVote(formID);

                //preiau Rezultatele din baza de date pentru fiecare intrebare si raspuns
                VoteResultDTO voteResult = new VoteResultDTO();
                voteResult.Questions = new List<VoteQuestionResultDTO>();

                VoteQuestionResultDTO voteQuestionResult;
                VoteAnswerResultDTO voteAnswerResult;
                List<Answer> listAnswer;

                foreach (VoteDTO voteDTO in voteListDTO.Answers)
                {
                    listAnswer = _dataAccess.AnswerRepository.FindAllBy(answer => answer.QuestionID == voteDTO.Question).ToList();
                    voteQuestionResult = new VoteQuestionResultDTO();

                    // Id-ul intrebare
                    voteQuestionResult.QuestionID = voteDTO.Question;
                    voteQuestionResult.Answers = new List<VoteAnswerResultDTO>();

                    foreach (Answer a in listAnswer)
                    {
                        voteAnswerResult = new VoteAnswerResultDTO();
                        voteAnswerResult.AnswerID = a.AnswerID;
                        voteAnswerResult.AnswerNrVotes = Decimal.ToInt32(_dataAccess.AnswerRepository.FindFirstBy(answer => answer.AnswerID == a.AnswerID).NrVotes);
                        voteQuestionResult.Answers.Add(voteAnswerResult);
                    }
                    voteResult.NrVotes= Decimal.ToInt32(_dataAccess.FormRepository.FindFirstBy(form => form.FormID == formID).NrVotes);
                    voteResult.Questions.Add(voteQuestionResult);
                }

                VotedForm voted = new VotedForm();
                voted.UserID = userID;
                voted.FormID = formID;

                // Adauga sondajul in lista sondajelor votate de userul respectiv
                _dataAccess.VotedFormsRepository.Add(voted);
                return voteResult;
            }
            else throw new Exception("Something bad happened");
        }
    }
}

