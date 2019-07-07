using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using Retrospective.Data;
using Retrospective.Data.Model;

namespace demodata
{
  public class DemoData
  {
    Database database;
    string owner = "demo@localhost";

    public DemoData(Database database)
    {
      this.database = database;
    }

    public void Initialize()
    {
      this.FixUser(this.owner);
      var team = this.FixTeam(owner);
      this.FixMeetings(team.Id.ToString());
    }

    private void FixUser(string owner)
    {
      //is there a user for this email address
      var users = this.database.Users.FindUserByEmail(owner);

      if (users.Count == 0)
      {
        //need to create the user
        User newUser = new User();
        newUser.Email = owner;
        newUser.IsDemoUser = true;
        newUser.Name = "Demo User";

        this.database.Users.Save(newUser);
      }

    }

    private Retrospective.Data.Model.Team FixTeam(string owner)
    {
      var teams = this.database.Teams.GetOwnedTeams(owner);
      if (teams.Count == 0)
      {
        //need to create a team
        Team newTeam = new Team();
        newTeam.Owner = owner;
        newTeam.Name = "Demo Dev Team";
        newTeam.Members = new TeamMember[1]
        {
            new TeamMember()
            {
                UserName=owner,
                Role="manager"
            }
        };
        this.database.Teams.Save(newTeam);
        return newTeam;
      }
      else
      {
        return teams.First();
      }
    }

    private void FixMeetings(string teamid)
    {
      var meetings = this.database.Meetings.GetMeetings(teamid);

      if (meetings.Count == 0)
      {
        Meeting meeting = new Meeting();
        meeting.TeamId = new MongoDB.Bson.ObjectId(teamid);

        this.FixMeeting(meeting);
        this.database.Meetings.Save(meeting);
        this.CreateComments(meeting.Id.ToString());
      }

      if (meetings.Count > 0)
      {
        for (int i = 0; i < meetings.Count; i++)
        {
          if (i == 0)
          {
            this.FixMeeting(meetings[i]);
            this.DeleteComments(meetings[i].Id.ToString());
            this.CreateComments(meetings[i].Id.ToString());
          }
          else
          {
            this.DeleteComments(meetings[i].Id.ToString());
          }
        }
      }
    }

    private Meeting FixMeeting(Retrospective.Data.Model.Meeting meeting)
    {
      meeting.Name = "Sprint 1 Retrospective";
      meeting.Categories = new Retrospective.Data.Model.Category[]
      {
                new Retrospective.Data.Model.Category(1, "What went well?"),
                new Retrospective.Data.Model.Category(2, "What did we learn?"),
                new Retrospective.Data.Model.Category(4, "What do we want to do differently?")
      };

      return meeting;
    }

    private void DeleteComments(string meetingid)
    {
      var comments = this.database.Comments.GetComments(meetingid);
      foreach (var comment in comments)
      {
        this.database.Comments.Delete((ObjectId)comment.Id);
      }
    }

    private void CreateComments(string meetingid)
    {
      this.database.Comments.SaveComment(CreateComment(meetingid, 1, "Completed our first deployment to production"));
      this.database.Comments.SaveComment(CreateComment(meetingid, 1, "Starting grooming the next set of epics"));
      this.database.Comments.SaveComment(CreateComment(meetingid, 1, "We quickly addressed all the critical defects that were required for prod"));

      this.database.Comments.SaveComment(CreateComment(meetingid, 2, "The e2e tests don't have enough coverage for login"));
      this.database.Comments.SaveComment(CreateComment(meetingid, 2, "The deployment automation is a pattern to keep"));

      this.database.Comments.SaveComment(CreateComment(meetingid, 3, "We need to have better acceptance critria on the user stories"));
    }

    private Comment CreateComment(string meetingid, int category, string text)
    {
      Comment comment = new Comment();
      comment.MeetingId = new ObjectId(meetingid);
      comment.LastUpdateUser = this.owner;
      comment.LastUpdateDate = DateTime.UtcNow;
      comment.CategoryNumber = category;
      comment.Text = text;
      return comment;
    }
  }
}