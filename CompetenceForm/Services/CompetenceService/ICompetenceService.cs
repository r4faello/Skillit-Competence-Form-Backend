﻿using CompetenceForm.Common;
using CompetenceForm.DTOs;
using CompetenceForm.Models;

namespace CompetenceForm.Services.CompetenceService
{
    public interface ICompetenceService
    {
        public Task<(Result, CompetenceSetDto?)> SpitCompetenceSet();
        public Task<Result> SaveAnsweredQuestion(User user, string competenceSetId, string competenceId, string answerId);
    }
}
