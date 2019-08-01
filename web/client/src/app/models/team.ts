import { Meeting } from "./meeting";
import { TeamMember } from "./teammember";
import { Invitation } from "./invitation";

export class Team {
  teamId: string;
  name: string;
  owner: string
  members: TeamMember[];
  invited: Invitation[];
  meetings: Meeting[];
  message: string;
}