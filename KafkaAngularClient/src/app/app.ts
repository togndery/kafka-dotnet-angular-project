import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, CommonModule, FormsModule],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App {
  sensorId = 'Docker_Sensor_A';
  temperature = 24;
  statusText = '';
  constructor(private http:HttpClient){}

  sendToBackend(){
    const url = 'http://localhost:5000/api/temperature/report'

    this.http.post(url,{sensorId:this.sensorId ,temperature:this.temperature})
    .subscribe({
      next:(res:any)=>{
        this.statusText=`Server respons: ${res.message}`;
      },
      error:(err) => {
        this.statusText=`Server not respons`
        console.error(err);
      }
    })
  }
  protected readonly title = signal('KafkaAngularClient');
}
