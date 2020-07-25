using CommonLayer.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Context
{
    public class AppDBContext:DbContext
    {
        public AppDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        }

        public DbSet<RegistrationModel> Registrations { get; set; }
        public DbSet<PostModel> Posts { get; set; }

        public DbSet<LikesModel> Likes { get; set; }

        public DbSet<CommentsModel> Comments { get; set; }

        public DbSet<ShareModel> Share { get; set; }
        public DbSet<AddFreindModel> AddFriends { get; set; }
    }
}
