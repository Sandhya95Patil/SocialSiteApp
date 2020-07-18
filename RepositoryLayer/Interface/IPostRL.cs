﻿using CloudinaryDotNet.Actions;
using CommonLayer.Model;
using CommonLayer.Response;
using CommonLayer.Show;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace RepositoryLayer.Interface
{
    public interface IPostRL
    {
        Task<PostModel> AddPost(IFormFile file, int userId, string text, string siteUrl);

        Task<string> DeletePost(int userId, int postId);
        IList<PostModel> GetAllPosts(int userId);

        IList<PostModel> GetAllPostsWithComments(int userId);

        Task<LikesModel> Like(LikeShowModel likeShowModel, int likeById, int postId);

        IList<LikesModel> LikesForPost(int userId, int postId);

        Task<CommentResponseModel> AddComment(CommentShowModel commentShowModel, int commentById, int postId);
        IList<CommentsModel> GetAllComments(int userId, int postId);
    
        Task<ShareModel> SharePost(int shareById, int postId);

        IList<ShareModel> NumberOfShares(int userId, int postId);
    }
}
