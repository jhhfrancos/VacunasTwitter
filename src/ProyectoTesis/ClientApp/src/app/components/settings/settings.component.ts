import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css']
})
export class SettingsComponent{

  //UpdateDB button
  @Output() updateDB = new EventEmitter<any>();
  @Input() updateDBdisabled: boolean;

  constructor() { }

  UpdateDB(){
    this.updateDB.emit();
  }

}
