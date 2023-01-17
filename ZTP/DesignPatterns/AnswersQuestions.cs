﻿using ZTP.DesignPatterns.Iterator;
using ZTP.Models;
using ZTP.Models.ModelView;
using ZTP.State;

namespace ZTP.DesignPatterns
{
    public class AnswersQuestions
    {
        private readonly ZTPDbContext _context;
        private int _userId;

        public Context ContextState;
        public IteratorQuestion Iterator { get; set; }

        public AnswersQuestions(ZTPDbContext context, int userId)
        {
            _context = context;
            _userId = userId;
        }

        public void GenerateQuestions(int number)
        {
            List<QuestionViewModel> Questions = new List<QuestionViewModel>();

            for (int i = 0; i < number; i++)
            {
                QuestionViewModel question = GetQuestionFromDB();
                question.QuestionNumber = i + 1;
                Questions.Add(question);
            }

            Iterator = new IteratorQuestion(Questions);

            if (ContextState.CheckState() is LearningState)
            {
                Remove(Questions);
            }
            else
            {
                Update(Questions);
            }
        }

        public QuestionViewModel GetQuestion()
        {
            if (!Iterator.IsDone())
            {
                QuestionViewModel currentQuestion = Iterator.Next();

                return currentQuestion;
            }

            return null;
        }

        private QuestionViewModel GetQuestionFromDB()
        {
            Answers answers = new Answers(_context, _userId, ContextState);

            QuestionViewModel questionViewModel = new QuestionViewModel();
            questionViewModel.Answers = answers.GetAnswersList();
            questionViewModel.CorrectWord = answers.CorrectAnswer;

            return questionViewModel;
        }

        private void Remove(List<QuestionViewModel> questions)
        {
            foreach (var question in questions)
            {
                UserWord userWord = _context.UserWords.Where(x => x.UserId == _userId && x.WordId == question.CorrectWord.Id).FirstOrDefault();
                _context.Remove(userWord);
            }

            _context.SaveChanges();
        }

        private void Update(List<QuestionViewModel> questions)
        {
            foreach (var question in questions)
            {
                UserWord userWord = _context.UserWords.Where(x => x.UserId == _userId && x.WordId == question.CorrectWord.Id).FirstOrDefault();
                userWord.IsLearned = false;
                _context.Update(userWord);
            }

            _context.SaveChanges();
        }
    }
}