﻿using SAM.Taskboard.DataProvider;
using SAM.Taskboard.DataProvider.Models;
using SAM.Taskboard.Logic.Utility;
using SAM.Taskboard.Model;
using SAM.Taskboard.Model.Project;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAM.Taskboard.Logic.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly int projectsPageSize = 9;
        private readonly int boardsPageSize = 9;
        public ProjectService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public ProjectsViewModel GetProjects(string userId, int currentPage)
        {
            try
            {
                var result = unitOfWork.ProjectUser.Get(
                    projectsPageSize,
                    (currentPage - 1) * projectsPageSize,
                    x => x.Id,
                    u => u.UserId == userId
                    ).ToList();

                List<ProjectInfo> projects = new List<ProjectInfo>();

                foreach (var projectUser in result)
                {
                    int projectId = projectUser.ProjectId;
                    Project project = unitOfWork.Projects.Get(projectId);
                    string projectAbout = project.About;
                    string projectTitle = project.Title;
                    projects.Add(new ProjectInfo { Id = projectId, About = projectAbout, ProjectName = projectTitle });
                }

                int rowsCount = unitOfWork.ProjectUser.Count(u => u.UserId == userId);
                ProjectsViewModel projectsViewModel = new ProjectsViewModel { Projects = projects, CurrentPage = currentPage, PageSize = 9, RowsCount = rowsCount };
                projectsViewModel.GetPages();

                return projectsViewModel;
            }

            //TODO: Error handling
            catch (SystemException e)
            {
                return null;
            }
        }
        public GenericServiceResult CreateNewProject(string userId, CreateProjectViewModel model)
        {
            try
            {
                ProjectSettings projectSettings = new ProjectSettings
                {
                    AccessToChangeProject = (int)model.AccessToChangeProject,
                    AccessToDeleteBoard = (int)model.AccessToDeleteBoard,
                    AccessToCreateBoard = (int)model.AccessToCreateBoard
                };

                Project project = new Project { Title = model.Title, About = model.About, Settings = projectSettings };
                User user = unitOfWork.UserManager.Users.FirstOrDefault(x => x.Id == userId);

                ProjectUser projectUser = new ProjectUser { Project = project, Role = (int)CustomRoles.Administrator, User = user };
                unitOfWork.ProjectUser.Create(projectUser);

                return GenericServiceResult.Success;
            }

            catch
            {
                return GenericServiceResult.Error;
            }
        }
        public ProjectViewModel GetBoards(string userId, int projectId, int currentPage)
        {
            try
            {
                var result = unitOfWork.BoardUser.Get(
                    boardsPageSize,
                    (currentPage - 1) * boardsPageSize,
                    x => x.Id,
                    u => u.UserId == userId && u.Board.ProjectId == projectId
                    ).ToList();

                List<BoardInfo> boards = new List<BoardInfo>();

                int roleNumber = unitOfWork.ProjectUser.GetFirstOrDefaultWhere(x => x.UserId == userId).Role;
                CustomRoles userProjectRole = (CustomRoles)roleNumber;

                foreach (var boardUser in result)
                {
                    int boardId = boardUser.BoardId;
                    string boardTitle = unitOfWork.Boards.Get(boardId).Title;
                    boards.Add(new BoardInfo { Id = boardId, Title = boardTitle });
                }

                ProjectViewModel projectViewModel = new ProjectViewModel { Boards = boards };

                return projectViewModel;
            }

            //TODO: Error handling
            catch (SystemException e)
            {
                return null;
            }
        }

        //TODO: Activities
        public void GetRecentActivities()
        {
        }
    }
}
