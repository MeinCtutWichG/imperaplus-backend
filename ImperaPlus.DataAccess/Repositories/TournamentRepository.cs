﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Domain.Tournaments;

namespace ImperaPlus.DataAccess.Repositories
{
    public class TournamentRepository : GenericRepository<Tournament>, ITournamentRepository
    {
        public TournamentRepository(DbContext context) 
            : base(context)
        {
        }

        public Tournament GetById(Guid id)
        {
            return this.Set.First(x => x.Id == id);
        }

        public IEnumerable<Tournament> Get(params TournamentState[] states)
        {
            if (states == null || states.Length == 0)
            {
                states = new[] { TournamentState.Open, TournamentState.Groups, TournamentState.Knockout, TournamentState.Closed };
            }

            return this.Set.Where(x => states.Contains(x.State));
        }

        public bool ExistsWithName(string name)
        {
            return this.DbSet.Any(x => x.Name == name);
        }

        public IEnumerable<Tournament> GetAllFull()
        {
            return this.Set;
        }

        private IQueryable<Tournament> Set
        {
            get
            {
                // Include Games/Teams/Players so we can synchronize
                return this.DbSet
                    .Include(x => x.Teams)
                        .ThenInclude(t => t.Participants)
                        .ThenInclude(p => p.User)
                    .Include(x => x.Pairings)
                        .ThenInclude(p => p.Games)
                        .ThenInclude(g => g.Teams)
                        .ThenInclude(t => t.Players)
                    .Include(x => x.Groups)
                    .Include(x => x.Options);
            }
        }
    }
}
