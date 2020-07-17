﻿using CommonLayer.Model;
using CommonLayer.Show;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using RepositoryLayer.PostImage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class PostRL : IPostRL
    {
        private readonly AppDBContext appDBContext;
        IConfiguration configuration;

        public PostRL(AppDBContext appDBContext, IConfiguration configuration)
        {
            this.appDBContext = appDBContext;
            this.configuration = configuration;
        }
        public async Task<PostModel> AddPost(IFormFile file, int userId)
        {
            try
            {
                var checkEmailId = this.appDBContext.Registrations.FirstOrDefault(g => g.Id==userId);
                if (checkEmailId != null)
                {
                    ImageUpload imageUpload = new ImageUpload(this.configuration, file);
                    var imageUrl = imageUpload.Upload(file);

                    var postDetails = new PostModel()
                    {
                       UserId=userId,
                       Post= imageUrl,
                       CreatedDate=DateTime.Now

                    };
                    this.appDBContext.Posts.Add(postDetails);
                    var result = await this.appDBContext.SaveChangesAsync();
                    if (result > 0)
                    {
                        var response = new PostModel()
                        {
                            Id = postDetails.Id,
                            UserId=postDetails.UserId,
                            Post=postDetails.Post,
                            CreatedDate=postDetails.CreatedDate
                        };
                        return response;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }
    }
}
