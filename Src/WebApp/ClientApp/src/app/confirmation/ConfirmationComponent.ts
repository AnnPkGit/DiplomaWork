import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-confirmation',
  templateUrl: './confirmation.component.html',
  styleUrls: ['./confirmation.component.css']
})
export class ConfirmationComponent implements OnInit {

  loading:boolean = true;
  smthWrong: boolean = false;
  constructor(private route: ActivatedRoute, private http: HttpClient) { }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      const id = params['id'];
      const token = params['token'];

      if(!id && !token) {
        this.smthWrong = true;
        return;
      }

      console.log('ID:', id);
      console.log('Token:', token);

      this.http.get('api/v1/auth/confirmation/email?id=' + id + '&token=' + token).subscribe((response) => {
        this.loading = false;
        },
        (error) => {
            this.loading = false;
            this.smthWrong = true;
        }
        );
      });
  }
}