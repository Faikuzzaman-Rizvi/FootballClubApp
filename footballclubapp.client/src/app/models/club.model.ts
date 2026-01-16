export interface Club {
  clubId: number;
  clubCode: string;
  clubName: string;
  foundedDate: string;
  stadiumName: string;
  isActive: boolean;
  clubLogo: string;
  players?: Player[];
}

export interface Player {
  playerId: number;
  playerCode: string;
  playerName: string;
  dateOfBirth: string;
  nationality: string;
  jerseyNumber: number;
  isActive: boolean;
  playerPhoto: string;
  clubId: number;
  club?: Club;
  playerDetails?: PlayerDetail[];
}

export interface PlayerDetail {
  playerDetailId: number;
  playerId: number;
  positionId: number;
  contractStart: string;
  contractEnd: string;
  annualSalary: number;
  goalsScored: number;
  assists: number;
  matchesPlayed: number;
  player?: Player;
  position?: Position;
}

export interface Position {
  positionId: number;
  positionName: string;
  description: string;
}
