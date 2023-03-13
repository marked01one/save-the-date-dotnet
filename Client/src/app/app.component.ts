import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Dating App';
  users: any;

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.http.get("https://localhost:5001/api/users").subscribe({
      // This will return if request is successful...
      next: res => this.users = res,
      // ...if request is NOT successful
      error: e => console.log(e),
      // ...and return regardless of results
      complete: () => console.log('Request has been completed')
    })
  }


}
