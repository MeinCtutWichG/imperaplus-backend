﻿using System;

namespace ImperaPlus.Domain.Tournaments
{
    public class TournamentParticipant
    {
        protected TournamentParticipant()
        {
            this.Id = Guid.NewGuid();
        }

        public TournamentParticipant(TournamentTeam team, User user)
            : this()
        {
            this.TeamId = team.Id;
            this.Team = team;

            this.UserId = user.Id;
            this.User = user;
        }

        public Guid Id { get; set; }
        
        public string UserId { get; protected set; }
        public User User { get; protected set; }

        public Guid TeamId { get; protected set; }
        public TournamentTeam Team { get; protected set; }
    }
}
