import { Component, OnInit, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ClubService } from '../../services/club.service';
import { Club } from '../../models/club.model';

declare var bootstrap: any;

@Component({
  selector: 'app-club-list',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './club-list.component.html',
  styleUrl: './club-list.component.css'
})
export class ClubListComponent implements OnInit, AfterViewInit {
  clubs: Club[] = [];
  loading = false;

  constructor(private clubService: ClubService) { }

  ngOnInit() {
    this.loadClubs();
  }

  ngAfterViewInit() {
    // Initialize carousel after view loads
    setTimeout(() => {
      const carouselElement = document.querySelector('#heroCarousel');
      if (carouselElement && typeof bootstrap !== 'undefined') {
        new bootstrap.Carousel(carouselElement, {
          interval: 4000,
          ride: 'carousel',
          wrap: true,
          touch: true,
          keyboard: true
        });
      }
    }, 100);
  }

  loadClubs() {
    this.loading = true;
    this.clubService.getAllClubs().subscribe({
      next: (data) => {
        this.clubs = data;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error:', error);
        alert('Failed to load clubs!');
        this.loading = false;
      }
    });
  }

  deleteClub(id: number) {
    if (confirm('Are you sure you want to delete this club?')) {
      this.clubService.deleteClub(id).subscribe({
        next: () => {
          alert('Club deleted successfully!');
          this.loadClubs();
        },
        error: (error) => {
          console.error('Error:', error);
          alert('Failed to delete club!');
        }
      });
    }
  }

  getImageUrl(path: string): string {
    return `https://localhost:7291${path}`;
  }

  getTotalGoals(club: Club): number {
    if (!club.players) return 0;
    return club.players.reduce((total, player) => {
      if (!player.playerDetails) return total;
      return total + player.playerDetails.reduce((sum, detail) => sum + (detail.goalsScored || 0), 0);
    }, 0);
  }

  getTotalMatches(club: Club): number {
    if (!club.players) return 0;
    const matchSet = new Set<number>();
    club.players.forEach(player => {
      if (player.playerDetails) {
        player.playerDetails.forEach(detail => {
          if (detail.matchesPlayed) matchSet.add(detail.matchesPlayed);
        });
      }
    });
    return Array.from(matchSet).reduce((a, b) => Math.max(a, b), 0);
  }

  getTotalPlayers(): number {
    return this.clubs.reduce((total, club) => total + (club.players?.length || 0), 0);
  }

  getTotalGoalsAll(): number {
    return this.clubs.reduce((total, club) => total + this.getTotalGoals(club), 0);
  }

  // Scroll to clubs section
  scrollToClubs(): void {
    const element = document.getElementById('clubsSection');
    if (element) {
      element.scrollIntoView({ behavior: 'smooth', block: 'start' });
    }
  }
}
