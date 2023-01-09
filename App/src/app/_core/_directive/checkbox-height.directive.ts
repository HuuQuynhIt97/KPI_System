import { DataService } from 'src/app/_core/_service/data.service';
import {
  AfterViewInit,
  Directive,
  ElementRef,
  HostBinding,
  Input,
  OnDestroy,
  OnInit,
  Renderer2,
} from '@angular/core';
import { fromEvent, Subject, Subscription } from 'rxjs';
import { debounceTime, throttleTime } from 'rxjs/operators';
import { Custom, CustomCheckBox } from '../_model/contribution';


@Directive({
  selector: '[checkboxHeight]',
  outputs: [
		"checkboxHeight",
	]
})
export class CheckboxHeightDirective implements AfterViewInit, OnDestroy , OnInit {
  @Input()
  checkboxHeight: string;
  data: Array<Custom> = [];
  subscription: Subscription[] = [];
  subject = new Subject<string>();
  constructor(
    private renderer: Renderer2,
    private dataService: DataService,
    private elementRef: ElementRef
    ) {

  }

  ngAfterViewInit() {
  }

  ngOnInit(): void {
    setTimeout(() => {
      this.contentHeight(this.elementRef.nativeElement, this.checkboxHeight);
    }, 500);
  }


  ngOnDestroy(){
    this.subscription.forEach(item => item.unsubscribe());
  }

  contentHeight(parent: HTMLElement, className) {
    var height = parent.offsetHeight;
    var behaviorId = className.id;
    var data = new CustomCheckBox();
    data.value = height;
    data.behaviorId = behaviorId;
    this.dataService.changeMessageCheckbox(data)

  }

}
