import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { AttitudeScoreService } from 'src/app/_core/_service/attitude-score.service';
import { EnvService } from 'src/app/_core/_service/env.service';

@Component({
  selector: 'app-l2-download-file',
  templateUrl: './l2-download-file.component.html',
  styleUrls: ['./l2-download-file.component.scss']
})
export class L2DownloadFileComponent implements OnInit {
  @Input() campaignID: any;
  @Input() type: any;
  files = [];
  base_download: any
  base: any
  constructor(
    public activeModal: NgbActiveModal,
    private attitudeScoreService: AttitudeScoreService,
    private env: EnvService
  ) {
    this.base = this.env.apiUrl.replace('/api/', '')
    this.base_download = this.env.apiUrl
   }

  ngOnInit() {
  }

  downloadFile(item) {
    const file_open_brower = ['png', 'jpg','pdf']
    var ext =  item.name.split('.').pop();
    if(file_open_brower.includes(ext)) {
      window.open(this.base + item.path,'_blank')
    } else {
      const url = `${this.base_download}UploadFile/DownloadFileScore/${item.name}`
      this.attitudeScoreService.download(url).subscribe(data =>{
        const blob = new Blob([data]);
        const downloadURL = window.URL.createObjectURL(data);
        const link = document.createElement('a');
        link.href = downloadURL;
        const ct = new Date();
        link.download = `${item.name}`;
        link.click();
      })
    }
  }

}
