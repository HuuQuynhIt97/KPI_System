import { EnvService } from './../../../../../_core/_service/env.service';
import { Todolist2Service } from 'src/app/_core/_service/todolist2.service';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FileInfo, RemovingEventArgs, SelectedEventArgs, UploaderComponent } from '@syncfusion/ej2-angular-inputs';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-score-upload-file',
  templateUrl: './score-upload-file.component.html',
  styleUrls: ['./score-upload-file.component.scss']
})
export class ScoreUploadFileComponent implements OnInit {

  @ViewChild('defaultupload')
  public uploadObj: UploaderComponent;
  @Input() data: any;
  @Input() scoreBy: any;
  @Input() scoreTo: any;
  @Input() currentTime: any;
  base = environment.apiUrl;
  totalSize: number = 0;
  size: string;
  public allowExtensions: string = '.doc, .docx, .xls, .xlsx, .pdf, .png, .jpg, .msg';
  constructor(
    public activeModal: NgbActiveModal,
    public service: Todolist2Service,
    public env: EnvService

    ) { }
  files = [
  ]
  public dropElement: HTMLElement = document.getElementsByClassName('control_wrapper')[0] as HTMLElement;

  public path: Object = {
    saveUrl: ''
  };

  ngOnInit() {
    this.loadData();
    this.path = {
      saveUrl: `${this.env.apiUrl}UploadFile/SaveScore?campaignID=${this.data.campaignID}
      &headingID=${this.data.attitudeHeadingID}
      &uploadFrom=${this.scoreBy}
      &uploadTo=${this.scoreTo}
      `,
      removeUrl: `${this.env.apiUrl}UploadFile/removeScore?campaignID=${this.data.campaignID}
      &headingID=${this.data.attitudeHeadingID}
      &uploadFrom=${this.scoreBy}
      &uploadTo=${this.scoreTo}
      `
    }
    this.dropElement = document.getElementsByClassName('control_wrapper')[0] as HTMLElement;
  }
  public onFileRemove(args: RemovingEventArgs): void {
    args.postRawFile = false;
  }
  ngOnDestroy(): void {
    this.service.changeUploadMessage(false);
  }
  success(args) {
    this.service.changeUploadMessage(true);

  }
  loadData() {
    this.files = [];
    this.service.getAttackFilesScore(this.data.campaignID, this.data.attitudeHeadingID,this.scoreBy, this.scoreTo).subscribe(res => {
      this.files = res as any || [];
    });
  }
  beforeUpload(args) {
    if(args.response.statusCode == 400) {
      args.statusText = "File already exists ! 此檔案已存在，請更改檔案名稱、重新上傳！";
    }else {

      args.statusText = args.response.statusText;
    }
  }
  public onSelected(args : SelectedEventArgs) : void {
    args.filesData.splice(5);
    let filesData : FileInfo[] = this.uploadObj.getFilesData();
    let allFiles : FileInfo[] = filesData.concat(args.filesData);
    if (allFiles.length > 5) {
        for (let i : number = 0; i < allFiles.length; i++) {
            if (allFiles.length > 5) {
                allFiles.shift();
            }
        }
        args.filesData = allFiles;
        args.modifiedFilesData = args.filesData;
    }
    args.isModified = true;

    for (let file of args.filesData) {
        this.totalSize = this.totalSize + file.size;
        this.size = this.uploadObj.bytesToSize(this.totalSize);
    }

    
  }

}
