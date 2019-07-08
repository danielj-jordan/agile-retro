using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Retrospective.Data;
using Retrospective.Data.Model;

namespace demodata
{
  public class DemoData
  {
    Database database;
    ILogger<DemoData> logger;

    public DemoData(Database database, ILogger<DemoData> logger)
    {
      this.database = database;
      this.logger = logger;
    }

    public void Initialize()
    {
      User demoUser = this.FixUser("demo@localhost");
      var team = this.FixTeam(demoUser);
      this.FixMeetings(team.Id.ToString(), demoUser);
    }

    private User FixUser(string owner)
    {
      //is there a user for this email address
      var users = this.database.Users.FindUserByEmail(owner);

      if (users.Count == 0)
      {
        this.logger.LogInformation("User: {0} has not been found", owner);
        //need to create the user
        User newUser = new User();
        newUser.Email = owner;
        newUser.IsDemoUser = true;
        newUser.Name = "Demo User";
        this.logger.LogInformation("Creating the a new user record for {0}", owner);
        return this.database.Users.Save(newUser);
      }
      this.logger.LogInformation("Found a record for user {0}", owner);
      return users.First();
    }

    private Retrospective.Data.Model.Team FixTeam(User user)
    {
      var teams = this.database.Teams.GetOwnedTeams((ObjectId)user.Id);
      this.logger.LogInformation("Found {0} teams for the userid:{1} {2}", teams.Count, user.Id.ToString(), user.Name);

      if (teams.Count == 0)
      {
        //need to create a team
        Team newTeam = new Team();
        newTeam.Name = "Demo Dev Team";
        newTeam.Members = new TeamMember[1]
        {
            new TeamMember()
            {
                UserId=(ObjectId)user.Id,
                Role= TeamRole.Owner,
                StartDate=DateTime.UtcNow
            }
        };
        this.database.Teams.Save(newTeam);
        this.logger.LogInformation("Created a new team with id: {0}", newTeam.Id.ToString());
        return newTeam;
      }
      return teams.First();
    }

    private void FixMeetings(string teamid, User user)
    {
      var meetings = this.database.Meetings.GetMeetings(teamid);
      this.logger.LogInformation("Found {0} meetngs for the teamid: {1}", meetings.Count, teamid);

      //delete all existing meetings
      foreach (var meeting in meetings)
      {
        this.logger.LogInformation("Deleting comments for meeting id: {0}", meeting.Id.ToString());
        this.DeleteComments(meeting);
      }

      if (meetings.Count == 0)
      {
        this.logger.LogInformation("Creating a new meeting.");
        Meeting meeting = new Meeting();
        meeting.TeamId = new MongoDB.Bson.ObjectId(teamid);

        this.FixMeeting(meeting);
        this.CreateComments(meeting, user);
      }
      else
      {
        this.FixMeeting(meetings.First());
        this.CreateComments(meetings.First(), user);
      }
    }

    private Meeting FixMeeting(Retrospective.Data.Model.Meeting meeting)
    {
      meeting.Name = "Sprint 1 Retrospective";
      meeting.Categories = new Retrospective.Data.Model.Category[]
      {
        new Retrospective.Data.Model.Category(1, "What went well?"),
        new Retrospective.Data.Model.Category(2, "What did we learn?"),
        new Retrospective.Data.Model.Category(3, "What do we want to do differently?")
      };

      this.database.Meetings.Save(meeting);
      this.logger.LogInformation("Saved meeting id: {0}", meeting.Id);
      return meeting;
    }

    private void DeleteComments(Meeting meeting)
    {
      var comments = this.database.Comments.GetComments((ObjectId)meeting.Id);
      foreach (var comment in comments)
      {
        this.database.Comments.Delete((ObjectId)comment.Id);
      }
    }

    private void CreateComments(Meeting meeting, User user)
    {
      CreateComment((ObjectId)meeting.Id, 1, "Completed our first deployment to production", user);
      CreateComment((ObjectId)meeting.Id, 1, "Starting grooming the next set of epics", user);
      CreateComment((ObjectId)meeting.Id, 1, "We quickly addressed all the critical defects that were required for prod", user);

      CreateComment((ObjectId)meeting.Id, 2, "The e2e tests don't have enough coverage for login", user);
      CreateComment((ObjectId)meeting.Id, 2, "The deployment automation is a pattern to keep", user);

      CreateComment((ObjectId)meeting.Id, 3, "We need better acceptance critria on the user stories", user);
    }

    private Comment CreateComment(ObjectId meetingid, int category, string text, User user)
    {
      Comment comment = new Comment();
      comment.MeetingId = meetingid;
      comment.LastUpdateUserId = (ObjectId)user.Id;
      comment.LastUpdateDate = DateTime.UtcNow;
      comment.CategoryNumber = category;
      comment.Text = text;

      this.database.Comments.SaveComment(comment);
      this.logger.LogInformation("Saved comment id: {0} in category: {1} for meetingid: {2}", comment.Id.ToString(), category, meetingid);
      return comment;
    }
  }
}