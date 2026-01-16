import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ClubService } from '../../services/club.service';
import { Club } from '../../models/club.model';

@Component({
  selector: 'app-club-list',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './club-list.component.html',
  styleUrl: './club-list.component.css'
})
export class ClubListComponent implements OnInit {
  clubs: Club[] = [];
  loading = false;

  constructor(private clubService: ClubService) { }

  ngOnInit() {
    this.loadClubs();
  }

  loadClubs() {
    this.loading = true;
    this.clubService.getAllClubs().subscribe({
      next: (data) => {
        this.clubs = data;
        this.loading = false;
        console.log('Clubs:', data);
      },
      error: (error) => {
        console.error('Error:', error);
        alert('Failed to load clubs!');
        this.loading = false;
      }
    });
  }

  deleteClub(id: number) {
    if (confirm('Are you sure?')) {
      this.clubService.deleteClub(id).subscribe({
        next: () => {
          alert('Deleted successfully!');
          this.loadClubs();
        },
        error: (error) => {
          console.error('Error:', error);
          alert('Failed to delete!');
        }
      });
    }
  }

  getImageUrl(path: string): string {
    return `https://localhost:7291${path}`;
  }
}
