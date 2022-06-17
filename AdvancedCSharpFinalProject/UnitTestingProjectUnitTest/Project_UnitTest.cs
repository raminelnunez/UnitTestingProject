using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AdvancedCSharpFinalProject.Data.BLL;
using AdvancedCSharpFinalProject.Data.DAL;
using AdvancedCSharpFinalProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace UnitTestingProjectUnitTest
{
    [TestClass]
    public class Project_UnitTest
    {
        private ProjectBusinessLogic _projectBLL;
        private List<Project> allProjects;
        private List<ProjectTask> ProjectTasks;
        private List<Project> pmProjects;
        private List<ApplicationUser> allUsers;


        [TestInitialize]
        public void Initialize()
        {

            // Creating MOCK Repo (based on DAL)
            Mock<ProjectRepository> mockRepo = new Mock<ProjectRepository>();
            _projectBLL = new ProjectBusinessLogic(mockRepo.Object);


            //// Creating MOCK Users

            ApplicationUser mockUser = new ApplicationUser
            {
                Id = "mockGUID",
                Email = "mockDev@mitt.ca",
                NormalizedEmail = "MOCKDEV@MITT.CA",
                UserName = "mockDev@mitt.ca",
                NormalizedUserName = "MOCKDEV@MITT.CA",
                EmailConfirmed = true,
                //
                DailySalary = 120,
                IsDeveloper = true,
                IsProjectManager = false,

            };
            ApplicationUser mockUser2 = new ApplicationUser
            {
                Id = "mockGUID2",
                Email = "mockDev2@mitt.ca",
                NormalizedEmail = "MOCKDEV2@MITT.CA",
                UserName = "mockDev2@mitt.ca",
                NormalizedUserName = "MOCKDEV2@MITT.CA",
                EmailConfirmed = true,
                //
                DailySalary = 135,
                IsDeveloper = true,
                IsProjectManager = false,

            };

            ApplicationUser mockUser3 = new ApplicationUser
            {
                Id = "mockGUID3",
                Email = "mockDev3@mitt.ca",
                NormalizedEmail = "MOCKDEV3@MITT.CA",
                UserName = "mockDev3@mitt.ca",
                NormalizedUserName = "MOCKDEV3@MITT.CA",
                EmailConfirmed = true,
                //
                DailySalary = 155,
                IsDeveloper = true,
                IsProjectManager = false,

            };
            ApplicationUser mockProjectManager = new ApplicationUser
            {
                Id = "mockProjectManager",
                Email = "mockPM@mitt.ca",
                NormalizedEmail = "MOCKPM@MITT.CA",
                UserName = "mockPM@mitt.ca",
                NormalizedUserName = "MOCKPM@MITT.CA",
                EmailConfirmed = true,
                //
                DailySalary = 225,
                IsDeveloper = true,
                IsProjectManager = false,
            };

            ApplicationUser mockProjectManager2 = new ApplicationUser
            {
                Id = "mockProjectManager2",
                Email = "mockPM2@mitt.ca",
                NormalizedEmail = "MOCKPM2@MITT.CA",
                UserName = "mockPM2@mitt.ca",
                NormalizedUserName = "MOCKPM2@MITT.CA",
                EmailConfirmed = true,
                //
                DailySalary = 305,
                IsDeveloper = true,
                IsProjectManager = false,
            };

            allUsers = new List<ApplicationUser> { mockUser, mockUser2 };


            // Creating MOCK Projects - adding a Users List
            Project mockProject1 = new Project {
                Id = 1,
                Title = "The First Project",
                Priority = Priority.Medium,
                Deadline = new DateTime(2022, 06, 24),
                AssignedBudget = (double)12300,
                ProjectManager = mockProjectManager,
            };

            Project mockProject2 = new Project
            {
                Id = 2,
                Title = "The Second Project",
                Priority = Priority.High,
                Deadline = new DateTime(2022, 07, 13),
                AssignedBudget = (double)34500,
                ProjectManager = mockProjectManager,
            };

            Project mockProject3 = new Project
            {
                Id = 3,
                Title = "The Third Project",
                Priority = Priority.Low,
                Deadline = new DateTime(2022, 06, 21),
                AssignedBudget = (double)5540,
                ProjectManager = mockProjectManager2,
            };

            allProjects = new List<Project> { mockProject1, mockProject2, mockProject3 };
            allUsers = new List<ApplicationUser> { mockUser, mockUser2, mockUser3, mockProjectManager, mockProjectManager2};

            mockRepo.Setup(repo => repo.Get(It.Is<int>(i => i == 1))).Returns(mockProject1);
            mockRepo.Setup(repo => repo.Get(It.Is<int>(i => i == 2))).Returns(mockProject2);
            mockRepo.Setup(repo => repo.Get(It.Is<int>(i => i == 3))).Returns(mockProject3);
            mockRepo.Setup(repo => repo.GetAll()).Returns(allProjects);
            mockRepo.Setup(repo => repo.GetList(It.IsAny<Func<Project, bool>>())).Returns(pmProjects);
            mockRepo.Setup(repo => repo.Add(It.IsAny<Project>())).Callback<Project>((project) => allProjects.Add(project));
            mockRepo.Setup(repo => repo.Remove(It.IsAny<Project>())).Callback<Project>((project) => allProjects.Remove(project));

        }

        // TESTING - Arrange - Act - Assert
        // GetProjectById
        [TestMethod]
        public void GetProjectById_Passes_WithCorrectParametersPassed()
        {
            var realProject = _projectBLL.GetProjectById(1);
            var mockProject = allProjects.ToList()[0];

            Assert.AreEqual(realProject, mockProject);
        }

        [TestMethod]
        public void GetProjectById_Fails_WithIncorrectIdPassed()
        {
            var realProject = _projectBLL.GetProjectById(1);
            var mockProject = allProjects.ToList()[2];

            Assert.AreNotEqual(realProject, mockProject); ;
        }

        [TestMethod]
        public void GetProjectById_ThrowsErrorOn_NonExistingProjectId()
        {
            Assert.ThrowsException<Exception>(() => _projectBLL.GetProjectById(10));
        }

        [TestMethod]
        public void GetProjectById_ThrowsErrorOn_NoIdPassedIn()
        {
            Assert.ThrowsException<Exception>(() => _projectBLL.GetProjectById('x'));
        }


        // GetAllProjects
        [TestMethod]
        public void GetAllProjectsList_Passes_WithCorrectParametersPassed()
        {
            var AllRealProjects = _projectBLL.GetAllProjects().ToList();
            var AllMockProjects = allProjects.ToList();

            CollectionAssert.AreEqual(AllMockProjects, AllRealProjects);
        }

        [TestMethod]
        public void GetCurrentProjects_Passes_WithCorrectParametersPassed()
        {
            var pm = allUsers[1];

            var ProjectList = _projectBLL.GetCurrentProjects(pm);

            List<Project> expectedAssignedProjects = new List<Project> { allProjects[0], allProjects[1] };

            Assert.AreEqual(expectedAssignedProjects, ProjectList);
        }


        [TestMethod]
        public void CreateProject_Passes_WithCorrectParametersPassed()
        {
            _projectBLL.CreateProject(allUsers[3], "testing new project creation", 1000, Priority.Medium, new DateTime(2022, 08, 05));
            _projectBLL.CreateProject(allUsers[4], "another project creation", 2000, Priority.High, new DateTime(2022, 09, 25));

            var result = allProjects[3];
            var result2 = allProjects[4];

            CollectionAssert.Contains(allProjects, result);
            CollectionAssert.Contains(allProjects, result2);
        }


        [TestMethod]
        public void EditProject_Passes_WithCorrectParametersPassed()
        {
            Project mockProject1 = allProjects[0];

            _projectBLL.EditProject(mockProject1.Id, "Changed Title", 777, Priority.High, new DateTime(2032, 08, 21));
            var editedProject = allProjects[0];

            Assert.AreNotEqual(mockProject1, editedProject);
        }

        [TestMethod]
        public void EditProject_ThrowsErrorOn_WithNonExistingProjectId()
        {
            Assert.ThrowsException<Exception>(() => _projectBLL.EditProject(2355, "Changed Title", 777, Priority.High, new DateTime(2032, 08, 21)));
        }


    }
}