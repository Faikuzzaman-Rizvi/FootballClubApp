import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Club, Position } from '../models/club.model';

@Injectable({
  providedIn: 'root'
})
export class ClubService {
  private apiUrl = 'https://localhost:7291/api/Clubs';

  constructor(private http: HttpClient) { }

  getAllClubs(): Observable<Club[]> {
    return this.http.get<Club[]>(this.apiUrl);
  }

  getClubById(id: number): Observable<Club> {
    return this.http.get<Club>(`${this.apiUrl}/${id}`);
  }

  addClub(formData: FormData): Observable<Club> {
    return this.http.post<Club>(this.apiUrl, formData);
  }

  updateClub(id: number, formData: FormData): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, formData);
  }

  deleteClub(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  getPositions(): Observable<Position[]> {
    return this.http.get<Position[]>(`${this.apiUrl}/GetPositions`);
  }
}
