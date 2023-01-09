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
import { Custom } from '../_model/contribution';


@Directive({
  selector: '[targetHeight]',
  outputs: [
		"targetHeight",
	]
})
export class TargetHeightDirective implements AfterViewInit, OnDestroy , OnInit {
  @Input()
  targetHeight: string;
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
      this.contentHeight(this.elementRef.nativeElement, this.targetHeight);
    }, 500);
  }


  ngOnDestroy(){
    this.subscription.forEach(item => item.unsubscribe());
  }

  contentHeight(parent: HTMLElement, className) {
    var height = parent.offsetHeight;
    var month = className;
    // var actionId = className.actionId;
    var data = new Custom();
    data.value = height;
    data.month = month;
    // data.actionId = actionId;
    this.dataService.changeMessageTarget(data)

  }

}
