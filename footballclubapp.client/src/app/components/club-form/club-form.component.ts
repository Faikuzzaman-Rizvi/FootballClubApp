import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { ClubService } from '../../services/club.service';
import { Position } from '../../models/club.model';

@Component({
  selector: 'app-club-form',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './club-form.component.html',
  styleUrl: './club-form.component.css'
})
export class ClubFormComponent implements OnInit {

  clubData = {
    clubCode: '',
    clubName: '',
    foundedDate: '',
    stadiumName: '',
    isActive: true,
    clubLogo: '',
    players: [] as any[]
  };

  clubLogoFile: File | null = null;
  playerPhotoFiles: File[] = [];
  positions: Position[] = [];
  isEditMode = false;
  clubId: number = 0;

  constructor(
    private clubService: ClubService,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.loadPositions();
    this.route.params.subscribe(params => {
      if (params['id']) {
        this.isEditMode = true;
        this.clubId = +params['id'];
        this.loadClub(this.clubId);
      }
    });
  }

  loadPositions() {
    this.clubService.getPositions().subscribe({
      next: (data) => {
        this.positions = data;
      }
    });
  }

  loadClub(id: number) {
    this.clubService.getClubById(id).subscribe({
      next: (data) => {
        this.clubData = {
          clubCode: data.clubCode,
          clubName: data.clubName,
          foundedDate: data.foundedDate.split('T')[0],
          stadiumName: data.stadiumName,
          isActive: data.isActive,
          clubLogo: data.clubLogo, 
          players: data.players?.map(p => ({
            playerCode: p.playerCode,
            playerName: p.playerName,
            dateOfBirth: p.dateOfBirth.split('T')[0],
            nationality: p.nationality,
            jerseyNumber: p.jerseyNumber,
            playerPhoto: p.playerPhoto, 
            playerDetails: p.playerDetails?.map(pd => ({
              positionName: pd.position?.positionName || '',
              contractStart: pd.contractStart.split('T')[0],
              contractEnd: pd.contractEnd.split('T')[0],
              annualSalary: pd.annualSalary,
              goalsScored: pd.goalsScored,
              assists: pd.assists,
              matchesPlayed: pd.matchesPlayed
            })) || []
          })) || []
        };
      }
    });
  }

  onClubLogoSelect(event: any) {
    this.clubLogoFile = event.target.files[0];
  }

  onPlayerPhotoSelect(event: any, index: number) {
    this.playerPhotoFiles[index] = event.target.files[0];
  }

  addPlayer() {
    this.clubData.players.push({
      playerCode: '',
      playerName: '',
      dateOfBirth: '',
      nationality: '',
      jerseyNumber: 0,
      playerPhoto: '', 
      playerDetails: []
    });
  }

  removePlayer(index: number) {
    this.clubData.players.splice(index, 1);
    this.playerPhotoFiles.splice(index, 1);
  }

  addPlayerDetail(playerIndex: number) {
    this.clubData.players[playerIndex].playerDetails.push({
      positionName: '',
      contractStart: '',
      contractEnd: '',
      annualSalary: 0,
      goalsScored: 0,
      assists: 0,
      matchesPlayed: 0
    });
  }

  removePlayerDetail(playerIndex: number, detailIndex: number) {
    this.clubData.players[playerIndex].playerDetails.splice(detailIndex, 1);
  }

  // Helper method to extract filename from path
  getFileName(path: string): string {
    if (!path) return '';
    const parts = path.split('/');
    return parts[parts.length - 1];
  }

  onSubmit() {
    const formData = new FormData();
    formData.append('clubData', JSON.stringify(this.clubData));

    // Only append club logo if a new file is selected
    if (this.clubLogoFile) {
      formData.append('clubLogo', this.clubLogoFile);
      console.log('Club logo file added');
    } else {
      console.log('No club logo file selected');
    }

    // Create a mapping of player indices to their photo files
    const photoIndices: number[] = [];
    this.clubData.players.forEach((player, index) => {
      if (this.playerPhotoFiles[index]) {
        photoIndices.push(index);
        formData.append('playerPhotos', this.playerPhotoFiles[index]);
        console.log(`Player ${index} photo added:`, this.playerPhotoFiles[index].name);
      } else {
        console.log(`Player ${index} - no new photo, existing:`, player.playerPhoto);
      }
    });

    formData.append('photoIndices', JSON.stringify(photoIndices));
    console.log('Photo indices:', photoIndices);

    if (this.isEditMode) {
      this.clubService.updateClub(this.clubId, formData).subscribe({
        next: (response) => {
          console.log('Update response:', response);
          alert('Updated successfully!');
          this.router.navigate(['/clubs']);
        },
        error: (error) => {
          console.error('Update Error:', error);
          console.error('Error details:', error.error);
          alert('Failed to update: ' + (error.error?.message || error.message));
        }
      });
    } else {
      this.clubService.addClub(formData).subscribe({
        next: (response) => {
          console.log('Add response:', response);
          alert('Added successfully!');
          this.router.navigate(['/clubs']);
        },
        error: (error) => {
          console.error('Add Error:', error);
          console.error('Error details:', error.error);
          alert('Failed to add: ' + (error.error?.message || error.message));
        }
      });
    }
  }
}
