import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core'
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap'
import { TreeGridComponent } from '@syncfusion/ej2-angular-treegrid'
import { ModalDirective } from 'ngx-bootstrap/modal'
import { AlertifyService } from 'src/app/_core/_service/alertify.service'

import { OcService } from './../../../../_core/_service/oc.service'

@Component({
  selector: 'app-oc',
  templateUrl: './oc.component.html',
  styleUrls: ['./oc.component.scss']
})
export class OcComponent implements OnInit {

  @ViewChild('content', { static: true }) content: TemplateRef<any>;
  toolbar: object;
  data: any;
  editing: any;
  contextMenuItems: any;
  pageSettings: any;
  editparams: { params: { format: string } };
  @ViewChild('childModal', { static: false }) childModal: ModalDirective;
  @ViewChild("treegrid")
  public treeGridObj: TreeGridComponent;
  @ViewChild("buildingModal")
  buildingModal: any;
  oc: { id: 0; name: ""; level: 1; parentID: null };
  edit: { id: 0; name: ""; level: 0; parentID: 0 };
  title: string;
  modalReference: NgbModalRef
  constructor(
    private ocService: OcService,
    private modalServices: NgbModal,
    private alertify: AlertifyService

  ) {}

  ngOnInit() {
    this.editing = { allowDeleting: true, allowEditing: true, mode: "Row" };
    this.toolbar = ["Delete", "Search", "Update", "Cancel"];
    this.optionTreeGrid();
    this.onService();

  }

  validation() {
    if (this.oc.name === '') {
      this.alertify.warning('Please enter building name!');
      return false;
    } else {
      return true;
    }
  }

  createOC() {
    if (this.validation()) {
      this.ocService.addOC(this.oc).subscribe(res => {
        this.alertify.success('The OC has been created!!');
        this.modalReference.close();
        this.oc = {
          id: 0,
          name: "",
          parentID: null,
          level: 1
        };
        this.getBuildingsAsTreeView();
      });
    }
  }

  optionTreeGrid() {
    this.contextMenuItems = [
      {
        text: "Add-Sub Item",
        iconCss: " e-icons e-add",
        target: ".e-content",
        id: "Add-Sub-Item",
      },
      {
        text: "Delete",
        iconCss: " e-icons e-delete",
        target: ".e-content",
        id: "DeleteOC",
      },
    ];
    const lang = localStorage.getItem('lang') ;
    if(lang === 'en') {
      this.toolbar = [

        {
          text: 'Add',
          tooltipText: 'Add',
          prefixIcon: 'e-add',
          id: '_gridcontrol_add',
        },
        {
          text: 'Add lower level',
          prefixIcon: 'e-add',
          tooltipText: 'Add lower level',
          id: '_gridcontrol_addsub',
        },
        {
          text: 'Cancel',
          prefixIcon: 'e-cancel',
          id: '_gridcontrol_cancel',
        },
        {
          text: 'ExpandAll',
          id: '_gridcontrol_expandall',
          prefixIcon: 'e-expand'
        },
        {
          text: 'CollapseAll',
          prefixIcon: 'e-collapse',
          id: '_gridcontrol_collapseall',
        }
      ];
    }
    else {
      this.toolbar = [
        {
          text: '新增',
          tooltipText: '新增',
          prefixIcon: 'e-add',
          id: '_gridcontrol_add',
        },
        {
          text: '新增下一階',
          tooltipText: '新增下一階',
          prefixIcon: 'e-add',
          id: '_gridcontrol_addsub',
        },

        {
          text: '取消',
          prefixIcon: 'e-cancel',
          id: '_gridcontrol_cancel',
        },
        {
          text: '展開全部',
          prefixIcon: 'e-expand',
          id: '_gridcontrol_expandall',
        },
        {
          text: '全部收縮',
          prefixIcon: 'e-collapse',
          id: '_gridcontrol_collapseall',
        }
      ];
    }
    // this.toolbar = [
    //   "Search",
    //   "Add",
    //   "新增下一階",
    //   "Cancel",
    //   "ExpandAll",
    //   "CollapseAll",
    // ];
    this.editing = {
      allowEditing: true,
      allowAdding: true,
      allowDeleting: true,
      mode: "Row",
    };
    this.pageSettings = { pageSize: 20 };
    this.editparams = { params: { format: "n" } };
  }

  created() {
    this.getBuildingsAsTreeView();
  }

  onService() {
    this.ocService.currentMessage.subscribe((arg) => {
      if (arg === 200) {
        this.getBuildingsAsTreeView();
      }
    });
  }

  toolbarClick(args) {
    switch (args.item.id) {
      case "_gridcontrol_add":
        args.cancel = true;
        this.openMainModal();
        break;
      case "_gridcontrol_addsub":
        if(this.oc?.parentID !== undefined && this.oc?.parentID !== null) {
          args.cancel = true;
          this.openSubModal();
        } else {
          this.alertify.error("Please choose parent the OC!!!");
        }
        break;
      default:
        break;
    }
  }

  contextMenuClick(args) {
    switch (args.item.id) {
      case "DeleteOC":
        this.delete(args.rowInfo.rowData.entity);
        break;
      case "Add-Sub-Item":
        this.openSubModal();
        break;
      default:
        break;
    }
  }

  delete(data) {
    this.alertify.confirm(
      "Delete OC",
      'Are you sure you want to delete this OC "' + data.name + '" ?',
      () => {
        this.ocService.delete(data.id).subscribe(
          (res) => {
            this.getBuildingsAsTreeView();
            this.alertify.success("The OC has been deleted!!!");
          },
          (error) => {
            this.alertify.error("Failed to delete the OC!!!");
          }
        );
      }
    );
  }

  actionComplete(args) {
    if (args.requestType === "save") {
      this.edit.name = args.data.entity.name;
      this.rename();
    }
  }

  rowSelected(args) {
    this.edit = {
      id: args.data.entity.id,
      name: args.data.entity.name,
      level: args.data.entity.level,
      parentID: args.data.entity.parentId,
    };
    this.oc = {
      id: 0,
      name: "",
      parentID: args.data.entity.id,
      level: args.data.entity.level + 1,
    };
  }

  getBuildingsAsTreeView() {
    this.ocService.getOCs().subscribe((res) => {
      this.data = res;
    });
  }

  clearFrom() {
    this.oc = {
      id: 0,
      name: "",
      parentID: null,
      level: 1,
    };
  }
  rename() {
    this.ocService.rename(this.edit).subscribe((res) => {
      this.getBuildingsAsTreeView();
      this.alertify.success("The oc has been changed!!!");
    });
  }
  openMainModal() {
    this.clearFrom();
    this.modalReference = this.modalServices.open(this.content, { size: "sm"});
    this.title = "Add Main OC";
    this.oc = this.oc;
    this.modalReference.result.then((result) => {
    }, (reason) => {
    });

  }
  openSubModal() {
    this.modalReference = this.modalServices.open(this.content, {
      size: "sm",
    });
    this.title = "Add Sub OC";
    this.oc = this.oc;

    this.modalReference.result.then((result) => {
    }, (reason) => {
    });
  }

}
