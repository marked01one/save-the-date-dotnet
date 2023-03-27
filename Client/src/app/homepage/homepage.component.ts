import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-homepage',
  templateUrl: './homepage.component.html',
  styleUrls: ['./homepage.component.css']
})
export class HomepageComponent implements OnInit {
  registerMode = false;
  users: any;

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.getUsers();
  }

  registerToggle() {
    this.registerMode = !this.registerMode;
  }

  getUsers() {
    this.http.get("https://localhost:5001/api/users").subscribe({
      // This will return if request is successful...
      next: res => this.users = res,
      // ...if request is NOT successful
      error: e => console.log(e),
      // ...and return regardless of results
      complete: () => console.log('Request has been completed')
    })
  }

  cancelRegisterMode(event: boolean) {
    this.registerMode = event;
  }

}
