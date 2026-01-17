import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { ClubService } from '../../services/club.service';
import { Club } from '../../models/club.model';

@Component({
  selector: 'app-club-detail',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './club-detail.component.html',
  styleUrl: './club-detail.component.css'
})
export class ClubDetailComponent implements OnInit {
  club: Club | null = null;
  loading = false;

  constructor(
    private clubService: ClubService,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      const id = +params['id'];
      this.loadClub(id);
    });
  }

  loadClub(id: number) {
    this.loading = true;
    this.clubService.getClubById(id).subscribe({
      next: (data) => {
        this.club = data;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error:', error);
        this.loading = false;
      }
    });
  }

  getImageUrl(path: string): string {
    return `https://localhost:7291${path}`;
  }

  getTotalGoals(): number {
    if (!this.club || !this.club.players) return 0;
    return this.club.players.reduce((total, player) => {
      if (!player.playerDetails) return total;
      return total + player.playerDetails.reduce((sum, detail) => sum + (detail.goalsScored || 0), 0);
    }, 0);
  }
}
