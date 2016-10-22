﻿using Autofac;
using Hangfire;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Domain.Bots;
using ImperaPlus.Domain.Services;

namespace ImperaPlus.Application.Jobs
{
    [Queue(JobQueues.Normal)]
    [AutomaticRetry(Attempts = 0)]
    public class BotJob : Job
    {
        private IUnitOfWork unitOfWork;
       
        public BotJob(ILifetimeScope scope)
            : base(scope)
        {
            this.unitOfWork = this.LifetimeScope.Resolve<IUnitOfWork>();
        }

        public void Play(long gameId)
        {
            var game = this.unitOfWork.Games.Find(gameId);
            if (game.State != Domain.Enums.GameState.Active)
            {
                return;
            }

            var mapTemplateProvider = this.LifetimeScope.Resolve<IMapTemplateProvider>();
            var mapTemplate = mapTemplateProvider.GetTemplate(game.MapTemplateName);
            var attackService = this.LifetimeScope.Resolve<IAttackService>();
            var randomGen = this.LifetimeScope.Resolve<IRandomGen>();

            var bot = new Bot(game, mapTemplate, attackService, randomGen);

            bot.PlayTurn();

            this.unitOfWork.Commit();
        }
    }
}

