import { Meeting } from "./meeting";
import {TeamMember} from "./teammember";

export class Team{
  teamId: string;
  name: string;
  owner: string
  members: TeamMember[];
  meetings: Meeting[];



}